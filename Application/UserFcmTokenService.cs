using System;
using System.Threading.Tasks;
using Domain;  // Assuming FcmToken is defined in Domain
using Infrastructure;  // Assuming GenericRepository is in Infrastructure

namespace Application
{
    public class UserFcmTokenService
    {
        private readonly GenericRepository<UserFcmToken> _fcmTokenRepository;

        public UserFcmTokenService()
        {
            _fcmTokenRepository = new GenericRepository<UserFcmToken>();
        }

        public async Task<List<string>> GetTokensByTopic(string topic)
        {
            try
            {
                var tokens = await _fcmTokenRepository.GetAll(
                    columnNames: "FcmToken",
                    tableName: "UserFcmToken",
                    compParameter: "Topic",
                    compValue: topic
                );

                return tokens.Select(t => t.FcmToken).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching tokens: {ex.Message}");
                return new List<string>();
            }
        }

        // Add a new FcmToken
        public async Task<int> AddFcmToken(UserFcmToken fcmToken)
        {
            try
            {
                // Use GenericRepository to add the FcmToken to the database
                return await _fcmTokenRepository.Add(fcmToken);
            }
            catch (Exception ex)
            {
                // Log the exception (optional, depends on your logging strategy)
                throw new Exception("Error adding FcmToken", ex);
            }
        }

        // Get FcmToken by a specific property, for example, UserId
        public async Task<UserFcmToken> GetFcmTokenByUserId(string userId)
        {
            try
            {
                // Retrieve the FcmToken using the Find method with the UserId
                return await _fcmTokenRepository.Find("Token", "UserId", userId);
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                throw new Exception("Error retrieving FcmToken by UserId", ex);
            }
        }

        // Update an existing FcmToken
        public async Task UpdateFcmToken(UserFcmToken fcmToken)
        {
            try
            {
                // Use GenericRepository to update the FcmToken
                await _fcmTokenRepository.Update(fcmToken);
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                throw new Exception("Error updating FcmToken", ex);
            }
        }

        // Delete FcmToken by Id
        public async Task DeleteFcmTokenById(int id)
        {
            try
            {
                // Use GenericRepository to delete the FcmToken by Id
                await _fcmTokenRepository.DeleteById(id);
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                throw new Exception("Error deleting FcmToken by Id", ex);
            }
        }
    }
}
