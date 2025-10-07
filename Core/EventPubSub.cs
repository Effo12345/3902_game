namespace ECS.Core;

// ROS-inspired pub-sub system for passing data between systems
// TODO: Consider doing actual topics rather than purely by event type
public class EventPubSub
{
    private Dictionary<Type, List<Action<IEvent>>> Subscriptions = [];

    public bool Publish<T>(T eventInput) where T : IEvent
    {
        var type = typeof(T);
        if (!Subscriptions.TryGetValue(type, out var subscriptionList))
            return false;

        foreach (var subscription in subscriptionList)
        {
            subscription(eventInput);
        }

        return true;
    }

    public void CreateSubscription<T>(Action<IEvent> eventHandler) where T : IEvent
    {
        var type = typeof(T);
        if (!Subscriptions.ContainsKey(type))
            Subscriptions[type] = new List<Action<IEvent>>();

        Subscriptions[type].Add(eventHandler);
    }
}