using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
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
            Auction? auction = _context.Auctions.Include(a => a.Item).FirstOrDefault(a => a.Id == id);

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


        [HttpPost]
        public ActionResult<Guid> CreateAuction([FromBody] CreateAuctionDto request)
        {
            try
            {
                Auction a = new Auction()
                {
                    Id = Guid.NewGuid(),
                    AuctionEnd = request.AuctionEnd,
                    ReservePrice = request.ReservePrice,
                    Item = new()
                    {
                        Make = request.Make,
                        Model = request.Model,
                        Year = request.Year,
                        Color = request.Color,
                        Mileage = request.Mileage,
                        ImageUrl = request.ImageUrl
                    }
                };


                _context.Auctions.Add(a);

                if (_context.SaveChanges() == 0) return BadRequest();

                return CreatedAtAction(nameof(GetAuctionById), new { a.Id });

            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        public ActionResult<AuctionDto> UpdateAuction(Guid id, [FromBody] UpdateAuctionDto request)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    Auction? auction = _context.Auctions.Include(a => a.Item).FirstOrDefault(a => a.Id == id);

                    if (auction == null) return BadRequest();


                    auction.Item.Model = request.Model ?? auction.Item.Model;
                    auction.Item.Make = request.Make ?? auction.Item.Make;
                    auction.Item.Year = request.Year ?? auction.Item.Year;
                    auction.Item.Color = request.Color ?? auction.Item.Color;
                    auction.Item.Mileage = request.Mileage ?? auction.Item.Mileage;

                    _context.SaveChanges();
                    transaction.Commit();
                    return Ok(new AuctionDto
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
                    });
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return BadRequest();
                }
            }
        }

        [HttpDelete("{id}")]
        public ActionResult<Guid> DeleteAuction(Guid id)
        {
            Auction? auction = _context.Auctions.FirstOrDefault(a => a.Id == id);

            if (auction != null)
            {
                _context.Auctions.Remove(auction);
                _context.SaveChanges();

                return Ok(id);
            }

            return BadRequest();
        }




    }
}