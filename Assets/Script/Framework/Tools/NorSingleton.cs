// 泛型单例
using System;
namespace HachiFramework
{
    public class NorSingleton<T> where T : new()
    {
        private static readonly Lazy<T> _Instance = new Lazy<T>(() => new T());

        public static T Instance => _Instance.Value;
    }
}