namespace admEventEase.Models;

public sealed class EventRegistrationRecord
{
    public int EventId { get; init; }

    public string EventName { get; init; } = string.Empty;

    public string Name { get; init; } = string.Empty;

    public string Email { get; init; } = string.Empty;

    public DateTime RegisteredAt { get; init; }

    public bool HasAttended { get; set; }
}
