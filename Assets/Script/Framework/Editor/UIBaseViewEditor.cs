using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;
using System;
using System.Linq;
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using Sirenix.Utilities;

namespace Frame.Editor
{
    /// <summary>
    /// 使用Odin优化的UIBaseView编辑器
    /// </summary>
    [CustomEditor(typeof(UIBaseView), true)]
    public class UIBaseViewEditor : OdinEditor
    {
        // 静态缓存，提升反射性能
        private static readonly Dictionary<Type, FieldInfo> PropertyNameFieldCache = new Dictionary<Type, FieldInfo>();
        private static readonly Dictionary<BoundComponentType, Type> ComponentTypeMap = new Dictionary<BoundComponentType, Type>
        {
            { BoundComponentType.Button, typeof(BoundButton) },
            { BoundComponentType.Text, typeof(BoundText) },
        };

        private UIBaseView _targetView;
        private List<ComponentBindingInfo> _bindings;
        private bool _showBindingDetails;
        private Vector2 _scrollPosition;

        protected override void OnEnable()
        {
            base.OnEnable();
            _targetView = target as UIBaseView;
            RefreshBindings();
        }

        public override void OnInspectorGUI()
        {
            // 绘制默认Inspector
            base.OnInspectorGUI();

            GUILayout.Space(10);
            DrawBindingSection();
        }

        private void RefreshBindings()
        {
            _bindings = _targetView?.ComponentBindings ?? new List<ComponentBindingInfo>();
        }

        private void DrawBindingSection()
        {
            SirenixEditorGUI.BeginBox("UI组件绑定管理");

            // 状态信息
            DrawBindingStatus();

            GUILayout.Space(5);

            // 主要操作按钮
            DrawMainButtons();

            GUILayout.Space(5);

            // 详细信息（可折叠）
            DrawBindingDetails();

            SirenixEditorGUI.EndBox();
        }

        private void DrawBindingStatus()
        {
            var validBindings = _bindings?.Where(b => !string.IsNullOrEmpty(b.Name) && !string.IsNullOrEmpty(b.Path)).Count() ?? 0;
            var totalBindings = _bindings?.Count ?? 0;

            if (totalBindings == 0)
            {
                SirenixEditorGUI.MessageBox("在代码中没有定义绑定信息", MessageType.Info);
                return;
            }

            string statusText = validBindings == totalBindings
                ? $"发现 {totalBindings} 个有效绑定配置"
                : $"发现 {validBindings}/{totalBindings} 个有效绑定配置";

            var messageType = validBindings == totalBindings ? MessageType.Info : MessageType.Warning;
            SirenixEditorGUI.MessageBox(statusText, messageType);
        }

        private void DrawMainButtons()
        {
            GUILayout.BeginHorizontal();

            // 主绑定按钮
            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("🔗 自动绑定UI组件", GUILayout.Height(35)))
            {
                ApplyBindingsWithUndo();
            }
            GUI.backgroundColor = Color.white;

            // 刷新按钮
            if (GUILayout.Button("🔄", GUILayout.Width(35), GUILayout.Height(35)))
            {
                RefreshBindings();
            }

            // 验证按钮
            if (GUILayout.Button("✓", GUILayout.Width(35), GUILayout.Height(35)))
            {
                ValidateBindings();
            }

            GUILayout.EndHorizontal();
        }

        private void DrawBindingDetails()
        {
            if (_bindings == null || _bindings.Count == 0) return;

            _showBindingDetails = SirenixEditorGUI.Foldout(_showBindingDetails, $"绑定详情 ({_bindings.Count})");

            if (_showBindingDetails)
            {
                EditorGUI.indentLevel++;
                _scrollPosition = GUILayout.BeginScrollView(_scrollPosition, GUILayout.MaxHeight(200));

                foreach (var binding in _bindings)
                {
                    DrawBindingItem(binding);
                }

                GUILayout.EndScrollView();
                EditorGUI.indentLevel--;
            }
        }

