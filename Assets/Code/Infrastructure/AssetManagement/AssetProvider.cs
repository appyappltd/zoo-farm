using UnityEngine;
using Object = UnityEngine.Object;

namespace Infrastructure.AssetManagement
{
    public class AssetProvider : IAssetProvider
    {
        public GameObject Instantiate(string path, Vector3 at)
        {
            var prefab = Resources.Load<GameObject>(path);
            return Object.Instantiate(prefab, at, Quaternion.identity);
        }

        public GameObject Instantiate(string path, Vector3 at, Quaternion rotation)
        {
            var prefab = Resources.Load<GameObject>(path);
            return Object.Instantiate(prefab, at, rotation);
        }
        
        public GameObject Instantiate(string path, Transform inside)
        {
            var prefab = Resources.Load<GameObject>(path);
            return Object.Instantiate(prefab, inside);
        }

        public GameObject Instantiate(string path)
        {
            var prefab = Resources.Load<GameObject>(path);
            return Object.Instantiate(prefab);
        }

        public Sprite LoadSprite(string path)
        {
            var sprite = Resources.Load<Sprite>(path);
            return sprite;
        }

        public void Cleanup() { }
    }
}