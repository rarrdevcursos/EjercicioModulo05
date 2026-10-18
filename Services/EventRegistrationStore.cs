using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using admEventEase.Models;

namespace admEventEase.Services;

public enum RegistrationResult
{
    Created,
    AlreadyRegistered,
    Invalid
}

public sealed class EventRegistrationStore
{
    private readonly ConcurrentDictionary<string, EventRegistrationRecord> registrations = new();

    public RegistrationResult TryRegister(
        int eventId,
        EventRegistrationModel model,
        out EventRegistrationRecord? registration)
    {
        registration = null;
        var eventItem = EventCatalog.FindById(eventId);
        if (eventItem is null || model is null || !IsValid(model))
        {
            return RegistrationResult.Invalid;
        }

        var name = model.Name?.Trim();
        var emailForDisplay = model.Email?.Trim();
        if (string.IsNullOrWhiteSpace(name)
            || name.Length < 2
            || name.Length > 80
            || string.IsNullOrWhiteSpace(emailForDisplay)
            || emailForDisplay.Length > 254)
        {
            return RegistrationResult.Invalid;
        }

        var normalizedEmail = emailForDisplay.ToLowerInvariant();
        var key = GetKey(eventId, normalizedEmail);
        var candidate = new EventRegistrationRecord
        {
            EventId = eventItem.Id,
            EventName = eventItem.Name,
            Name = name,
            Email = emailForDisplay,
            RegisteredAt = DateTime.UtcNow
        };

        if (registrations.TryAdd(key, candidate))
        {
            registration = candidate;
            return RegistrationResult.Created;
        }

        registration = registrations[key];
        return RegistrationResult.AlreadyRegistered;
    }

    public int GetAttendanceCount(int eventId) =>
        registrations.Values.Count(registration => registration.EventId == eventId && registration.HasAttended);

    public bool HasAttended(int eventId, string email) =>
        !string.IsNullOrWhiteSpace(email)
        && registrations.TryGetValue(GetKey(eventId, email.Trim().ToLowerInvariant()), out var registration)
        && registration.HasAttended;

    public bool MarkAttendance(int eventId, string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return false;
        }

        return registrations.TryGetValue(GetKey(eventId, email.Trim().ToLowerInvariant()), out var registration)
            && SetAttendance(registration);
    }

    private static bool IsValid(EventRegistrationModel model)
    {
        var validationResults = new List<ValidationResult>();
        return Validator.TryValidateObject(
            model,
            new ValidationContext(model),
            validationResults,
            validateAllProperties: true);
    }

    private static string GetKey(int eventId, string normalizedEmail) =>
        $"{eventId}:{normalizedEmail}";

    private static bool SetAttendance(EventRegistrationRecord registration)
    {
        lock (registration)
        {
            if (registration.HasAttended)
            {
                return false;
            }

            registration.HasAttended = true;
            return true;
        }
    }
}
