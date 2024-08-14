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
            _cloud0.transform.DOMoveY(
                _cloud0.transform.position.y + Random.Range(-.75f, .75f),
                1.0f
            );
            _cloud1.transform.DOMoveY(
                _cloud1.transform.position.y + Random.Range(-.75f, .75f),
                1.0f
            );
            _cloud2.transform.DOMoveY(
                _cloud2.transform.position.y + Random.Range(-.75f, .75f),
                1.0f
            );
            _cloud3.transform.DOMoveY(
                _cloud3.transform.position.y + Random.Range(-.75f, .75f),
                1.0f
            );
        }

        public void MoveClouds()
        {
            _cloud0.transform.DOMove(
                GetMovementVector(_cloud0.transform.position.x, _cloud0.transform.position.y),
                .75f
            );
            _cloud1.transform.DOMove(
                GetMovementVector(_cloud1.transform.position.x, _cloud1.transform.position.y),
                .75f
            );
            _cloud2.transform.DOMove(
                GetMovementVector(_cloud2.transform.position.x, _cloud2.transform.position.y),
                .75f
            );
            _cloud3.transform.DOMove(
                GetMovementVector(_cloud3.transform.position.x, _cloud3.transform.position.y),
                .75f
            );
        }

        private Vector3 GetMovementVector(float x, float y)
        {
            return new Vector3(x + Random.Range(-.25f, .25f), y + Random.Range(-.1f, .1f), 0f);
        }
    }
}
