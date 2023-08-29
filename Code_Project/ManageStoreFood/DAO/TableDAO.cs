using ManageStoreFood.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageStoreFood.DAO
{
    internal class TableDAO
    {
        private static TableDAO instance;
        public static TableDAO Instance
        {
            get { 
                if (instance == null) 
                instance = new TableDAO(); 
                return instance; }
            private set { TableDAO.instance = value; }
        }
        private TableDAO() { }

        public static int TableWidth = 100;
        public static int TableHeight = 100;

        public void SwitchTable(int id1, int id2)
        {
            Dataprovider.Instance.ExecuteQuery("UserProvider_SwitchTable @idTable1, @idTable2", new object[] { id1, id2 });
        }
        
        public List<Table> LoadTableList()
        {
            List<Table> tablelist = new List<Table>();

            DataTable datatb = Dataprovider.Instance.ExecuteQuery("EXEC dbo.UserProvider_TableList");

            foreach (DataRow item in datatb.Rows) 
            {
                Table table = new Table(item);
                tablelist.Add(table);
            }
            return tablelist;
        }
        public List<Table> GetListTable()
        {
            List<Table> list = new List<Table>();

            string query = "SELECT * FROM TableFood";

            DataTable data = Dataprovider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                Table table = new Table(item);
                list.Add(table);
            }
            return list;
        }
    }
}
