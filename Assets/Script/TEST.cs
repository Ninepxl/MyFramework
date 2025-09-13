// using System;
// using System.Collections.Generic;
// using Cysharp.Threading.Tasks;
// using GameDemo;
// using Sirenix.OdinInspector;
// using UnityEngine;
// public struct MessageTestEvent
// {
//     public int value;
// }
// public class TEST : MonoBehaviour
// {
//     private GameObject cubePerfab;
//     List<IDisposable> handlers = new List<IDisposable>();

//     /// <summary>
//     /// 加载cubePerfab
//     /// </summary>
//     [Button("加载方块预制体")]
//     private void LoadCubePerfab()
//     {
//         GameEntry.Asset.LoadAsset<GameObject>("MyCube", handle =>
//         {
//             if (handle.Success)
//             {
//                 cubePerfab = handle.Result;
//             }
//         });
//     }

//     /// <summary>
//     /// 对象池获取对象测试
//     /// </summary>
//     /// <param name="count"></param>
//     [Button("对象池测试")]
//     private void PoolTESTRent()
//     {
//         if (cubePerfab != null)
//         {
//             GameObject cubeInstacne = GameEntry.GameObjectPool.Rent(cubePerfab, transform);
//             _ = PoolTESTReturn(cubeInstacne);
//         }
//     }

//     private async UniTask PoolTESTReturn(GameObject instance)
//     {
//         await UniTask.Delay(2000);
//         GameEntry.GameObjectPool.Return(instance);
//     }
//     [Button("发布消息")]
//     private void MessageTESTPublish()
//     {
//         Debug.Log("发布消息");
//         GameEntry.Message.Publish(new MessageTestEvent { value = 1 });
//     }
//     [Button("订阅消息")]
//     private void MessageTESTSubscribe()
//     {
//         Debug.Log("订阅消息");
//         var handler = GameEntry.Message.Subscribe<MessageTestEvent>((e) =>
//         {
//             Debug.Log(e.value);
//         });
//         handlers.Add(handler);
//     }
//     [Button("取消订阅消息")]
//     private void MessageTESTUnsubscribe()
//     {
//         Debug.Log("取消订阅消息");
//         foreach (var handler in handlers)
//         {
//             handler.Dispose();
//         }
//     }
// }
