using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Profiling;

public class MyTread : MonoBehaviour
{
    bool isLoad = false;
    void Start()
    {
        StartCoroutine(MyCoroutine());
    }

    void Update()
    {
        if (!isLoad)
        {
            Debug.Log("UPDATE");
        }
    }

    // 传入一个委托在协程执行完毕后执行
    IEnumerator MyCoroutine()
    {
        var handle = Addressables.LoadAssetAsync<GameObject>("MyCube");
        yield return handle;
        if (handle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
        {
            GameObject cube = Instantiate(handle.Result, transform);
            Debug.Log(cube.name);
            isLoad = true;
        }
    }
}
