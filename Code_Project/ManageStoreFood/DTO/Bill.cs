using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageStoreFood.DTO
{
    public class Bill
    {
        public Bill(int id, DateTime? dataCheckIn, DateTime? dataCheckOut, int status, int discount =0) 
        { 
            this.id = id;
            this.dateCheckIn = dateCheckIn;
            this.dateCheckOut = dateCheckOut;
            this.status = status;
            this.discount = discount;
        }
        public Bill(DataRow row) 
        {
            this.id = (int)row["id"] ;
            this.dateCheckIn = (DateTime?)row["dateCheckIn"];
            var dateCheckOutTemp = row["dateCheckOut"];
            if(dateCheckOutTemp.ToString() != "") //các trường hợp bị null thì trả ra rỗng
                this.dateCheckOut = (DateTime?)dateCheckOutTemp;

            this.status = (int)row["status"];
            if(row["discount"].ToString() != "")
            this.discount = (int)row["discount"];
        }
        private int discount;
        private int id;
        public int Id { get => id; set => id = value; }
       
        private DateTime? dateCheckIn;
        public DateTime? DataCheckIn { get => dateCheckIn; set => dateCheckIn = value; }
        private DateTime? dateCheckOut;
        public DateTime? DateCheckOut { get => dateCheckIn; set => dateCheckIn = value; }  
        private int status;
        public int Status { get => status; set => status = value; }
        public int Discount { get => discount; set => discount = value; }
    }

}
