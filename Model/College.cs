using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReferMeAPI.Model
{
    public class College
    {
        [Key]
        [JsonProperty(PropertyName = "id")]
        public string college_id { get; set; }
        public string user_id { get; set; }
        public string course_attended { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public string attended_from { get;  set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public string attended_till { get; set; }
        public bool ongoing { get; set; }
    }
}
