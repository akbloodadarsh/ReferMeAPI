using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReferMeAPI.Model
{
    public class Company
    {
        [Key]
        [JsonProperty(PropertyName = "id")]
        public string company_id { get; set; }
        public string user_id { get; set; }
        public string comp_name { get; set; }
        public DateTime worked_from { get; set; }
        public DateTime worked_till { get; set; }
        public string employment_type { get; set; }
        public string work_location { get; set; }
        public string work_description { get; set; }
    }
}
