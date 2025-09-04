using System.Collections.Generic;
using UnityEngine;

namespace Frame
{
    public class GameObjectPoolTool
    {
        private static readonly Dictionary<GameObject, Stack<GameObject>> m_Pools = new();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Init()
        {
            m_Pools.Clear();
        }

        public static GameObject Rent(GameObject go)
        {
        }

        public static void Return(GameObject o)
        {

        }

        // 清空所有的对象池
        public void Dispose()
        {
            foreach (var goStack in m_Pools.Values)
            {
                foreach (var go in goStack)
                {
                    Object.Destroy(go);
                }
            }
        }
        private static Stack<GameObject> GetOrCreatPool(GameObject go)
        {
            if (!m_Pools.TryGetValue(go, out var pool))
            {
                pool = new Stack<GameObject>();
                m_Pools.Add(go, pool);
            }
            return pool;
        }
    }
}