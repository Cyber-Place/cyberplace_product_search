using cyberplace_product_search_ms.Controllers.Models;
using cyberplace_product_search_ms.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cyberplace_product_search_ms.Services
{
    public class SearchItemService
    {

        private IMongoCollection<SearchItem> _searchitem;

        public SearchItemService(IProductSearchSettings settings)
        {
            string server = "mongodb://" + settings.Username + ":" + settings.Password + "@" + settings.Host + ":" + settings.Port;
            var cliente = new MongoClient(server);
            var database = cliente.GetDatabase(settings.DataBase);
            _searchitem = database.GetCollection<SearchItem>(settings.Collection[nameof(SearchItem)]);
        }

        public List<SearchItem> GetAll()
        {
            return _searchitem.Find(data => true).ToList();
        }

        public SearchItem Get(string id)
        {
            return _searchitem.Find(data => data.Id == id).ToList().FirstOrDefault();
        }

        public SearchItem Create(SearchItem searchItem)
        {
            searchItem.SearchTime = DateTime.Now;
            _searchitem.InsertOne(searchItem);
            return searchItem;
        }

        public SearchItem Update(string id, SearchItem searchItem)
        {
            _searchitem.ReplaceOne(data => data.Id == id, searchItem);
            return searchItem;
        }

        public void Delete(string id)
        {
            _searchitem.DeleteOne(data => data.Id == id);
        }
    }
}
