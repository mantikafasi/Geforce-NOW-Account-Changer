using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using GeforceNowAccountChanger.Properties;
using Microsoft.Win32;

namespace GeforceNowAccountChanger
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Settings settings = Settings.Default;
        AccountManager acmanager = AccountManager.getInstance();
        DatabaseManager databaseManager = DatabaseManager.getInstance();
        public MainWindow()
        {


        if (string.IsNullOrEmpty(settings.GeforceNOWFolder) || !Directory.Exists(settings.GeforceNOWFolder)){ settings.GeforceNOWFolder= Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\NVIDIA Corporation\\GeForceNOW";
            if (!Directory.Exists(settings.GeforceNOWFolder))
            {
                MessageBox.Show("Couldnt Find GeforceNow Directory,Please Select GeforceNow Folder Manually");selectFolder(null,null);
            }
        }

        InitializeComponent();

            loadAccounts();
        }
        private void addAccount(object sender, RoutedEventArgs e)
        {   
            AddAccount addAccount = new AddAccount();
            if (addAccount.ShowDialog() == true)
            {
                loadAccounts();
            }

        }

        private void openAccount(object sender, MouseButtonEventArgs e)
        {   
            Process process = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    RedirectStandardOutput = true, FileName = "cmd.exe", WindowStyle = ProcessWindowStyle.Hidden,
                    RedirectStandardError = true,UseShellExecute = false,
                    Arguments = "/C taskkill /f /im GeForceNOWReliabilityMonitor.exe && taskkill /f /im GeForceNOW.exe"
                }
            };
            process.Start();
            process.WaitForExit();
            Account account = (Account)(sender as DockPanel).DataContext;
            acmanager.ChangeAccount(account);
            new Process()
            {
                StartInfo = new ProcessStartInfo(acmanager.GeforceNowLocation +
                                                 "\\CEF\\GeForceNOW.exe")
                { UseShellExecute = false, WindowStyle = ProcessWindowStyle.Hidden }
            }.Start();
        }
        private void selectFolder(object sender, RoutedEventArgs e)
        {
            
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter= "sharedstorage.json file (*.json)|*.json";
            dialog.InitialDirectory = settings.GeforceNOWFolder;
            if (dialog.ShowDialog() == true)
            {

                settings.GeforceNOWFolder = System.IO.Path.GetDirectoryName(dialog.FileName);
                settings.Save();
                
            }
        }

        private void deleteAccount(object sender, RoutedEventArgs e)
        {
            Account account = (Account)(sender as Button).DataContext;
            databaseManager.DeleteData(account);
            loadAccounts();
        }

        public void loadAccounts()
        {
            AccountList.ItemsSource = databaseManager.GetData();
        }
    }
}
