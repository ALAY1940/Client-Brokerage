﻿using Azure.Storage.Blobs;
using Lab4.Data;
using Lab4.Models.ViewModels;
using Lab4.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Azure;
using System.IO;

namespace Lab4.Controllers
{
    public class AdvertisementsController : Controller
    {
        private readonly MarketDbContext _context;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string containerName = "advertisements";

        public AdvertisementsController(MarketDbContext context, BlobServiceClient blobServiceClient)
        {
            _context = context;
            _blobServiceClient = blobServiceClient;
        }

        public IActionResult Index(string ID)
        {
            var brokerage = _context.Brokerages.Where(brokerage => brokerage.ID == ID).FirstOrDefault();
            var advertisementViewModel = new AdsViewModel();
            advertisementViewModel.Brokerage = brokerage;
            var brokerageAds = _context.AdvertisementBrokerages.Where(brokerage => brokerage.BrokerageId == ID).Include(brokerage => brokerage.Advertisement);

            advertisementViewModel.Advertisements = brokerageAds.Select(brokerage => brokerage.Advertisement).ToList();
            return View(advertisementViewModel);
        }



        public IActionResult Create(string ID)
        {
            return View(_context.Brokerages.Find(ID));
        }

        public async Task<IActionResult> Delete(int? ID)
        {
            if (ID == null)
            {
                return NotFound();
            }

            var image = await _context.Advertisements
                .FirstOrDefaultAsync(brokerage => brokerage.ID == ID);
            if (image == null)
            {
                return NotFound();
            }

            return View(image);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int ID)
        {
            var image = await _context.Advertisements.FindAsync(ID);
            var adsViewModel = new AdsViewModel();

            var advBrokerage = _context.AdvertisementBrokerages
                .Where(adv => adv.AdvertisementId == ID).Include(ac => ac.Brokerage).Single();

            var brokerage = advBrokerage.Brokerage;

            adsViewModel.Brokerage = brokerage;

            var ads = _context.AdvertisementBrokerages.Where(c => c.BrokerageId == brokerage.ID)
                .Include(brokerage => brokerage.Advertisement)
                .Select(ac => ac.Advertisement).Where(adv => adv.ID != image.ID).ToList();

            adsViewModel.Advertisements = ads;

            BlobContainerClient containerClient;
            // Get the container and return a container client object
            try
            {
                containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            }
            catch (RequestFailedException)
            {
                return View("Error");
            }

            try
            {
                // Get the blob that holds the data
                var blockBlob = containerClient.GetBlobClient(image.FileName);
                if (await blockBlob.ExistsAsync())
                {
                    await blockBlob.DeleteAsync();
                }

                _context.Advertisements.Remove(image);
                await _context.SaveChangesAsync();

            }
            catch (RequestFailedException)
            {
                return View("Error");
            }

            //_context.AdvertisementCommunity.Remove(advCommunity);

            return View("Index", adsViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(string ID, IFormFile file)
        {
            BlobContainerClient containerClient;
            // Create the container and return a container client object
            try
            {
                containerClient = await _blobServiceClient.CreateBlobContainerAsync(containerName);
                // Give access to public
                containerClient.SetAccessPolicy(Azure.Storage.Blobs.Models.PublicAccessType.BlobContainer);
            }
            catch (RequestFailedException)
            {
                containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            }


            try
            {
                // create the blob to hold the data
                var blockBlob = containerClient.GetBlobClient(file.FileName);
                if (await blockBlob.ExistsAsync())
                {
                    await blockBlob.DeleteAsync();
                }

                using (var memoryStream = new MemoryStream())
                {
                    // copy the file data into memory
                    await file.CopyToAsync(memoryStream);

                    // navigate back to the beginning of the memory stream
                    memoryStream.Position = 0;

                    // send the file to the cloud
                    await blockBlob.UploadAsync(memoryStream);
                    memoryStream.Close();
                }

                // add the photo to the database if it uploaded successfully
                var image = new Advertisement();
                image.URL = blockBlob.Uri.AbsoluteUri;
                image.FileName = file.FileName;

                _context.Advertisements.Add(image);
                _context.SaveChanges();

                var brokerageAds = new AdvertisementBrokerage();
                brokerageAds.AdvertisementId = image.ID;
                brokerageAds.BrokerageId = ID;

                _context.AdvertisementBrokerages.Add(brokerageAds);
                _context.SaveChanges();
            }
            catch (RequestFailedException)
            {
                View("Error");
            }

            return RedirectToAction("Index", new { id = ID });
        }
    }
}