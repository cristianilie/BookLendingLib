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
        #region Private Properties

        private object currentView;
        private HomeViewModel homeVM;
        private ReaderManagingViewModel readerManagingVM;
        private BookManagingViewModel bookManagingVM;
        private ProductManagementViewModel productManagingVM;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor for MainWindow's ViewModel
        /// </summary>
        public MainWindowViewModel()
        {
            homeVM = new HomeViewModel(this);
            readerManagingVM = new ReaderManagingViewModel(homeVM);
            bookManagingVM = new BookManagingViewModel(homeVM);
            productManagingVM = new ProductManagementViewModel(homeVM);
            CurrentView = homeVM;

            ChangeViewToHomeView = new DefCommand(DisplayHomeView);
            ChangeViewToReaderManagView = new DefCommand(DisplayReaderManagingView);
            ChangeViewToBookManagView = new DefCommand(DisplayBookManagingView);
            ChangeViewToProductManagView = new DefCommand(DisplayProductManagingView);
        }

        #endregion

        #region Public Properties

        //The view that is currently displayed in the content control(HomeView is default)
        public object CurrentView
        {
            get { return currentView; }
            set { currentView = value; RaisePropertyChanged(); }
        }

        //Commands to switch between the views
        public DefCommand ChangeViewToHomeView { get; private  set; }
        public DefCommand ChangeViewToReaderManagView { get; private set; }
        public DefCommand ChangeViewToBookManagView { get; private set; }
        public DefCommand ChangeViewToProductManagView { get; private set; }

        #endregion

        #region Public Methods

        //Display the Home(default) view
        public void DisplayHomeView()
        {
            CurrentView = homeVM;
        }

        //Display the ReaderManaging View
        public void DisplayReaderManagingView()
        {
            CurrentView = readerManagingVM;
        }

        //Display the BookManaging View
        public void DisplayBookManagingView()
        {
            CurrentView = bookManagingVM;
        }

        //Display the ProductManaging View
        public void DisplayProductManagingView()
        {
            CurrentView = productManagingVM;
        }

        #endregion
    }
}
