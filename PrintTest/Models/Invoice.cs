using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace PrintTest.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        public DateTime DateTime {get;set;}
        public decimal Total { get; set; }
        public string? Description { get; set; }
        public List<string> Lines { get; set; } = new List<string>();
    }
}
