using WorldOfZuul.Events;  
using System.Text.Json;

namespace WorldOfZuul.Factories
{
    public static class EventFactory
    {
        public static IEvent Create(EventData data)
        {
            Type? eventType = Type.GetType($"WorldOfZuul.Events.types.{data.Type}");

            if (eventType == null)
                throw new Exception($"Unknown event type: {data.Type}");

            var ctor = eventType.GetConstructors().First();

            var args = ctor.GetParameters()
                .Select(p =>
                {
                    if (p.Name == "id") return data.Id;

                    if (data.Params != null && data.Params.TryGetValue(p.Name!, out var json))
                        return JsonSerializer.Deserialize(json.GetRawText(), p.ParameterType);

                    throw new Exception($"Missing parameter '{p.Name}' for event {data.Id}");
                })
                .ToArray();

            return (IEvent)Activator.CreateInstance(eventType, args)!;
        }
    }
}