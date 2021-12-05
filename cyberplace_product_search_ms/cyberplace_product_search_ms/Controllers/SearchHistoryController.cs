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

        public ActionResult<List<SearchHistory>> GetAll()
        {
            return _searchHistoryService.GetAll();
        }

        [HttpGet("{id}")]
        public ActionResult<SearchHistory> Get(string id)
        {
            if (!(ObjectId.TryParse(id, out _))) return NotFound();
            var searchHistory = _searchHistoryService.Get(id);
            if (searchHistory is null) return NotFound();
            return searchHistory;
        }

        [HttpGet("user/{username}")]
        public ActionResult<SearchHistory> GetByUsername(string username)
        {
            if (String.IsNullOrEmpty(username)) return BadRequest();
            username = username.ToLower();
            var searchHistory = _searchHistoryService.GetByUsername(username);
            if (searchHistory is null) return NotFound();
            return searchHistory;
        }

        [HttpPost]
        public ActionResult<SearchHistory> Create(SearchHistory searchHistory)
        {
            if (String.IsNullOrEmpty(searchHistory.Username)) return BadRequest();
            searchHistory.Username = searchHistory.Username.ToLower();
            var search_history = _searchHistoryService.GetByUsername(searchHistory.Username);
            if (search_history is not null)return BadRequest();
            _searchHistoryService.Create(searchHistory);
            return Ok(searchHistory);
        }

        [HttpPut("{id}")]
        public ActionResult<SearchHistory> Update(string id, SearchHistory updatedHistory)
        {
            if (!(ObjectId.TryParse(id, out _))) return NotFound();
            var searchHistory = _searchHistoryService.Get(id);
            if (searchHistory is null) return NotFound();
            updatedHistory.Username = updatedHistory.Username.ToLower();
            var search_history = _searchHistoryService.GetByUsername(updatedHistory.Username);
            if (search_history is not null) return BadRequest();
            searchHistory.Username = updatedHistory.Username;
            _searchHistoryService.Update(id, searchHistory);
            return Ok(searchHistory);
        }

        [HttpDelete("{id}")]
        public ActionResult<SearchHistory> Delete(string id)
        {
            if (!(ObjectId.TryParse(id, out _))) return NotFound();
            var search_history = _searchHistoryService.Get(id);
            if (search_history is null) return NotFound();
            _searchHistoryService.Delete(id);
            return NoContent();
        }

        [HttpPost("additem/{idHistory}")]
        public ActionResult<SearchHistory> AddItem(string idHistory, SearchItem searchItem)
        {
            if (!(ObjectId.TryParse(idHistory, out _))) return NotFound();
            if (searchItem.ProductId < 0) return BadRequest();
            var search_history = _searchHistoryService.Get(idHistory);
            if (search_history is null) return NotFound();
            _searchItemService.Create(searchItem);
            _searchHistoryService.AddItem(idHistory, searchItem);
            return Ok(_searchHistoryService.Get(idHistory));
        }

        [HttpDelete("removeitem/{idHistory}")]
        public ActionResult<SearchHistory> RemoveItem(string idHistory, string idItem)
        {
            if (!(ObjectId.TryParse(idHistory, out _)) || !(ObjectId.TryParse(idItem, out _))) return NotFound();
            var search_history = _searchHistoryService.Get(idHistory);
            if (search_history is null) return NotFound();
            _searchHistoryService.RemoveItem(idHistory, idItem);
            return Ok(_searchHistoryService.Get(idHistory));
        }

        [HttpDelete("removeallitems/{idHistory}")]
        public ActionResult<SearchHistory> RemoveAllItems(string idHistory)
        {
            if (!(ObjectId.TryParse(idHistory, out _))) return NotFound();
            var search_history = _searchHistoryService.Get(idHistory);
            if (search_history is null) return NotFound();
            return Ok(_searchHistoryService.RemoveAllItems(idHistory));
        }
    }
}
