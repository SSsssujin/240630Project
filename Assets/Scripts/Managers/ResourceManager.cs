using UnityEngine;
using INeverFall.Util;
using UnityEngine.AddressableAssets;

namespace INeverFall.Manager
{
    public class ResourceManager : Singleton<ResourceManager>
    {
        public GameObject Instantiate(string key, Transform parent = null)
        {
            return Addressables.InstantiateAsync(key, parent).WaitForCompletion();
        }
    }
}
