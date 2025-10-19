using System;
using System.Collections.Generic;
using UnityEngine;
using ActGame;
using Sirenix.OdinInspector;
public class TEST : MonoBehaviour
{
    public List<IDisposable> messageNodes = new();
    private void OnEnable()
    {
        var node = GameEntry.Message.Subscribe<string>((val) =>
        {
            Debug.Log(val);
        });
        messageNodes.Add(node);
    }
    [Button("消息系统测试")]
    public void MessageTEST()
    {
        GameEntry.Message.Publish<string>("pxl");
    }
    private void OnDisable()
    {
        {
            foreach (var item in messageNodes)
            {
                item.Dispose();
            }
        }
    }
}
