using BookLendingLib.Models;
using BookLendingLib.ViewModels.Comands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLendingLib.ViewModels
{
    public class ReaderManagingViewModel : ViewModelBase
    {
        public ReaderManagingViewModel()
        {
            AddR = new TblQryCommand(AddReader);
            EditR = new TblQryCommand(UpdateReader);
            DeleteR = new TblQryCommand(DeleteReader);
            ClearR = new TblQryCommand(ClearReaderFields);
        }

        public Reader CurrentReader { get; set; }

        public TblQryCommand AddR { get; private set; }
        public TblQryCommand EditR { get; private set; }
        public TblQryCommand DeleteR { get; private set; }
        public TblQryCommand ClearR { get; private set; }

        private void AddReader()
        {
            BookDBDataContext db = new BookDBDataContext();
            Reader rObj = new Reader();
            rObj.FullName = CurrentReader.FullName;
            rObj.SerialNumber = CurrentReader.SerialNumber;
            rObj.Id = CurrentReader.Id;
            rObj.Adress = CurrentReader.Adress;
            rObj.AltContactMethods = CurrentReader.AltContactMethods;

            db.Readers.InsertOnSubmit(rObj);
            db.Readers.Context.SubmitChanges();
            Console.WriteLine("adding");
        }

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
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.WriteLine("editing");
        }

        private void DeleteReader()
        {
            //BookDBDataContext db = new BookDBDataContext();
            //var q = (from r in db.Readers
            //         where r.Id == SelectedReader.Id
            //         select r).SingleOrDefault();
            //db.Readers.DeleteOnSubmit(q);
            //db.SubmitChanges();
            //Console.WriteLine("deleting");
        }

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
            //SelectedReader = r;

            Console.WriteLine("clearing rdr");
        }
    }
}
