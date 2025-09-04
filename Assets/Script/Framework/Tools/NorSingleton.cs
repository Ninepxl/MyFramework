// 泛型单例
using System;
using System.Collections.Generic;
namespace Frame
{
    public class NorSingleton<T> where T : new()
    {
        private NorSingleton() { }
        private static T _Instance;
        public static T Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new T();
                }
                return _Instance;
            }
        }
    }
}