using Microsoft.AspNetCore.SignalR;

namespace Web__Api
{
    public class PickupHub : Hub
    {
        // Method to notify clients about a new pickup
        public async Task SendPickupNotification(int customerId, string message)
        {
            await Clients.All.SendAsync("ReceivePickupNotification", customerId, message);
        }

        // Method to notify clients about a confirmed pickup
        public async Task ConfirmPickupNotification(int customerId, string pickupStatus)
        {
            await Clients.All.SendAsync("ReceiveConfirmation", customerId, pickupStatus);
        }
    }
}
