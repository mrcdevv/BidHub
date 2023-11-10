using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Controller
{
  [ApiController]
  [Route("api/search")]
  public class SearchController : ControllerBase
  {

    [HttpGet]
    public async Task<ActionResult<List<Item>>> SearchItems([FromQuery] SearchParams searchParams)
    {
      var query = DB.PagedSearch<Item, Item>();

      if (!string.IsNullOrEmpty(searchParams.SearchTerm))
      {
        query.Match(Search.Full, searchParams.SearchTerm).SortByTextScore();
      }

      query = searchParams.OrderBy switch
      {
        "make" => query.Sort(i => i.Ascending(x => x.Make)),
        "new" => query.Sort(i => i.Descending(x => x.CreatedAt)),
        _ => query.Sort(i => i.Ascending(x => x.AuctionEnd))
      };

      query = searchParams.FilterBy switch
      {
        "finished" => query.Match(i => i.AuctionEnd < DateTime.UtcNow),
        "endingSoon" => query.Match(i => i.AuctionEnd > DateTime.UtcNow && i.AuctionEnd < DateTime.UtcNow.AddHours(6)),
        _ => query.Match(i => i.AuctionEnd > DateTime.UtcNow)
      };

      if (!string.IsNullOrEmpty(searchParams.Seller))
      {
        query.Match(i => i.SellerUsername == searchParams.Seller);
      }

      if (!string.IsNullOrEmpty(searchParams.Winner))
      {
        query.Match(i => i.WinnerUsername == searchParams.Winner);
      }

      query.PageNumber(searchParams.PageNumber);
      query.PageSize(searchParams.PageSize);

      var result = await query.ExecuteAsync();

      return Ok(new
      {
        results = result.Results,
        pageCount = result.PageCount,
        totalCount = result.TotalCount

      });
    }

  }
}