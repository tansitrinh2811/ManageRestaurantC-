using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageStoreFood.DTO
{
    public class BillInFo
    {
        public BillInFo(int id, int billID, int foodID, int count) 
        {
            this.id = id;
            this.billID = billID;
            this.foodID = foodID;
            this.count = count;
        }
        public BillInFo(DataRow row) // lấy ra các ô trong billinfo
        {
            this.id = (int)row["id"];
            this.billID = (int)row["idBill"];
            this.foodID = (int)row["idFood"];
            this.count = (int)row["count"];
        }
        private int id;
        public int Id { get => id; set => id = value; }
      
        private int billID;
        public int BillID { get => billID; set => billID = value; }

        private int foodID;
        public int FoodID { get => foodID; set => foodID = value; }

        private int count;
        public int Count { get => count; set => count = value; }
    }
}