        private void DrawBindingItem(ComponentBindingInfo binding)
        {
            if (string.IsNullOrEmpty(binding.Name) || string.IsNullOrEmpty(binding.Path))
            {
                SirenixEditorGUI.ErrorMessageBox($"无效绑定: {binding.Name} -> {binding.Path}");
                return;
            }

            GUILayout.BeginHorizontal(EditorStyles.helpBox);

            // 状态图标
            var targetTransform = _targetView.transform.Find(binding.Path);
            var icon = targetTransform != null ? "✅" : "❌";
            GUILayout.Label(icon, GUILayout.Width(20));

            // 绑定信息
            GUILayout.BeginVertical();
            GUILayout.Label($"{binding.Type}: {binding.Name}", EditorStyles.boldLabel);
            GUILayout.Label($"路径: {binding.Path}", EditorStyles.miniLabel);
            GUILayout.EndVertical();

            // 操作按钮
            if (targetTransform != null && GUILayout.Button("定位", GUILayout.Width(50)))
            {
                Selection.activeGameObject = targetTransform.gameObject;
                EditorGUIUtility.PingObject(targetTransform.gameObject);
            }

            GUILayout.EndHorizontal();
        }

        private void ApplyBindingsWithUndo()
        {
            if (_bindings == null || _bindings.Count == 0)
            {
                EditorUtility.DisplayDialog("提示", "没有找到需要绑定的组件信息", "确定");
                return;
            }

            // 记录撤销操作
            Undo.RegisterCompleteObjectUndo(_targetView.gameObject, "UI组件绑定");

            try
            {
                EditorUtility.DisplayProgressBar("正在绑定UI组件", "初始化...", 0f);

                var results = ApplyBindingsInternal();

                EditorUtility.ClearProgressBar();

                // 显示结果
                ShowBindingResults(results);

                // 标记修改
                EditorUtility.SetDirty(_targetView);
            }
            catch (Exception ex)
            {
                EditorUtility.ClearProgressBar();
                Debug.LogError($"绑定过程中发生错误: {ex.Message}", _targetView);
                EditorUtility.DisplayDialog("错误", $"绑定失败: {ex.Message}", "确定");
            }
        }

        private BindingResult ApplyBindingsInternal()
        {
            var result = new BindingResult();

            for (int i = 0; i < _bindings.Count; i++)
            {
                var binding = _bindings[i];
                float progress = (float)i / _bindings.Count;
                EditorUtility.DisplayProgressBar("正在绑定UI组件", $"处理: {binding.Name}", progress);

                if (string.IsNullOrEmpty(binding.Name) || string.IsNullOrEmpty(binding.Path))
                {
                    result.AddSkipped(binding.Name, "Name或Path为空");
                    continue;
                }

                var targetTransform = _targetView.transform.Find(binding.Path);
                if (targetTransform == null)
                {
                    result.AddFailed(binding.Name, $"找不到路径: {binding.Path}");
                    continue;
                }

                if (!ComponentTypeMap.TryGetValue(binding.Type, out Type componentType))
                {
                    result.AddFailed(binding.Name, $"不支持的组件类型: {binding.Type}");
                    continue;
                }

                try
                {
                    if (AddBoundComponent(targetTransform.gameObject, componentType, binding.Name))
                    {
                        result.AddSuccess(binding.Name);
                    }
                    else
                    {
                        result.AddFailed(binding.Name, "添加组件失败");
                    }
                }
                catch (Exception ex)
                {
                    result.AddFailed(binding.Name, ex.Message);
                }
            }

            return result;
        }

        private bool AddBoundComponent(GameObject target, Type componentType, string propertyName)
        {
            // 检查必需组件
            if (!ValidateRequiredComponents(target, componentType))
            {
                return false;
            }

            var component = target.GetComponent(componentType);
            if (component == null)
            {
                component = target.AddComponent(componentType);
            }

            return SetPropertyName(component, propertyName);
        }

