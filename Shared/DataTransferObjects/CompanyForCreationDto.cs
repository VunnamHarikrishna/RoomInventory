using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects
{
    public record CompanyForCreationDto
    {
        [Required(ErrorMessage = "Employee name is a required field.")]
        [MaxLength(60, ErrorMessage = "Maximum length for the Name is 30 characters.")]
        public string? Name { get; init; }

        [Required(ErrorMessage = "Employee name is a required field.")]
        [MaxLength(60, ErrorMessage = "Maximum length for the Name is 30 characters.")]
        public string? Address { get; init; }

        public string? Country { get; init; }

        IEnumerable<EmployeeForCreationDto> Employees;
    }
}