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
            var lst = _context.Auctions.Include(a => a.Item).ToList();

            return Ok();
        }

    }
}