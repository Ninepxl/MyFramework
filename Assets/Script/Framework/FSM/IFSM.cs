namespace Frame
{
    public interface IFSM
    {
        IStateBase curState { get; set; }
        void HandleInput();
    }
}