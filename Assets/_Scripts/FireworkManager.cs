using Somatu.Assets._Scripts;
using UnityEngine;

namespace Assets._Scripts
{
    public class FireworkManager : MonoBehaviour
    {
        [SerializeField]
        private Firework _fireworkPrefab;
        private Firework _firework1;
        private Firework _firework2;

        public void ThrowFireworks()
        {
            if (!_fireworkPrefab.isActiveAndEnabled)
            {
                _firework1 = Instantiate(
                    _fireworkPrefab,
                    new Vector3(-10.5f, 2.5f, -6f),
                    Quaternion.identity
                );
                _firework1.transform.SetParent(transform, true);

                _firework2 = Instantiate(
                    _fireworkPrefab,
                    new Vector3(-10.5f, 2.5f, -6f),
                    Quaternion.identity
                );
                _firework2.transform.SetParent(transform, true);
            }
            _firework1.Enabled = true;
            _firework1.Play();

            _firework2.Enabled = true;
            _firework2.Play();
        }

        public void StopFireworks()
        {
            if (_firework1 != null)
            {
                _firework1.Enabled = false;
            }
            if (_firework2 != null)
            {
                _firework2.Enabled = false;
            }
        }
    }
}
