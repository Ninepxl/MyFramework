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
        }

        private void LoadCubeForAddressable()
        {
           AsyncOperationHandle<GameObject> h = Addressables.LoadAssetAsync<GameObject>("MyCube");
            h.Completed += OnLoadCubeCompleted;
        }

        private void OnLoadCubeCompleted(AsyncOperationHandle<GameObject> obj)
        {
            if (obj.Status == AsyncOperationStatus.Succeeded)
            {
                GameObject cube = obj.Result;
                Instantiate(cube, Vector3.zero, Quaternion.identity);
                Debug.Log("Cube loaded successfully.");
            }
            else
            {
                Debug.LogError("Failed to load cube: " + obj.OperationException);
            }
        }
    }
}