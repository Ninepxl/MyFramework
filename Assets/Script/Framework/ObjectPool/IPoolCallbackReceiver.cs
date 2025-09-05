namespace Frame
{
    public interface IPoolCallbackReceiver
    {
        void OnRent();
        void OnReturn();
    }
}