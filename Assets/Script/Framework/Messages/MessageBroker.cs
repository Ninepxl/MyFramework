using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;
using Unity.Collections.LowLevel.Unsafe;
namespace HachiFramework
{
    [Preserve]
    public class MessageBroker<T> : IMessageSubscriber<T>, IMessagePublisher<T>, IDisposable
    {
        public static readonly MessageBroker<T> Default = new();
        object gate;
        readonly MessageHandlerList<T> syncHandlers; // 所有同步action
        bool isDisposed;
        public bool IsDisposed => isDisposed;
        public MessageBroker()
        {
            gate = new();
            syncHandlers = new(gate);
        }
        public void Publish(T message)
        {
            {
                var node = syncHandlers.Root;
                var version = syncHandlers.GetVersion();

                while (node != null)
                {
                    // 如果有新的节点在Publish时加入就先不执行
                    if (node.Version > version) break;
                    UnsafeUtility.As<MessageHandlerNode<T>, MessageHandler<T>>(ref node)!.Handle(message);
                    node = node.NextNode;
                }
            }
        }

        public IDisposable Subscribe(MessageHandler<T> handler)
        {
            return SubscribeCore(handler);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        MessageHandler<T> SubscribeCore(MessageHandler<T> handler)
        {
            syncHandlers.Add(handler);
            return handler;
        }
        public void Dispose()
        {
            lock (gate)
            {
                isDisposed = true;
            }
            syncHandlers.Dispose();
        }
    }
}