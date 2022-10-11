using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReferMeAPI.Model
{
    public class User
    {
        [Key]
        [JsonProperty(PropertyName = "id")]
        public string user_id { get; set; }
        public string user_name { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public string date_of_birth{ get; set; }
        public List<string> college { get; set; }
        public string gender { get; set; }
        public List<string> skill { get; set; }
        public string profile_pic { get; set; }
        public string gmail { get; set; }
        public string resume { get; set; }

    }
}
