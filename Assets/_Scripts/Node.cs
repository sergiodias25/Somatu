using Assets.Scripts.CustomAnimation;
using CandyCabinets.Components.Colour;
using DG.Tweening;
using UnityEngine;

public class Node : MonoBehaviour
{
    private Block _blockInNode;

    private bool _isCorrect = false;

    [SerializeField]
    private SpriteRenderer _sprite;

    [SerializeField]
    private SpriteRenderer _resultCornerSpace;

    [SerializeField]
    private SpriteRenderer _shadowSprite;

    internal static Node Init(Node nodePrefab, int i, int j, string parentName)
    {
        Node node = Instantiate(
            nodePrefab,
            new Vector2(GetAdjustedPosition(i), GetAdjustedPosition(j)),
            Quaternion.identity
        );
        node.name = string.Concat("Node_", i, "_", j);
        node.transform.SetParent(GameObject.Find(parentName).transform, true);
        if (parentName == "GeneratedNodes")
        {
            node.transform.localScale = new Vector3(15, 15, 15);
            node._resultCornerSpace.gameObject.SetActive(false);
        }
        else
        {
            node.transform.localScale = new Vector3(14, 14, 14);
            node._shadowSprite.transform.localPosition = new Vector3(
                node._shadowSprite.transform.localPosition.x / 2,
                node._shadowSprite.transform.localPosition.y / 2,
                node._shadowSprite.transform.localPosition.z
            );
        }
        node.UpdateColor(
            ColourManager.Instance.SelectedPalette().Colours[Constants.COLOR_NODE_NEUTRAL]
        );
        CustomAnimation.NodeLoad(node.transform);
        return node;
    }

    private static float GetAdjustedPosition(float value)
    {
        return value switch
        {
            0 => value + 0.15f,
            1 => value + 0.05f,
            2 => value - 0.05f,
            3 => value - 0.15f,
            _ => value,
        };
    }

    public void SetBlockInNode(Block block)
    {
        _blockInNode = block;
    }

    public Block GetBlockInNode()
    {
        return _blockInNode;
    }

    public void UpdateColor(Color newColor)
    {
        _sprite.DOColor(newColor, 0.1f);
    }

    private void OnDestroy()
    {
        DOTween.Kill(this);
        DOTween.Kill(transform);
        DOTween.Kill(_sprite);
        DOTween.Kill(_sprite.transform);
    }

    public Color GetColor()
    {
        return _sprite.color;
    }

    internal void UpdateResultIcon(bool showIcon, bool resultIsCorrect)
    {
        if (showIcon)
        {
            CustomAnimation.AnimateVisualAidSpace(
                _resultCornerSpace.transform,
                _resultCornerSpace.GetComponent<RectTransform>(),
                !resultIsCorrect,
                _isCorrect
            );
        }
        else
        {
            CustomAnimation.AnimateVisualAidSpace(
                _resultCornerSpace.transform,
                _resultCornerSpace.GetComponent<RectTransform>(),
                false,
                _isCorrect
            );
        }
        _isCorrect = resultIsCorrect;
    }

    internal void HideResultIcon()
    {
        _resultCornerSpace.gameObject.SetActive(false);
    }
}
