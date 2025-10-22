using System.Runtime.CompilerServices;
using UnityEngine;

namespace HachiFramework
{
    /// <summary>
    /// 消息节点
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class MessageHandler<T> : MessageHandlerNode<T>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Handle(T message)
        {
            HandleCore(message);
        }

        protected virtual void HandleCore(T message)
        {
        }
    }
}