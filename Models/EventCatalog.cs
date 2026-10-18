namespace admEventEase.Models;

public static class EventCatalog
{
    private static readonly IReadOnlyList<EventItem> events = Array.AsReadOnly(new[]
    {
        new EventItem
        {
            Id = 1,
            Name = "Cumbre de Innovación",
            Date = new DateTime(2027, 3, 18, 9, 0, 0),
            Location = "Centro de Convenciones, Ciudad de México",
            Description = "Un encuentro para compartir ideas y tendencias que están transformando los negocios."
        },
        new EventItem
        {
            Id = 2,
            Name = "Gala de Primavera",
            Date = new DateTime(2027, 4, 24, 19, 0, 0),
            Location = "Jardín Botánico, Guadalajara",
            Description = "Una velada para celebrar, conectar y disfrutar de una experiencia especial."
        },
        new EventItem
        {
            Id = 3,
            Name = "Foro de Marketing Digital",
            Date = new DateTime(2027, 5, 12, 10, 0, 0),
            Location = "Auditorio Central, Monterrey",
            Description = "Ideas y conversaciones sobre estrategias y herramientas para el marketing digital."
        }
    });

    public static IReadOnlyList<EventItem> Events { get; } =
        Array.AsReadOnly(events.Where(eventItem => eventItem.IsValid).ToArray());

    public static EventItem? FindById(int id) =>
        Events.FirstOrDefault(eventItem => eventItem.Id == id);
}
