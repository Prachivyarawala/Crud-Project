using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class City
    {
        public int c_cityid { get; set; }
        public int c_userid { get; set; }
        public string c_cityname { get; set; }
        public string c_type { get; set; }
        public string c_city_facility { get; set; }
        public string c_city_photo { get; set; }
        
        public DateTime? c_date { get; set; }

        public int? c_stateid { get; set; }
        [ForeignKey("c_stateid")]
        public virtual State? State { get; set; }
        

        [ForeignKey("c_userid")]
        public virtual User user{get; set;}
    }
}