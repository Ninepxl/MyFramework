
using System;
using System.Reflection;
using HachiFramework;
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
#endif

#if UNITY_EDITOR
namespace HachiEditor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(MonoBehaviour), true)]
    public class HachiButtonEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            Debug.Log("HachiButton");
            MonoBehaviour targetObject = (MonoBehaviour)target;

            MethodInfo[] methods = targetObject.GetType().GetMethods(
                BindingFlags.Instance | BindingFlags.Static |
                BindingFlags.Public | BindingFlags.NonPublic
            );

            foreach (var method in methods)
            {
                HachiButton hachiButtonAttribute = method.GetCustomAttribute<HachiButton>();
                if (hachiButtonAttribute != null)
                {
                    var buttonName = String.IsNullOrEmpty(hachiButtonAttribute.buttonName) ? method.Name : hachiButtonAttribute.buttonName;
                    GUILayout.Space(10);
                    if (GUILayout.Button(buttonName, GUILayout.Height(hachiButtonAttribute.height)))
                    {
                        var parameterInfos = method.GetParameters();
                        if (parameterInfos.Length == 0)
                        {
                            method.Invoke(targetObject, null);
                        }
                    }
                }
            }
        }
    }
}
#endif