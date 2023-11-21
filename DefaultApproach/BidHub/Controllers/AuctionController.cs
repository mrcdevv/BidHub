using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BidHub.DTOs;
using BidHub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BidHub.Controllers
{
    [ApiController]
    [Route("api/auctions")]
    public class AuctionController : ControllerBase
    {
        private readonly BidHubContext _context = new();

        [HttpGet]
        public ActionResult<List<AuctionDto>> GetAuctions()
        {
            IQueryable<Auction> lst = _context.Auctions.Include(a => a.Item);

            List<AuctionDto> response = lst.Select(a => new AuctionDto
            {
                Id = a.Id,
                CreatedAt = a.CreatedAt,
                UpdatedAt = a.UpdatedAt,
                AuctionEnd = a.AuctionEnd,
                Seller = a.Seller,
                Winner = a.Winner,
                Make = a.Item.Make,
                Model = a.Item.Model,
                Year = a.Item.Year,
                Color = a.Item.Color,
                Mileage = a.Item.Mileage,
                ImageUrl = a.Item.ImageUrl,
                Status = a.Status.ToString(),
                ReservePrice = a.ReservePrice,
                SoldAmount = a.SoldAmount,
                CurrentHighBid = a.CurrentHighBid,
            }).ToList();

            return Ok(response);
        }

        [HttpGet("{id}")]
        public ActionResult<AuctionDto> GetAuctionById(Guid id)
        {
            Auction? auction = _context.Auctions.FirstOrDefault(a => a.Id == id);

            if (auction != null)
            {
                AuctionDto response = new()
                {
                    Id = auction.Id,
                    CreatedAt = auction.CreatedAt,
                    UpdatedAt = auction.UpdatedAt,
                    AuctionEnd = auction.AuctionEnd,
                    Seller = auction.Seller,
                    Winner = auction.Winner,
                    Make = auction.Item.Make,
                    Model = auction.Item.Model,
                    Year = auction.Item.Year,
                    Color = auction.Item.Color,
                    Mileage = auction.Item.Mileage,
                    ImageUrl = auction.Item.ImageUrl,
                    Status = auction.Status.ToString(),
                    ReservePrice = auction.ReservePrice,
                    SoldAmount = auction.SoldAmount,
                    CurrentHighBid = auction.CurrentHighBid,
                };

                return Ok(response);
            }

            return NotFound();
        }




    }
}