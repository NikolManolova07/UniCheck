using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace UniCheckManagementSystem.Models
{
    public class Lecturer
    {
        public int LecturerId { get; set; }

        [Display(Name = "Име")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Името трябва да има минимална дължина 2 символа и максимална - 50 символа.")]
        [Required(ErrorMessage = "Това поле е задължително.")]
        public string FirstName { get; set; }

        [Display(Name = "Фамилия")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Фамилията трябва да има минимална дължина 2 символа и максимална - 50 символа.")]
        [Required(ErrorMessage = "Това поле е задължително.")]
        public string LastName { get; set; }

        [Display(Name = "Дата на назначаване")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime HireDate { get; set; }

        [Display(Name = "Кабинет")]
        [Required(ErrorMessage = "Това поле е задължително.")]
        public string Office { get; set; }

        [Display(Name = "Изображение")]
        public string ImagePath { get; set; }

        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }

        public int DepartmentId { get; set; }

        public virtual Department Department { get; set; }

        public virtual ICollection<Course> Courses { get; set; }
    }
}