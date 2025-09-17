using Cysharp.Threading.Tasks;
using Frame;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace ActGame
{
    public class ProcedureLaunch : FsmState<ProcedureComponent>
    {
        public override void OnInit(IFsm<ProcedureComponent> fsm)
        {
            base.OnInit(fsm);
        }

        public override void OnEnter(IFsm<ProcedureComponent> fsm)
        {
            base.OnEnter(fsm);
            Debug.Log("=== 进入ProcedureLaunch - 开始预加载 ===");
            InitializeGame(fsm).Forget();
        }

        private async UniTask InitializeGame(IFsm<ProcedureComponent> fsm)
        {
            try
            {
                await GameEntry.Asset.InitializeAsync();
                // ValidateComponents();
                await PreloadCoreAssets();
                Debug.Log("=== 预加载完成，切换到主游戏流程 ===");
                fsm.Owner.procedureFsm.ChangeState<ProcedureMain>();
            }
            catch (System.Exception e)
            {
                Debug.LogError($"❌ 初始化失败: {e.Message}");
                Debug.LogError($"堆栈跟踪: {e.StackTrace}");
            }
        }

        private void ValidateComponents()
        {
            // 验证关键组件
            bool hasErrors = false;

            if (GameEntry.Asset == null)
            {
                Debug.LogError("❌ Asset 组件未初始化");
                hasErrors = true;
            }

            if (GameEntry.GameObjectPool == null)
            {
                Debug.LogWarning("⚠️ GameObjectPool 组件未找到");
            }

            if (GameEntry.Message == null)
            {
                Debug.LogWarning("⚠️ Message 组件未找到");
            }

            if (GameEntry.procedureManager == null)
            {
                Debug.LogError("❌ ProcedureManager 组件未初始化");
                hasErrors = true;
            }

            if (!hasErrors)
            {
                Debug.Log("✓ 所有关键组件验证通过");
            }
        }

        private async UniTask PreloadCoreAssets()
        {
            await Addressables.LoadSceneAsync("MainScene", UnityEngine.SceneManagement.LoadSceneMode.Additive);
            // TODO: 预加载游戏必需的核心资源
            // 例如:
            // - UI相关的资源
            // - 音效资源
            // - 玩家角色预制体
            // - 核心游戏逻辑资源
        }

        public override void OnLeave(IFsm<ProcedureComponent> fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
            Debug.Log("=== 离开ProcedureLaunch ===");
        }
    }
}