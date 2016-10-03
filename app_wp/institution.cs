using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotaFiscal
{
    class institution
    {
        public int id { get; set; }

        [JsonProperty(PropertyName = "cnpj")]
        public string cnpj { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string name { get; set; }

    }
}
