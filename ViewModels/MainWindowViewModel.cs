using BookLendingLib.Models;
using BookLendingLib.ViewModels.Comands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLendingLib.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private Reader selectedReader;
        private Book selectedBook;

        private object currentView;

        private ObservableCollection<Book> bookList;
        private ObservableCollection<Reader> readerList;
        
        public MainWindowViewModel()
        {
            SelectedReader = new Reader();


            SelectedBook = new Book();

            BookDBDataContext bdb = new BookDBDataContext();
            BookDBDataContext rdb = new BookDBDataContext();

            ReadersList = new ObservableCollection<Reader>(rdb.Readers);
            BookList = new ObservableCollection<Book>(bdb.Books);

           

        }

        public object CurrentView
        {
            get { return currentView; }
            set { currentView = value; RaisePropertyChanged(); }
        }

        public BookListFilter BookListViewMode { get; set; }

       


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

        #region Selected Items

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

        public Book SelectedBook
        {
            get { return selectedBook; }
            set
            {
                if (selectedBook != value)
                {
                    selectedBook = value;
                    RaisePropertyChanged();
                }
            }
        }

        #endregion


    }
}
