using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace GeforceNowAccountChanger
{
    /// <summary>
    /// Interaction logic for AddAccount.xaml
    /// </summary>
    public partial class AddAccount : Window
    {
        private Account account;
        AccountManager manager = AccountManager.getInstance();
        public AddAccount()
        {
            account = new Account();
            account.Path = manager.GeforceNowLocation + "\\sharedstorage.json";
            DataContext = account;
            
            InitializeComponent();
        }

        private void selectPath(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog  = new OpenFileDialog();
            if ((bool)openFileDialog.ShowDialog())
            {
                account.AccountData=File.ReadAllText(openFileDialog.FileName);
            }
        }


        DatabaseManager databaseManager=DatabaseManager.getInstance();
        private void saveAccount(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(account.AccountName))
            {
                MessageBox.Show("Account Name Can't be Empty");}
            account.AccountData = manager.GetCurrentAccount(account.Path);
            databaseManager.AddData(account);

            DialogResult = true;
        }
    }
}
