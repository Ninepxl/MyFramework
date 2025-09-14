using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Frame.Editor;
namespace UITEST
{
    /// <summary>
    /// View 应该持有 ViewModel
    /// </summary>
    public class PalyerView : MonoBehaviour
    {
        public Dictionary<string, string> pathList = new()
        {
            {"AddBtn", "Panel/AddBtn"},
            {"SubBtn", "Panel/SubBtn"},
        };
        private PalyerViewModel ViewModel;
        [SerializeField, FindUIComponent("LV")]
        private TextMeshProUGUI LvText;

        [SerializeField, FindUIComponent("AddBtn")]
        private Button UpBtn;
        private void Awake()
        {
            ViewModel = new();
            // 手动绑定
            ViewModel.Lv.Subscribe((oldVal, newVal) =>
            {
                LvText.text = newVal;
            });
            ViewModel.onLevelUp.Subscribe((oldHandler, newHandler) =>
            {
                if (oldHandler != null)
                    UpBtn.onClick.RemoveListener(oldHandler);
                UpBtn.onClick.AddListener(newHandler);
            }, true);
        }
    }
}