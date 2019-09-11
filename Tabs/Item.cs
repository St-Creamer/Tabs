using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tabs
{
    public enum Categories
    {
        Food,
        Utilities,
        clothes,
        Misc
    }
    public class Item
    {
        public int Id { get; set; }

        public string ItemName { get; set; }

        public float ItemPrice { get; set; }

        public DateTime Date { get; set; }

        public Categories Category { get; set; }    
    }
}
