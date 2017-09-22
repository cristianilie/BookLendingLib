using BookLendingLib.Models;
using BookLendingLib.ViewModels.Comands;
using System;
using System.Linq;

namespace BookLendingLib.ViewModels
{
    public class ReaderManagingViewModel : ViewModelBase
    {
        #region Private Properties

        private static Reader currentReader;

        #endregion

        #region Constructor
        /// <summary>
        /// Reader Managing cosntructor
        /// </summary>
        /// <param name="hvm">refrence used to switch the view from HomeView for editing the selected reader's attributes</param>
        public ReaderManagingViewModel(HomeViewModel hvm)
        {
            if (CurrentReader == null)
                CurrentReader = new Reader();

            CurrentReader = hvm.SelectedReader;
            Hvm = hvm;

            AddR = new DefCommand(AddReader);
            EditR = new DefCommand(UpdateReader);
            DeleteR = new DefCommand(DeleteReader);
            ClearR = new DefCommand(ClearReaderFields);
        }

        #endregion

        #region Public Properties

        //Current reader we are currently executing actions upon.
        public Reader CurrentReader
        {
            get { return currentReader; }
            set
            {
                if (currentReader != value)
                {
                    currentReader = value;
                    RaisePropertyChanged();
                }
            }
        }

        //HomeViewModel reference, to link with the selected reader we want to edit/delete.
        public HomeViewModel Hvm { get; set; }

        //Commands for Edit/Delete an existing user/Add a new user & Clear the fields
        public DefCommand AddR { get; private set; }
        public DefCommand EditR { get; private set; }
        public DefCommand DeleteR { get; private set; }
        public DefCommand ClearR { get; private set; }

        #endregion

        #region Private Methods
        
        //Add a new reader to the database(readers/renters table)
        private void AddReader()
        {
            BookDBDataContext db = new BookDBDataContext();
            Reader rObj = new Reader();
            rObj.FullName = CurrentReader.FullName;
            rObj.SerialNumber = CurrentReader.SerialNumber;
            rObj.Id = CurrentReader.Id;
            rObj.Adress = CurrentReader.Adress;
            rObj.AltContactMethods = CurrentReader.AltContactMethods;


            try
            {
                db.Readers.InsertOnSubmit(rObj);
                db.Readers.Context.SubmitChanges();
                Hvm.LoadReaders();
                ClearReaderFields();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            //Console.WriteLine("addingrrrr");
        }

        //Updates the readers info and saves it
        private void UpdateReader()
        {
            BookDBDataContext db = new BookDBDataContext();
            var qry = from reader in db.Readers
                      where reader.Id == CurrentReader.Id
                      select reader;
            foreach (Reader r in qry)
            {
                r.FullName = CurrentReader.FullName;
                r.SerialNumber = CurrentReader.SerialNumber;
                r.IdNumber = CurrentReader.IdNumber;
                r.Adress = CurrentReader.Adress;
                r.AltContactMethods = CurrentReader.AltContactMethods;
            }

            try
            {
                db.SubmitChanges();
                Hvm.LoadReaders();
                ClearReaderFields();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            //Console.WriteLine("editinrrrrr");
        }

        //Delete a reader from the database
        private void DeleteReader()
        {
            BookDBDataContext db = new BookDBDataContext();
            var rbTbl = (from r in db.RentedBooks
                     where r.ReaderId == CurrentReader.Id
                     select r);
            var rTbl = (from r in db.Readers
                       where r.Id == CurrentReader.Id
                       select r).SingleOrDefault();
            try
            {
                foreach (var rbr in rbTbl)
                {
                    db.RentedBooks.DeleteOnSubmit(rbr);
                }

                db.Readers.DeleteOnSubmit(rTbl);
                db.SubmitChanges();
                Hvm.LoadReaders();
                ClearReaderFields();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Console.WriteLine("deleting");
        }

        //Clear the fields from Reader Managing View
        private void ClearReaderFields()
        {
            var r = new Reader()
            {
                FullName = "",
                SerialNumber = "",
                IdNumber = "",
                Adress = "",
                AltContactMethods = ""
            };
            CurrentReader = r;

            //Console.WriteLine("clearing rdr");
        }

        #endregion
    }
}
