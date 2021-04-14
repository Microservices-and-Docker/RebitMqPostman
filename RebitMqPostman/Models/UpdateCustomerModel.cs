using System;
using System.ComponentModel.DataAnnotations;

namespace RebitMqPostman.Models
{
    public class UpdateCustomerModel
    {
        [Required] public string FirstName { get; set; }

        [Required] public string LastName { get; set; }

        public DateTime? Birthday { get; set; }

        public int? Age { get; set; }
    }
}
