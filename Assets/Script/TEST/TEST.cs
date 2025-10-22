using System;
using System.Collections.Generic;
using UnityEngine;
using HachiFramework;
public class TEST : MonoBehaviour
{
    public List<IDisposable> messageNodes = new();
    private void OnEnable()
    {
        var node = ActGame.GameEntry.Message.Subscribe<string>((val) =>
        {
            Debug.Log(val);
        });
        messageNodes.Add(node);
    }

    [HachiButton]
    public void MessageTEST()
    {
        ActGame.GameEntry.Message.Publish<string>("pxl");
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
