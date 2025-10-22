using System;
using System.Collections.Generic;

namespace HachiFramework
{
    public class MessageComponent : GameComponent
    {
        private Dictionary<Type, object> m_messageBokers = new Dictionary<Type, object>();
        public MessageBroker<T> GetOrCreateBoker<T>()
        {
            if (!m_messageBokers.TryGetValue(typeof(T), out var boker))
            {
                boker = MessageBroker<T>.Default;
                m_messageBokers.Add(typeof(T), boker);
            }
            return (MessageBroker<T>)boker;
        }
        public void Publish<T>(T message)
        {
            GetOrCreateBoker<T>().Publish(message);
        }
        public IDisposable Subscribe<T>(MessageHandler<T> handler)
        {
            return GetOrCreateBoker<T>().Subscribe(handler);
        }
        public IDisposable Subscribe<T>(Action<T> handler)
        {
            return GetOrCreateBoker<T>().Subscribe(handler);
        }
        protected override void Awake()
        {
            base.Awake();
        }
        protected override void OnDestroy()
        {
            foreach (var boker in m_messageBokers.Values)
            {
                if (boker is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }
            base.OnDestroy();
        }
    }
}