using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab4.Models.ViewModels
{
    public class ClientSubscriptionViewModel
    {
        public Client Client { get; set; }

        public IEnumerable<BrokerageSubscriptionViewModel> Memberships { get; set; }
    }
}