using BLL.DTO;

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
