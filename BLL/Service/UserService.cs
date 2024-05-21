using AutoMapper;
using BLL.Interface;
using DAL.Data.Entities;
using DAL.Interface;
using DTO.Helpers;
using DTO.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Service
{

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<User> RegisterUserAsync(UserRegisterModel registerUserDto)
        {
            if (await _userRepository.GetUserByEmailAsync(registerUserDto.Email) != null)
            {
                throw new ArgumentException("User with the same email already exists.");
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                CreateTime = DateTime.Now,
                FullName = registerUserDto.FullName,
                Password = registerUserDto.Password,
                Email = registerUserDto.Email,
                BirthDate = registerUserDto.BirthDate,
                Gender = registerUserDto.Gender,
                phoneNumber = registerUserDto.phoneNumber
            };

            return await _userRepository.AddUserAsync(user);
        }
        public async Task<User> LoginUserAsync(LoginCredentials loginUserDto)
        {
            var user = await _userRepository.GetUserByEmailAsync(loginUserDto.Email);
            if (user == null || user.Password != loginUserDto.Password)
            {
                throw new ArgumentException("Invalid email or password.");
            }

            return user;
        }
        public async Task<User> GetUserByIdAsync(Guid userId)
        {
            return await _userRepository.GetUserByIdAsync(userId);
        }
        public async Task<User> EditUserProfileAsync(Guid userId, UserEditModel editProfileDto)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new ArgumentException("User not found.");
            }

            user.Email = editProfileDto.Email;
            user.FullName = editProfileDto.FullName;
            user.BirthDate = editProfileDto.BirthDate;
            user.Gender = editProfileDto.Gender;
            user.phoneNumber = editProfileDto.phoneNumber;

            await _userRepository.UpdateUserAsync(user);
            return user;
        }
    }
}
