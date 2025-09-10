using System;
using Frame;

public interface IMessageSubscriber<T>  
{
    IDisposable Subscribe(MessageHandler<T> handler);
}