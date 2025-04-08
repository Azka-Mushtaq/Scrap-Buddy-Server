using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Application;
using Domain;
using System.Runtime.Serialization;
using System.Diagnostics;
using Microsoft.AspNetCore.SignalR;

namespace Web__Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IHubContext<PickupHub> _pickupHub;
        private readonly UserService _userService;
        private readonly PickupService _pickupService;
        private readonly FirebaseNotificationService _firebaseNotificationService;
        private readonly UserFcmTokenService _userFcmTokenService;

        public UserController(UserFcmTokenService userFcmTokenService, UserService userService, 
            PickupService pickupService,IHubContext<PickupHub> pickupHub,
            FirebaseNotificationService firebaseNotificationService)
        {
            _userFcmTokenService = userFcmTokenService;
            _firebaseNotificationService = firebaseNotificationService;
            _pickupService = pickupService;
            _userService = userService;
            _pickupHub = pickupHub;
        }


        [HttpPost]
        [Route("addUserFcmToken")]
        public async Task<int> AddUserFcmToken(UserFcmToken webFcmToken)
        {
            if (webFcmToken == null)
            {
                throw new ArgumentNullException(nameof(webFcmToken), "FcmToken cannot be null");
            }

            try

            {

                var fcmToken = new Domain.UserFcmToken
                {
                    UserId = webFcmToken.UserId,
                    Topic = webFcmToken.Topic,
                    FcmToken = webFcmToken.FcmToken,
                    CreatedAt = DateTime.ParseExact(webFcmToken.CreatedAt, "yyyy-mm-dd", null),
                    UpdatedAt = DateTime.ParseExact(webFcmToken.CreatedAt, "yyyy-mm-dd", null),
                    IsActive = webFcmToken.IsActive

                };
                var tokenId = await _userFcmTokenService.AddFcmToken(fcmToken);
                return tokenId;
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding FcmToken", ex);
            }
        }


        [HttpGet]
        [Route("getAllUsers")]
        public async Task<List<User>> GetAllUsers()
        {
            // Fetch the list of domain users from the service
            var domainUsers = await _userService.GetAll();

            // Convert the list of domain users to web users
            var webUsers = domainUsers.Select(user => new User
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role,
                CreatedAt = user.CreatedAt.ToString("yyyy-MM-dd"), // Format as ISO 8601 for consistency
                UpdatedAt = user.UpdatedAt.ToString("yyyy-MM-dd"), // Format as ISO 8601 for consistency
                Addresses = user.Addresses?.Select(address => new Address
                {
                    Id = address.Id,
                    UserId = address.UserId,
                    Street = address.Street,
                    City = address.City,
                    Zipcode = address.Zipcode
                }).ToList(),
                Pickups = user.Pickups?.Select(pickup => new PickUp
                {
                    Id = pickup.Id,
                    AddressId = pickup.AddressId,
                    CustomerId = pickup.CustomerId,
                    RiderId = pickup.RiderId,
                    PickupDate = pickup.PickupDate.ToString("yyyy-MM-dd"), // Format as needed
                    PickupTime = pickup.PickupTime.ToString(@"hh\:mm\:ss"),  // Format as needed
                    Status = pickup.Status,
                    TotalWeight = pickup.TotalWeight,
                    CreatedAt = pickup.CreatedAt.ToString("yyyy-MM-dd"), // Format as ISO 8601 for consistency
                    UpdatedAt = pickup.UpdatedAt.ToString("yyyy-MM-dd") // Format as ISO 8601 for consistency
                }).ToList()
            }).ToList();

            return webUsers;
        }

        //[HttpGet("{id}")]
        //Or
        [HttpPost]
        [Route("authenticate")]
        public async Task<User> AuthenticateUser(LoginRequest loginCredentials)
        {


            Domain.User user = await _userService.Find(loginCredentials.email, loginCredentials.password);

            // Define the format for DateTime and TimeSpan
            var dateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            var timeSpanFormat = @"hh\:mm\:ss";

            var webUser = new User
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role,
                CreatedAt = user.CreatedAt.ToString(dateTimeFormat),
                UpdatedAt = user.UpdatedAt.ToString(dateTimeFormat),
                Addresses = user.Addresses?.Select(a => new Address
                {
                    Id = a.Id,
                    UserId = user.Id,
                    Street = a.Street,
                    City = a.City,
                    Zipcode = a.Zipcode
                }).ToList(),
                Pickups = user.Pickups?.Select(p => new PickUp
                {
                    Id = p.Id,
                    AddressId = p.AddressId,
                    CustomerId = p.CustomerId,
                    RiderId = p.RiderId,
                    PickupDate = p.PickupDate.ToString(dateTimeFormat),
                    PickupTime = p.PickupTime.TotalHours >= 0
                        ? p.PickupTime.ToString(timeSpanFormat)
                        : null,
                    Status = p.Status,
                    TotalWeight = p.TotalWeight,
                    CreatedAt = p.CreatedAt.ToString(dateTimeFormat),
                    UpdatedAt = p.UpdatedAt.ToString(dateTimeFormat)
                }).ToList()
            };

            return webUser;
        }

        [HttpPost]
        [Route("add")]
        public async Task<int> AddNewUser(User webUser)
        {
            // Convert WebUser to domain User
            var user = new Domain.User
            {

                Name = webUser.Name,
                Email = webUser.Email,
                Password = webUser.Password,
                PhoneNumber = webUser.PhoneNumber,
                Role = webUser.Role,
            };


            /*
             I don't want to send notification to all users.. i just want to send to some users... I have stored all users in db.. we will get specific users out of it.. and then need to send to them....
And tell me about 
             */
            return await _userService.Add(user);
        }


        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> UpdateUser(User webUser)
        {
            try
            {
                // Convert WebUser to domain User
                var user = new Domain.User
                {
                    Id = webUser.Id,
                    Name = webUser.Name,
                    Email = webUser.Email,
                    PhoneNumber = webUser.PhoneNumber,
                    UpdatedAt = DateTime.Now,
                    CreatedAt = DateTime.Now
                };

                // Update the user
                await _userService.Update(user);

                // Return 200 OK response
                return Ok();
            }
            catch (Exception ex)
            {
                // Log the exception (consider using a logging framework)
                Trace.WriteLine($"Exception: {ex.Message}");
                Debug.WriteLine($"Exception: {ex.Message}");

                // Return 500 Internal Server Error response with a message
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }



        [HttpPost]
        [Route("addAddress")]
        public async Task<int> AddAddress(Address webAddress)
        {

            var address = new Domain.Address
            {
                Id = webAddress.Id,
                UserId = webAddress.UserId,
                Street = webAddress.Street,
                City = webAddress.City,
                Zipcode = webAddress.Zipcode
            };

            return await _userService.AddAddress(address);

        }

        [HttpPost]
        [Route("updateAddress")]
        public async Task<IActionResult> UpdateAddress(Address webAddress)
        {
            try
            {
                var address = new Domain.Address
                {
                    Id = webAddress.Id,
                    UserId = webAddress.UserId,
                    Street = webAddress.Street,
                    City = webAddress.City,
                    Zipcode = webAddress.Zipcode
                };

                await _userService.UpdateAddress(address);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete]
        [Route("deleteAddress")]
        public async Task<IActionResult> DeleteAddress(int id)
        {
            try
            {
                await _userService.DeleteAddress(id);
                return Ok();
            }
            catch (Exception ex)
            {
                // Log the exception (consider using a logging framework)
                Trace.WriteLine($"Exception: {ex.Message}");
                Debug.WriteLine($"Exception: {ex.Message}");

                // Return 500 Internal Server Error response with a message
                return StatusCode(500, ex.Message);
            }

        }
        [HttpPost]
        [Route("addPickup")]
        public async Task<int> AddPickup(PickUp webPickup)
        {

            Domain.Pickup pickup = new Domain.Pickup
            {
                CustomerId = webPickup.CustomerId,
                AddressId = webPickup.AddressId,
                PickupDate = DateTime.ParseExact(webPickup.PickupDate, "yyyy-mm-dd", null),
                PickupTime = TimeSpan.ParseExact(webPickup.PickupTime, "hh\\:mm\\:ss", null),
                Status = webPickup.Status,
                TotalWeight = webPickup.TotalWeight,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };


            try
            {
                var tokens = await _userFcmTokenService.GetTokensByTopic("scrap picker");
                await _firebaseNotificationService.SendNotificationToTokensAsync(tokens, "New Pickup Added", $"A new pickup with ID has been scheduled.Click to confirm");

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return await _pickupService.Add(pickup);

        }

        [HttpPost]
        [Route("confirmPickup")]
        public async Task<IActionResult> ConfirmPickup(PickUp webPickup)
        {
            try
            {


                Domain.Pickup pickup = new Domain.Pickup
                {
                    CustomerId = webPickup.CustomerId,
                    AddressId = webPickup.AddressId,
                    PickupDate = DateTime.ParseExact(webPickup.PickupDate, "yyyy-mm-dd", null),
                    PickupTime = TimeSpan.ParseExact(webPickup.PickupTime, "hh\\:mm\\:ss", null),
                    Status = webPickup.Status,
                    TotalWeight = webPickup.TotalWeight,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                await _pickupHub.Clients.All.SendAsync("ReceiveConfirmation", pickup.CustomerId, "Pickup confirmed");
                await _pickupService.Update(pickup);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPut]
        [Route("updatePickup")]
        public async Task UpdatePickup(PickUp webPickup)
        {

            Domain.Pickup pickup = new Domain.Pickup
            {
                Id = webPickup.Id,
                RiderId = webPickup.RiderId,
                Status = webPickup.Status,
            };

            await _pickupService.Update(pickup);

        }



        [HttpGet]
        [Route("getAllPickups")]
        public async Task<List<Web__Api.PickUp>> GetNewPickups()
        {
            List<PickUp> webPickups = new List<PickUp>();

            List<Pickup> newDomainPickups = await _pickupService.GetAll();

            foreach (var pickup in newDomainPickups)
            {
                Web__Api.PickUp webPickup = new Web__Api.PickUp
                {
                    Id = pickup.Id,
                    CustomerId = pickup.CustomerId,
                    RiderId = pickup.RiderId,
                    AddressId = pickup.AddressId,
                    PickupDate = pickup.PickupDate.ToString("yyyy-MM-dd"),
                    PickupTime = pickup.PickupTime.ToString("hh\\:mm\\:ss"),
                    Status = pickup.Status.Trim(),
                    TotalWeight = pickup.TotalWeight.Trim(),
                    CreatedAt = pickup.CreatedAt.ToString("yyyy-MM-dd"),
                    UpdatedAt = pickup.UpdatedAt.ToString("yyyy-MM-dd")
                };
                webPickups.Add(webPickup);
            }

            return webPickups;

        }


        [HttpDelete]
        [Route("delete/{id}")]
        public async Task Delete(int id)
        {
            await _userService.Delete(id);
        }



    }
}
