using Frame;

namespace Game.Models
{
    /// <summary>
    /// Model层：负责最核心、最纯粹的数据和业务逻辑。
    /// 它不关心数据如何显示，只关心数据本身和其规则。
    /// </summary>
    public class PlayerModel
    {
        /// <summary>
        /// 玩家的生命值。这是一个可观察属性，当值变化时会通知订阅者。
        /// </summary>
        public BindableProperty<int> Hp { get; } = new(100);

        /// <summary>
        /// 业务逻辑方法：改变生命值。
        /// 可以在这里加入各种规则，例如Hp不能超过上限，不能低于0等。
        /// </summary>
        /// <param name="amount">要改变的数值，可正可负。</param>
        public void ChangeHp(int amount)
        {
            // 示例规则：生命值上限为200，下限为0
            int newHp = Hp.Value + amount;
            if (newHp > 200) newHp = 200;
            if (newHp < 0) newHp = 0;
            Hp.Value = newHp;
        }
    }
}
