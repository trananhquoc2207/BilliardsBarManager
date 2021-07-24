using QuanLyQuanBida.DAO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyQuanBida
{
    public partial class Admin : Form
    {
        BindingSource accountList = new BindingSource();
        public Admin()
        {
            InitializeComponent();
            Load1();

        }
        void Load1()
        {
            dataGridView5.DataSource = accountList;
            AddAccountBiding();
            LoadListBillByDate();
            LoadListFood();
            LoadAccount();
            AddFoodBinding();
        }

        private void AddAccountBiding()
        {
            textBox9.DataBindings.Add(new Binding("Text", dataGridView5.DataSource, "UseName", true, DataSourceUpdateMode.Never));
            textBox8.DataBindings.Add(new Binding("Text", dataGridView5.DataSource, "DisplayName", true, DataSourceUpdateMode.Never));

        }

        void AddFoodBinding()
        {
            textFoodName.DataBindings.Add(new Binding("Text",dataGridView3.DataSource,"Name",true,DataSourceUpdateMode.Never));
            txtID.DataBindings.Add(new Binding("Text", dataGridView3.DataSource, "ID"));
            numericUpDown1.DataBindings.Add(new Binding("Value", dataGridView3.DataSource, "Price",true,DataSourceUpdateMode.Never));
        }

        void LoadListFood()
        {
            dataGridView3.DataSource = FoodDAO.Instance.GetListFood();
            //AddFoodBinding();
        }
        void LoadListBillByDate()
        {
           //dataGridView2.DataSource = BillDAO.Instance.GetBillListByDate(checkIn, checkOut);
            dataGridView2.DataSource = BillDAO.Instance.GetBillList();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            LoadListBillByDate();
        }

        private void btnXem_Click(object sender, EventArgs e)
        {
            LoadListFood();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            string name = textFoodName.Text;
            float pice = (float)numericUpDown1.Value;

            if (FoodDAO.Instance.InsertFood(name, 1, pice))
            {
                MessageBox.Show("Them thanh cong");
                    LoadListFood();
                
            }
            else
            {
                MessageBox.Show("them co loi");
            }

        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            string name = textFoodName.Text;
            float pice = (float)numericUpDown1.Value;
            int id = Convert.ToInt32(txtID.Text);

            if (FoodDAO.Instance.UpdateFood(id, name, 1, pice))
            {
                MessageBox.Show("Sua thanh cong");
                LoadListFood();

            }
            else
            {
                MessageBox.Show("Sua co loi");
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txtID.Text);
            if (FoodDAO.Instance.DeleteFood(id))
            {
                MessageBox.Show("Xoa thanh cong");
                LoadListFood();

            }
            else
            {
                MessageBox.Show("Xoa co loi");
            }
        }

        private void btnXemtk_Click(object sender, EventArgs e)
        {
            LoadAccount();
        }

        private void LoadAccount()
        {
            accountList.DataSource = AccountDAO.Instance.GetListAccount();
        }
        void AddAccount(string userName, string displayName, int type)
        {
            if (AccountDAO.Instance.InsertAccount(userName, displayName, type))
            {
                MessageBox.Show("Thêm tài khoản thành công");
            }
            else
            {
                MessageBox.Show("Thêm tài khoản thất bại");
            }

            LoadAccount();
        }

        void EditAccount(string userName, string displayName, int type)
        {
            if (AccountDAO.Instance.UpdateAccount(userName, displayName, type))
            {
                MessageBox.Show("Cập nhật tài khoản thành công");
            }
            else
            {
                MessageBox.Show("Cập nhật tài khoản thất bại");
            }

            LoadAccount();
        }

        void DeleteAccount(string userName)
        {
            
            if (AccountDAO.Instance.DeleteAccount(userName))
            {
                MessageBox.Show("Xóa tài khoản thành công");
            }
            else
            {
                MessageBox.Show("Xóa tài khoản thất bại");
            }

            LoadAccount();
        }

        void ResetPass(string userName)
        {
            if (AccountDAO.Instance.ResetPassword(userName))
            {
                MessageBox.Show("Đặt lại mật khẩu thành công");
            }
            else
            {
                MessageBox.Show("Đặt lại mật khẩu thất bại");
            }
        }

        private void btnThemTk_Click(object sender, EventArgs e)
        {
            string userName = textBox9.Text;
            string displayName = textBox8.Text;
            int type = (int)numericUpDown1.Value;
            AddAccount(userName, displayName, type);
        }

        private void btnXoaTk_Click(object sender, EventArgs e)
        {
            string userName = textBox8.Text;

            DeleteAccount(userName);
        }

        private void btnSuaTk_Click(object sender, EventArgs e)
        {
            string userName = textBox9.Text;
            string displayName = textBox8.Text;
            int type = (int)numericUpDown1.Value;

            EditAccount(userName, displayName, type);
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }



}
