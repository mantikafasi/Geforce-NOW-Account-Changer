using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GeforceNowAccountChanger.Annotations;

namespace GeforceNowAccountChanger
{
    class DatabaseManager
    {
        public DatabaseManager()
        {
            con = new SQLiteConnection("Data Source=accounts.db");
            con.Open();
            new SQLiteCommand(@"CREATE TABLE IF NOT EXISTS Accounts (ID INTEGER PRIMARY KEY AUTOINCREMENT,AccountName VARCHAR(45),AccountData text,unique(AccountName))", con).ExecuteNonQuery();

        }

        SQLiteConnection con;
        private static DatabaseManager instance;
        public static DatabaseManager getInstance()
        {
            if (instance == null){instance = new DatabaseManager();}
            return instance;
        }

        public void AddData(Account account)
        {
            SQLiteCommand command = new SQLiteCommand("INSERT INTO Accounts(AccountName,AccountData) VALUES(@accName,@accData)", con);
            command.Parameters.AddWithValue("accData", account.AccountData );
            command.Parameters.AddWithValue("accName", account.AccountName);
            try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show("err", e.ToString());
            }
        }


        public List<Account> GetData()
        {
            List<Account> list = new List<Account>();
            SQLiteDataReader reader = new SQLiteCommand("SELECT * FROM Accounts", con).ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Account(reader));
            }
            return list;
        }
        public void DeleteData(Account account)
        {

            new SQLiteCommand(string.Format("DELETE FROM Accounts where ID={0}", account.ID), con).ExecuteNonQuery();
        }

        public bool UpdateData(Account account)
        {
            SQLiteCommand command = new SQLiteCommand("UPDATE Accounts SET AccountName=@acName,AccountData=@accData WHERE ID=@id", con);
            command.Parameters.AddWithValue("acData", account.AccountData);
            command.Parameters.AddWithValue("acName", account.AccountName);
            command.Parameters.AddWithValue("id", account.ID);
            int rows = command.ExecuteNonQuery();
            return rows > 0;
        }

    }

    public class Account:INotifyPropertyChanged
    {
        public Account(SQLiteDataReader reader)
        {
            ID = reader.GetInt32(reader.GetOrdinal("ID"));
            AccountName = reader.GetString(reader.GetOrdinal("AccountName"));
            //Liste koyulup alınabilir yap 
            AccountData = reader.GetString(reader.GetOrdinal("AccountData"));
        }

        public Account()
        {

        }
        public int ID { get; set; }

        public string Path
        {
            get => _path;
            set { _path = value;OnPropertyChanged(); }
        }

        private string _accountName;
        public string AccountName
        {
            get { return _accountName;}
            set { _accountName = value;OnPropertyChanged(); }
        }



        String _accountData;
        private string _path;

        public String AccountData
        {
            get { return _accountData; }
            set
            {
                _accountData = value;
                OnPropertyChanged();
            }
        } 



        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
