using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Frame
{
    public class AssetGameObjectHandle : AssetHandleBase
    {
        private AsyncOperationHandle<GameObject> m_handle;
        public GameObject Result;
        public AssetGameObjectHandle(AsyncOperationHandle<GameObject> handle, object key)
        {
            CurType = AssetType.GameObject;
            m_handle = handle;
            Key = key;
            Success = false;
            IsDone = false;
        }

        /// <summary>
        /// 得到资源加载的进度
        /// </summary>
        /// <returns></returns>
        public override float GetProgress()
        {
            return m_handle.PercentComplete;
        }
        public override void Release()
        {
            // 如果资源没有被释放
            if (m_handle.IsValid())
            {
                Addressables.ReleaseInstance(m_handle);
            }
        }
    }
}
