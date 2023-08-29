using ManageStoreFood.DAO;
using ManageStoreFood.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ManageStoreFood
{
    public partial class fManage : Form
    {
        private Account loginAccount;

        public Account LoginAccount { get => loginAccount; set { loginAccount = value; ChangeAccount(loginAccount.Type); } }
        
        public fManage(Account acc)
        {       
            InitializeComponent();
            this.LoginAccount = acc;//fix 2 ngày =))
            LoadTable();
            LoadCategory();
            LoadComboboxTable(cbxSwitchTable);
            
        }

        #region Method
        void ChangeAccount(int type)
        {
            adminToolStripMenuItem.Enabled = type == 1;
            thôngTinTàiKhoảnToolStripMenuItem.Text += " ( " + LoginAccount.DisplayName + " )";
        }
        void LoadCategory()
        {
            List<Category> listcategory = CategoryDAO.Instance.GetListCategory();
            cbxCategory.DataSource = listcategory;
            cbxCategory.DisplayMember = "name";
        }

        void LoadFoodListByCategory(int id)
        {
            List<Food> listFood = FoodDAO.Instance.GetFoodByCategoryID(id);
            cbxFood.DataSource = listFood;
            cbxFood.DisplayMember = "name";
        }

        void LoadTable()
        {
            flpTable.Controls.Clear();
            List<Table> tableList = TableDAO.Instance.LoadTableList();

            foreach (Table item in tableList)
            { 
                Button btn = new Button() { Width = TableDAO.TableWidth, Height = TableDAO.TableHeight };
                btn.Text = item.Name + Environment.NewLine+ item.Status;
                //tạo sự kiện click để hiện id table
                btn.Click += btn_Click;
                btn.Tag = item;
            
                switch(item.Status)
                {
                    case "Trống":
                        btn.BackColor = Color.Aqua;
                        break;
                    default:
                        btn.BackColor = Color.Red;
                        break;
                }

                flpTable.Controls.Add(btn);
            }
        }
        void ShowBill(int id)
        {
            lsvBill.Items.Clear();
            List<LMenu> listBillInfo = MenuDAO.Instance.GetListMenuByTable(id);
            float totalPrice = 0;
            foreach (LMenu item in listBillInfo)
            {
                ListViewItem lsvItem = new ListViewItem(item.FoodName.ToString());
                lsvItem.SubItems.Add(item.Count.ToString());
                lsvItem.SubItems.Add(item.Price.ToString());
                lsvItem.SubItems.Add(item.TotalPrice.ToString());
                totalPrice += item.TotalPrice;
                lsvBill.Items.Add(lsvItem);
            }
            //chọn giá tiền việt nam
            CultureInfo cultureInfo = new CultureInfo("vi-VN");
            txbTotalPrice.Text = totalPrice.ToString("c",cultureInfo);
        }
        void LoadComboboxTable(ComboBox cb)
        {
            cb.DataSource = TableDAO.Instance.LoadTableList();
            cb.DisplayMember = "Name";
        }
        #endregion


        #region Events
        void btn_Click(object sender, EventArgs e)
        {
            int tableID = ((sender as Button).Tag as Table).ID;
            lsvBill.Tag=(sender as Button).Tag;//click vào bàn nào thì listview sẽ tag bàn đó
            ShowBill(tableID);
        }
        #endregion 
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cbxCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = 0;

            ComboBox cb=sender as ComboBox;
            if (cb.SelectedItem == null)
                return;
            Category selected =cb.SelectedItem as Category;
            id = selected.ID;

            LoadFoodListByCategory(id);
        }

        private void button1_Click(object sender, EventArgs e)
        {
               
            int id1 = (lsvBill.Tag as Table).ID;

            int id2=(cbxSwitchTable.SelectedItem as Table).ID;
            if (MessageBox.Show(string.Format("Bạn có thật sự muốn chuyển bàn {0} qua bàn {1}", (lsvBill.Tag as Table).Name, (cbxSwitchTable.SelectedItem as Table).Name), "Thông báo", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
            {
                TableDAO.Instance.SwitchTable(id1, id2);
                LoadTable();
            }

        }

        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void thôngTinCáNhânToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAccountProfile f = new fAccountProfile(LoginAccount);
            f.UpdateAccount += f_UpdateAccount;
            f.ShowDialog();
        }
        void f_UpdateAccount(object sender, AccountEvent e)
        {
            thôngTinTàiKhoảnToolStripMenuItem.Text = "Thông tin tài khoản ( " + e.Acc.DisplayName + ")";
        }

        private void adminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAdmin f = new fAdmin();
            f.loginAccount = LoginAccount;
            f.InserFood += f_InsertFood;
            f.DeleteFood += f_DeleteFood;
            f.UpdateFood += f_UpdateFood;
            f.ShowDialog(); 
        }
        void f_InsertFood(object sender, EventArgs e)
        {
            LoadFoodListByCategory((cbxCategory.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as Table).ID);
        }
        void f_DeleteFood(object sender, EventArgs e)
        {
            LoadFoodListByCategory((cbxCategory.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as Table).ID);
            LoadTable();    
        }
        void f_UpdateFood(object sender, EventArgs e)
        {
            LoadFoodListByCategory((cbxCategory.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as Table).ID);
        }
        private void btnAddFood_Click(object sender, EventArgs e)
        {
            
             Table table = lsvBill.Tag as Table;
             if(table == null)
             {
                MessageBox.Show("Hãy chọn bàn");
                return;
             }    
             int idBill = BillDAO.Instance.GetUnCheckBillIDByTableID(table.ID);
             int foodID = (cbxFood.SelectedItem as Food).ID;
             int count = (int)nmFoodCount.Value;
             if (idBill == -1)// trường hợp chưa có bill của bàn
                {
                    BillDAO.Instance.InsertBill(table.ID);
                    BillInfoDAO.Instance.InsertBillInfo(BillDAO.Instance.GetMaxBill(), foodID, count);
                }
             else
                {
                    BillInfoDAO.Instance.InsertBillInfo(idBill, foodID, count);//bill đã tồn tại
                }
             ShowBill(table.ID);
             LoadTable();
           
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table;

            int idBill = BillDAO.Instance.GetUnCheckBillIDByTableID(table.ID);
            int discount = (int)nmDiscount.Value;

            double totalPrice = Convert.ToDouble(txbTotalPrice.Text.Split(',')[0]);
            double finalTotalPrice = (totalPrice - (totalPrice / 100) * discount)*1000;
            if (idBill != -1)
            {
                if (MessageBox.Show(String.Format("Bạn có chắc chắn thanh toán hóa đơn cho {0}\n Tổng tiền - (Tổng tiền/100) * Giảm giá = {1} - ({1} / 100) * {2} = {3}", table.Name, totalPrice, discount, finalTotalPrice), "Thông báo", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {
                    BillDAO.Instance.CheckOut(idBill, discount, (float)finalTotalPrice); 
                    ShowBill(table.ID);

                    LoadTable();
                }
            }   

        }

        private void fManage_Load(object sender, EventArgs e)
        {

        }
    }
}