        private bool ValidateRequiredComponents(GameObject target, Type componentType)
        {
            var requiredAttribute = componentType.GetCustomAttribute<RequireComponent>();
            if (requiredAttribute == null) return true;

            // 检查必需的组件是否存在
            if (requiredAttribute.m_Type0 != null && !target.GetComponent(requiredAttribute.m_Type0))
            {
                Debug.LogWarning($"GameObject '{target.name}' 缺少必需组件: {requiredAttribute.m_Type0.Name}", target);
                return false;
            }

            return true;
        }

        private bool SetPropertyName(Component component, string propertyName)
        {
            var componentType = component.GetType();

            if (!PropertyNameFieldCache.TryGetValue(componentType, out FieldInfo fieldInfo))
            {
                fieldInfo = componentType.GetField("propertyName", BindingFlags.NonPublic | BindingFlags.Instance);
                PropertyNameFieldCache[componentType] = fieldInfo;
            }

            if (fieldInfo == null)
            {
                Debug.LogError($"在组件 {componentType.Name} 中找不到 'propertyName' 字段", component);
                return false;
            }

            fieldInfo.SetValue(component, propertyName);
            return true;
        }

        private void ValidateBindings()
        {
            if (_bindings == null || _bindings.Count == 0)
            {
                EditorUtility.DisplayDialog("验证结果", "没有绑定信息需要验证", "确定");
                return;
            }

            var issues = new List<string>();

            foreach (var binding in _bindings)
            {
                if (string.IsNullOrEmpty(binding.Name) || string.IsNullOrEmpty(binding.Path))
                {
                    issues.Add($"无效绑定: {binding.Name} -> {binding.Path}");
                    continue;
                }

                var targetTransform = _targetView.transform.Find(binding.Path);
                if (targetTransform == null)
                {
                    issues.Add($"路径不存在: {binding.Path} ({binding.Name})");
                    continue;
                }

                if (!ComponentTypeMap.ContainsKey(binding.Type))
                {
                    issues.Add($"不支持的组件类型: {binding.Type} ({binding.Name})");
                }
            }

            string message = issues.Count == 0
                ? "所有绑定配置都有效！"
                : $"发现 {issues.Count} 个问题:\n" + string.Join("\n", issues);

            EditorUtility.DisplayDialog("验证结果", message, "确定");
        }

        private void ShowBindingResults(BindingResult result)
        {
            string message = $"绑定完成!\n\n" +
                           $"成功: {result.SuccessCount}\n" +
                           $"失败: {result.FailedCount}\n" +
                           $"跳过: {result.SkippedCount}";

            if (result.FailedCount > 0)
            {
                message += $"\n\n失败详情:\n{string.Join("\n", result.FailedItems.Take(5))}";
                if (result.FailedItems.Count > 5)
                {
                    message += $"\n...还有 {result.FailedItems.Count - 5} 项";
                }
            }

            EditorUtility.DisplayDialog("绑定结果", message, "确定");
        }

        // 结果数据结构
        private class BindingResult
        {
            public List<string> SuccessItems = new List<string>();
            public List<string> FailedItems = new List<string>();
            public List<string> SkippedItems = new List<string>();

            public int SuccessCount => SuccessItems.Count;
            public int FailedCount => FailedItems.Count;
            public int SkippedCount => SkippedItems.Count;

            public void AddSuccess(string name) => SuccessItems.Add(name);
            public void AddFailed(string name, string reason) => FailedItems.Add($"{name}: {reason}");
            public void AddSkipped(string name, string reason) => SkippedItems.Add($"{name}: {reason}");
        }
    }

    /// <summary>
    /// 扩展：组件类型注册器
    /// 允许插件化注册新的绑定组件类型
    /// </summary>
    public static class BoundComponentRegistry
    {
        private static Dictionary<BoundComponentType, Type> _customTypes = new Dictionary<BoundComponentType, Type>();

        public static void RegisterComponent<T>(BoundComponentType type) where T : Component, IBoundComponent
        {
            _customTypes[type] = typeof(T);
        }

        public static Type GetComponentType(BoundComponentType type)
        {
            return _customTypes.TryGetValue(type, out Type customType) ? customType : null;
        }

        public static IEnumerable<KeyValuePair<BoundComponentType, Type>> GetAllRegistrations()
        {
            return _customTypes;
        }
    }
}