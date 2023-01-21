using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace UniCheckManagementSystem.Models
{
    public class Student
    {
        public int StudentId { get; set; }

        [Display(Name = "Факултетен номер")]
        [StringLength(10, ErrorMessage = "Факултетният номер трябва да е с дължина 10 символа.")]
        [Required(ErrorMessage = "Това поле е задължително.")]
        public string FacultyNumber { get; set; }

        [Display(Name = "Име")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Името трябва да има минимална дължина 2 символа и максимална - 50 символа.")]
        [Required(ErrorMessage = "Това поле е задължително.")]
        public string FirstName { get; set; }

        [Display(Name = "Фамилия")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Фамилията трябва да има минимална дължина 2 символа и максимална - 50 символа.")]
        [Required(ErrorMessage = "Това поле е задължително.")]
        public string LastName { get; set; }

        [Display(Name = "Дата на записване")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Това поле е задължително.")]
        public DateTime EnrollmentDate { get; set; }

        [Display(Name = "Изображение")]
        public string ImagePath { get; set; }

        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }

        public int SpecialtyId { get; set; }

        public virtual Specialty Specialty { get; set; }

        public virtual ICollection<Enrollment> Enrollments { get; set; }
    }
}