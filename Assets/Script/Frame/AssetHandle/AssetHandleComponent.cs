using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace Frame
{
    /// <summary>
    /// AssetHandleComponent 组件相当于AssetHandle的管理器，
    /// </summary>
    public class AssetHandleComponent : GameComponent
    {
        private Dictionary<object, AssetHandleBase> m_AssetHandleCache = new Dictionary<object, AssetHandleBase>();
        protected override void Awake()
        {
            base.Awake();
            m_AssetHandleCache.Clear();
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            ClearAllCache();
            Addressables.InternalIdTransformFunc = null;
            Addressables.LoadAssetAsync<GameObject>("");
        }
        private void ClearAllCache()
        {
            foreach (var handle in m_AssetHandleCache)
            {
                handle.Value.Release();
            }
            m_AssetHandleCache.Clear();
        }
        public async UniTask StartupCoroutine()
        {
            Debug.Log("Addressables 初始化中...");
            AsyncOperationHandle initializeHandle = Addressables.InitializeAsync(false);
            await initializeHandle;
            if (initializeHandle.Status == AsyncOperationStatus.Succeeded)
            {
                Debug.Log("Addressables 初始化 成功");
            }
            else
            {
                Debug.LogError($"Addressables 初始化 失败 {initializeHandle.OperationException}");
            }

            // 在Demo开发中，初始化句柄通常可以立即释放
            Addressables.Release(initializeHandle);
        }
    }
}