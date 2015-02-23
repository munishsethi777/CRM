using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using MVCModels.Validator;
using MVCModels;
namespace MVCModels.metadata
{
    // [MetadataType(typeof(Calendar.MetaData.CalendarEventMetaData))]
    public class CalendarEventMetaData
    {
        public int id { get; set; }
        [Display(Name = "Event Title")]
        [Required]
        public string title { get; set; }
         [Display(Name = "Description")]
        public string description { get; set; }
         [Display(Name = "Start Date")]
        public System.DateTime startdate { get; set; }
        [Display(Name = "End Date")]
        public Nullable<System.DateTime> enddate { get; set; }
        public Nullable<int> parenteventseq { get; set; }
        public string durationoption { get; set; }        
        [RequiredIf("durationoption",Comparison.IsEqualTo,DurationOptions.Untill)]
        public Nullable<System.DateTime> durationuntill { get; set; }
        [RequiredIf("durationoption", Comparison.IsEqualTo, DurationOptions.DurationInMunites)]
        public Nullable<int> durationinminutes { get; set; }
        [Display(Name = "Repeat this event")]
        public bool isrepeated { get; set; }
        [Display(Name = "Repeat weekly,creating altogather")]
        [RequiredIf("isrepeated", Comparison.IsEqualTo, true)]
        public Nullable<int> repeatedweekly { get; set; }
        public string eventtype { get; set; }
        public int deparmentid { get; set; }
    }

   
}