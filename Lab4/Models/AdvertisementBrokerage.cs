using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab4.Models
{
    public class AdvertisementBrokerage
    {
        public int Id { get; set; }
        public int AdvertisementId { get; set; }
        public Advertisement Advertisement { get; set; }
        public string BrokerageId { get; set; }
        public Brokerage Brokerage { get; set; }
    }
}