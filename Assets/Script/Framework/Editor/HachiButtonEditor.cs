
using UnityEditor;
using UnityEngine;

namespace HachiFramework.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(TEST), true)]
    public class HachiButtonEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("点我执行操作"))
            {
                // 在这里放入点击后要执行的代码
                Debug.Log("按钮被点击了！");

            }
        }
    }
}