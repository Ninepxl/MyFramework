using System;
using System.Runtime.CompilerServices;

namespace Frame
{
    public static class MessageSubscriberExtensions
    {
        public static IDisposable Subscribe<T>(this IMessageSubscriber<T> subscriber, Action<T> handler)
        {
            return subscriber.Subscribe(new AnonymousMessageHandler<T>(handler));
        }
    }
}