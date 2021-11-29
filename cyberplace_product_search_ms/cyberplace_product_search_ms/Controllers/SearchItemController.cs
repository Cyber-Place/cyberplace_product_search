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
        [HttpGet("{id}")]
        public ActionResult<List<SearchItem>> Get(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                List<SearchItem> searchItems = _searchItemService.GetAll();
                return searchItems;
            }
            else if (!(ObjectId.TryParse(id, out _)))
            {
                return NotFound();
            }
            return _searchItemService.Get(id);
        }

        [HttpPost]
        public ActionResult<SearchItem> Create(SearchItem searchItem)
        {
            if(!(searchItem.Product is not null)) return BadRequest();
            if(!(searchItem.Product.Id is not null)) return BadRequest();
            _searchItemService.Create(searchItem);
            return Ok(searchItem);
        }

        [HttpPut]
        public ActionResult<SearchItem> Update(SearchItem searchItem)
        {
            _searchItemService.Update(searchItem.Id, searchItem);
            return Ok(searchItem);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            _searchItemService.Delete(id);
            return Ok();
        }
    }
}
