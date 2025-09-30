using System.Data;

namespace PhoneBookApp.Database
{
    public interface IDatabaseConnection
    {
        void Open();
        void Close();
        string AddToList();
        string SelectModifyListMember();
        string ModifyListMember();
        string DeleteFromList();
        string DisplayAllListMember();
        string DisplayListMember();
        string SearchUser();
        string GetAllContacts();
        IDbCommand CreateCommand();
        void ExecuteNonQuery(string query);
        IDataReader ExecuteReader(string query);
    }
}
