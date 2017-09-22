namespace BookLendingLib.ViewModels
{

    //Filter to select/display books in the Books ListView
    public enum BookListFilter
    {
        AllBooks,       //AllBooks - the listview displays all the books in the database
        RentedBooks     //RentedBooks - the listview displays only the books rented by the selected reader
    }

    //Filter to select/display readers in the Readers ListView
    public enum ReaderListMode
    {
        AllReaders,     //AllReaders - the listview displays all the readers in the database
        RentedBooks     //RentedBooks - the listview displays only the readers that are currenlty renting by the selected book
    }
    
}
