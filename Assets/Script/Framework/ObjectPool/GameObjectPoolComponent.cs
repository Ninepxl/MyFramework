using System;
using UnityEngine;

namespace Frame
{
    public class GameObjectPoolComponent : GameComponent
    {
        // 被观察 可以通知 观察者 
        private readonly GameObjectPoolTool m_Pool = new();
        
        protected override void Awake()
        {
            base.Awake();
            m_Pool.Init();
        }
        protected override void OnDestroy()
        {
            m_Pool.Dispose();
            base.OnDestroy();
        }
        public GameObject Rent(GameObject go) => m_Pool.Rent(go);

        public GameObject Rent(GameObject original, Transform parent) => m_Pool.Rent(original, parent);

        public GameObject Rent(GameObject original, Vector3 position, Quaternion rotation) => m_Pool.Rent(original, position, rotation);

        public GameObject Rent(GameObject original, Vector3 position, Quaternion rotation, Transform parent) => m_Pool.Rent(original, position, rotation, parent);

        public TComponent Rent<TComponent>(TComponent original) where TComponent : Component => m_Pool.Rent(original);

        public TComponent Rent<TComponent>(TComponent original, Vector3 position, Quaternion rotation, Transform parent) where TComponent : Component => m_Pool.Rent(original, position, rotation, parent);

        public TComponent Rent<TComponent>(TComponent original, Vector3 position, Quaternion rotation) where TComponent : Component => m_Pool.Rent(original, position, rotation);

        public TComponent Rent<TComponent>(TComponent original, Transform parent) where TComponent : Component => m_Pool.Rent(original, parent);

        public void Return(GameObject instance) => m_Pool.Return(instance);

        public void Prewarm(GameObject original, int count) => m_Pool.Prewarm(original, count);
    }
}