using System.Data;
using PhoneBookApp.Database;
using PhoneBookApp.Models;

namespace PhoneBookApp.Services
{
    public class PhoneBookService
    {
        private readonly IDatabaseConnection _dbConnection;
        private IDbCommand commandHolder;

        public PhoneBookService(IDatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection;
            _dbConnection.Open();
            commandHolder = _dbConnection.CreateCommand();
            CreateTable();
        }

        public void CreateTable()
        {
            string createTableQuery = @"
                CREATE TABLE IF NOT EXISTS Persons (
                    person_id INTEGER PRIMARY KEY AUTOINCREMENT,
                    name TEXT,
                    surname TEXT,
                    phone_number TEXT,
                    mail TEXT,
                    date_of_birth TEXT
                );";

            _dbConnection.ExecuteNonQuery(createTableQuery);
        }

        public async Task<bool> AddContact(Contact contact)
        {
            return await Task.Run(() =>
            {
                try
                {
                    commandHolder.CommandText = _dbConnection.AddToList();
                    commandHolder.Parameters.Clear();

                    AddParameter("@name", contact.FirstName);
                    AddParameter("@surname", contact.LastName);
                    AddParameter("@phoneNumber", contact.PhoneNumber);
                    AddParameter("@email", contact.Email);
                    AddParameter("@dob", contact.DateOfBirthString);

                    int rowsAffected = commandHolder.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error adding contact: {ex.Message}");
                    return false;
                }
            });
        }

        public async Task<List<Contact>> GetAllContacts()
        {
            return await Task.Run(() =>
            {
                var contact = _editingContact ?? new Contact();
                contact.FirstName = FirstNameEntry.Text.Trim();
                contact.LastName = LastNameEntry.Text.Trim();
                contact.PhoneNumber = PhoneNumberEntry.Text?.Trim() ?? "";
                contact.Email = EmailEntry.Text?.Trim() ?? "";
                contact.DateOfBirth = DateOfBirthPicker.Date;

                bool success;
                if (_editingContact == null)
                {
                    success = await _phoneBookService.AddContact(contact);
                }
                else
                {
                    success = await _phoneBookService.UpdateContact(contact);
                }

                if (success)
                {
                    await DisplayAlert("Sukces",
                        _editingContact == null ? "Kontakt został dodany" : "Kontakt został zaktualizowany",
                        "OK");
                    ContactSaved?.Invoke(this, EventArgs.Empty);
                    await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Błąd", "Wystąpił błąd podczas zapisywania kontaktu", "OK");
                }
            }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}