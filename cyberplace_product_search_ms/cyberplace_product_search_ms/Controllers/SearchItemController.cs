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
    public class SearchItemController : ControllerBase
    {
        public SearchItemService _searchItemService;

        public SearchItemController(SearchItemService searchItemService)
        {
            _searchItemService = searchItemService;
        }

        [HttpGet]
        public ActionResult<List<SearchItem>> Get()
        {
            return _searchItemService.GetAll();
        }

        [HttpGet("{id}")]
        public ActionResult<SearchItem> Get(string id)
        {
            if (!(ObjectId.TryParse(id, out _)))return NotFound();
            return _searchItemService.Get(id);
        }

        [HttpPost]
        public ActionResult<SearchItem> Create(SearchItem searchItem)
        {   
            if(searchItem.ProductId < 0) return BadRequest();
            _searchItemService.Create(searchItem);
            return Ok(searchItem);
        }

        [HttpPut("{id}")]
        public ActionResult<SearchItem> Update(string id, SearchItem updatedItem)
        {
            if (!(ObjectId.TryParse(id, out _))) return NotFound();
            var searchItem = _searchItemService.Get(id);
            if (searchItem is null) return NotFound();
            updatedItem.Id = searchItem.Id;
            updatedItem.SearchTime = searchItem.SearchTime;
            _searchItemService.Update(id, updatedItem);
            return Ok(updatedItem);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            if (!(ObjectId.TryParse(id, out _))) return NotFound();
            var search_Item = _searchItemService.Get(id);
            if (search_Item is null) return NotFound();
            _searchItemService.Delete(id);
            return NoContent();
        }
    }
}
