using AutoMapper;
using BLL.DTO;
using BLL.Infrastructure;
using BLL.Infrastructure.SD;
using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IEnumerable<UserDTO> GetAllUsers()
        {
            var users = _unitOfWork.Users.GetAll();
            return _mapper.Map<IEnumerable<UserDTO>>(users);
        }

        public UserDTO GetUserById(int userId)
        {
            var user = _unitOfWork.Users.GetFirstOrDefault(u => u.Id == userId);
            return _mapper.Map<UserDTO>(user);
        }

        public UserDTO GetUserByNamePass(string login, string password)
        {
            if (login == null)
                throw new ValidationException("Login is Null", "");
            var user = _unitOfWork.Users.GetFirstOrDefault(u => u.Login == login);
            if (user == null)
                throw new ValidationException("User not Found", "");
            if (password != user.Password)
                throw new ValidationException("Password is Incorrect", "");
            return _mapper.Map<UserDTO>(user);
        }

        public void RegisterUser(UserDTO userDto)
        {
            User user = _unitOfWork.Users.GetFirstOrDefault(u => u.Login == userDto.Login);

            if (user != null)
                throw new ValidationException("User is already exists", "");

            userDto.Role = SD.RoleWorker;
            user = _mapper.Map<User>(userDto);
            _unitOfWork.Users.Add(user);
            _unitOfWork.Save();
        }

        public void UpdateUser(UserDTO userDto)
        {
            var user = _mapper.Map<User>(userDto);
            _unitOfWork.Users.Update(user);
            _unitOfWork.Save();
        }

        public void DeleteUser(int userId)
        {
            var user = _unitOfWork.Users.GetFirstOrDefault(u => u.Id == userId);
            _unitOfWork.Users.Remove(user);
            _unitOfWork.Save();
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}
