using System;
using System.Collections.Generic;
using UnityEngine.Events;
namespace Frame
{
    public static class UIUtil
    {
        public static Dictionary<string, IBindableProperty> BindButton(Action handler)
        {
            return new Dictionary<string, IBindableProperty>
            {
                { "onClick", new BindableProperty<Action>(handler) },
                {"active", new BindableProperty<bool>(true)}
            };
        }
    }
}