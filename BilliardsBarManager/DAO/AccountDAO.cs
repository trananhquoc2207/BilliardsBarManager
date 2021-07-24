using QuanLyQuanBida.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanBida.DAO
{
    public class AccountDAO
    {
        static private AccountDAO instance;

        public static AccountDAO Instance {
            get { if (instance == null) instance = new AccountDAO(); return instance; }
            private set => instance = value; }

        private AccountDAO() { }


         public bool login(string userName, string passWord)
        {
            //string query = "SELECT * FROM dbo.Account WHERE UseName = N'" + userName + " ' AND PassWord = N' " + passWord + "' ";
            string query = "SELECT * FROM dbo.Account WHERE UseName =N'" + userName +"' AND PassWord = N'"+passWord+"'";
            DataTable result = DataProvider.Instance.ExecuteOuery(query);


            return result.Rows.Count > 0;
        }
        public DataTable GetListAccount()
        {
            return DataProvider.Instance.ExecuteOuery("SELECT UseName, DisplayName, Type FROM dbo.Account");
        }

        public Account GetAccountByUserName(string userName)
        {
            DataTable data = DataProvider.Instance.ExecuteOuery("Select * from account where useName = '" + userName + "'");

            foreach (DataRow item in data.Rows)
            {
                return new Account(item);
            }

            return null;
        }
        public bool InsertAccount(string name, string displayName, int type)
        {
            string query = string.Format("INSERT dbo.Account ( UseName, DisplayName,PassWord , Type )VALUES  ( N'{0}', N'{1}',N'1', {2})", name, displayName, type);
            int result = DataProvider.Instance.ExecuteNonOuery(query);

            return result > 0;
        }
        public bool UpdateAccount(string name, string displayName, int type)
        {
            string query = string.Format("UPDATE dbo.Account SET DisplayName = N'{1}', Type = {2} WHERE UseName = N'{0}'", name, displayName, type);
            int result = DataProvider.Instance.ExecuteNonOuery(query);

            return result > 0;
        }

        public bool DeleteAccount(string name)
        {
            string query = string.Format("Delete Account where UseName = N'{0}'", name);
            int result = DataProvider.Instance.ExecuteNonOuery(query);

            return result > 0;
        }

        public bool ResetPassword(string name)
        {
            string query = string.Format("update account set password = N'0' where UseName = N'{0}'", name);
            int result = DataProvider.Instance.ExecuteNonOuery(query);

            return result > 0;
        }
    }
}
