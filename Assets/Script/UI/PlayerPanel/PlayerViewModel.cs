using System;
using Frame;
using UnityEngine;
using UnityEngine.Events;

namespace UITEST
{
    /// <summary>
    /// PlayerViewModel 中只修改数据
    /// </summary>
    public class PalyerViewModel
    {
        // 有多个属性 
        // 使用自定义特性行吗
        public BindableProperty<string> Lv;
        public BindableProperty<UnityAction> onLevelUp;
        public BindableProperty<UnityAction> onLevelDown;
        public PalyerViewModel()
        {
            // 初始化属性
            Lv = new BindableProperty<string>("");
            onLevelUp = new BindableProperty<UnityAction>(() =>
            {
                Lv.Value = (int.Parse(Lv.Value) + 1).ToString();
            });
            onLevelDown = new BindableProperty<UnityAction>(() =>
            {
            });
        }

    }
}