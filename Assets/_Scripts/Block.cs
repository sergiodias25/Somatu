using UnityEngine;
using TMPro;
using System.Linq;

public class Block : MonoBehaviour
{
    public int Value;
    [SerializeField] public TextMeshPro _text;
    [SerializeField] public SpriteRenderer _sprite;
    Vector3 mousePositionOffset;
    Node originalNode;
    private bool isInteractible = false;

    public Node GetNode() {
        return originalNode;
    }
    
    public Block Init(int value, bool interactible, Node node) {
        Value = value;
        _text.text = value.ToString();
        isInteractible = interactible;
        gameObject.name = string.Concat("Block_", value.ToString());
        if (!interactible) {
            _sprite.color = new Color32(245, 245, 159, 255);
            _text.color = new Color32(0, 0, 0, 255);
        }
        originalNode = node;
        return this;
    }

   private void OnMouseDown()
    {
        if (isInteractible)
        {
            UpdateOffsetPosition();
            originalNode = GetNodeTouched();
            _text.alpha = 0.4f;
        }
    }

    private void UpdateOffsetPosition()
    {
        mousePositionOffset = gameObject.transform.position - GetWorldMousePosition();
    }

    private void OnMouseDrag()
   {
        if (isInteractible)
        {
            transform.position = GetWorldMousePosition() + mousePositionOffset;
        }
   }

   private void OnMouseUp()
   {
        if (isInteractible)
        {
            _text.alpha = 1f;
            Node nodeWhereBlockIsDropped = GetNodeTouched();
            if (nodeWhereBlockIsDropped != null)
            {
                nodeWhereBlockIsDropped.GetBlockInNode().transform.position = originalNode.transform.position;
                var tempBlock = originalNode.GetBlockInNode();
                originalNode.SetBlockInNode(nodeWhereBlockIsDropped.GetBlockInNode());
                nodeWhereBlockIsDropped.SetBlockInNode(tempBlock);
                gameObject.transform.position = nodeWhereBlockIsDropped.transform.position;
                UpdateOffsetPosition();
                FindObjectOfType<GameManager>().CheckResult();
            }
            else
            {
                UpdateOffsetPosition();
                gameObject.transform.position = originalNode.transform.position;
            }
        }
   }

    private static Node GetNodeTouched()
    {
        RaycastHit[] hits;
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        hits = Physics.RaycastAll(camRay);
        if (hits != null && hits.Length > 0) {
            //Debug.Log("Touched node " + hits.First().collider.transform.name);
            if (hits.First().collider.GetComponent<Node>().GetBlockInNode().isInteractible) {
                return hits.First().collider.GetComponent<Node>();
            }
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

   public void DisableInteraction() {
        isInteractible = false;
   }
}
