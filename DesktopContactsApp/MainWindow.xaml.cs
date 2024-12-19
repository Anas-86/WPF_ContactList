using DesktopContactsApp.Classes;
using SQLite;
using System.Windows;
using System.Windows.Controls;

namespace DesktopContactsApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Contact> contacts;

        public MainWindow()
        {
            InitializeComponent();
            contacts = new List<Contact>();
            ReadDatabase();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NewContactWindow newContactWindow = new NewContactWindow();
            newContactWindow.ShowDialog();  // This method is going to return, only when the newContactWindow has been closed.

            ReadDatabase();  // Refresh list after dialog
        }

        void ReadDatabase()
        {
            using SQLiteConnection conn = new(App.databasePath);
            conn.CreateTable<Contact>();

            // Populate contacts only if not already filled
            var dbContacts = conn.Table<Contact>().OrderBy(c => c.Name).ToList();
            if (dbContacts.Any())
            {
                contacts.Clear();
                contacts.AddRange(dbContacts);
                // Force UI refresh
                ContactsListView.ItemsSource = null; 
                ContactsListView.ItemsSource = contacts;
            }
            else
            {
                ContactsListView.ItemsSource = null;
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox? searchTextBox = sender as TextBox;

            // Filter contacts without resetting the main list
            var filteredList = contacts
                .Where(c => c.Name.ToLower().Contains(searchTextBox.Text.ToLower()))
                .ToList();

            ContactsListView.ItemsSource = filteredList;  // Display filtered results
        }

        private void ContactsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Contact selectedContact = (Contact)ContactsListView.SelectedItem;
            if (selectedContact != null)
            {
                ContactDetailWindow contactDetailWindow=new ContactDetailWindow(selectedContact);
                contactDetailWindow.ShowDialog();
                ReadDatabase();
            }
        }
    }
}