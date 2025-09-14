using System.Collections.Generic;
using Frame;
using UnityEngine;
using UnityEngine.Events;

namespace UITEST
{
    /// <summary>
    /// PlayerViewModel 中只修改数据
    /// </summary>
    public class PalyerViewModel
    {
        public Dictionary<string, IBindableProperty> addBtn;
        // 再执行ViewModel
        public PalyerViewModel()
        {
            addBtn = UIUtil.BindButton(() =>
            {
                Debug.Log("Click Add Btn");
            });
        }
    }
}