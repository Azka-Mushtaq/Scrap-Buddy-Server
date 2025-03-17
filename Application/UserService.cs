using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class UserService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Address> _addressRepository;
        private readonly PickupService _pickupService;
        public UserService(IRepository<User> userRepository,
            IRepository<Address> addressRepository, PickupService pickupService,
            IRepository<Vehicle> vehicleRepository)
        {
            _userRepository = userRepository;
            _addressRepository = addressRepository;
            _pickupService = pickupService;
        }

        public async Task<int> Add(User user)
        {
            //Insert into User table
            string columnNames = "Name,Email,PhoneNumber,Password,Role";
            int userId = await _userRepository.Add(columnNames, user, "User");

            return userId;
        }

        public async Task Update(User user)
        {
            //Insert into User table
            string columnNames = "Name,Email,PhoneNumber,UpdatedAt";
            await _userRepository.Update(columnNames, user);

        }
        public async Task<int> AddAddress(Address address)
        {
            return await _addressRepository.Add(address);
        }
        public async Task UpdateAddress(Address address)
        {
             await _addressRepository.Update(address);
        }
        public async Task DeleteAddress(int id)
        {
  
             await _addressRepository.DeleteById(id);
        }
        public async Task<List<User>> GetAll()
        {
            // Retrieve the user data from the User table
            string columnNames = "Id, Name, Email, Password, PhoneNumber, Role, CreatedAt, UpdatedAt";
            var users = await _userRepository.GetAll(columnNames);

            // Retrieve the associated addresses if the user exists
            foreach (var user in users)
            {
                user.Addresses = await _addressRepository.GetAll(tableName: "Address", compParameter: "userId", compValue: user.Id.ToString());
                user.Pickups = await _pickupService.GetAll(customerId:user.Id);
                
            
            }

            return users;
        }


        public async Task<User> Find(string email,string password)
        {
            // Retrieve the user data from the User table
            string columnNames = "Id,Name,Email,Password,PhoneNumber,Role,CreatedAt,UpdatedAt";
            string compColumns = "Email,Password";
            string compValues = $"{email.Trim()},{password.Trim()}";
            var user = await _userRepository.FindByAttribute(columnNames: columnNames,comparisonColumns:compColumns, comparisonValues:compValues);


            // Retrieve the associated addresses if the user exists
            if (user != null)
            {
                user.Addresses = await _addressRepository.GetAll(tableName: "Address", compParameter: "userId", compValue: user.Id.ToString());
                if (user.Role.Trim() == "user")
                    user.Pickups = await _pickupService.GetAll(customerId: user.Id);
                else
                    user.Pickups = await _pickupService.GetAll(riderId: user.Id);
            }


            return user;
        }

        public async Task Delete(int id)
        {
  
            await _userRepository.DeleteById(id);
        }
    }
}
