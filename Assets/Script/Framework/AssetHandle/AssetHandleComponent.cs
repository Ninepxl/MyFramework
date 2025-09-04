using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Frame
{
    /// <summary>
    /// AssetHandleComponent 组件相当于AssetHandle的管理器，
    /// </summary>
    public class AssetHandleComponent : GameComponent
    {
        /// <summary>
        /// 所有资源加载器的缓存
        /// </summary>
        public bool isDebug = false;
        private Dictionary<object, AssetHandleBase> m_AssetHandleCache = new Dictionary<object, AssetHandleBase>();

        #region DEBUG
        // 监控不进行缓存的资源
        private Dictionary<object, AssetHandleBase> m_TempAssetHandleCache;
        // 监控实例化的GameObject
        private Dictionary<object, AssetGameObjectHandle> m_InstantiatedHandles;
        public int GetLoadedAssetCount() => m_AssetHandleCache.Count;
        public Dictionary<object, AssetHandleBase> GetAssetHandleCache() => m_AssetHandleCache;
        public int GetTempAssetHandleCount() => m_TempAssetHandleCache.Count;
        public Dictionary<object, AssetHandleBase> GetTempAssetHandleCache() => m_TempAssetHandleCache;

        public int GetInstantiatedHandleCount() => m_InstantiatedHandles.Count;
        public Dictionary<object, AssetGameObjectHandle> GetInstantiatedHandles() => m_InstantiatedHandles;
        #endregion

        protected override void Awake()
        {
            base.Awake();
            m_AssetHandleCache = new Dictionary<object, AssetHandleBase>();
            if (isDebug)
            {
                m_TempAssetHandleCache = new Dictionary<object, AssetHandleBase>();
                m_InstantiatedHandles = new Dictionary<object, AssetGameObjectHandle>();
            }
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            ClearAllCache();
            Addressables.InternalIdTransformFunc = null;
        }

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">资源标签</param>
        /// <param name="callback">回调函数</param>
        /// <param name="cached">是否缓存这个资源加载器</param>
        /// <returns></returns>
        public AssetHandle<T> LoadAsset<T>(object key, Action<AssetHandle<T>> callback, bool cached = false)
        {
            if (cached && m_AssetHandleCache.ContainsKey(key))
            {
                if (m_AssetHandleCache[key] is AssetHandle<T> cacheHandle)
                {
                    callback?.Invoke(cacheHandle);
                    return cacheHandle;
                }
                Debug.LogWarning($"资源缓存类型不匹配，将重新加载. Key: {key}, CachedType: {m_AssetHandleCache[key].GetType()}, RequestType: {typeof(AssetHandle<T>)}");
                m_AssetHandleCache.Remove(key);
            }
            AsyncOperationHandle<T> h = Addressables.LoadAssetAsync<T>(key);
            AssetHandle<T> assetHandle = new AssetHandle<T>(h, key);
            h.Completed += handle =>
            {
                if (h.Status == AsyncOperationStatus.Succeeded)
                {
                    assetHandle.Success = true;
                    assetHandle.Result = handle.Result;
                    if (cached)
                    {
                        m_AssetHandleCache[key] = assetHandle;
                    }
                    else
                    {
                        if (isDebug && m_TempAssetHandleCache != null)
                            m_TempAssetHandleCache[key] = assetHandle;
                    }
                }
                else
                {
                    assetHandle.Success = false;
                    assetHandle.Release();
                }
                assetHandle.IsDone = true;
                callback?.Invoke(assetHandle);
            };
            return assetHandle;
        }
        public async UniTask<AssetHandle<T>> LoadAssetAsync<T>(object key, bool cached = false)
        {
            if (cached && m_AssetHandleCache.ContainsKey(key))
            {
                if (m_AssetHandleCache[key] is AssetHandle<T> cacheHandle)
                {
                    return cacheHandle;
                }

                Debug.LogWarning($"资源缓存类型不匹配，将重新加载. Key: {key}, CachedType: {m_AssetHandleCache[key].GetType()}, RequestType: {typeof(AssetHandle<T>)}");
                m_AssetHandleCache.Remove(key);
            }
            AsyncOperationHandle<T> h = Addressables.LoadAssetAsync<T>(key);
            AssetHandle<T> assetHandle = new AssetHandle<T>(h, key);
            await h.Task;
            if (h.Status == AsyncOperationStatus.Succeeded)
            {
                assetHandle.Success = true;
                assetHandle.Result = h.Result;
                if (cached)
                {
                    m_AssetHandleCache[key] = assetHandle;
                }
                else
                {
                    if (isDebug && m_TempAssetHandleCache != null)
                        m_TempAssetHandleCache[key] = assetHandle;
                }
            }
            else
            {
                assetHandle.Success = false;
                // 异步加载失败时，不需要手动释放，Addressables内部会处理
                // assetHandle.Release();
            }
            return assetHandle;
        }
        /// <summary>
        /// 实例化GameObject
        /// </summary>
        /// <param name="key"></param>
        /// <param name="parent"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public AssetGameObjectHandle InstantiateGo(object key, Transform parent, Action<AssetGameObjectHandle> callback)
        {
            // 异步加载资源
            var h = Addressables.InstantiateAsync(key, parent);
            AssetGameObjectHandle assetHandle = new AssetGameObjectHandle(h, key);
            // Addressables 加载资源完成后触发
            h.Completed += handle =>
            {
                // 如果加载成功
                if (h.Status == AsyncOperationStatus.Succeeded)
                {
                    assetHandle.Success = true;
                    assetHandle.Result = handle.Result;
                    if (isDebug && m_InstantiatedHandles != null)
                        m_InstantiatedHandles[key] = assetHandle;
                }
                else
                {
                    assetHandle.Success = false;
                }
                assetHandle.IsDone = true;
                callback?.Invoke(assetHandle);
            };
            return assetHandle;
        }

        public async UniTask<AssetGameObjectHandle> InstantiateGoAsync(object key, Transform parent)
        {
            var h = Addressables.InstantiateAsync(key, parent);
            AssetGameObjectHandle assetHandle = new AssetGameObjectHandle(h, key);
            await h.Task;
            if (h.Status == AsyncOperationStatus.Succeeded)
            {
                assetHandle.Success = true;
                assetHandle.Result = h.Result;
                if (isDebug && m_InstantiatedHandles != null)
                    m_InstantiatedHandles[key] = assetHandle;
            }
            else
            {
                assetHandle.Success = false;
            }
            assetHandle.IsDone = true;
            return assetHandle;
        }

        /// <summary>
        /// 释放某个handler
        /// </summary>
        /// <param name="ah"></param>
        public void Release(AssetHandleBase ah)
        {
            if (ah == null) return;

            // 如果有缓存，还要删除缓存的数据
            if (m_AssetHandleCache.ContainsKey(ah.Key))
            {
                m_AssetHandleCache.Remove(ah.Key);
            }
            if (isDebug)
            {
                m_TempAssetHandleCache?.Remove(ah.Key);
                m_InstantiatedHandles?.Remove(ah.Key);
            }

            ah.Release();
        }

        /// <summary>
        /// 释放所有加载进内存的数据
        /// </summary>
        public void ClearAllCache()
        {
            foreach (var handle in m_AssetHandleCache)
            {
                handle.Value.Release();
            }
            m_AssetHandleCache.Clear();

            if (isDebug)
            {
                if (m_TempAssetHandleCache != null)
                {
                    foreach (var handle in m_TempAssetHandleCache) handle.Value.Release();
                    m_TempAssetHandleCache.Clear();
                }

                if (m_InstantiatedHandles != null)
                {
                    foreach (var handle in m_InstantiatedHandles) handle.Value.Release();
                    m_InstantiatedHandles.Clear();
                }
            }
        }

        /// <summary>
        /// AssetHandleComponent 初始化
        /// </summary>
        /// <returns></returns>
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

            Addressables.Release(initializeHandle);
        }
    }
}