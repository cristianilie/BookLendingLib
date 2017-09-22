using BookLendingLib.Models;
using BookLendingLib.ViewModels.Comands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace BookLendingLib.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        #region Private Properties

        private Reader selectedReader;
        private Book selectedBook;
        private BookListFilter selectedFilter;
        private string bookSrcBoxText;
        private string readerSrcBoxText;

        private bool canRentBook;
        private bool canUnRentBook;

        private ObservableCollection<Book> bookList;
        private ObservableCollection<Reader> readerList;

        #endregion

        #region Constructor

        /// <summary>
        /// HomeView(default view)'s viewmodel constructor
        /// </summary>
        /// <param name="_mwvm">Reference to the main viewmodel</param>
        public HomeViewModel(MainWindowViewModel _mwvm)
        {
            Mwvm = _mwvm;
            SelectedReader = new Reader();
            SelectedBook = new Book();
            SelectedFilter = BookListFilter.AllBooks;
            CanRentBook = true;
            CanUnRentBook = true;
            BookDBDataContext rdb = new BookDBDataContext();
            TempSelectedBook = new Book();
            ReaderSrcBoxText = String.Empty;
            BookSrcBoxText = String.Empty;
            RefreshLists();

            EditReaderSwitch = new DefCommand(EditReaderInfo);
            EditBookSwitch = new DefCommand(EditBookInfo);
            AddToRentedBookLstCmd = new DefCommand(AddToRentedBookList);
            RemoveFromRentedBookLstCmd = new DefCommand(DeleteFromRentedBookList);
            DisplayRenters = new DefCommand(DisplayCurrentRenters);
            DisplayRentedBooks = new DefCommand(DisplayBooks);
            RefreshLst = new DefCommand(RefreshLists);
            SearchBook = new DefCommand(DisplaySearchResultsBooks);
            SearchReader = new DefCommand(DisplaySearchResultsReaders);
        }

        #endregion

        #region Public Properties

        #region Commands

        public DefCommand EditReaderSwitch { get; private set; }
        public DefCommand EditBookSwitch { get; private set; }
        public DefCommand AddToRentedBookLstCmd { get; private set; }
        public DefCommand RemoveFromRentedBookLstCmd { get; private set; }
        public DefCommand DisplayRenters { get; private set; }
        public DefCommand DisplayRentedBooks { get; private set; }
        public DefCommand RefreshLst { get; private set; }
        public DefCommand SearchBook { get; private set; }
        public DefCommand SearchReader { get; private set; }

        #endregion

        //MainWindowViewModel instance, to connect with HomeViewModel & switch Views
        public MainWindowViewModel Mwvm { get; set; }

        //Check if  a book is available to rent(if its not, the rent button is disabled)
        public bool CanRentBook
        {
            get { return canRentBook; }
            set
            {
                canRentBook = value;
                RaisePropertyChanged();
            }
        }

        //Check if  a book is available to remove from a reader's rented book list(if its not, the remove button is disabled)
        public bool CanUnRentBook
        {
            get { return canUnRentBook; }
            set
            {
                canUnRentBook = value;
                RaisePropertyChanged();
            }
        }

        //I am using a temporary variable to hold the current selected book, because after/while i execute a command the standard selected book resets to null
        public Book TempSelectedBook { get; set; }

        //Book List
        public ObservableCollection<Book> BookList
        {
            get { return bookList; }
            set
            {
                if (bookList != value)
                {
                    bookList = value;
                    RaisePropertyChanged();
                }
            }
        }

        //Reader List
        public ObservableCollection<Reader> ReadersList
        {
            get { return readerList; }
            set
            {
                if (readerList != value)
                {
                    readerList = value;
                    RaisePropertyChanged();
                }
            }
        }

        //Current Selected Reader
        public Reader SelectedReader
        {
            get { return selectedReader; }
            set
            {
                if (selectedReader != value)
                {
                    selectedReader = value;
                    RaisePropertyChanged();
                }
            }
        }

        //Current Selected Book
        public Book SelectedBook
        {
            get { return selectedBook; }
            set
            {
                if (selectedBook != value)
                {
                    selectedBook = value;
                    RaisePropertyChanged();
                    CanRentBookCheck();
                    CanUnRentBookCheck();
                }
            }
        }

        //Filter to display the way we want to show the book list
        //AllBooks - All the books in database
        //RentedBooks - Books curently rented by the selected reader
        public BookListFilter SelectedFilter
        {
            get { return selectedFilter; }
            set
            {
                selectedFilter = value;
                RaisePropertyChanged();
                GetBookList();
            }
        }

        //Bound to the Reader list search box Text property
        public string ReaderSrcBoxText
        {
            get { return readerSrcBoxText; }
            set
            {
                readerSrcBoxText = value;
                RaisePropertyChanged();
            }
         }

        //Bound to the Book list search box Text property
        public string BookSrcBoxText
        {
            get { return bookSrcBoxText; }
            set
            {
                bookSrcBoxText = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Private Methods

        //Check if the selected book has a qty vailable so it can be added into rented books tbl & user rented book list
        private void CanRentBookCheck()
        {
            try
            {
                if (SelectedBook.Quantity < 1 || SelectedBook.Quantity == null)
                    CanRentBook = false;
                else
                    CanRentBook = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        //Check if the selected book has a qty vailable so it can be removed from rented books tbl & user rented book list
        private void CanUnRentBookCheck()
        {
            try
            {
                if (SelectedBook.RezervedQty < 1 || SelectedBook.RezervedQty == null)
                    CanUnRentBook = false;
                else
                    CanUnRentBook = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        //Load the Book list from the database so it can be displayed in the listview
        //Depending on the SelectedFilter's value it displays all the books in the database,
        //or just the books rented by the selected reader
        private void GetBookList()
        {
            BookDBDataContext bdb = new BookDBDataContext();
            BookList = new ObservableCollection<Book>(bdb.Books);
            var qry2 = (from b in bdb.Books
                        select b).OrderBy(b => b.Title).ToList();



            if (SelectedFilter == BookListFilter.RentedBooks && SelectedReader != null)
            {
                var qry1 = (from b in bdb.Books
                            join rb in bdb.RentedBooks on b.Id equals rb.BookId
                            where rb.ReaderId == SelectedReader.Id && b.Id == rb.BookId
                            select b).OrderBy(b => b.Title).ToList();
                BookList.Clear();
                foreach (var item in qry1)
                {
                    BookList.Add(item);
                }
            }
            else
            {
                BookList.Clear();
                foreach (var item in qry2)
                {
                    BookList.Add(item);
                }
            }
        }

        //Switches the view to the "Edit Book" section
        private void EditBookInfo()
        {

            Mwvm.DisplayBookManagingView();
            var tmpBook = new BookManagingViewModel(this);
            Console.WriteLine("EditBook");
        }

        //Switches the view to the "Edit Reader" section
        private void EditReaderInfo()
        {

            Mwvm.DisplayReaderManagingView();
            var tmpReader = new ReaderManagingViewModel(this);

            Console.WriteLine("EditReader");

        }

        //Display in the Reader's listview, the users that are currently renting the selected book
        private void DisplayCurrentRenters()
        {
            TempSelectedBook = SelectedBook;

            BookDBDataContext rbtbl = new BookDBDataContext();

            var qry1 = (from r in rbtbl.Readers
                        join b in rbtbl.RentedBooks on r.Id equals b.ReaderId
                        where b.BookId == TempSelectedBook.Id
                        select r).Distinct().OrderBy(r => r.FullName).ToList();

            ReadersList = new ObservableCollection<Reader>();

            if (TempSelectedBook != null)
            {
                ReadersList.Clear();
                foreach (var item in qry1)
                {
                    ReadersList.Add(item);
                }
            }
        }

        //Changes the value of the SelectedFilter so its displaying the rented books and calls GetBooklist method
        //so it can display the books rented by the selected reader 
        private void DisplayBooks()
        {
            SelectedFilter = BookListFilter.RentedBooks;
            GetBookList();
        }

        //Refreshes the Reader's list 
        private void RefreshLists()
        {
            LoadReaders();
            LoadBooks();
            ReaderSrcBoxText = string.Empty;
            BookSrcBoxText = string.Empty;
        }

        //Insert into RentedBooks table
        private void AddToRentedBookList()
        {
            TempSelectedBook = SelectedBook;
            BookDBDataContext rentedBLdb = new BookDBDataContext();
            BookDBDataContext book_db = new BookDBDataContext();
            RentedBook rbObj = new RentedBook();

            Book b = (from book in book_db.Books
                      where book.Id == SelectedBook.Id
                      select book).Single();
            try
            {
                b.Quantity--;
                b.RezervedQty++;

                rbObj.BookId = SelectedBook.Id;
                rbObj.ReaderId = SelectedReader.Id;
                rbObj.RentStartDate = DateTime.Today;
                rbObj.RentEndDate = null;
                if (rbObj.RezervedQty == null)
                    rbObj.RezervedQty = 1;
                else
                    rbObj.RezervedQty++; ;
                CanRentBook = true;


                book_db.SubmitChanges();
                rentedBLdb.RentedBooks.InsertOnSubmit(rbObj);
                rentedBLdb.RentedBooks.Context.SubmitChanges();
                SelectedBook = (from book in book_db.Books
                                where book.Id == TempSelectedBook.Id
                                select book).Single();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.WriteLine("addin2rentedbook >>> >>> >>>");
        }

        //Delete from RentedBooks table
        private void DeleteFromRentedBookList()
        {
            TempSelectedBook = SelectedBook;
            BookDBDataContext rentedBLdb = new BookDBDataContext();
            RentedBook rbObj = new RentedBook();
            BookDBDataContext book_db = new BookDBDataContext();

            try
            {
                Book b = (from book in book_db.Books
                          where book.Id == SelectedBook.Id
                          select book).Single();

                List<RentedBook> rbQry = (from rb in rentedBLdb.RentedBooks
                                          where rb.BookId == SelectedBook.Id &&
                                          rb.ReaderId == SelectedReader.Id
                                          select rb).ToList();

                b.Quantity++;
                b.RezervedQty--;

                foreach (var item in rbQry)
                {
                    rentedBLdb.RentedBooks.DeleteOnSubmit(item);
                }

                book_db.SubmitChanges();
                rentedBLdb.SubmitChanges();
                CanUnRentBook = true;
                SelectedBook = (from book in book_db.Books
                                where book.Id == TempSelectedBook.Id
                                select book).Single();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.WriteLine("delfromrentedbook <<< <<< <<<");
        }

        #endregion

        #region Public Methods

        //Load reader list for refreshing
        public void LoadReaders()
        {
            using (BookDBDataContext rdrDC = new BookDBDataContext())
            {
                if (ReadersList != null)
                {
                    ReadersList.Clear();
                }

                var q = (from r in rdrDC.Readers
                         select r).OrderBy(r => r.FullName).ToList();
                ReadersList = new ObservableCollection<Reader>(q);
            }
        }

        //Load book list for refreshing
        public void LoadBooks()
        {
            using (BookDBDataContext bDC = new BookDBDataContext())
            {

                if (BookList != null)
                {
                    BookList.Clear();
                    SelectedFilter = BookListFilter.AllBooks;
                }
                var q = (from b in bDC.Books
                         select b).OrderBy(b => b.Title).ToList();
                BookList = new ObservableCollection<Book>(q);
            }
        }

        //Display the Readers that contain the searched string in their FullName property
        public void DisplaySearchResultsReaders()
        {
            BookDBDataContext rdrDC = new BookDBDataContext();
            ReadersList.Clear();
            if (ReaderSrcBoxText.Count() > 0)
            {
                var q = (from r in rdrDC.Readers
                         where r.FullName.Contains(ReaderSrcBoxText)
                         select r).OrderBy(r => r.FullName).ToList();
                foreach (var rdr in q)
                {
                    ReadersList.Add(rdr);
                }
            }
        }

        //Display the Books that contain the searched string in their Title & Author properties
        public void DisplaySearchResultsBooks()
        {
            BookDBDataContext bdb = new BookDBDataContext();

            if (BookSrcBoxText.Count() > 0)
            {
                var qry3 = (from b in bdb.Books
                            where b.Title.Contains(BookSrcBoxText) || b.Author.Contains(BookSrcBoxText)
                            select b).OrderBy(b => b.Title).ToList();

                BookList.Clear();
                foreach (var item in qry3)
                {
                    BookList.Add(item);
                }
            }
        }

        #endregion
    }
}
