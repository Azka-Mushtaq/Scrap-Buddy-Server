using FirebaseAdmin.Messaging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application
{
    public class FirebaseNotificationService
    {
        // Method to send notification to a list of FCM tokens
        public async Task SendNotificationToTokensAsync(List<string> tokens, string title, string body)
        {
            foreach (var token in tokens)
            {
                var message = new Message()
                {
                    Token = token,
                    Notification = new Notification
                    {
                        Title = title,
                        Body = body,
                    }
                };

                try
                {
                    string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
                    Console.WriteLine($"Notification sent to {token}: {response}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to send to {token}: {ex.Message}");
                }
            }
        }
    }
}
