using trying.Controllers.Dto;
using trying.Model;

namespace trying.Interfaces
{
    public interface IUserService
    {
        IEnumerable<User> GetUsers();
        User GetUserById(int id);
        User GetMyProfile(int id);
        User GetUserByEmailAndPassword(string email, string password);
        void CreateUser(UserDto userDto);
        void UpdateUser(User user);
        void DeleteUser(int id);
    }

}
