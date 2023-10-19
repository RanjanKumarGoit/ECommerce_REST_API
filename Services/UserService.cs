using Microsoft.EntityFrameworkCore;
using trying.Controllers.Dto;
using trying.Data;
using trying.Model;
using trying.Interfaces;
using System.Collections.Generic;
using System.Linq;


namespace trying.Services
{

    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<User> GetUsers()
        {
            return _context.Users.Include(u => u.Address).ToList();
        }


        public User GetUserById(int id)
        {
            return _context.Users.Include(u => u.Address).FirstOrDefault(u => u.Id == id);
        }

        public User GetMyProfile(int id)
        {
            return GetUserById(id);
        }
        public void CreateUser(UserDto userDto)
        {
            var user = new User
            {
                Name = userDto.Name,
                Email = userDto.Email,
                Phone = userDto.Phone,
                Password = userDto.Password,
                Role = userDto.Role,
            };

            _context.Users.Add(user);
            _context.SaveChanges(); // Save changes to generate user.Id

            var address = new Address
            {
                Country = userDto.Address.Country,
                State = userDto.Address.State,
                City = userDto.Address.City,
                UserID = user.Id // Use user.Id after it's generated
            };

            _context.Addresses.Add(address);
            _context.SaveChanges(); // Save changes to generate address.Id
        }


        public void UpdateUser(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeleteUser(int id)
        {
            var user = _context.Users.Find(id);

            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }

        public User GetUserByEmailAndPassword(string email, string password)
        {
            // Validate user credentials by checking email and password
            return _context.Users
                .Include(u => u.Address)
                .FirstOrDefault(u => u.Email == email);
        }
    }
}