using System;
using System.Collections.Generic;
using ActGame;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Frame
{
    /// <summary>
    /// 流程状态机拥有者
    /// </summary>
    public class ProcedureComponent : GameComponent
    {
        public Fsm<ProcedureComponent> procedureFsm;
        public FsmState<ProcedureComponent>[] states = new FsmState<ProcedureComponent>[]
        {
            new ProcedureLaunch(),
            new ProcedureMain(),  // 添加这一行
        };
        
        private async UniTask Start()
        {
            procedureFsm = Fsm<ProcedureComponent>.Create("ProcedureFsm", this, states);
            await UniTask.WaitForSeconds(1);
            procedureFsm.Start<ProcedureLaunch>();
        }
    }
}