using UnityEngine;
using UnityEngine.UI;
using System;
using HachiFramework;
namespace UITEST
{
    /// <summary>
    /// View 应该持有 ViewModel
    /// </summary>
    public class PalyerView : MonoBehaviour
    {

        [SerializeField, FindUIComponent("AddBtn")]
        private Button addBtn;
        private PropertyBinder binder;
        private PalyerViewModel viewModel;
        // 先执行View
        private void Awake()
        {
            binder = new();
            viewModel = new();
            binder.Add(viewModel.addBtn["active"] as BindableProperty<bool>, (oldVal, newVal) =>
            {
                addBtn.gameObject.SetActive(newVal);
            });
            binder.Add(viewModel.addBtn["onClick"] as BindableProperty<Action>, (oldVal, newVal) =>
            {
                if (oldVal != null)
                    addBtn.onClick.RemoveListener(oldVal.Invoke);
                addBtn.onClick.AddListener(newVal.Invoke);
            }, true);
        }
    }
}