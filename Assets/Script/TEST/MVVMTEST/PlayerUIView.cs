using Frame;
using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
public class PlayerUIView : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI palyHpText;
    [SerializeField]
    public Button AddBtn;
    [SerializeField]
    public Button SubBtn;
    PropertyBinder<PlayerViewModel> binder = new();
    private PlayerViewModel viewModel = new();
    private void Awake()
    {
        // 处理绑定
        binder.Add<int>(nameof(viewModel.hp), (oldHp, newHp) =>
        {
            palyHpText.text = newHp.ToString();
        });
        binder.Bind(viewModel);
    }
}