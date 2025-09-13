using System;

namespace Frame
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TVlaue">绑定数据的类型</typeparam>
    public interface IBindableProperty<TVlaue>
    {
        Type PropertyType { get; }
    }

    public readonly struct ValueChangedEventArgs<TValue>
    {
        public readonly TValue OldValue { get; }
        public readonly TValue NewValue { get; }

        public ValueChangedEventArgs(TValue oldValue, TValue newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }

    public class BindableProperty<TValue> : IBindableProperty<TValue>, IDisposable
    {
        private TValue m_value;
        private readonly MessageBroker<ValueChangedEventArgs<TValue>> m_messageBroker = new();
        private bool m_disposed;
        public Type PropertyType => typeof(TValue);
        public TValue Value
        {
            get => m_value;
            set
            {
                SetValue(value);
            }
        }
        public BindableProperty(TValue value = default)
        {
            m_value = value;
            m_messageBroker = new MessageBroker<ValueChangedEventArgs<TValue>>();
        }
        private void SetValue(TValue newVal)
        {
            if (m_disposed) return;

            var oldValue = m_value;

            if (Equals(oldValue, newVal)) return;

            m_value = newVal;

            // 使用MessageBroker发布变化事件
            var eventArgs = new ValueChangedEventArgs<TValue>(oldValue, newVal);
            m_messageBroker.Publish(eventArgs);
        }
        /// <summary>
        /// 订阅值变化事件
        /// </summary>
        public IDisposable Subscribe(Action<TValue, TValue> onValueChanged)
        {
            if (m_disposed) return null;

            return m_messageBroker.Subscribe(args => onValueChanged(args.OldValue, args.NewValue));
        }

        /// <summary>
        /// 订阅值变化事件 - 提供更详细的事件参数
        /// </summary>
        public IDisposable Subscribe(Action<ValueChangedEventArgs<TValue>> onValueChanged)
        {
            if (m_disposed) return null;

            return m_messageBroker.Subscribe(onValueChanged);
        }

        /// <summary>
        /// 订阅值变化事件 - 支持立即触发初始值
        /// </summary>
        public IDisposable Subscribe(Action<TValue, TValue> onValueChanged, bool invokeImmediately)
        {
            var subscription = Subscribe(onValueChanged);

            if (invokeImmediately && !m_disposed)
            {
                // 传入默认值和初始值
                onValueChanged(default(TValue), m_value);
            }

            return subscription;
        }
        
        public void Dispose()
        {
            if (m_disposed) return;

            m_disposed= true;
            m_messageBroker?.Dispose();
            m_value = default;
        }
    }

}