using UnityEngine;

public class Node : MonoBehaviour
{
    private Block _blockInNode;

    internal void SetName(int i, int j)
    {
        name = string.Concat("Node_", i, "_", j);
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
}
