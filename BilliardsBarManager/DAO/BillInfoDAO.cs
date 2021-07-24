using QuanLyQuanBida.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanBida.DAO
{
     public class BillInfoDAO
    {
        private static BillInfoDAO instance;
        public static BillInfoDAO Instance
        {
            get { if (instance == null) instance = new BillInfoDAO(); return BillInfoDAO.instance; }
            private set { BillInfoDAO.instance = value; }
        }
        private BillInfoDAO() { }

        public List<BillInfo> GetListBillInfo(int id)
        {
            List<BillInfo> lisBillInfo = new List<BillInfo>();
            DataTable data = DataProvider.Instance.ExecuteOuery("SELECT * FROM dbo.billInfo WHERE idBill = "+id);
            foreach (DataRow item in data.Rows)
            {
                BillInfo info = new BillInfo(item);
                lisBillInfo.Add(info);
            }
            return lisBillInfo;
        }
        public void InsertBillInfo(int idBill = 0, int idFood =0 , int count = 0)
        {
            DataProvider.Instance.ExecuteNonOuery("USP_InsertBillInfo @idBill , @idFood , @count", new object[] { idBill, idFood, count });
        }
        public void DeleteBillInfoByFoodID(int id)
        {
            DataProvider.Instance.ExecuteOuery("delete dbo.BillInfo WHERE idFood = " + id);
        }
    }
}
