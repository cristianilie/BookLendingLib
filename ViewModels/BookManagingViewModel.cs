using BookLendingLib.Models;
using BookLendingLib.ViewModels.Comands;
using System;
using System.Linq;

namespace BookLendingLib.ViewModels
{
    public class BookManagingViewModel : ViewModelBase
    {
        #region Private Properties

        private static Book currentBook;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="hvm">Reference from so that i can re-laod the book list, after i make changes in the edit section</param>
        public BookManagingViewModel(HomeViewModel hvm)
        {
            if(CurrentBook  == null)
                CurrentBook =new Book();

            CurrentBook = hvm.SelectedBook;
            Hvm = hvm;
            AddB = new DefCommand(AddBook);
            EditB = new DefCommand(UpdateBook);
            DeleteB = new DefCommand(DeleteBook);
            ClearB = new DefCommand(ClearBookFields);
        }

        #endregion

        #region Public Properties

        //The current book we are handling
        public Book CurrentBook
        {
            get { return currentBook; }
            set
            {
                if (currentBook != value)
                {
                    currentBook = value;
                    RaisePropertyChanged();
                }
            }
        }

        //HomeViewModel reference
        public HomeViewModel Hvm { get; set; }

        //Commands to handle books
        public DefCommand AddB { get; private set; }
        public DefCommand EditB { get; private set; }
        public DefCommand DeleteB { get; private set; }
        public DefCommand ClearB { get; private set; }

        #endregion

        #region Private Methods
        
        //Add a new book to the database
        private void AddBook()
        {
            BookDBDataContext db = new BookDBDataContext();
            Book bObj = new Book();
            bObj.Title = CurrentBook.Title;
            bObj.Author = CurrentBook.Author;
            bObj.Isbn = CurrentBook.Isbn;
            bObj.Quantity = CurrentBook.Quantity;
            bObj.RezervedQty = 0;

            try
            {
                db.Books.InsertOnSubmit(bObj);
                db.Books.Context.SubmitChanges();
                Hvm.LoadBooks();
                ClearBookFields();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Console.WriteLine("addingbbbb");
        }

        //Update book info/save changes
        private void UpdateBook()
        {
            BookDBDataContext db = new BookDBDataContext();
            Book qry = (from book in db.Books
                      where book.Id == CurrentBook.Id
                      select book).Single();
          
                qry.Title = CurrentBook.Title;
                qry.Author = CurrentBook.Author;
                qry.Isbn = CurrentBook.Isbn;
                qry.Quantity = CurrentBook.Quantity;
            
            try
            {
                db.SubmitChanges();
                Hvm.LoadBooks();
                ClearBookFields();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.WriteLine("editingbbbb");
        }

        //Delete a book from the database
        private void DeleteBook()
        {
            BookDBDataContext db = new BookDBDataContext();
            var brbTbl = (from b in db.RentedBooks
                         where b.BookId == CurrentBook.Id
                         select b);
            var bTbl = (from b in db.Books
                        where b.Id == CurrentBook.Id
                        select b).SingleOrDefault();
            try
            {
                foreach (var b in brbTbl)
                {
                    db.RentedBooks.DeleteOnSubmit(b);
                }

                db.Books.DeleteOnSubmit(bTbl);
                db.SubmitChanges();
                Hvm.LoadBooks();
                ClearBookFields();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Console.WriteLine("deleting");

        }

        //Clear fields from book managing view
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

        #endregion
    }
}
