using BookLendingLib.Models;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLendingLib.ViewModels
{
    public sealed class BookCustomMap:CsvClassMap<Book>
    {
        public BookCustomMap()
        {
            Map(m => m.Id).Index(0);
            Map(m => m.Title).Index(1);
            Map(m => m.Author).Index(2);
            Map(m => m.Isbn).Index(3);
            Map(m => m.Quantity).Index(4);

        }
    }
}
