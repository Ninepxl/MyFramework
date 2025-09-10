using System;
using System.Threading;

namespace Frame
{
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
        { }
    }
}