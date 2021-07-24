using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanBida.DTO
{
    public class Bill
    {
        public Bill(int id, DateTime? dateCheckIn, DateTime? dateCheckOut, int soGioChoi,int status)
        {
            this.ID = id;
            this.DateCheckIn = dateCheckIn;
            this.DateCheckout = dateCheckOut;
            this.soGioChoi = soGioChoi;
            this.Status = status;
        }

        public Bill(DataRow row)
        {
            this.ID = (int)row["id"];
            this.DateCheckIn = (DateTime?)row["DataCheckIn"];
            var dataCheckOutTemp = row["DataCheckOut"];
            if (dataCheckOutTemp.ToString() != "")
            {
                this.DateCheckout = (DateTime?)row["DataCheckOut"];
            }

            //this.SoGioChoi = (DateTime?)row["soGioChoi"];
            this.Status = (int)row["status"];
        }
        private int iD;
        private DateTime? dateCheckIn;
        private DateTime? dateCheckout;
        private int iDTable;
        private int soGioChoi;
        private int status;

        public int ID { get => iD; set => iD = value; }
        public DateTime? DateCheckIn { get => dateCheckIn; set => dateCheckIn = value; }
        public DateTime? DateCheckout { get => dateCheckout; set => dateCheckout = value; }
        public int IDTable { get => iDTable; set => iDTable = value; }
        public int SoGioChoi { get => soGioChoi; set => soGioChoi = value; }
        public int Status { get => status; set => status = value; }
    }
}
