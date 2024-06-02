using BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IUserService
    {
        IEnumerable<UserDTO> GetAllUsers();
        UserDTO GetUserById(int userId);
        UserDTO GetUserByNamePass(string login, string password);
        void RegisterUser(UserDTO userDto);
        void UpdateUser(UserDTO userDto);
        void DeleteUser(int userId);
        void Dispose();
    }
}
