using System;

namespace Frame
{
    /// <summary>
    /// 存储单个UI组件的绑定配置信息。
    /// 这个类定义了如何将一个ViewModel属性与场景中的一个UI组件关联起来。
    /// </summary>
    [Serializable]
    public class ComponentBindingInfo
    {
        /// <summary>
        /// 在ViewModel中对应的属性逻辑名称。
        /// 例如: "LoginButton", "PlayerNameText"。
        /// </summary>
        public string Name;

        /// <summary>
        /// 要绑定的UI组件的类型。
        /// </summary>
        public BoundComponentType Type;

        /// <summary>
        /// 该UI组件在View根节点下的相对路径。
        /// 例如: "Buttons/LoginBtn", "InfoPanel/AvatarImage"。
        /// </summary>
        public string Path;
    }
}
