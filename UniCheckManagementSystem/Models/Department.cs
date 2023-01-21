using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace UniCheckManagementSystem.Models
{
    public class Department
    {
        public int DepartmentId { get; set; }

        [Display(Name = "Име")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Името трябва да има минимална дължина 2 символа и максимална - 50 символа.")]
        [Required(ErrorMessage = "Това поле е задължително.")]
        public string Name { get; set; }

        [Display(Name = "Факултет")]
        [Required(ErrorMessage = "Това поле е задължително.")]
        public string Faculty { get; set; }

        [Display(Name = "Начална дата")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        public virtual ICollection<Lecturer> Lecturers { get; set; }
    }
}