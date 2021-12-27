/*using System.Threading.Tasks;
using UnityEngine;

namespace Utility
{
    public static class ResourceLoader
    {
        public static async Task<GameObject> Load(string objectName, Transform parentTransform)
        {
            if (string.IsNullOrEmpty(objectName))
            {
                Debug.LogWarning($"Trying to load the entire contents of the Resources folder."+
                                        $" ABORT");
                return null;
            }
            var resourceRequest = Resources.LoadAsync(objectName);
                await ResourceRequestAwaiter(resourceRequest);
            return InstantiateObject(resourceRequest, parentTransform);
        }
        private static async Task ResourceRequestAwaiter(ResourceRequest resourceRequest)
        { 
            while (!resourceRequest.isDone)
                await Task.Yield();
        }
        private static GameObject InstantiateObject(ResourceRequest resourceRequest, Transform parent)
        {
            var gameObject = (GameObject)resourceRequest.asset;
            return gameObject == null ? null : Object.Instantiate(gameObject, parent);
        }
    }
}*/