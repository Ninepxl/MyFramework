// C#
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Frame
{
    /// <summary>
    /// 一个简洁的属性绑定器，关联一个特定的 ViewModel 类型。
    /// </summary>
    /// <typeparam name="TViewModel">ViewModel 的具体类型</typeparam>
    public class PropertyBinder<TViewModel> : IDisposable where TViewModel : class
    {
        // 保存“如何进行绑定”的操作列表
        private readonly List<Action<TViewModel>> _bindingActions = new();
        
        // 保存当前激活的订阅，用于解绑
        private readonly List<IDisposable> _activeSubscriptions = new();

        /// <summary>
        /// 添加一条绑定规则。
        /// </summary>
        /// <param name="propertyName">ViewModel 中的公共属性名</param>
        /// <param name="valueChangedHandler">属性值变化时的回调</param>
        public void Add<TValue>(string propertyName, Action<TValue, TValue> valueChangedHandler)
        {
            // 关键改动1：使用 GetProperty 来查找公共属性，而不是字段。
            var propertyInfo = typeof(TViewModel).GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public);

            if (propertyInfo == null)
            {
                throw new ArgumentException($"在 ViewModel '{typeof(TViewModel).Name}' 中找不到名为 '{propertyName}' 的公共属性。");
            }

            _bindingActions.Add(viewModel =>
            {
                var value = propertyInfo.GetValue(viewModel);
                if (value is BindableProperty<TValue> property)
                {
                    // 关键改动2：使用 Subscribe(..., true) 来立即触发一次回调，以设置UI初始状态。
                    var subscription = property.Subscribe(valueChangedHandler, true);
                    _activeSubscriptions.Add(subscription);
                }
                else
                {
                    throw new InvalidCastException($"属性 '{propertyName}' 的类型不是 BindableProperty<{typeof(TValue).Name}>。");
                }
            });
        }

        /// <summary>
        /// 将所有规则绑定到 ViewModel 实例上。
        /// </summary>
        public void Bind(TViewModel viewModel)
        {
            // 关键改动3：每次绑定前，先清理掉旧的订阅，防止内存泄漏和重复绑定。
            Unbind();

            if (viewModel == null) return;

            // 执行所有已添加的绑定操作
            foreach (var action in _bindingActions)
            {
                action(viewModel);
            }
        }

        /// <summary>
        /// 解除所有当前激活的绑定。
        /// </summary>
        public void Unbind()
        {
            foreach (var subscription in _activeSubscriptions)
            {
                subscription?.Dispose();
            }
            _activeSubscriptions.Clear();
        }

        /// <summary>
        /// 彻底清理 Binder，释放所有资源。
        /// </summary>
        public void Dispose()
        {
            Unbind();
            _bindingActions.Clear();
        }
    }
}