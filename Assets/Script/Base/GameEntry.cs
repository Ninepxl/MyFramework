using HachiFramework;
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
            Asset = HachiFramework.GameEntry.GetComponent<AssetHandleComponent>();
            GameObjectPool = HachiFramework.GameEntry.GetComponent<GameObjectPoolComponent>();
            Message = HachiFramework.GameEntry.GetComponent<MessageComponent>();
            procedureManager = HachiFramework.GameEntry.GetComponent<ProcedureComponent>();
        }

    }
}