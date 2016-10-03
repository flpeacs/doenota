using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotaFiscal
{
    class images_data
    {
        [JsonProperty(PropertyName = "id")]
        public int id { get; set; }

        [JsonProperty(PropertyName = "user_id")]
        public int user_id { get; set; }

        [JsonProperty(PropertyName = "cnpj")]
        public string cnpj { get; set; }

        [JsonProperty(PropertyName = "emission_date")]
        public string emission_date { get; set; }

        [JsonProperty(PropertyName = "coupon_code")]
        public string coupon_code { get; set; }

        [JsonProperty(PropertyName = "status")]
        public string status { get; set; }

        [JsonProperty(PropertyName = "purchase_value")]
        public double purchase_value { get; set; }

        [JsonProperty(PropertyName = "file_path")]
        public string file_path { get; set; }

        [JsonProperty(PropertyName = "platform")]
        public string platform { get; set; }

        [JsonProperty(PropertyName = "identificator")]
        public string identificator { get; set; }

        [JsonProperty(PropertyName = "date")]
        public string date { get; set; }

        [JsonProperty(PropertyName = "institution_id")]
        public int institution_id { get; set; }
                
       
        [JsonProperty(PropertyName = "files")]
        public byte[] files { get; set; }

        [JsonProperty(PropertyName = "next_donation")]
        public long next_donation { get; set; }

        [JsonProperty(PropertyName = "__deleted")]
        public byte __deleted { get; set; }



    }
}
