using Cysharp.Threading.Tasks;
using Frame;
using UnityEngine;
using UnityEngine.AddressableAssets;
namespace GameDemo
{
    public class GameEntry : MonoBehaviour
    {
        public static AssetHandleComponent Asset;
        public static GameObjectPoolComponent GameObjectPool;
        public static MessageComponent Message;

        private void Start()
        {
            Asset = Frame.GameEntry.GetComponent<AssetHandleComponent>();
            GameObjectPool = Frame.GameEntry.GetComponent<GameObjectPoolComponent>();
            Message = Frame.GameEntry.GetComponent<MessageComponent>();
            Init().Forget();
        }

        public async UniTask Init()
        {
            await Asset.InitializeAsync();
            await Addressables.LoadSceneAsync("MainScene", UnityEngine.SceneManagement.LoadSceneMode.Additive);
        }
    }
}