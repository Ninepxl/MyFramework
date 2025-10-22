//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System;
using System.Collections.Generic;
namespace HachiFramework
{
    public sealed class Fsm<T> : IFsm<T> where T : class
    {
        private T m_Owner;
        public T Owner => m_Owner;
        private readonly Dictionary<Type, FsmState<T>> m_States;
        private FsmState<T> m_CurrentState;
        private bool m_IsDestroyed;
        private string m_Name;

        /// <summary>
        /// 初始化有限状态机的新实例。
        /// </summary>
        public Fsm()
        {
            m_Owner = null;
            m_States = new Dictionary<Type, FsmState<T>>();
            m_CurrentState = null;
            m_IsDestroyed = true;
        }

        /// <summary>
        /// 获取有限状态机持有者类型。
        /// </summary>
        public Type OwnerType => typeof(T);

        /// <summary>
        /// 获取有限状态机中状态的数量。
        /// </summary>
        public int FsmStateCount => m_States.Count;

        /// <summary>
        /// 获取有限状态机是否正在运行。
        /// </summary>
        public bool IsRunning => m_CurrentState != null;

        /// <summary>
        /// 获取有限状态机是否被销毁。
        /// </summary>
        public bool IsDestroyed => m_IsDestroyed;

        /// <summary>
        /// 获取当前有限状态机状态。
        /// </summary>
        public FsmState<T> CurrentState
        {
            get
            {
                return m_CurrentState;
            }
        }

        /// <summary>
        /// 获取当前有限状态机状态名称。
        /// </summary>
        public string CurrentStateName
        {
            get
            {
                return m_CurrentState != null ? m_CurrentState.GetType().FullName : null;
            }
        }
        public string Name => m_Name;

        /// <summary>
        /// 创建有限状态机。
        /// </summary>
        /// <param name="name">有限状态机名称。</param>
        /// <param name="owner">有限状态机持有者。</param>
        /// <param name="states">有限状态机状态集合。</param>
        /// <returns>创建的有限状态机。</returns>
        public static Fsm<T> Create(string name, T owner, params FsmState<T>[] states)
        {
            if (owner == null)
            {
                throw new Exception("FSM owner is invalid.");
            }

            if (states == null || states.Length < 1)
            {
                throw new Exception("FSM states is invalid.");
            }

            Fsm<T> fsm = new Fsm<T>();
            fsm.m_Name = name;
            fsm.m_Owner = owner;
            fsm.m_IsDestroyed = false;
            foreach (FsmState<T> state in states)
            {
                if (state == null)
                {
                    throw new Exception("FSM state is invalid.");
                }

                Type stateType = state.GetType();
                if (fsm.m_States.ContainsKey(stateType))
                {
                    throw new Exception(string.Format("FSM '{0}' state '{1}' is already exist.", name, stateType.FullName));
                }

                fsm.m_States.Add(stateType, state);
                state.OnInit(fsm);
            }

            return fsm;
        }

        /// <summary>
        /// 获取有限状态机状态。
        /// </summary>
        /// <typeparam name="TState">要获取的有限状态机状态类型。</typeparam>
        /// <returns>要获取的有限状态机状态。</returns>
        public TState GetState<TState>() where TState : FsmState<T>
        {
            FsmState<T> state = null;
            if (m_States.TryGetValue(typeof(TState), out state))
            {
                return (TState)state;
            }

            return null;
        }

        /// <summary>
        /// 获取有限状态机状态。
        /// </summary>
        /// <param name="state">状态的类型</param>
        /// <returns></returns>
        public FsmState<T> GetState(Type state)
        {
            FsmState<T> fsmState = null;
            if (m_States.TryGetValue(state, out fsmState))
            {
                return fsmState;
            }
            return null;
        }


        /// 开始有限状态机。
        /// </summary>
        /// <typeparam name="TState">要开始的有限状态机状态类型。</typeparam>
        public void Start<TState>() where TState : FsmState<T>
        {
            if (IsRunning)
            {
                throw new Exception("FSM is already running.");
            }

            FsmState<T> state = GetState<TState>();
            if (state == null)
            {
                throw new Exception(string.Format("FSM '{0}' can not start state '{1}' which is not exist.", m_Name, typeof(TState).FullName));
            }

            m_CurrentState = state;
            m_CurrentState.OnEnter(this);
        }
        /// <summary>
        /// 开始有限状态机。
        /// </summary>
        /// <param name="stateType">要开始的有限状态机状态类型。</param>
        public void Start(Type stateType)
        {
            if (IsRunning)
            {
                throw new Exception("FSM is already running.");
            }

            FsmState<T> state = GetState(stateType);
            if (state == null)
            {
                throw new Exception(string.Format("FSM '{0}' can not start state '{1}' which is not exist.", m_Name, stateType.FullName));
            }

            m_CurrentState = state;
            m_CurrentState.OnEnter(this);
        }

        public void ChangeState<TState>()
        {
            ChangeState(typeof(TState));
        }
        public void ChangeState(Type stateType)
        {
            FsmState<T> state = GetState(stateType);
            m_CurrentState.OnLeave(this, false);
            m_CurrentState = state;
            m_CurrentState.OnEnter(this);
        }
    }
}