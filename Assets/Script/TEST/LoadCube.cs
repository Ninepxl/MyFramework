using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Script.TEST
{
    public class LoadCube : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                LoadCubeForAddressable();
            }

            if (Input.GetKeyDown((KeyCode.A)))
            {
                Debug.Log("按下A");
            }
        }

        public async void LoadCubeForAddressable()
        {
            var h = Addressables.LoadAssetAsync<GameObject>("MyCube");
            await h.Task;
            Instantiate(h.Result);
        }
    }
}