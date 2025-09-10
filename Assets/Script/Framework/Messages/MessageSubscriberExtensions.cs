using System;
using System.Runtime.CompilerServices;

public static class MessageSubscriberExtensions
{
    public static IDisposable Subscribe<T>(this IMessageSubscriber<T> subscriber, Action<T> handler)
    {
        return null;
    }
}