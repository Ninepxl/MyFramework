
using System;
using System.Collections.Generic;

namespace HachiFramework
{
    public class PropertyBinder : IDisposable
    {
        public List<IDisposable> _unBinds = new();
        public void Add<T>(BindableProperty<T> bindableProperty, Action<T, T> handler, bool inovekNow = false)
        {
            IDisposable disposable = bindableProperty.Subscribe(handler, inovekNow);
            _unBinds.Add(disposable);
        }

        public void BindButton()
        { 

        }
        public void Dispose()
        {
            foreach (var unBind in _unBinds)
            {
                unBind.Dispose();
            }
            _unBinds.Clear();
        }
    }
}