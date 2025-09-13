using System.Collections.Generic;
using UnityEngine;

namespace Frame
{
    /// <summary>
    /// 所有UI视图脚本的抽象基类。
    /// 它提供了一个核心的虚拟属性 ComponentBindings，允许子类在代码中声明其UI结构和绑定信息。
    /// </summary>
    public abstract class UIBaseView : MonoBehaviour
    {
        /// <summary>
        /// 定义此视图所管理的所有UI组件的绑定信息列表。
        /// 开发者需要在一个具体的视图脚本中重写此属性，并提供所有需要绑定的组件的配置。
        /// 编辑器脚本将读取此列表来自动完成绑定过程。
        /// </summary>
        public virtual List<ComponentBindingInfo> ComponentBindings => new List<ComponentBindingInfo>();
    }
}
