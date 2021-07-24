using QuanLyQuanBida.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanBida.DAO
{
    public class BillDAO
    {
        private static BillDAO instance;
        public static BillDAO Instance
        {
            get { if (instance == null) instance = new BillDAO(); return BillDAO.instance; }
            private set { BillDAO.instance = value; }
        }
        private BillDAO() { }

        public int GetUncheckBillbyTableID(int id)
        {
            DataTable data = DataProvider.Instance.ExecuteOuery("SELECT * FROM dbo.Bill WHERE idTable ="+ id + "  AND status =0");
            if (data.Rows.Count > 0)
            {
                Bill bill = new Bill(data.Rows[0]);
                return bill.ID;
            }
            return -1;
        }
        public void CheckOut(int id, int discount, float totalPrice)
        {
            string query = "UPDATE dbo.Bill SET dateCheckOut = GETDATE(), status = 1, " + "discount = " + discount + ", totalPrice = " + totalPrice + " WHERE id = " + id;
            DataProvider.Instance.ExecuteNonOuery(query);
        }
        public void InsertBill(int id = 0)
        {
            DataProvider.Instance.ExecuteNonOuery("exec USP_InsertBill @idTable", new object[] { id });
        }

        public DataTable GetBillListByDate(DateTime checkIn, DateTime checkOut)
        {
            return DataProvider.Instance.ExecuteOuery("exec USP_GetListBillByDate @checkIn , @checkOut", new object[] { checkIn, checkOut });
        }
        public DataTable GetBillList()
        {
            string query = "SELECT t.name,b.totalPrice, idTable,DataCheckIn,DataCheckOut FROM dbo.Bill AS b, dbo.TablePlay AS t, dbo.billInfo AS bi,dbo.Food AS f WHERE b.status = 1 AND t.id = b.idTable AND b.id = bi.idBill AND bi.idFood = f.id";
            return DataProvider.Instance.ExecuteOuery(query);
        }

        public DataTable GetBillListByDateAndPage(DateTime checkIn, DateTime checkOut, int pageNum)
        {
            return DataProvider.Instance.ExecuteOuery("exec USP_GetListBillByDateAndPage @checkIn , @checkOut , @page", new object[] { checkIn, checkOut, pageNum });
        }

        public int GetNumBillListByDate(DateTime checkIn, DateTime checkOut)
        {
            return (int)DataProvider.Instance.ExecuteScalar("exec USP_GetNumBillByDate @checkIn , @checkOut", new object[] { checkIn, checkOut });
        }

        public int GetMaxIDBill()
        {
            try
            {
                return (int)DataProvider.Instance.ExecuteScalar("SELECT MAX(id) FROM dbo.Bill");
            }
            catch
            {
                return 1;
            }
        }

        internal int GetUncheckBillIDByTableID(int iD)
        {
            DataTable data = DataProvider.Instance.ExecuteOuery("SELECT * FROM dbo.Bill WHERE idTable = " + iD + " AND status = 0");

            if (data.Rows.Count > 0)
            {
                Bill bill = new Bill(data.Rows[0]);
                return bill.ID;
            }

            return -1;
        }

        internal void CheckOut(int idBill,float totalPrice)
        {
            string query1 = "UPDATE dbo.Bill SET DataCheckOut = GETDATE() , status = 1,totalPrice = "+ totalPrice+" WHERE id = " + idBill;
            string query2 = "UPDATE dbo.Bill SET soGioChoi = DATEDIFF (MINUTE, DataCheckIn,DataCheckOut)";

            DataProvider.Instance.ExecuteNonOuery(query1);
            DataProvider.Instance.ExecuteNonOuery(query2);

        }
        internal void CheckOutByTime()
        {
            string query1 = "UPDATE dbo.Bill SET DataCheckOut = GETDATE()";
            string query2 = "UPDATE dbo.Bill SET soGioChoi = DATEDIFF (MINUTE, DataCheckIn,DataCheckOut)";

            DataProvider.Instance.ExecuteNonOuery(query1);
            DataProvider.Instance.ExecuteNonOuery(query2);

        }

        internal int GetSoGio( int idBill)
        {
            int i = 0;
            string query  = "SELECT soGioChoi FROM dbo.bill WHERE id =  " + idBill;
            i = (int)DataProvider.Instance.ExecuteScalar(query);

            return i;
        }
    }

}

