using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GeforceNowAccountChanger.Properties;

namespace GeforceNowAccountChanger
{
    class AccountManager
    {
        private static AccountManager acmanager;
        public static AccountManager getInstance()
        {
            if (acmanager == null)
            {
                acmanager = new AccountManager();
            }

            return acmanager;
        }

        private Settings settings = Settings.Default;
        public string GeforceNowLocation
        {
            get { return settings.GeforceNOWFolder; }
        }

        public string GetCurrentAccount(string path=null)
        {
            if (path != null)
            {
                return File.ReadAllText(path);
            }
            return File.ReadAllText(GeforceNowLocation+ "\\sharedstorage.json");
        }

        public void ChangeAccount(Account account)
        {
            try
            {
                
                File.WriteAllText(GeforceNowLocation +"\\sharedstorage.json", account.AccountData);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(),"Error",MessageBoxButton.OK,MessageBoxImage.Error);
            }
            
        }


    }
}
