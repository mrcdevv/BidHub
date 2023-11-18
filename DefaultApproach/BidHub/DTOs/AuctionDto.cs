using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BidHub.DTOs
{
    public class AuctionDto
    {
        public Guid Id { get; set; }
        public int CreatedAt { get; set; }
        public int UpdatedAt { get; set; }
        public int Seller { get; set; }
        public int Winner { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string Color { get; set; }
        public int Mileage { get; set; }
        public string ImageUrl { get; set; }
        public string Status { get; set; }
        public int ReservePrice { get; set; }
        public int SoldAmount { get; set; }
        public int CurrentHighBid { get; set; }
    }
}