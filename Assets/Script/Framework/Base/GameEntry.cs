using System;
using System.Collections.Generic;
using UnityEngine;

namespace HachiFramework
{
    public static class GameEntry
    {
        private static Dictionary<Type, GameComponent> m_Components = new Dictionary<Type, GameComponent>();

        public static void RegisterComponent(GameComponent component)
        {
            // 如果这个Component已经加入存储中就什么都不做
            if (m_Components.ContainsKey(component.GetType()))
            {
                Debug.LogError("当前Component 已经被注册过了: " + component.GetType().Name);
                return;
            }

            Type type = component.GetType();
            m_Components[type] = component;
        }

        public static T GetComponent<T>() where T : GameComponent
        {
            return (T)GetComponent(typeof(T));
        }

        private static GameComponent GetComponent(Type type)
        {
            if (m_Components.TryGetValue(type, out var component))
            {
                return component;
            }

            return null;
        }

        /// <summary>
        /// 关闭游戏框架
        /// </summary>
        private static void Shutdown(ShutdownType shutdownType)
        {
            // TODO: 1. 执行游戏退出程序, 如保持玩家数据，清理内存等操作
            m_Components.Clear();
            if (shutdownType == ShutdownType.None)
            {
                return;
            }

            if (shutdownType == ShutdownType.Restart)
            {
                // TODO: 重新执行游戏流程 
                return;
            }

            if (shutdownType == ShutdownType.Quit)
            {
                Application.Quit();
                return;
            }
        }
    }
}