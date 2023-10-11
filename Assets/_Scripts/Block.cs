using UnityEngine;
using TMPro;
using System.Linq;

public class Block : MonoBehaviour
{
    public int Value;
    [SerializeField] private TextMeshPro _text;
    Vector3 mousePositionOffset;
    Node originalNode;
    
    public Block Init(int value) {
        Value = value;
        _text.text = value.ToString();
        gameObject.name = string.Concat("Block_", value.ToString());
        return this;
    }

   private void OnMouseDown()
    {
        UpdateOffsetPosition();
        originalNode = GetNodeTouched();
        _text.alpha = 0.4f;
    }

    private void UpdateOffsetPosition()
    {
        mousePositionOffset = gameObject.transform.position - GetWorldMousePosition();
    }

    private void OnMouseDrag()
   {
        transform.position = GetWorldMousePosition() + mousePositionOffset;
   }

   private void OnMouseUp()
   {
        _text.alpha = 1f;
        Node nodeTouched = GetNodeTouched();
        if (nodeTouched != null) {
            nodeTouched.GetBlockInNode().transform.position = originalNode.transform.position;
            UpdateOffsetPosition();
            nodeTouched.GetBlockInNode().originalNode = originalNode;
            nodeTouched.SetBlockInNode(gameObject.GetComponent<Block>());
            gameObject.transform.position = nodeTouched.transform.position;
            originalNode = nodeTouched;
        } else {
            UpdateOffsetPosition();
            gameObject.transform.position = originalNode.transform.position;
        }
   }

    private static Node GetNodeTouched()
    {
        RaycastHit[] hits;
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        hits = Physics.RaycastAll(camRay);
        if (hits != null && hits.Length > 0) {
            //Debug.Log("Touched node " + hits.First().collider.transform.name);
            return hits.First().collider.GetComponent<Node>();
        }
        return null;
    }
    /*
    private static Block GetBlockTouched() {
    RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null)
            {
                if (hit.collider != null)
                {
                    Debug.Log("Touched Block " + hit.collider.transform.name);
                    return hit.collider.GetComponent<Block>();
                }
            }
        return null;
    }
    */
    private Vector3 GetWorldMousePosition() {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
   }
}
