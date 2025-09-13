using System;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;

namespace Frame
{
    /// <summary>
    /// “智能”文本绑定组件。
    /// 负责将ViewModel中的TextBinding属性与Unity场景中的TextMeshProUGUI组件进行关联。
    /// </summary>
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class BoundText : MonoBehaviour, IBoundComponent
    {
        [SerializeField]
        private string propertyName;
        public string PropertyName => propertyName;

        private TextMeshProUGUI _tmproText;
        private List<IDisposable> _subscriptions = new List<IDisposable>();

        private void Awake()
        {
            _tmproText = GetComponent<TextMeshProUGUI>();
        }

        public void Bind(object viewModel)
        {
            PropertyInfo propertyInfo = viewModel.GetType().GetProperty(PropertyName);
            if (propertyInfo == null)
            {
                Debug.LogError($"在 ViewModel '{viewModel.GetType().Name}' 中找不到名为 '{PropertyName}' 的属性。", this);
                return;
            }

            var textBinding = propertyInfo.GetValue(viewModel) as TextComponent;
            if (textBinding == null)
            {
                Debug.LogError($"ViewModel中的属性 '{PropertyName}' 不是一个有效的 TextBinding。", this);
                return;
            }

            // 1. 绑定文本内容
            var textSubscription = textBinding.Text.Subscribe((oldValue, newValue) =>
            {
                _tmproText.text = newValue;
            }, true);
            _subscriptions.Add(textSubscription);


            // 2. 绑定激活状态
            var isActiveSubscription = textBinding.IsActive.Subscribe((oldValue, newValue) =>
            {
                if (gameObject.activeSelf != newValue)
                {
                    gameObject.SetActive(newValue);
                }
            }, true);
            _subscriptions.Add(isActiveSubscription);
        }

        private void OnDestroy()
        {
            // 组件销毁时，清理所有订阅
            foreach (var sub in _subscriptions)
            {
                sub?.Dispose();
            }
            _subscriptions.Clear();
        }
    }
}
