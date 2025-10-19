using System;
using System.Threading;
using UnityEngine;

namespace Frame
{
    /// <summary>
    /// 侵入式链表中的指针
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class MessageHandlerNode<T> : IDisposable
    {
        public MessageHandlerList<T> Parent = default!;
        public MessageHandlerNode<T> PreviousNode;
        public MessageHandlerNode<T> NextNode;
        public ulong Version;
        private bool disposed;
        public bool Disosed => disposed;
        public void Dispose()
        {
            if (disposed) return;
            if (Parent != null)
            {
                Parent.Remove(this);
                Volatile.Write(ref Parent!, null);
            }

            Volatile.Write(ref disposed, true);

            DisposeCore();
        }
        public virtual void DisposeCore()
        {
            Debug.Log($"取消订阅: {typeof(T)}");
        }
    }
}