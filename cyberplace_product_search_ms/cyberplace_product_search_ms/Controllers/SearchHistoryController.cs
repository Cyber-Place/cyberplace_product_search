using cyberplace_product_search_ms.Controllers.Models;
using cyberplace_product_search_ms.Models;
using cyberplace_product_search_ms.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cyberplace_product_search_ms.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SearchHistoryController : ControllerBase
    {
        public SearchHistoryService _searchHistoryService;
        public SearchItemService _searchItemService;

        public SearchHistoryController(SearchHistoryService searchHistoryService, SearchItemService searchItemService)
        {
            _searchHistoryService = searchHistoryService;
            _searchItemService = searchItemService;
        }

        [HttpGet]
        [HttpGet("{id}")]
        public ActionResult<List<SearchHistory>> Get(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                List<SearchHistory> searchHistories = _searchHistoryService.GetAll();
                return searchHistories;
            }
            else if (!(ObjectId.TryParse(id, out _)))
            {
                return NotFound();
            }
            return _searchHistoryService.Get(id);
        }

        [HttpPost]
        public ActionResult<SearchHistory> Create(SearchHistory searchHistory)
        {
            if (!(searchHistory.Username is not null) || !(searchHistory.Email is not null)) return BadRequest();
            _searchHistoryService.Create(searchHistory);
            return Ok(searchHistory);
        }

        [HttpPut]
        public ActionResult<SearchHistory> Update(SearchHistory searchHistory)
        {
            _searchHistoryService.Update(searchHistory.Id, searchHistory);
            return Ok(searchHistory);
        }

        [HttpDelete("{id}")]
        public ActionResult<SearchHistory> Delete(string id)
        {
            if (!(ObjectId.TryParse(id, out _))) return NotFound();
            _searchHistoryService.Delete(id);
            return Ok();
        }

        [HttpPost("additem/{id}")]
        public ActionResult<SearchHistory> AddItem(string id, SearchItem searchItem)
        {
            if (!(ObjectId.TryParse(id, out _))) return NotFound();
            if (!(searchItem.Product is not null)) return BadRequest();
            if (!(searchItem.Product.Id is not null)) return BadRequest();
            _searchItemService.Create(searchItem);
            _searchHistoryService.AddItem(id, searchItem);
            return Ok(_searchHistoryService.Get(id));
        }

        [HttpDelete("removeitem")]
        public ActionResult<SearchHistory> RemoveItem(string idHistory, string idItem)
        {
            if (!(ObjectId.TryParse(idHistory, out _)) || !(ObjectId.TryParse(idItem, out _))) return NotFound();
            _searchHistoryService.RemoveItem(idHistory, idItem);
            return Ok(_searchHistoryService.Get(idHistory));
        }

        [HttpDelete("removeallitems/{idHistory}")]
        public ActionResult<SearchHistory> RemoveAllItems(string idHistory)
        {
            if (!(ObjectId.TryParse(idHistory, out _))) return NotFound();
            return Ok(_searchHistoryService.RemoveAllItems(idHistory));
        }
    }
}
