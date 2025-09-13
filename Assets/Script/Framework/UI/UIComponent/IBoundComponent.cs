namespace Frame
{
    /// <summary>
    /// 所有“绑定组件”必须实现的接口。
    /// 它定义了所有绑定组件的通用契约，使其可以被自动化工具统一处理。
    /// </summary>
    public interface IBoundComponent
    {
        /// <summary>
        /// 获取该组件在ViewModel中对应的属性名称。
        /// 这个名称由编辑器脚本在自动化绑定时设置。
        /// </summary>
        string PropertyName { get; }

        /// <summary>
        /// 执行绑定操作的核心方法。
        /// </summary>
        /// <param name="viewModel">要绑定的ViewModel实例。</param>
        void Bind(object viewModel);
    }
}
