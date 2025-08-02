using System.Collections;
using System.Collections.Generic;
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

        public override float GetProgress()
        {
            return m_handle.PercentComplete;
        }
        public override void Release()
        {
            // 如果资源没有被释放
            if (m_handle.IsValid())
            {
                Addressables.ReleaseInstance(Result);
                Addressables.Release(Result);
            }
        }
    }
}
