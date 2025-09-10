using Cysharp.Threading.Tasks;
using Frame;
using Sirenix.OdinInspector;
using UnityEngine;

public class TEST : MonoBehaviour
{
    private AssetHandleComponent assetHandleComponent;
    private GameObjectPoolComponent poolComponent;
    private GameObject cubePerfab;
    private void Start()
    {
        assetHandleComponent = GameEntry.GetComponent<Frame.AssetHandleComponent>();
        poolComponent = GameEntry.GetComponent<GameObjectPoolComponent>();
        if (assetHandleComponent == null || poolComponent == null)
        {
            Debug.Log("框架加载失败");
        }
        else
        {
            Debug.Log("框架加载成功");
            _ = InitAsset();
        }
    }

    /// <summary>
    /// 初始化资源加载器
    /// </summary>
    private async UniTask InitAsset()
    {
        await assetHandleComponent.StartupCoroutine();
    }

    /// <summary>
    /// 加载cubePerfab
    /// </summary>
    [Button("加载方块预制体")]
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
    [Button("对象池测试")]
    private void PoolTESTRent()
    {
        if (cubePerfab != null)
        {
            GameObject cubeInstacne = poolComponent.Rent(cubePerfab, transform);
            _ = PoolTESTReturn(cubeInstacne);
        }
    }

    private async UniTask PoolTESTReturn(GameObject instance)
    {
        await UniTask.Delay(2000);
        poolComponent.Return(instance);
    }
}
