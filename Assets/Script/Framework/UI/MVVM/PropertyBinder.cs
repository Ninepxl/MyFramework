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
        private readonly List<Action<TViewModel>> _bindingActions = new();
        private readonly List<IDisposable> _activeSubscriptions = new();

        /// <summary>
        /// 添加一条绑定规则。
        /// </summary>
        /// <param name="propertyName">ViewModel 中的公共属性名</param>
        /// <param name="valueChangedHandler">属性值变化时的回调</param>
        public void Add<TValue>(string propertyName, Action<TValue, TValue> valueChangedHandler)
        {
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
        public void Dispose()
        {
            Unbind();
            _bindingActions.Clear();
        }
    }
}