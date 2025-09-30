using System.Data;
using Microsoft.Data.Sqlite;

namespace PhoneBookApp.Database
{
    public class SQLiteDatabaseConnection : IDatabaseConnection
    {
        private SqliteConnection connection;

        public SQLiteDatabaseConnection(string connectionString)
        {
            connection = new SqliteConnection(connectionString);
        }

        public void Open()
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
        }

        public void Close()
        {
            if (connection.State != ConnectionState.Closed)
            {
                connection.Close();
            }
        }

        public string AddToList()
        {
            return "INSERT INTO Persons (name, surname, phone_number, mail, date_of_birth) " +
                   "VALUES (@name, @surname, @phoneNumber, @email, @dob)";
        }

        public string SelectModifyListMember()
        {
            return "SELECT * FROM Persons WHERE person_id = @id";
        }

        public string ModifyListMember()
        {
            return "UPDATE Persons SET " +
                   "name = COALESCE(NULLIF(@newName, ''), name), " +
                   "surname = COALESCE(NULLIF(@newSurname, ''), surname), " +
                   "phone_number = COALESCE(NULLIF(@newPhoneNumber, ''), phone_number), " +
                   "mail = COALESCE(NULLIF(@newMail, ''), mail), " +
                   "date_of_birth = COALESCE(NULLIF(@newDateOfBirth, ''), date_of_birth) " +
                   "WHERE person_id = @id";
        }

        public string DeleteFromList()
        {
            return "DELETE FROM Persons WHERE person_id = @id";
        }

        public string DisplayListMember()
        {
            return "SELECT * FROM Persons WHERE name LIKE @search OR surname LIKE @search ORDER BY name, surname";
        }

        public string DisplayAllListMember()
        {
            return "SELECT COUNT(*) FROM Persons";
        }

        public string SearchUser()
        {
            return "SELECT * FROM Persons WHERE name LIKE @search OR surname LIKE @search ORDER BY name, surname";
        }

        public string GetAllContacts()
        {
            return "SELECT * FROM Persons ORDER BY name, surname";
        }

        public IDbCommand CreateCommand()
        {
            return connection.CreateCommand();
        }

        public void ExecuteNonQuery(string query)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            using (var command = CreateCommand())
            {
                command.CommandText = query;
                command.ExecuteNonQuery();
            }
        }

        public IDataReader ExecuteReader(string query)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            var command = CreateCommand();
            command.CommandText = query;
            return command.ExecuteReader();
        }
    }
}
