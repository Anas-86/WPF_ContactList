using DesktopContactsApp.Classes;
using SQLite;
using System.Windows;

namespace DesktopContactsApp
{
    /// <summary>
    /// Interaction logic for ContactDetailWindow.xaml
    /// </summary>
    public partial class ContactDetailWindow : Window
    {
        Contact _contact;
        public ContactDetailWindow(Contact contact)
        {
            InitializeComponent();
            _contact = contact;
            nameTextBox.Text = contact.Name;
            emailTextBox.Text = contact.Email;
            phoneTextBoy.Text = contact.Phone;
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            using SQLiteConnection connection=new SQLiteConnection(App.databasePath);
            connection.Delete(_contact);
            Close();
        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            _contact.Email=emailTextBox.Text;
            _contact.Name=nameTextBox.Text;
            _contact.Phone=phoneTextBoy.Text;
            using SQLiteConnection connection = new SQLiteConnection(App.databasePath);
            connection.Update(_contact);
            Close ();
        }
    }
}
