using System;
using System.Collections.Generic;
using Sirenix.OdinInspector.Editor;
using UnityEngine;

namespace HachiFramework.Editor
{
    /// <summary>
    /// 自动绑定UI属性
    /// </summary>
    public class AutoBindPropertyProcessor : OdinAttributeProcessor
    {
        public override void ProcessSelfAttributes(InspectorProperty property, List<Attribute> attributes)
        {
            // 查找字段上的 AutoBindAttribute 特性
            var autoBindAttr = property.GetAttribute<FindUIComponentAttribute>();
            if (autoBindAttr == null)
                return;

            // 如果字段已经有值，则不进行自动绑定，允许手动覆盖
            if (property.ValueEntry.WeakSmartValue != null)
            {
                return;
            }

            // 1. 找到根物体。根物体就是这个 property 所属的 Component。
            // 我们可以通过 property.Tree.UnitySerializedObject.targetObject 来获取。
            var rootComponent = property.Tree.UnitySerializedObject.targetObject as Component;
            if (rootComponent == null)
            {
                Debug.LogWarning("[AutoBind] 无法在非 Component 对象上使用。");
                return;
            }
            Transform rootTransform = rootComponent.transform;

            // 2. 确定查找路径
            string searchPath = autoBindAttr.Path;
            if (string.IsNullOrEmpty(searchPath))
            {
                // 如果特性中没有提供路径，则默认使用字段的名称作为路径
                searchPath = property.Name;
            }

            // 3. 根据路径查找目标 Transform
            Transform targetTransform = rootTransform.Find(searchPath);

            if (targetTransform == null)
            {
                Debug.LogError($"[AutoBind] 在 '{rootTransform.name}' 上未找到路径为 '{searchPath}' 的对象。", rootComponent);
                return;
            }

            // 4. 从目标 Transform 上获取所需类型的组件
            Type componentType = property.Info.TypeOfValue;
            Component foundComponent = targetTransform.GetComponent(componentType);

            // 5. 如果找到组件，则赋值给字段
            if (foundComponent != null)
            {
                property.ValueEntry.WeakSmartValue = foundComponent;
            }
            else
            {
                Debug.LogError($"[AutoBind] 在对象 '{targetTransform.name}' (路径: '{searchPath}') 上未找到类型为 '{componentType.Name}' 的组件。", rootComponent);
            }
        }
    }
}