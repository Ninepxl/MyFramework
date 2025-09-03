using System;
using UnityEngine;

namespace Frame
{
    public class GameComponent : MonoBehaviour
    {
        protected virtual void Awake()
        {
            GameEntry.RegisterComponent(this);
        }

        protected virtual void OnDestroy()
        {
            
        }
    }
}