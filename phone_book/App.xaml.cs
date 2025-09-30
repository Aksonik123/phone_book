using PhoneBookApp.Database;
using PhoneBookApp.Services;

namespace PhoneBookApp
{
    public partial class App : Application
    {
        public static PhoneBookService PhoneBookService { get; private set; }

        public App()
        {
            InitializeComponent();

            // Initialize database
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "phonebook.db");
            var dbConnection = new SQLiteDatabaseConnection($"Data Source={dbPath};");
            PhoneBookService = new PhoneBookService(dbConnection);

            MainPage = new NavigationPage(new MainPage(PhoneBookService))
            {
                BarBackgroundColor = Color.FromArgb("#0078D7"),
                BarTextColor = Colors.White
            };
        }
    }
}