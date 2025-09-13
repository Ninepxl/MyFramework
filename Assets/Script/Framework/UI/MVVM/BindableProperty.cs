// using System;
// using System.Collections.Generic;

// namespace Frame
// {
//     /// <summary>
//     /// 
//     /// </summary>
//     /// <typeparam name="TValue">监听的数据类型</typeparam>
//     public class BindableProperty<TValue>
//     {
//         private TValue m_Value;
//         public List<Action<TValue, TValue>> OnValueChanged = new();
//         public MessageBroker<(TValue oldVal, TValue newVal)> messageBroker = new();
//         public TValue Value
//         {
//             get { return m_Value; }
//             set
//             {
//                 TValue oldVal = m_Value;
//                 m_Value = value;
//                 foreach (var handler in OnValueChanged)
//                 {
//                     handler(oldVal, value);
//                 }
//                 // TODO: 切换成框架中的消息系统
//                 // messageBroker.Publish((oldVal, value));
//             }
//         }
//     }
// }