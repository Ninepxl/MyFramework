using System;

namespace Frame
{
    public class ButtonComponent : UIBaseComponent
    {
        public Action OnClick;
        public ButtonComponent(Action onClick)
        {
            OnClick = onClick;
        }
    }
}