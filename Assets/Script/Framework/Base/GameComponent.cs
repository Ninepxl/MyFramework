using UnityEngine;

namespace Frame
{
    public class GameComponent : MonoBehaviour
    {
        protected virtual void Awake()
        {
            GameEntry.RegisterComponent(this);
            Debug.Log($"框架模块注册完成: {this.GetType()}");
        }

        protected virtual void OnDestroy()
        {
            
        }
    }
}