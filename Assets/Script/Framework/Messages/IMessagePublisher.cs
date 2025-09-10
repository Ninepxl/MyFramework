using System.Threading;
using Cysharp.Threading.Tasks;

namespace Frame
{
    public interface IMessagePublisher<T>
    {
        void Publish(T message);
    }
}