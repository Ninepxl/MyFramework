namespace Frame
{
    public interface IStateBase
    {
        IStateBase HandleInput();
        void OnEnter();
        void OnUpdate();
        void OnExit();
    }
}