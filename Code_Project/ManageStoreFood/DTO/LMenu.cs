using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageStoreFood.DTO
{
    public class LMenu
    {
        public LMenu(String foodName, int count, float price, float totalPrice=0) 
        {
            this.foodName = foodName;
            this.count = count;
            this.price = price;
            this.totalPrice = totalPrice;
        }
        public LMenu(DataRow row)
        {
            this.foodName = row["name"].ToString();
            this.count = (int)Convert.ToDouble(row["count"].ToString());
            this.price = (float)Convert.ToDouble(row["price"].ToString());
            this.totalPrice = (float)Convert.ToDouble(row["totalPrice"].ToString());
        }

        private float totalPrice;
        public float TotalPrice { get => totalPrice; set => totalPrice = value; }
        private float price;
        public float Price { get => price; set => price = value; }
        private int count;
        public int Count { get => count; set => count = value; }
        private string foodName;
        public string FoodName { get => foodName; set => foodName = value; }
        
        
       
    }
}
