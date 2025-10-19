namespace Frame
{
    public interface IMessagePublisher<T>
    {
        void Publish(T message);
    }
}