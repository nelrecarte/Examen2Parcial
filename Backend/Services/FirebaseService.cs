using Google.Cloud.Firestore;

namespace Backend.Services;

public class FirebaseService
{
    private readonly FirestoreDb _firestoreDb;

    public FirebaseService(IConfiguration configuration)
    {
        var credentialsPath = Path.Combine(AppContext.BaseDirectory, "Config", "firebase-key.json");

        _firestoreDb = new FirestoreDbBuilder
        {
            ProjectId = configuration["Firebase:ProjectId"],
            CredentialsPath = credentialsPath
        }.Build();
    }

    public CollectionReference GetCollection(string collectionName)
    {
        return _firestoreDb.Collection(collectionName);
    }
}
