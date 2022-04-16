using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab4.Models.ViewModels
{
    public class FileInputViewModel
    {
        public string BrokerageId { get; set; }
        public string BrokerageTitle { get; set; }
        public IFormFile File { get; set; }

    }
}