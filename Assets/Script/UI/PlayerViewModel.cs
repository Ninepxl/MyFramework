using Frame;
using Game.Models;

namespace Game.ViewModels
{
    /// <summary>
    /// ViewModel层：UI的“大脑”。
    /// 它连接Model和View，负责所有UI的显示逻辑和状态管理。
    /// </summary>
    public class PlayerViewModel
    {
        private readonly PlayerModel _model = new PlayerModel();

        /// <summary>
        /// 用于显示玩家HP的文本属性。
        /// </summary>
        public TextComponent HpText { get; } = new TextComponent();

        /// <summary>
        /// 增加HP的按钮属性。
        /// </summary>
        public ButtonComponent AddHpButton { get; }

        /// <summary>
        /// 减少HP的按钮属性。
        /// </summary>
        public ButtonComponent SubtractHpButton { get; }

        public PlayerViewModel()
        {
            // 1. 将Model的数据“翻译”为View可用的格式
            _model.Hp.Subscribe((oldValue, newValue) =>
            {
                HpText.Text.Value = $"{newValue}";
            }, true); // 立即执行一次以设置初始文本

            // 2. 定义View的用户操作（命令）
            AddHpButton = new ButtonComponent(() => _model.ChangeHp(10));
            SubtractHpButton = new ButtonComponent(() => _model.ChangeHp(-10));
        }
    }
}
