using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cyberplace_product_search_ms.Models
{
    public class RabbitMQSettings : IRabbitMQSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
    }

    public interface IRabbitMQSettings
    {
        string Host { get; set; }
        int Port { get; set; }
    }
}
