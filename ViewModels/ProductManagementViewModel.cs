using BookLendingLib.Models;
using BookLendingLib.ViewModels.Comands;
using CsvHelper;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;

namespace BookLendingLib.ViewModels
{
    public class ProductManagementViewModel : ViewModelBase
    {
        #region Private Properties

        private string filePathName;
        private ObservableCollection<Book> exclBookList;

        #endregion


        #region Constructor

        /// <summary>
        /// Product Management Constructor
        /// </summary>
        /// <param name="hvm"></param>
        public ProductManagementViewModel(HomeViewModel hvm)
        {
            Hvm = hvm;
            FilePathName = String.Empty;
            ExclBookList = new ObservableCollection<Book>();

            BrowseFileCommand = new DefCommand(GetFilePath);
            ClearTblCommand = new DefCommand(ClearTable);
            ImportDataCommand = new DefCommand(ImportBookList);
            ExportBookDbCommand = new DefCommand(GetAdressToSaveFile);

        }
        #endregion


        #region Public Properties

        //Commands to browse/import/export files & vlear table
        public DefCommand BrowseFileCommand { get; private set; }
        public DefCommand ImportDataCommand { get; private set; }
        public DefCommand ExportBookDbCommand { get; private set; }
        public DefCommand ClearTblCommand { get; private set; }

        //Holds the path to the file we want to import
        public string FilePathName
        {
            get
            {
                return filePathName;
            }

            set
            {
                filePathName = value;
                RaisePropertyChanged();
            }
        }

        //HomeViewModel reference
        public HomeViewModel Hvm { get; set; }

        //List of  Book objects, that holds the objects from the imported file 
        public ObservableCollection<Book> ExclBookList
        {
            get { return exclBookList; }
            set
            {
                if (exclBookList != value)
                {
                    exclBookList = value;
                    RaisePropertyChanged();
                }
            }
        }

        #endregion


        #region Private Methods

        //Import excel data, and loads it into a collection bound to the datagrid so we can preview it
        private void GetExcelData(string path)
        {
            ClearTable();
            //string sheetName = "Sheet1";

            using (StreamReader textReader = new StreamReader(path))
            {
                var csv = new CsvReader(textReader);
                csv.Configuration.HasHeaderRecord = true;
                csv.Configuration.IsHeaderCaseSensitive = false;
                csv.Configuration.RegisterClassMap<BookCustomMap>();
                var records = csv.GetRecords<Book>();
            
            
            //var excelFile = new ExcelQueryFactory(path);
            //var xx = from a in excelFile.Worksheet(sheetName) select a;
                try
                {
                    foreach (var b in records)
                    {
                        ExclBookList.Add(b);
                    }
                }
                catch (Exception e)
                {
                    if (e.ToString().Contains("column name does not exist"))
                        MessageBox.Show("The Column names do not match the default import format(Id; Title; Author; Isbn; Quantity)");
                    else
                        MessageBox.Show(e.ToString());
                }
            }
        }

        //Gets the adress of the excel file we want to import
        private void GetFilePath()
        {
            var dialog = new OpenFileDialog();
            dialog.DefaultExt = ".csv";
            dialog.Filter = "CSV Files (*.csv)|*.csv";
            if (dialog.ShowDialog() == true)
            {
                FilePathName = dialog.FileName;
            }

            //{
            //    var dialog = new OpenFileDialog();
            //    dialog.DefaultExt = ".xls|.xlsx";
            //    dialog.Filter = "Excel documents (*.xls, *.xlsx)|*.xls;*.xlsx";

            //    if (dialog.ShowDialog() == true)
            //    {
            //        FilePathName = dialog.FileName;
            //    }

            GetExcelData(FilePathName);
        }

        //Add to the database the imported data
        private void ImportBookList()
        {
            BookDBDataContext booksDbDc = new BookDBDataContext();
            var tempBookLst = (from b in booksDbDc.Books select b).ToList();
            for (int i = 0; i < ExclBookList.Count; i++)
            {
                bool bookExists = tempBookLst.Any(b => b.Title == ExclBookList[i].Title && b.Author == ExclBookList[i].Author && b.Isbn == ExclBookList[i].Isbn);
                if (bookExists)
                {
                    try
                    {
                        Book book = (from b in booksDbDc.Books
                                     where b.Title == ExclBookList[i].Title && b.Author == ExclBookList[i].Author && b.Isbn == ExclBookList[i].Isbn
                                     select b).Single();
                        book.Quantity += ExclBookList[i].Quantity;
                        booksDbDc.SubmitChanges();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
                else
                {
                    Book bObj = new Book();
                    bObj.Title = ExclBookList[i].Title;
                    bObj.Author = ExclBookList[i].Author;
                    bObj.Isbn = ExclBookList[i].Isbn;
                    bObj.Quantity = ExclBookList[i].Quantity;
                    bObj.RezervedQty = 0;

                    try
                    {
                        booksDbDc.Books.InsertOnSubmit(bObj);
                        booksDbDc.Books.Context.SubmitChanges();

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }

            Hvm.LoadBooks();
            ClearTable();

        }

        //Clears the datagrid
        private void ClearTable()
        {
            ExclBookList = new ObservableCollection<Book>();
            FilePathName = string.Empty;
        }


        //Browse to an adress where we can save the "csv", and calls the method to export/save the data export.
        private void GetAdressToSaveFile()
        {
            var dialog = new SaveFileDialog();
            dialog.DefaultExt = ".csv";
            dialog.Filter = "CSV Files (*.csv)|*.csv";
            if (dialog.ShowDialog() == true)
            {
                FilePathName = dialog.FileName;
            }
            ExportBookDb(FilePathName);
        }

        //Export the Book Database
        private void ExportBookDb(string saveFilePath)
        {

            BookDBDataContext bDC = new BookDBDataContext();
            //Check if the booklist is empty, and if its not, clear it, and add the book  list to it
            if (ExclBookList != null)
            {
                ExclBookList.Clear();
            }
            ExclBookList = new ObservableCollection<Book>(bDC.Books);

            Book bPropNames = new Book();

            //Writing the booklist to a csv file
            using (StreamWriter textWriter = File.CreateText(saveFilePath + ".csv"))
            {
                var csv = new CsvWriter(textWriter);
                csv.WriteRecords(ExclBookList);
            }

        }

        #endregion 
    }
}
