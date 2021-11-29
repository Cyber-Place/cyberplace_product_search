using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cyberplace_product_search_ms.Models
{
    public class ProductSearchSettings : IProductSearchSettings
    {
        public string Host { get; set; }
        public string Port { get; set; }
        public string DataBase { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public Dictionary<string, string> Collection { get; set; }
        
    }

    public interface IProductSearchSettings
    {
        string Host { get; set; }
        string Port { get; set; }
        string DataBase { get; set; }
        string Username { get; set; }
        string Password { get; set; }
        Dictionary<string, string> Collection { get; set; }
        
    }
}
