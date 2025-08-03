using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;

namespace Frame
{
    /// <summary>
    /// 加载Texture2D、AudioClip、Sprite 等资源Handle
    /// </summary>
    /// <typeparam name="T">资源类型</typeparam>
    public class AssetHandle<T> : AssetHandleBase
    {
        private AsyncOperationHandle<T> m_handle;
        public T Result;
        public AssetHandle(AsyncOperationHandle<T> handle, object key)
        {
            CurType = AssetType.Common;
            m_handle = handle;
            Key = key;
            Success = false;
            IsDone = false;
        }

        public override float GetProgress()
        {
            return m_handle.PercentComplete;
        }
        public override void Release()
        {
            // 如果资源没有被释放
            if (m_handle.IsValid())
            {
                Addressables.Release(Result);
            }
        }
    }
}