using UnityEngine;

public class Node : MonoBehaviour
{
    private Block _blockInNode;

    [SerializeField]
    private SpriteRenderer _sprite;

    internal void Init(int i, int j, Block block, string parentName)
    {
        _sprite.color = Constants.UnselectedBlock;
        name = string.Concat("Node_", i, "_", j);
        SetBlockInNode(block);
        transform.SetParent(GameObject.Find(parentName).transform);
    }

    public void SetBlockInNode(Block block)
    {
        //if (_blockInNode != null) {
        //   Debug.Log("node '" + name + "' changed from '" + _blockInNode.name + "' to '" + block.name + "'");
        //}
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
