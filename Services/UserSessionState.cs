using admEventEase.Models;

namespace admEventEase.Services;

public sealed class UserSessionState
{
    public event Action? Changed;

    public EventRegistrationRecord? CurrentRegistration { get; private set; }

    public void SetRegistration(EventRegistrationRecord registration)
    {
        CurrentRegistration = registration;
        Changed?.Invoke();
    }

    public void NotifyChanged() => Changed?.Invoke();
}
