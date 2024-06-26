using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using INeverFall.Util;
using UnityEngine.AddressableAssets;

namespace INeverFall.Manager
{
    public class ResourceManager : Singleton<ResourceManager>
    {
        public GameObject Instantiate(string key)
        {
            return Addressables.InstantiateAsync(key).WaitForCompletion();
        }
    }
}
