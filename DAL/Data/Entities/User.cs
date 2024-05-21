using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DAL.Data.Enum.DataEnum;

namespace DAL.Data.Entities
{
    public class User
    {
        public required Guid Id { get; set; }
        public required DateTime CreateTime { get; set; }
        public required string FullName { get; set; }
        public required string Password { get; set; }
        public required string Email { get; set; }
        public required Gender Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? phoneNumber { get; set; }

    }
}
