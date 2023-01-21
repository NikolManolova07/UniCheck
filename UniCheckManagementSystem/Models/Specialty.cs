using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace UniCheckManagementSystem.Models
{
    public class Specialty
    {
        public int SpecialtyId { get; set; }

        [Display(Name = "Име")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Името трябва да има минимална дължина 2 символа и максимална - 50 символа.")]
        [Required(ErrorMessage = "Това поле е задължително.")]
        public string Name { get; set; }

        [Display(Name = "Професионално направление")]
        [Required(ErrorMessage = "Това поле е задължително.")]
        public string ProfessionalStream { get; set; }

        [Display(Name = "Образователно-квалификационна степен")]
        [Required(ErrorMessage = "Това поле е задължително.")]
        public string Degree { get; set; }

        [Display(Name = "Професионална квалификация")]
        [Required(ErrorMessage = "Това поле е задължително.")]
        public string Qualification { get; set; }

        [Display(Name = "Форма на обучение")]
        [Required(ErrorMessage = "Това поле е задължително.")]
        public string ModeOfStudy { get; set; }

        [Display(Name = "Продължителност на обучение")]
        [Required(ErrorMessage = "Това поле е задължително.")]
        public int Length { get; set; }

        public virtual ICollection<Student> Students { get; set; }
    }
}