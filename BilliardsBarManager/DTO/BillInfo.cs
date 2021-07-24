using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanBida.DTO
{
    public class BillInfo
    {
        private int iD;
        private int billID;
        private int foodId;
        private int count;

        public BillInfo(int id,int billId, int foodId,int count)
        {
            this.ID = id;
            this.BillID = billId;
            this.FoodId = foodId;
            this.Count = count;
        }
        public BillInfo(DataRow row)
        {
            this.ID = (int)row["id"];
            this.BillID = (int)row["idBill"];
            this.FoodId = (int)row["idFood"];
            this.Count = (int)row["count"];
        }

        public int ID { get => iD; set => iD = value; }
        public int BillID { get => billID; set => billID = value; }
        public int FoodId { get => foodId; set => foodId = value; }
        public int Count { get => count; set => count = value; }
    }
}
