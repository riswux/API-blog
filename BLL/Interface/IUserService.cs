using DAL.Data.Entities;
using DTO.Helpers;
using DTO.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interface
{
    public interface IUserService
    {
        Task<User> RegisterUserAsync(UserRegisterModel registerUserDto);
        Task<User> LoginUserAsync(LoginCredentials loginUserDto);
        Task<User> GetUserByIdAsync(Guid userId);
        Task<User> EditUserProfileAsync(Guid userId, UserEditModel editProfileDto);


    }
}
