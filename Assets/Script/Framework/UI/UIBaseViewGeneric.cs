using UnityEngine;

namespace Frame
{
    /// <summary>
    /// 带有泛型的UIBaseView，这是框架的运行时核心。
    /// 它负责在运行时自动实例化ViewModel并触发所有绑定组件的绑定过程。
    /// </summary>
    /// <typeparam name="TViewModel">此View绑定的ViewModel类型。</typeparam>
    public abstract class UIBaseView<TViewModel> : UIBaseView where TViewModel : class, new()
    {
        /// <summary>
        /// 此View绑定的ViewModel实例。
        /// </summary>
        public TViewModel ViewModel { get; private set; }

        protected virtual void Awake()
        {
            // 1. 自动创建ViewModel实例
            ViewModel = new TViewModel();
            
            // 2. 触发绑定
            BindViewModel();
        }

        /// <summary>
        /// 查找所有子物体上的绑定组件并执行绑定。
        /// </summary>
        private void BindViewModel()
        {
            // 获取所有实现了IBoundComponent接口的子组件（包括非激活的）
            var componentsToBind = GetComponentsInChildren<IBoundComponent>(true);
            foreach (var component in componentsToBind)
            {
                // 调用每个组件的Bind方法，将ViewModel实例传递给它们
                component.Bind(ViewModel);
            }
        }
    }
}
