using TMPro;
using UnityEngine;
using Frame;
using UnityEngine.UI;
public class BindablePropertyTEST : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI palyHpText;
    [SerializeField]
    private Button AddBtn;
    [SerializeField]
    private Button SubBtn;
    private BindableProperty<int> playerHp;
    public void Awake()
    {
        // 绑定数据完成数据相应
        playerHp = new BindableProperty<int>();
        playerHp.Subscribe((oldValue, newValue) =>
        {
            palyHpText.text = $"{newValue}";
        }, true);
        AddBtn.onClick.AddListener(() =>
        {
            playerHp.Value += 10;
        });
        SubBtn.onClick.AddListener(() =>
        {
            playerHp.Value -= 10;
        });
    }
}