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
    [Table(Name = "Receipts_Upload")]
    public class DB2
    {
        private int _id;

        [Column(Name = "Id", IsPrimaryKey = true, CanBeNull = false, IsDbGenerated = true)]
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private byte[] _file;

        [Column(DbType = "image", Name = "File", CanBeNull = false, UpdateCheck = UpdateCheck.Never)]
        public byte[] File
        {
            get { return _file; }
            set { _file = value; }
        }

        private int _institution_id;

        [Column(Name = "Institution_Id", CanBeNull = false)]
        public int Institution_Id
        {
            get { return _institution_id; }
            set { _institution_id = value; }
        }
        
    }
}
