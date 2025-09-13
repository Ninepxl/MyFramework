using System.Collections.Generic;
using Frame;
using Game.ViewModels;

namespace Game.Views
{
    /// <summary>
    /// View层：具体的UI视图脚本。
    /// 它只负责一件事：在代码中声明UI的结构。
    /// </summary>
    public class PlayerUIView : UIBaseView<PlayerViewModel>
    {
        /// <summary>
        /// 重写此属性，在代码中清晰地定义所有需要绑定的UI组件。
        /// </summary>
        public override List<ComponentBindingInfo> ComponentBindings => new List<ComponentBindingInfo>
        {
            // 逻辑名 "HpText" 对应 ViewModel中的 "HpText" 属性
            new ComponentBindingInfo { Name = "HpText",           Type = BoundComponentType.Text,   Path = "HP" },
            
            // 逻辑名 "AddHpButton" 对应 ViewModel中的 "AddHpButton" 属性
            new ComponentBindingInfo { Name = "AddHpButton",      Type = BoundComponentType.Button, Path = "AddBtn" },
            
            // 逻辑名 "SubtractHpButton" 对应 ViewModel中的 "SubtractHpButton" 属性
            new ComponentBindingInfo { Name = "SubtractHpButton", Type = BoundComponentType.Button, Path = "SubBtn" }
        };
    }
}
