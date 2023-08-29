using ManageStoreFood.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageStoreFood.DAO
{
    public class BillInfoDAO
    {
        private static BillInfoDAO instance;
        public static BillInfoDAO Instance
        {
            get { if (instance == null)
                    instance = new BillInfoDAO();
                    return BillInfoDAO.instance;}
            private set { BillInfoDAO.instance = value; }
        }
        private BillInfoDAO() { }
        public void DeleteBillInfoByFoodID (int id)
        {
            Dataprovider.Instance.ExecuteQuery("DELETE BillInfo WHERE idFood = "+ id);
        }
        public List<BillInFo> GetListBillInfo(int id)//id của thằng bill
        {
            List<BillInFo> listBillInfo = new List<BillInFo>();

            DataTable data = Dataprovider.Instance.ExecuteQuery("SELECT * FROM BillInfo WHERE idBill = " + id);
            foreach (DataRow item in data.Rows)
            {
                BillInFo info = new BillInFo(item);
                listBillInfo.Add(info);
            }

            return listBillInfo;
        }
        public void InsertBillInfo(int idBill, int idFood, int count)
        {
            Dataprovider.Instance.ExecuteNonQuery("Userprovider_InsertBillInfo @idBill , @idFood , @count", new object[] { idBill, idFood, count });
        }
    }
}
