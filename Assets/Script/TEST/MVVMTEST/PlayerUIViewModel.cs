using System;
using System.Collections.Generic;
using Frame;
public class PlayerUIViewModel
{
    public BindableProperty<int> Hp { get; } = new();
}