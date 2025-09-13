namespace Frame
{
    public class TextComponent : UIBaseComponent
    {
        public BindableProperty<string> Text { get; } = new("");
        public TextComponent() { }
    }
}