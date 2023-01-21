using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace UniCheckManagementSystem.Models
{
    public class Enrollment
    {
        public int EnrollmentId { get; set; }

        public int CourseId { get; set; }

        public int StudentId { get; set; }

        [Display(Name = "Оценка")]
        [DisplayFormat(NullDisplayText = "Няма оценка.")]
        public string Grade { get; set; }

        [Display(Name = "Дата на изпита")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Това поле е задължително.")]
        public DateTime ExamDate { get; set; }

        public virtual Course Course { get; set; }

        public virtual Student Student { get; set; }
    }
}