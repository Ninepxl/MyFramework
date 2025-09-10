using System;

public interface IMessageSubscriber<T>  
{
    IDisposable Subscribe(Action<T> handler);
}