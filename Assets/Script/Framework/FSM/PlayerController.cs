using Frame;
using UnityEngine;
namespace GameDemo
{
    public class PlayerController : MonoBehaviour, IFSM
    {
        public IStateBase curState { get; set; }

        public void HandleInput()
        {
            curState = curState.HandleInput();
        }

        private void Awake()
        {
            curState = new PlayerIdleState();
            curState.OnEnter();
        }
        void Update()
        {
            // HandleInput(); 
            curState?.OnUpdate();
        }
        private void Start()
        {

        }
    }
}