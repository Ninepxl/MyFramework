using Frame;
using UnityEngine;

public class MVCTESTControl : MonoBehaviour
{
    private PlayerModel m_playerModel;
    private MVCTESTView m_view;
    private PropertyBinder<PlayerModel> m_binder = new();
    private void Awake()
    {
        // 对UI某个属性的变化做出响应
        // 这样对属性进行Bind会将View和Model耦合在一起
        // m_playerModel.m_Hp.Subscribe((oldVal, newVal) =>
        // {
        //     m_view.SetHpText(newVal);
        // });
        // m_playerModel.Name.Subscribe((oldVal, newVal) =>
        // {
        //     Debug.Log($"Name Change {oldVal} -> {newVal}");
        // });
        m_binder.Add<int>("m_Hp", (oldValue, newValue) =>
        {
            m_view.SetHpText(newValue);
        });
    }
    private void OnEnable()
    {
        m_playerModel.Name.Value = "Hero";
        m_playerModel.m_Hp.Value = 100;
    }
    public MVCTESTControl(MVCTESTView view)
    {
        m_view = view;
        m_playerModel = new PlayerModel();
        m_view.AddBtn.onClick.AddListener(OnAddBtnClick);
        m_view.SubBtn.onClick.AddListener(OnSubBtnClick);
    }
    private void OnAddBtnClick()
    {
        m_playerModel.m_Hp.Value += 10;
    }
    private void OnSubBtnClick()
    {
        m_playerModel.m_Hp.Value -= 10;
    }
}