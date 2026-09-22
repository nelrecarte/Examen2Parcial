using Backend.DTOs;
using Backend.Models;
using Google.Cloud.Firestore;

namespace Backend.Services;

public class TaskService
{
    private readonly FirebaseService _firebaseService;

    public TaskService(FirebaseService firebaseService)
    {
        _firebaseService = firebaseService;
    }

    public async Task<TaskItem> Create(TaskDto dto, string userId)
    {
        if (string.IsNullOrWhiteSpace(dto.Title))
            throw new Exception("El título es obligatorio.");

        if (dto.PriorityLevel < 1 || dto.PriorityLevel > 3)
            throw new Exception("La prioridad debe estar entre 1 y 3.");

        var task = new TaskItem
        {
            Id = Guid.NewGuid().ToString(),
            Title = dto.Title,
            PriorityLevel = dto.PriorityLevel,
            Notes = dto.Notes ?? string.Empty,
            UserId = userId,
            CreatedAt = DateTime.UtcNow
        };

        await _firebaseService.GetCollection("tasks")
            .Document(task.Id)
            .SetAsync(new Dictionary<string, object>
            {
                { "Id", task.Id },
                { "Title", task.Title },
                { "PriorityLevel", task.PriorityLevel },
                { "Notes", task.Notes ?? string.Empty },
                { "UserId", task.UserId },
                { "CreatedAt", task.CreatedAt }
            });

        return task;
    }

    public async Task<List<TaskItem>> GetTasksByUser(string userId)
    {
        var snapshot = await _firebaseService.GetCollection("tasks")
            .WhereEqualTo("UserId", userId)
            .GetSnapshotAsync();

        var tasks = new List<TaskItem>();

        foreach (var doc in snapshot.Documents)
        {
            var data = doc.ToDictionary();

            tasks.Add(new TaskItem
            {
                Id = data["Id"].ToString()!,
                Title = data["Title"].ToString()!,
                PriorityLevel = Convert.ToInt32(data["PriorityLevel"]),
                Notes = data.ContainsKey("Notes") ? data["Notes"]?.ToString() : null,
                UserId = data["UserId"].ToString()!,
                CreatedAt = ((Timestamp)data["CreatedAt"]).ToDateTime()
            });
        }

        return tasks.OrderByDescending(t => t.CreatedAt).ToList();
    }

    public async Task<bool> DeleteTaskAsync(string id, string userId)
    {
        var docRef = _firebaseService.GetCollection("tasks").Document(id);
        var snapshot = await docRef.GetSnapshotAsync();

        if (!snapshot.Exists)
            return false;

        var data = snapshot.ToDictionary();

        if (data["UserId"].ToString() != userId)
            return false;

        await docRef.DeleteAsync();
        return true;
    }
}
