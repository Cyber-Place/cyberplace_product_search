using cyberplace_product_search_ms.Controllers.Models;
using cyberplace_product_search_ms.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cyberplace_product_search_ms.Services
{
    public class SearchHistoryService
    {
        private IMongoCollection<SearchHistory> _searchHistory;

        public SearchHistoryService(IProductSearchSettings settings)
        {
            string server = "mongodb://" + settings.Username + ":" + settings.Password + "@" + settings.Host + ":" + settings.Port;
            var cliente = new MongoClient(server);
            var database = cliente.GetDatabase(settings.DataBase);
            _searchHistory = database.GetCollection<SearchHistory>(settings.Collection[nameof(SearchHistory)]);
        }

        public List<SearchHistory> GetAll()
        {
            return _searchHistory.Find(data => true).ToList();
        }
        public List<SearchHistory> Get(string id)
        {
            return _searchHistory.Find(data => data.Id == id).ToList();
        }
        public SearchHistory Create(SearchHistory SearchHistory)
        {
            SearchHistory.Items = new List<SearchItem>();
            _searchHistory.InsertOne(SearchHistory);
            return SearchHistory;
        }
        public SearchHistory Update(string id, SearchHistory SearchHistory)
        {
            SearchHistory oldHistory = _searchHistory.Find(data => data.Id == id).ToList().First();
            SearchHistory.Items = oldHistory.Items;
            _searchHistory.ReplaceOne(data => data.Id == id, SearchHistory);
            return SearchHistory;
        }
        public void Delete(string id)
        {
            _searchHistory.DeleteOne(data => data.Id == id);
        }
        public void AddItem(string id, SearchItem searchItem)
        {
            var filter = Builders<SearchHistory>.Filter.Eq(data => data.Id, id);
            var update = Builders<SearchHistory>.Update.Push(data => data.Items, searchItem);
            var result = _searchHistory.FindOneAndUpdateAsync(filter, update);
        }

        public void RemoveItem(string idHistory, string idItem) {
            var filterHistory = Builders<SearchHistory>.Filter.Eq(data => data.Id, idHistory);
            var update = Builders<SearchHistory>.Update.PullFilter(data => data.Items, Builders<SearchItem>.Filter.Where(nm => nm.Id == idItem));
            var result = _searchHistory.FindOneAndUpdateAsync(filterHistory, update);
        }

        public SearchHistory RemoveAllItems(string idHistory)
        {
            SearchHistory searchHistory = _searchHistory.Find(data => data.Id == idHistory).ToList().First();
            searchHistory.Items = new List<SearchItem>();
            _searchHistory.ReplaceOne(data => data.Id == idHistory, searchHistory);
            return searchHistory;
        }
    }
}
