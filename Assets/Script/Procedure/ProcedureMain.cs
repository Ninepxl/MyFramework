using UnityEngine;
using HachiFramework;
namespace ActGame
{
    public class ProcedureMain : FsmState<ProcedureComponent>
    {
        public override void OnEnter(IFsm<ProcedureComponent> fsm)
        {
            base.OnEnter(fsm);
            Debug.Log("===进入游戏主流程===");
        }
    }
}