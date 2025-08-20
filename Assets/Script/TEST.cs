using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST : MonoBehaviour
{
    private Frame.AssetHandleComponent assetHandleComponent;
    private bool isInitAsset = false;
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
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            if (!isInitAsset)
            {
                Debug.Log("资源没有加载完成");
            }
            else
            {
                assetHandleComponent.InstantiateGo("MyCube", transform, h =>
                {
                    Debug.Log(h.Result.name);
                });
            }
        }
    }
    private async void InitAsset()
    {
        await assetHandleComponent.StartupCoroutine();
        isInitAsset = true;
    }
}
