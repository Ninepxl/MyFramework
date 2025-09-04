using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST : MonoBehaviour
{
    private Frame.AssetHandleComponent assetHandleComponent;
    private bool isInitAsset = false;
    private List<Frame.AssetGameObjectHandle> cubeHandles = new List<Frame.AssetGameObjectHandle>();
    private void Start()
    {
        assetHandleComponent = Frame.GameEntry.GetComponent<Frame.AssetHandleComponent>();
        if (assetHandleComponent == null)
        {
            Debug.Log("资源加载器加载失败");
        }
        else
        {
            Debug.Log("资源加载器加载成功");
            InitAsset();
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isInitAsset)
            {
                Debug.Log("资源没有加载完成");
            }
            else
            {
                LoadCube();
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ReleaseOneCube();
        }
    }
    private async void InitAsset()
    {
        await assetHandleComponent.StartupCoroutine();
        isInitAsset = true;
    }
    
    /// <summary>
    /// 正确的实例化GameObject的方式
    /// </summary>
    private void LoadCube()
    {
        // 使用  InstantiateGo 来创建和追踪GameObject实例
        // 注意：InstantiateGo/InstantiateGoAsync 创建的句柄通常不应该被“缓存”以供复用，
        // 因为每个句柄对应一个唯一的场景实例。这里的缓存是指 AssetHandleComponent内部的 m_AssetHandleCache。
        assetHandleComponent.InstantiateGo("MyCube", null, handle =>
        {
            if (handle.Success)
            {
                Debug.Log("Cube 实例化成功!");
                handle.Result.transform.position = new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));
                cubeHandles.Add(handle); // 保存句柄以便后续释放
            }
            else
            {
                Debug.LogError("Cube 实例化失败。");
            }
        });
    }

    private void ReleaseOneCube()
    {
        if (cubeHandles.Count > 0)
        {
            var handle = cubeHandles[0];
            assetHandleComponent.Release(handle); // 这会调用 Addressables.ReleaseInstance 并销毁场景中的对象
            cubeHandles.RemoveAt(0);
            Debug.Log("一个Cube已被释放。");
        }
        else
        {
            Debug.Log("场景中没有可释放的Cube。");
        }
    }
}
