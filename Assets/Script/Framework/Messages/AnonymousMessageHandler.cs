using System;

namespace Frame
{
    public sealed class AnonymousMessageHandler<T> : MessageHandler<T>
    {
        Action<T> handler;
        public AnonymousMessageHandler(Action<T> handler)
        {
            this.handler = handler;
        }
        protected override void HandleCore(T message)
        {
            handler(message);
        }
    }
}