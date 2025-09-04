// 泛型单例
using System;
using System.Collections.Generic;
namespace Frame
{
    public class NorSingleton<T> where T : new()
    {
        private static readonly Lazy<T> _Instance = new Lazy<T>(() => new T());

        public static T Instance => _Instance.Value;
    }
}