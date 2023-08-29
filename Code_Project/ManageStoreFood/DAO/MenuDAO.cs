using ManageStoreFood.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageStoreFood.DAO
{
    public class MenuDAO
    {
        private static MenuDAO instance;
        public static MenuDAO Instance
        {
            get { if(instance == null)
                    instance = new MenuDAO();
                    return MenuDAO.instance;}
            private set { MenuDAO.instance = value; }
        }
        private MenuDAO() { }
        public List<LMenu> GetListMenuByTable(int id)
        {
            List<LMenu> listMenu = new List<LMenu>();

            string query = "SELECT f.name, bf.count, f.price, f.price*bf.count AS TotalPrice FROM dbo.BillInfo AS bf, dbo.Bill AS b, dbo.Food AS f    WHERE bf.idBill = b.id AND bf.idFood =f.id AND b.status=0 AND b.idTable = "+ id;
            DataTable data = Dataprovider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows) 
            {
                LMenu menu = new LMenu(item);
                listMenu.Add(menu);
            }

            return listMenu;
        }
    }
}
