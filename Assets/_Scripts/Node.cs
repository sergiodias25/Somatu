using Assets.Scripts.CustomAnimation;
using CandyCabinets.Components.Colour;
using UnityEngine;

public class Node : MonoBehaviour
{
    private Block _blockInNode;

    [SerializeField]
    private SpriteRenderer _sprite;

    internal static Node Init(Node nodePrefab, int i, int j, string parentName)
    {
        Node node = Instantiate(
            nodePrefab,
            new Vector2(GetAdjustedPosition(i), GetAdjustedPosition(j)),
            Quaternion.identity
        );
        node.name = string.Concat("Node_", i, "_", j);
        node.transform.SetParent(GameObject.Find(parentName).transform);
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
        _sprite.color = newColor;
    }
}
