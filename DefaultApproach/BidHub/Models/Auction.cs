using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BidHub.Models
{
    public class Auction
    {
        public Guid Id { get; set; }
        public int ReservePrice { get; set; }
        public int Seller { get; set; }
        public int Winner { get; set; }
        public int SoldAmount { get; set; }
        public int CurrentHighBid { get; set; }
        public int CreatedAt { get; set; }
        public int UpdatedAt { get; set; }
        public int Status { get; set; }
        public Guid ItemId { get; set; }
        public Item Item { get; set; }
    }
}