using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace NotaFiscal
{
    [Table(Name = "Institutions")]
    public class DB
    {
        private int _id;

        [Column(Name = "Id", IsPrimaryKey = true, CanBeNull = false)]
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _institution_name;

        [Column(Name = "Institution_Name", CanBeNull = false)]
        public string Institution_Name
        {
            get { return _institution_name; }
            set { _institution_name = value; }
        }

        private bool _selected;

        [Column(Name = "Selected", CanBeNull = false)]
        public bool Selected
        {
            get { return _selected; }
            set { _selected = value; }
        }
    }
}
