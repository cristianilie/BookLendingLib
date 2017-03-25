using BookLendingLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLendingLib.ViewModels
{
    public class BookManagingViewModel : ViewModelBase
    {
        public Book CurrentBook { get; set; }

        public BookManagingViewModel()
        {
            CurrentBook = new Book();
        }

        private void AddBook()
        {

        }

        private void UpdateBook()
        {

        }

        private void DeleteBook()
        {

        }

        private void ClearBookFields()
        {
            var b = new Book()
            {
                Title = "",
                Author = "",
                Isbn = "",
                Quantity = 0
            };
            CurrentBook = b;
            Console.WriteLine("clearing b00k");
        }
    }
}
