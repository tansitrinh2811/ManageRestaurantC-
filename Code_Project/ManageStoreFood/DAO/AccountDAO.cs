using ManageStoreFood.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageStoreFood.DAO
{
    public class AccountDAO
    {
        private static AccountDAO instance;

        public static AccountDAO Instance
        {
            get { 
                if (instance == null)
                        instance = new AccountDAO();
                        return instance;
            }
            private set {instance = value;}
        }
        private AccountDAO() { }
        public bool Login(string userName, string passWord)
        {
            string query = "UserProvider_Login @userName , @passWord";

            DataTable result = Dataprovider.Instance.ExecuteQuery(query, new object[] {userName, passWord});
            //nếu trả ra dòng đúng của câu lệnh trên thì sẽ trả về đăng nhập 
            return result.Rows.Count > 0;
        }
        public bool UpdateAccount(string userName, string displayName, string pass, string newpass)
        {
            int result = Dataprovider.Instance.ExecuteNonQuery("EXEC UserProvider_UpdateAccount @userName , @displayName , @password , @newPassword", new object[] {userName, displayName, pass, newpass});
            return result > 0;
        }
        public DataTable GetListAccount()
        {
            return Dataprovider.Instance.ExecuteQuery("SELECT UserName, DisplayName, Type FROM Account");
        }
        public Account GetAccountByUserName(string userName)
        {
            DataTable data = Dataprovider.Instance.ExecuteQuery("SELECT * FROM Account WHERE userName = '" + userName + "'" );//Đánh dấu ' do không phải là kiểu chuổi
            foreach(DataRow item in data.Rows) 
            {
                return new Account(item);
            }
            return null;
        }
        public bool InsertAccount(string name, string displayName, int type)
        {
            string query = string.Format("INSERT dbo.Account ( UserName, DisplayName, Type, password )VALUES  ( N'{0}', N'{1}', {2}, N'{3}')", name, displayName, type, "123");
            int result = Dataprovider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool UpdateAccount2(string name, string displayName, int type)
        {
            string query = string.Format("UPDATE Account SET DisplayName = N'{1}', Type = {2} WHERE UserName = N'{0}'", name, displayName, type);
            int result = Dataprovider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool DeleteAccount(string name)
        {
            string query = string.Format("Delete Account where UserName = N'{0}'", name);
            int result = Dataprovider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool ResetPassword(string name)
        {
            string query = string.Format("update account set password = N'0' where UserName = N'{0}'", name);
            int result = Dataprovider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }
    }
}
