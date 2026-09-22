using Google.Cloud.Firestore;

namespace Backend.Services;

public class FirebaseService
{
    private readonly FirestoreDb _firestoreDb;

    public FirebaseService(IConfiguration configuration)
    {
        var projectId = configuration["Firebase:ProjectId"];
        var credentialsPath = configuration["Firebase:CredentialsPath"];

        _firestoreDb = new FirestoreDbBuilder
        {
            ProjectId = projectId,
            CredentialsPath = credentialsPath
        }.Build();
    }

    public FirestoreDb Db => _firestoreDb;

    public CollectionReference GetCollection(string collectionName)
    {
        return _firestoreDb.Collection(collectionName);
    }
}
