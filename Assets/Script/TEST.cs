using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Frame;
using Sirenix.OdinInspector;
using UnityEngine;

public class TEST : MonoBehaviour
{
    private Frame.AssetHandleComponent assetHandleComponent;
    private bool isInitAsset = false;
    private GameObject cubePerfab;
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

    /// <summary>
    /// 初始化资源加载器
    /// </summary>
    private async void InitAsset()
    {
        await assetHandleComponent.StartupCoroutine();
        isInitAsset = true;
    }

    /// <summary>
    /// 加载cubePerfab
    /// </summary>
    [Button("LoadCubePerfab")]
    private void LoadCubePerfab()
    {
        assetHandleComponent.LoadAsset<GameObject>("MyCube", handle =>
        {
            if (handle.Success)
            {
                cubePerfab = handle.Result;
            }
        });
    }

    /// <summary>
    /// 对象池获取对象测试
    /// </summary>
    /// <param name="count"></param>
    [Button("PoolTESTRent")]
    private async UniTask PoolTESTRent()
    {
        GameObject cubeInstacne = GameObjectPoolTool.Rent(cubePerfab, transform);
        _ = PoolTESTReturn(cubeInstacne);
    }
    private async UniTask PoolTESTReturn(GameObject instance)
    {
        await UniTask.Delay(10000);
        GameObjectPoolTool.Return(instance);
    }
}
