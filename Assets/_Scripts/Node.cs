using UnityEngine;

public class Node : MonoBehaviour
{
    public Vector2 Pos => transform.position;
    private Block _blockInNode;
    private int x;
    private int y;

    internal void SetName(int i, int j)
    {
        name = string.Concat("Node_", i, "_", j);
        x = i;
        y = j;
    }

    public void SetBlockInNode(Block block) {
        //if (_blockInNode != null) {
         //   Debug.Log("node '" + name + "' changed from '" + _blockInNode.name + "' to '" + block.name + "'");
        //}
        _blockInNode = block;
    }

    public Block GetBlockInNode() {
        return _blockInNode;
    }
}