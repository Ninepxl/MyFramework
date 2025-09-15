using Frame;
using UnityEngine;
namespace GameDemo
{
    public class PlayerIdleState : IStateBase
    {
        public IStateBase HandleInput()
        {
           return null; 
        }

        public void OnEnter()
        {
            Debug.Log("Enter Idle State");
        }

        public void OnExit()
        {
            Debug.Log("Exit Idle State");
        }

        public void OnUpdate()
        {
        }
    }
}