using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReferMeAPI.Model
{
    public class Post
    {
        [Key]
        [JsonProperty(PropertyName = "id")]
        public string post_id { get; set; }
        public string user_id { get; set; }
        public string role { get; set; }
        public string post_description { get; set; }
        public string for_company { get; set; }
        public string position { get; set; }
        public int experience_required { get; set; }
        public string job_location_country { get; set; }
        public string job_location_city { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public string delete_on { get; set; }
    }
}
