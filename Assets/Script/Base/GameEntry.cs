using Frame;
using UnityEngine;
namespace ActGame
{
    public class GameEntry : MonoBehaviour
    {
        public static AssetHandleComponent Asset;
        public static GameObjectPoolComponent GameObjectPool;
        public static MessageComponent Message;
        public static ProcedureComponent procedureManager;
        private void Start()
        {
            Asset = Frame.GameEntry.GetComponent<AssetHandleComponent>();
            GameObjectPool = Frame.GameEntry.GetComponent<GameObjectPoolComponent>();
            Message = Frame.GameEntry.GetComponent<MessageComponent>();
            procedureManager = Frame.GameEntry.GetComponent<ProcedureComponent>();
        }

    }
}