// using Frame;
// using TMPro;
// using UnityEngine;
// using UnityEngine.UI;

// public class PlayerView : MonoBehaviour
// {
//     [SerializeField]
//     private TextMeshProUGUI palyHpText;
//     [SerializeField]
//     private Button AddBtn;
//     [SerializeField]
//     private Button SubBtn;
//     private PlayerViewModel _viewModel;
//     private PropertyBinder<PlayerViewModel> _binder = new();
//     private void Awake()
//     {
//         _viewModel = new();
//         _binder = new();
//         _binder.Add<int>("hp", (oldValue, newValue) =>
//         {
//             palyHpText.text = $"{newValue}";
//         });
//         _binder.Bind(_viewModel);
//         _viewModel.hp.Value = 100;
//     }

// }