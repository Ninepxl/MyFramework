using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
public class UniTaskT : MonoBehaviour
{
    public Transform cubeTrs;
    void Start()
    {
        InstanceCube().Forget(); 
    }

    /// <summary>
    /// 根据帧来移动物体
    /// </summary>
    /// <param name="n">移动举例</param>
    /// <returns></returns>
    public IEnumerator MoveCubeForFrameByIE(int n, int speed)
    {
        for (int i = 0; i < n; i++)
        {
            Debug.Log("移动方块");
            cubeTrs.position = new Vector3(cubeTrs.position.x + 1, cubeTrs.position.y, cubeTrs.position.z);
            yield return new WaitForSeconds(speed);
        }
    }

    /// <summary>
    /// 移动方块 异步 
    /// </summary>
    /// <param name="n"></param>
    /// <param name="speed"></param>
    /// <returns></returns>
    public async UniTask MoveCubeForFrameByUniTask(int n, int speed)
    {
        for (int i = 0; i < n; i++)
        {
            Debug.Log("移动方块");
            cubeTrs.position = new Vector3(cubeTrs.position.x + 1, cubeTrs.position.y, cubeTrs.position.z);
            await UniTask.Delay(speed * 1000);
        }
        return;
    }
    public async UniTask<GameObject> LoadGameObject(string keyName)
    {
        AsyncOperationHandle<GameObject> h = Addressables.LoadAssetAsync<GameObject>(keyName);
        await h;
        return h.Result;
    }
    public async UniTask InstanceCube()
    {
        GameObject cube = await LoadGameObject("MyCube");
        Instantiate(cube);
        return;
    }
}
