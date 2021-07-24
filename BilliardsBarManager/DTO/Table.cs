using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanBida.DTO
{
    public class Table
    {
        private int iD;
        private string status;
        private string name;

        public string Status { get => status; set => status = value; }
        public int ID { get => iD; set => iD = value; }
        public string Name { get => name; set => name = value; }

        public Table(int id,string status,string name)
        {
            this.iD = id;
            this.Status = status;
            this.Name = name;
        }
        public Table(DataRow row)
        {
            this.ID = (int)row["id"];
            this.Name = row["name"].ToString();
            this.Status = row["status"].ToString();
        }
        

    }
}
