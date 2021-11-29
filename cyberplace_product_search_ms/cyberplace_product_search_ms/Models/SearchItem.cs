using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cyberplace_product_search_ms.Models
{
    public class SearchItem
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("search_time")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime SearchTime { get; set; }

        [BsonElement("product")]
        public Product Product { get; set; }

    }
}
