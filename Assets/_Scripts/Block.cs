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
    private Block _lastHoveredBlock;

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
        transform.SetParent(node.transform);
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
                        selectedBlock.transform.SetParent(_originalNode.transform);
                        selectedBlock._originalNode.SetBlockInNode(selectedBlock);
                        selectedBlock.IsSelected = false;

                        transform.position = tempPosition;
                        transform.SetParent(tempNode.transform);
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
            Node nodeWhereBlockIsHovering = GetNodeTouched();
            if (
                nodeWhereBlockIsHovering != null
                && nodeWhereBlockIsHovering.name != _originalNode.name
            )
            {
                UpdateOpacity(nodeWhereBlockIsHovering.GetBlockInNode(), 0.2f);
                if (nodeWhereBlockIsHovering != null) { }
                _lastHoveredBlock = nodeWhereBlockIsHovering.GetBlockInNode();
            }
            else
            {
                if (_lastHoveredBlock != null)
                {
                    UpdateOpacity(_lastHoveredBlock, 1f);
                    _lastHoveredBlock = null;
                }
            }
        }
    }

    private void OnMouseUp()
    {
        if (_isInteractible && Constants.SelectedControlMethod == Constants.ControlMethod.Drag)
        {
            Node nodeWhereBlockIsDropped = GetNodeTouched();
            if (nodeWhereBlockIsDropped != null)
            {
                nodeWhereBlockIsDropped.GetBlockInNode().transform.position = _originalNode
                    .transform
                    .position;
                UpdateOpacity(nodeWhereBlockIsDropped.GetBlockInNode(), 1f);
                var tempBlock = _originalNode.GetBlockInNode();

                _originalNode.SetBlockInNode(nodeWhereBlockIsDropped.GetBlockInNode());
                _originalNode.GetBlockInNode().transform.SetParent(_originalNode.transform);

                nodeWhereBlockIsDropped.SetBlockInNode(tempBlock);
                nodeWhereBlockIsDropped
                    .GetBlockInNode()
                    .transform.SetParent(nodeWhereBlockIsDropped.transform);
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

    private void UpdateOpacity(Block block, float value)
    {
        block._text.alpha = value;
    }
}
