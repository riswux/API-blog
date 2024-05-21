using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static DAL.Data.Enum.DataEnum;

namespace DTO.User
{
    public class UserDto
    {
        public required Guid Id { get; set; }
        public required DateTime CreateTime { get; set; }
        public required string FullName { get; set; }
        public DateTime? BirthDate { get; set; }
        public required Gender Gender { get; set; }
        public required string Email { get; set; }
        public string? phoneNumber { get; set; }
    }
}
