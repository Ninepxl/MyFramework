namespace Frame
{
    public class UIBaseComponent
    {
        public BindableProperty<bool> IsActive { get; } = new(true);
    }
}