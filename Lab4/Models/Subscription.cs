using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab4.Models
{
    public class Subscription
    {
        public int ID { get; set; }
        public int ClientId { get; set; }

        public string BrokerageId { get; set; }

        public Client Client { get; set; }

        public Brokerage Brokerages { get; set; }
    }
}