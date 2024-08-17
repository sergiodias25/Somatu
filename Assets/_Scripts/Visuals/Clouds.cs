using DG.Tweening;
using UnityEngine;

namespace GridSum.Assets._Scripts.Visuals
{
    public class Clouds : MonoBehaviour
    {
        public SpriteRenderer _cloud0;
        public SpriteRenderer _cloud1;
        public SpriteRenderer _cloud2;
        public SpriteRenderer _cloud3;

        private void Start()
        {
            _cloud0.transform.DOMoveY(_cloud0.transform.position.y + Random.Range(-.5f, .5f), 1.0f);
            _cloud1.transform.DOMoveY(_cloud1.transform.position.y + Random.Range(-.5f, .5f), 1.0f);
            _cloud2.transform.DOMoveY(_cloud2.transform.position.y + Random.Range(-.5f, .5f), 1.0f);
            _cloud3.transform.DOMoveY(_cloud3.transform.position.y + Random.Range(-.5f, .5f), 1.0f);
        }

        public void MoveClouds()
        {
            _cloud0.transform.DOMove(
                GetMovementVector(_cloud0.transform.position.x, _cloud0.transform.position.y),
                GetCloudAnimationDuration()
            );
            _cloud1.transform.DOMove(
                GetMovementVector(_cloud1.transform.position.x, _cloud1.transform.position.y),
                GetCloudAnimationDuration()
            );
            _cloud2.transform.DOMove(
                GetMovementVector(_cloud2.transform.position.x, _cloud2.transform.position.y),
                GetCloudAnimationDuration()
            );
            _cloud3.transform.DOMove(
                GetMovementVector(_cloud3.transform.position.x, _cloud3.transform.position.y),
                GetCloudAnimationDuration()
            );
        }

        private Vector3 GetMovementVector(float x, float y)
        {
            return new Vector3(x + Random.Range(-.08f, .08f), y + Random.Range(-0.02f, 0.02f), 0f);
        }

        private float GetCloudAnimationDuration()
        {
            return 3f;
        }
    }
}
