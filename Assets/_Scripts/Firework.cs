using UnityEngine;
using static UnityEngine.ParticleSystem;

namespace Somatu.Assets._Scripts
{
    public class Firework : MonoBehaviour
    {
        [SerializeField]
        private ParticleSystem _explosionParticle;
        private MainModule _mainModuleReference;
        private TrailModule _trailsModuleReference;
        public bool Enabled { get; set; }

        void Awake()
        {
            _mainModuleReference = _explosionParticle.main;
            _trailsModuleReference = _explosionParticle.trails;
        }

        public void Play()
        {
            _mainModuleReference.startDelay = Random.Range(0.1f, 0.9f);
            ChangePosition();
            ChangeColor();
            ChangeTrail();

            if (Enabled)
            {
                _explosionParticle.Play();
            }
        }

        private void ChangePosition()
        {
            _explosionParticle.transform.position = new Vector3(
                Random.Range(-12.5f, -8.5f),
                Random.Range(1.5f, 3.5f),
                _explosionParticle.transform.position.z
            );
        }

        private void ChangeColor()
        {
            _mainModuleReference.startColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        }

        private void ChangeTrail()
        {
            _trailsModuleReference.lifetime = new MinMaxCurve(Random.Range(0.2f, 1f));
            _trailsModuleReference.widthOverTrail = new MinMaxCurve(Random.Range(0.2f, 1.5f));
        }

        public void OnParticleSystemStopped()
        {
            if (Enabled)
            {
                Play();
            }
        }
    }
}
