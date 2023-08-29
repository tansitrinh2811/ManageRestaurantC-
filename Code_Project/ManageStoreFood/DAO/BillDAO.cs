using ManageStoreFood.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageStoreFood.DAO
{
    public class BillDAO
    {
        private static BillDAO instance;
        public static BillDAO Instance
        {
            get { if(instance == null)
                  instance = new BillDAO();
                  return BillDAO.instance;}
            private set { BillDAO.instance = value; }
        }
        private BillDAO() { }
        //Thành công: bill ID
        //Thất bại:-1
        public int GetUnCheckBillIDByTableID(int id)
        {
            DataTable data = Dataprovider.Instance.ExecuteQuery("SELECT * FROM Bill WHERE idTable = " + id + "AND status = 0 ");

            if(data.Rows.Count > 0)
            {
                Bill bill = new Bill(data.Rows[0]);
                return bill.Id;
            }
            return -1;
        }
        public void CheckOut(int id, int discount, float totalPrice)
        {
            string query = "UPDATE dbo.Bill SET dateCheckOut = GETDATE(), status = 1," + "discount = " + discount +", totalPrice = "+ totalPrice+ " WHERE id = "+id;
            Dataprovider.Instance.ExecuteQuery(query);
        }
        public void InsertBill(int id)
        {
            Dataprovider.Instance.ExecuteNonQuery("EXEC Userprovider_InsertBill @idTable", new object[] {id});
        }
        public DataTable GetListBillByDate(DateTime checkIn, DateTime checkOut)
        {
            return Dataprovider.Instance.ExecuteQuery("EXEC UserProvider_GetListBillByDate @checkIn , @checkOut", new object[] { checkIn, checkOut });
        }
        public int GetMaxBill()
        {
            try 
            {
                return (int)Dataprovider.Instance.ExecuteScalar("SELECT MAX(id) FROM Bill");
            }
            catch
            {
                return 1;
            }
            
        }
    }
}
