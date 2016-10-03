using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;

namespace NotaFiscal
{
    [Table(Name = "Wifi/3g")]
    public class DB3
    {
        private int _id;

        [Column(Name = "Id", IsPrimaryKey = true, CanBeNull = false)]
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private int _type;

        [Column(Name = "Type", CanBeNull = false)]
        public int Type
        {
            get { return _type; }
            set { _type = value; }
        }      
    }
}
