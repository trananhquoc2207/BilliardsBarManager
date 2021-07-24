using QuanLyQuanBida.DAO;
using QuanLyQuanBida.DTO;
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

namespace QuanLyQuanBida
{
    public partial class GiaoDienChinh : Form
    {
        public GiaoDienChinh()
        {
            InitializeComponent();
            LoadTable();
            
            LoadCategory();
            //LoadComboboxTable();
        }
        void LoadCategory()
        {
            List<Category> listCategory = CategoryDAO.Instance.GetListCategory();
            comboBox1.DataSource = listCategory;
            comboBox1.DisplayMember = "Name";
        }

        void LoadFoodListByCategoryID(int id)
        {
            List<Food> listFood = FoodDAO.Instance.GetFoodByCategoryID(id);
            comboBox2.DataSource = listFood;
            comboBox2.DisplayMember = "Name";
        }
        void LoadComboboxTable(ComboBox cb)
        {
            cb.DataSource = TableDAO.Instance.LoadTableList();
            cb.DisplayMember = "Name";
        }
        

        private void LoadTable()
        {
            flowLayoutPanel1.Controls.Clear();
            List<Table> tableList = TableDAO.Instance.LoadTableList();
            foreach (Table item in tableList)
            {
                Button btn = new Button() { Width = TableDAO.TableWith, Height = TableDAO.TableHeight };
                string loaiBan = "";
                if(item.ID <= 5)
                {
                    loaiBan = "Bida phăng";
                }
                else { loaiBan = "Bida lỗ"; }
                btn.Text = item.Name + Environment.NewLine + item.Status + Environment.NewLine + loaiBan;
                btn.Click += btn_Click;
                btn.Tag = item;

                switch (item.Status)
                {
                    case "trống":
                        btn.BackColor = Color.Brown;
                        break;
                    case "Trống":
                        btn.BackColor = Color.Brown;
                        break;
                    default :
                        btn.BackColor = Color.Red;
                        break;
                }

                flowLayoutPanel1.Controls.Add(btn);
            }
        }
        void ShowBill(int idTable)
        {
            listView1.Items.Clear();
            List<QuanLyQuanBida.DTO.Menu> listBillInfo = 
                MenuDAO.Instance.GetListMenuByTable(idTable);
            float totalPrice = 0;
            foreach(QuanLyQuanBida.DTO.Menu item in listBillInfo)
            {
                ListViewItem lsvItem = new ListViewItem(item.FoodName.ToString());
                lsvItem.SubItems.Add(item.Count.ToString());
                lsvItem.SubItems.Add(item.Price.ToString());
                lsvItem.SubItems.Add(item.TotalPrice.ToString());
                totalPrice += item.TotalPrice;
                listView1.Items.Add(lsvItem);
            }
            CultureInfo culture = new CultureInfo("vi-VN");
            Thread.CurrentThread.CurrentCulture = culture;

            textBox1.Text = totalPrice.ToString("c");
        }
        private void btn_Click(object sender, EventArgs e)
        {
            int idTable = ((sender as Button ).Tag as Table).ID;
            listView1.Tag = (sender as Button).Tag;
            ShowBill(idTable);
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void thôngTinCáNhânToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ThongTinTaiKhoan t = new ThongTinTaiKhoan();
            this.Hide();
            t.ShowDialog();
            this.Show();
        }

        private void adminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Admin a = new Admin();
            this.Hide();
            a.ShowDialog();
            this.Show();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = 0;

            ComboBox cb = sender as ComboBox;

            if (cb.SelectedItem == null)
                return;

            Category selected = cb.SelectedItem as Category;
            id = selected.ID;
            LoadFoodListByCategoryID(id);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Table table = listView1.Tag as Table;

            if (table == null)
            {
                MessageBox.Show("Hãy chọn bàn");
                return;
            }

            int idBill = BillDAO.Instance.GetUncheckBillbyTableID(table.ID);
            int foodID = (comboBox2.SelectedItem as Food).ID;
            int count = (int)numericUpDown1.Value;

            if (idBill == -1)
            {
                BillDAO.Instance.InsertBill(table.ID);
                BillInfoDAO.Instance.InsertBillInfo(BillDAO.Instance.GetMaxIDBill(), foodID, count);
            }
            else
            {
                BillInfoDAO.Instance.InsertBillInfo(idBill, foodID, count);
            }

            ShowBill(table.ID);

            LoadTable();
        }

        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            Table table = listView1.Tag as Table;

            int idBill = BillDAO.Instance.GetUncheckBillIDByTableID(table.ID);
            double totalPrice = Convert.ToDouble(textBox1.Text.Split(',')[0]);
            if (idBill != -1)
            {
                if (MessageBox.Show("Bạn có chắc thanh toán hóa đơn cho bàn " + table.Name, "Thông báo", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {
                    BillDAO.Instance.CheckOut(idBill,(float)totalPrice);
                    ShowBill(table.ID);

                    LoadTable();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Table table = listView1.Tag as Table;

            if (table == null)
            {
                MessageBox.Show("Hãy chọn bàn");
                return;
            }

            int idBill = BillDAO.Instance.GetUncheckBillbyTableID(table.ID);
            int foodID = (comboBox2.SelectedItem as Food).ID;
            int count = (int)numericUpDown1.Value;

            if (idBill == -1)
            {
                if (table.ID <= 5)
                {
                    BillDAO.Instance.InsertBill(table.ID);
                    BillInfoDAO.Instance.InsertBillInfo(BillDAO.Instance.GetMaxIDBill(), 5, 0);
                }
                else
                {
                    BillDAO.Instance.InsertBill(table.ID);
                    BillInfoDAO.Instance.InsertBillInfo(BillDAO.Instance.GetMaxIDBill(), 6, 0);
                }
            }
            else
            {
                BillInfoDAO.Instance.InsertBillInfo(idBill, foodID, count);
            }

            ShowBill(table.ID);

            LoadTable();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Table table = listView1.Tag as Table;

            int idBill = BillDAO.Instance.GetUncheckBillIDByTableID(table.ID);
            if (idBill != -1)
            {
                if (MessageBox.Show("Bạn có chắc kết thúc giờ cho bàn " + table.Name, "Thông báo", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {
                    BillDAO.Instance.CheckOutByTime();
                    MessageBox.Show("số giờ chơi của bàn là: " + (BillDAO.Instance.GetSoGio(idBill)).ToString());
                    

                }
                if (table.ID <= 5)
                {
                    BillInfoDAO.Instance.InsertBillInfo(idBill, 5, BillDAO.Instance.GetSoGio(idBill));
                }
                else
                {
                    BillInfoDAO.Instance.InsertBillInfo(idBill, 6, BillDAO.Instance.GetSoGio(idBill));
                }
                ShowBill(table.ID);
            }
        }
    }
}
