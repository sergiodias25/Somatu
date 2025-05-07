using DG.Tweening;
using UnityEngine;

public class Clouds : MonoBehaviour
{
    public SpriteRenderer _cloud0;
    public SpriteRenderer _cloud1;
    public SpriteRenderer _cloud2;
    public SpriteRenderer _cloud3;

    bool floatRight = true;
    readonly float cloudSpeed = 150f;
    readonly float cloudSpeedVariance = -50f;

    private void Start()
    {
        int randomizer = Random.Range(0, 2) == 0 ? -1 : 1;
        if (randomizer == -1)
        {
            floatRight = false;
        }
        MoveCloud(_cloud0);
        MoveCloud(_cloud1);
        MoveCloud(_cloud2);
        MoveCloud(_cloud3);
    }

    private void Update()
    {
        if (IsCloudOutOfScreen(_cloud0.transform.position))
        {
            DOTween.Kill(_cloud0.transform, true);
            MoveCloud(_cloud0);
        }
        if (IsCloudOutOfScreen(_cloud1.transform.position))
        {
            DOTween.Kill(_cloud1.transform, true);
            MoveCloud(_cloud1);
        }
        if (IsCloudOutOfScreen(_cloud2.transform.position))
        {
            DOTween.Kill(_cloud2.transform, true);
            MoveCloud(_cloud2);
        }
        if (IsCloudOutOfScreen(_cloud3.transform.position))
        {
            DOTween.Kill(_cloud3.transform, true);
            MoveCloud(_cloud3);
        }
    }

    private bool IsCloudOutOfScreen(Vector3 position)
    {
        if ((position.x < -14.5 && !floatRight) || (position.x > -6.8 && floatRight))
        {
            return true;
        }
        return false;
    }

    public void MoveCloud(SpriteRenderer cloud)
    {
        if (floatRight)
        {
            cloud.transform.position = new Vector3(
                -15f + Random.Range(0, 1f),
                VerticalPositionRandomizer(),
                cloud.transform.position.z
            );
            cloud.transform.DOMove(
                new Vector3(
                    -4,
                    cloud.transform.position.y + Random.Range(-1f, 1f),
                    cloud.transform.position.z
                ),
                cloudSpeed + Random.Range(0f, cloudSpeedVariance)
            );
        }
        else
        {
            cloud.transform.position = new Vector3(
                -6.8f + Random.Range(0, 1f),
                VerticalPositionRandomizer(),
                cloud.transform.position.z
            );
            cloud.transform.DOMove(
                new Vector3(
                    -18,
                    cloud.transform.position.y + Random.Range(-1f, 1f),
                    cloud.transform.position.z
                ),
                cloudSpeed + Random.Range(0f, cloudSpeedVariance)
            );
        }
    }

    private float VerticalPositionRandomizer()
    {
        bool topOfScreen = true;
        int randomizer = Random.Range(0, 2) == 0 ? -1 : 1;
        if (randomizer == -1)
        {
            topOfScreen = false;
        }
        if (topOfScreen)
        {
            return Random.Range(1.5f, 4.5f);
        }
        else
        {
            return Random.Range(-4.5f, -2f);
        }
    }
}
