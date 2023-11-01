using UnityEngine;
using TMPro;
using System.Linq;

public class Block : MonoBehaviour
{
    public int Value;

    [SerializeField]
    private TextMeshPro _text;

    [SerializeField]
    private SpriteRenderer _sprite;
    private Vector3 mousePositionOffset;
    private Node _originalNode;
    private bool _isInteractible = false;
    public bool IsSelected = false;

    public Node GetNode()
    {
        return _originalNode;
    }

    public Block Init(int value, bool interactible, Node node)
    {
        Value = value;
        _text.text = value.ToString();
        _isInteractible = interactible;
        gameObject.name = string.Concat("Block_", value.ToString());
        if (!interactible)
        {
            _sprite.color = new Color32(245, 245, 159, 255);
            _text.color = new Color32(0, 0, 0, 255);
        }
        _originalNode = node;
        return this;
    }

    private void OnMouseDown()
    {
        if (_isInteractible)
        {
            if (Constants.SelectedControlMethod == Constants.ControlMethod.Drag)
            {
                UpdateOffsetPosition();
                _originalNode = GetNodeTouched();
                _text.alpha = 0.4f;
            }
            else if (Constants.SelectedControlMethod == Constants.ControlMethod.DoubleClick)
            {
                Node nodeClickedOn = GetNodeTouched();
                Block selectedBlock = FindObjectOfType<GameManager>().GetSelectedBlock();
                if (selectedBlock == null)
                {
                    FindObjectOfType<GameManager>().ResetSelectedBlock();
                    IsSelected = true;
                    _sprite.color = Constants.SelectedBlock;
                }
                else if (selectedBlock._originalNode.name == _originalNode.name)
                {
                    FindObjectOfType<GameManager>().ResetSelectedBlock();
                    _sprite.color = Constants.UnselectedBlock;
                }
                else if (selectedBlock._originalNode.name != _originalNode.name)
                {
                    if (nodeClickedOn != null && nodeClickedOn.name != selectedBlock.GetNode().name)
                    {
                        var tempPosition = selectedBlock.transform.position;
                        var tempNode = selectedBlock._originalNode;

                        selectedBlock.transform.position = transform.position;
                        selectedBlock._sprite.color = Constants.UnselectedBlock;
                        selectedBlock._originalNode = _originalNode;
                        selectedBlock._originalNode.SetBlockInNode(selectedBlock);
                        selectedBlock.IsSelected = false;

                        transform.position = tempPosition;
                        _originalNode = tempNode;
                        _originalNode.SetBlockInNode(this);

                        FindObjectOfType<GameManager>().ResetSelectedBlock();
                        FindObjectOfType<GameManager>().CheckResult(true);
                    }
                }
            }
        }
    }

    private void UpdateOffsetPosition()
    {
        mousePositionOffset = gameObject.transform.position - GetWorldMousePosition();
    }

    private void OnMouseDrag()
    {
        if (_isInteractible && Constants.SelectedControlMethod == Constants.ControlMethod.Drag)
        {
            transform.position = GetWorldMousePosition() + mousePositionOffset;
        }
    }

    private void OnMouseUp()
    {
        if (_isInteractible && Constants.SelectedControlMethod == Constants.ControlMethod.Drag)
        {
            _text.alpha = 1f;
            Node nodeWhereBlockIsDropped = GetNodeTouched();
            if (nodeWhereBlockIsDropped != null)
            {
                nodeWhereBlockIsDropped.GetBlockInNode().transform.position = _originalNode
                    .transform
                    .position;
                var tempBlock = _originalNode.GetBlockInNode();
                _originalNode.SetBlockInNode(nodeWhereBlockIsDropped.GetBlockInNode());
                nodeWhereBlockIsDropped.SetBlockInNode(tempBlock);
                gameObject.transform.position = nodeWhereBlockIsDropped.transform.position;
                UpdateOffsetPosition();
                FindObjectOfType<GameManager>().CheckResult(true);
            }
            else
            {
                UpdateOffsetPosition();
                gameObject.transform.position = _originalNode.transform.position;
            }
        }
    }

    private static Node GetNodeTouched()
    {
        RaycastHit[] hits;
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        hits = Physics.RaycastAll(camRay);
        if (hits != null && hits.Length > 0)
        {
            //Debug.Log("Touched node " + hits.First().collider.transform.name);
            if (hits.First().collider.GetComponent<Node>().GetBlockInNode()._isInteractible)
            {
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
    private Vector3 GetWorldMousePosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public void DisableInteraction()
    {
        _isInteractible = false;
    }

    public void UpdateColor(Color newColor)
    {
        _sprite.color = newColor;
    }
}
