using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace UniCheckManagementSystem.Models
{
    public class Course
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Номер")]
        public int CourseId { get; set; }

        [Display(Name = "Име")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Името трябва да има минимална дължина 2 символа и максимална - 50 символа.")]
        [Required(ErrorMessage = "Това поле е задължително.")]
        public string Title { get; set; }

        [Display(Name = "Кредити")]
        [Range(0, 11)]
        [Required(ErrorMessage = "Това поле е задължително.")]
        public int Credits { get; set; }

        [Display(Name = "Описание")]
        public string Description { get; set; }

        public virtual ICollection<Enrollment> Enrollments { get; set; }

        public virtual ICollection<Lecturer> Lecturers { get; set; }
    }
}