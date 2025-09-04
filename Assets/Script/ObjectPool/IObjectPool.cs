using System;

namespace Frame
{
    public interface IObjectPool<T> : IDisposable
    {
        T Rent();
        void Return(T o);
    }
}
