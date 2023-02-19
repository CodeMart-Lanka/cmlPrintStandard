using PrintTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintTest.Data
{
    internal class InvoiceGenerator
    {
        public InvoiceGenerator()
        {
            Random = new Random();
        }
        public List<Invoice> GetInvoices(int count)
        {
            var list = new List<Invoice>(count);
            for (int i = 0; i < count; i++)
            {
                list.Add(GetInvoice(i + 1));
            }
            return list;
        }

        public Invoice GetInvoice(int? id = null)
        {
            var invoice = new Invoice()
            {
                DateTime = DateTime.Now,
                Description = DESCRIPTION,
                Id = id ?? Random.Next(1250250),
                Total = Random.Next(125125458)
            };
            int count = id ?? 1;
            for (int i = 0; i < count; i++)
                invoice.Lines.Add("Line " + (i + 1));
            return invoice;
        }
        Random Random { get; set; }
        const string DESCRIPTION = "If you’ve ever read a blog post, you’ve consumed content from a thought leader that is an expert in their industry. Chances are if the blog post was written effectively, you came away with helpful knowledge and a positive opinion about the writer or brand that produced the content.";
    }
}
