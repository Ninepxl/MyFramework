using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace Frame
{
    /// <summary>
    /// “智能”按钮绑定组件。
    /// 负责将ViewModel中的ButtonBinding属性与Unity场景中的Button组件进行关联。
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class BoundButton : MonoBehaviour, IBoundComponent
    {
        // 这个字段由编辑器脚本通过反射来设置，因此不需要public。
        [SerializeField]
        private string propertyName;
        public string PropertyName => propertyName;

        private Button _unityButton;
        private Action _onClickAction;
        private IDisposable _isActiveSubscription;

        private void Awake()
        {
            _unityButton = GetComponent<Button>();
        }

        public void Bind(object viewModel)
        {
            // 通过反射获取ViewModel中与此组件同名的属性
            PropertyInfo propertyInfo = viewModel.GetType().GetProperty(PropertyName);
            if (propertyInfo == null)
            {
                Debug.LogError($"在 ViewModel '{viewModel.GetType().Name}' 中找不到名为 '{PropertyName}' 的属性。", this);
                return;
            }

            // 获取属性的值，它应该是一个ButtonBinding实例
            var buttonBinding = propertyInfo.GetValue(viewModel) as ButtonComponent;
            if (buttonBinding == null)
            {
                Debug.LogError($"ViewModel中的属性 '{PropertyName}' 不是一个有效的 ButtonBinding。", this);
                return;
            }

            // 1. 绑定点击事件
            _onClickAction = buttonBinding.OnClick;
            if (_onClickAction != null)
            {
                _unityButton.onClick.AddListener(() => _onClickAction.Invoke());
            }

            // 2. 绑定激活状态
            _isActiveSubscription = buttonBinding.IsActive.Subscribe((oldValue, newValue) =>
            {
                if (gameObject.activeSelf != newValue)
                {
                    gameObject.SetActive(newValue);
                }
            }, true); // true表示立即执行一次，以设置初始状态
        }

        private void OnDestroy()
        {
            // 组件销毁时，清理所有绑定，防止内存泄漏
            if (_unityButton != null && _onClickAction != null)
            {
                _unityButton.onClick.RemoveListener(() => _onClickAction.Invoke());
            }
            _isActiveSubscription?.Dispose();
        }
    }
}
