using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace Frame
{
    [CustomEditor(typeof(AssetHandleComponent))]
    public class AssetHandleComponentEditor : OdinEditor
    {
        private AssetHandleComponent target_component;

        protected override void OnEnable()
        {
            base.OnEnable();
            target_component = target as AssetHandleComponent;
        }

        public override void OnInspectorGUI()
        {
            // 绘制原始的Inspector内容
            base.OnInspectorGUI();

            // 如果不在运行时，不显示运行时信息
            if (!Application.isPlaying)
            {
                EditorGUILayout.HelpBox("运行时资源信息只在Play Mode下显示", MessageType.Info);
                return;
            }

            if (target_component == null) return;

            // 添加分隔线
            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            // 标题
            var titleStyle = new GUIStyle(EditorStyles.boldLabel)
            {
                fontSize = 14,
                alignment = TextAnchor.MiddleCenter
            };
            EditorGUILayout.LabelField("资源管理器运行时信息", titleStyle);

            EditorGUILayout.Space(5);

            // 显示资源总数
            int loadedCount = target_component.GetLoadedAssetCount();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("已加载资源总数:", EditorStyles.boldLabel, GUILayout.Width(120));
            EditorGUILayout.LabelField(loadedCount.ToString(), GUILayout.Width(50));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(5);

            // 显示资源列表
            if (loadedCount > 0)
            {
                EditorGUILayout.LabelField("已加载资源列表:", EditorStyles.boldLabel);

                // 表格标题
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("资源Key", EditorStyles.centeredGreyMiniLabel, GUILayout.Width(150));
                EditorGUILayout.LabelField("类型", EditorStyles.centeredGreyMiniLabel, GUILayout.Width(80));
                EditorGUILayout.LabelField("成功", EditorStyles.centeredGreyMiniLabel, GUILayout.Width(50));
                EditorGUILayout.LabelField("完成", EditorStyles.centeredGreyMiniLabel, GUILayout.Width(50));
                EditorGUILayout.LabelField("进度", EditorStyles.centeredGreyMiniLabel, GUILayout.Width(100));
                EditorGUILayout.EndHorizontal();

                // 分隔线
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                // 滚动视图
                var scrollViewHeight = Mathf.Min(loadedCount * 20 + 10, 200);
                scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Height(scrollViewHeight));

                var allAssets = target_component.GetAssetHandleCache();
                foreach (var asset in allAssets)
                {
                    DrawAssetRow(asset.Key, asset.Value);
                }

                EditorGUILayout.EndScrollView();
            }
            else
            {
                EditorGUILayout.HelpBox("当前没有加载任何资源到缓存中", MessageType.Info);
            }

            EditorGUILayout.Space(10);

            // 操作按钮
            EditorGUILayout.BeginHorizontal();

            // 刷新按钮
            if (GUILayout.Button("刷新信息", GUILayout.Height(25)))
            {
                Repaint(); // 强制重绘Inspector
                Debug.Log($"当前已加载资源数量: {loadedCount}");
            }

            // 清理缓存按钮
            GUI.backgroundColor = Color.red;
            if (GUILayout.Button("清理所有缓存", GUILayout.Height(25)))
            {
                if (EditorUtility.DisplayDialog("确认操作", "确定要清理所有缓存资源吗？", "确定", "取消"))
                {
                    target_component.ClearAllCache();
                    Debug.Log("已清理所有缓存资源");
                }
            }
            GUI.backgroundColor = Color.white;

            EditorGUILayout.EndHorizontal();

            // 自动刷新
            if (Application.isPlaying)
            {
                EditorUtility.SetDirty(target);
                Repaint();
            }
        }

        private Vector2 scrollPosition;

        private void DrawAssetRow(object key, AssetHandleBase assetHandle)
        {
            EditorGUILayout.BeginHorizontal();

            // 资源Key
            string keyStr = key?.ToString() ?? "Unknown";
            if (keyStr.Length > 20)
                keyStr = keyStr.Substring(0, 17) + "...";
            EditorGUILayout.LabelField(keyStr, GUILayout.Width(150));

            // 资源类型
            EditorGUILayout.LabelField(assetHandle.CurType.ToString(), GUILayout.Width(80));

            // 成功状态
            var successStyle = assetHandle.Success ? EditorStyles.miniLabel : EditorStyles.centeredGreyMiniLabel;
            EditorGUILayout.LabelField(assetHandle.Success ? "✓" : "✗", successStyle, GUILayout.Width(50));

            // 完成状态
            var doneStyle = assetHandle.IsDone ? EditorStyles.miniLabel : EditorStyles.centeredGreyMiniLabel;
            EditorGUILayout.LabelField(assetHandle.IsDone ? "✓" : "✗", doneStyle, GUILayout.Width(50));

            // 进度条
            var rect = GUILayoutUtility.GetRect(100, 18);
            var progress = assetHandle.Progress;
            var progressColor = assetHandle.Success ? Color.green : (progress > 0 ? Color.yellow : Color.red);

            EditorGUI.DrawRect(rect, Color.grey);
            var progressRect = new Rect(rect.x, rect.y, rect.width * progress, rect.height);
            EditorGUI.DrawRect(progressRect, progressColor);

            var progressText = $"{(progress * 100):F1}%";
            var progressLabelRect = new Rect(rect.x, rect.y, rect.width, rect.height);
            EditorGUI.LabelField(progressLabelRect, progressText, EditorStyles.centeredGreyMiniLabel);

            EditorGUILayout.EndHorizontal();
        }
    }
}