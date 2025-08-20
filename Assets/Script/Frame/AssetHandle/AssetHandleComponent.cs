using System;
using System.Collections.Generic;
using System.Security.Cryptography;
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
        /// <summary>
        /// 所有资源加载器的缓存
        /// </summary>
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
        }

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">z资源标签/param>
        /// <param name="callback">回调函数</param>
        /// <param name="cached">是否缓存这个资源加载器</param>
        /// <returns></returns>
        public AssetHandle<T> LoadAsset<T>(object key, Action<AssetHandle<T>> callback, bool cached = false)
        {
            if (cached && m_AssetHandleCache.ContainsKey(key))
            {
                AssetHandle<T> cacheHandle = (AssetHandle<T>)m_AssetHandleCache[key];
                callback?.Invoke(cacheHandle);
                return cacheHandle;
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
                }
                else
                {
                    assetHandle.Success = false;
                }
                assetHandle.IsDone = true;
                callback.Invoke(assetHandle);
                assetHandle.Release();
            };
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

        /// <summary>
        /// 释放某个handler
        /// </summary>
        /// <param name="ah"></param>
        /// <param name="cached"></param>
        public void Release(AssetHandleBase ah, bool cached = false)
        {
            if (cached && ah.CurType == 0 && m_AssetHandleCache.ContainsKey(ah.Key))
            {
                m_AssetHandleCache.Remove(ah.Key);
            }

            ah.Release();
        }

        /// <summary>
        /// 释放所有加载进内存的数据
        /// </summary>
        private void ClearAllCache()
        {
            foreach (var handle in m_AssetHandleCache)
            {
                handle.Value.Release();
            }
            m_AssetHandleCache.Clear();
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