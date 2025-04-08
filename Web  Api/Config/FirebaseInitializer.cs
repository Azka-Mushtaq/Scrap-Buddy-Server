using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using System.IO;

public static class FirebaseInitializer
{
    public static void Initialize()
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "Config", "scrap-uncle-firebase-adminsdk.json");

        if (FirebaseApp.DefaultInstance == null)
        {
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile(path),
            });
        }
    }
}
