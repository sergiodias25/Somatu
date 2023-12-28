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
    private GameManager _gameManager;
    private AudioManager _audioManager;
    private AnimationsHandler _animationsHandler;
    private Vector3 mousePositionOffset;
    private Node _originalNode;
    private bool _isInteractible = false;
    public bool IsSelected = false;
    private Block _lastHoveredBlock;

    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _audioManager = FindObjectOfType<AudioManager>();
        _animationsHandler = FindObjectOfType<AnimationsHandler>();
    }

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
            _text.color = new Color32(0, 0, 0, 255);
        }
        _originalNode = node;
        transform.SetParent(node.transform);
        return this;
    }

    private void OnMouseDown()
    {
        _animationsHandler.RestoreGameplayBar();
        if (_isInteractible)
        {
            FindObjectOfType<GameManager>().RemoveHints();
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
                    nodeClickedOn.UpdateColor(Constants.SelectedBlock);
                    _audioManager.PlaySFX(_audioManager.DropBlock);
                }
                else if (selectedBlock._originalNode.name == _originalNode.name)
                {
                    FindObjectOfType<GameManager>().ResetSelectedBlock();
                    selectedBlock._originalNode.UpdateColor(Constants.UnselectedBlock);
                    _audioManager.PlaySFX(_audioManager.DropBlockUndo);
                }
                else if (selectedBlock._originalNode.name != _originalNode.name)
                {
                    if (nodeClickedOn != null && nodeClickedOn.name != selectedBlock.GetNode().name)
                    {
                        var tempNode = selectedBlock._originalNode;

                        selectedBlock._originalNode.UpdateColor(Constants.UnselectedBlock);
                        _gameManager.StoreUndoData(tempNode, _originalNode);
                        selectedBlock.SwitchToNode(_originalNode, selectedBlock._originalNode);
                        selectedBlock.IsSelected = false;

                        FindObjectOfType<GameManager>().ResetSelectedBlock();
                        FindObjectOfType<GameManager>().CheckResult(true);
                        _audioManager.PlaySFX(_audioManager.DropBlock);
                    }
                }
            }
        }
    }

    private void UpdateOffsetPosition()
    {
        mousePositionOffset = _originalNode.transform.position - GetWorldMousePosition();
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
                _gameManager.ResetAllBlocksOpacity();
                UpdateOpacity(nodeWhereBlockIsHovering.GetBlockInNode(), 0.2f);
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
                UpdateOpacity(nodeWhereBlockIsDropped.GetBlockInNode(), 1f);

                if (nodeWhereBlockIsDropped != _originalNode)
                {
                    _gameManager.StoreUndoData(_originalNode, nodeWhereBlockIsDropped);
                    SwitchToNode(_originalNode, nodeWhereBlockIsDropped);
                    FindObjectOfType<GameManager>().CheckResult(true);
                    _audioManager.PlaySFX(_audioManager.DropBlock);
                }
                else
                {
                    _audioManager.PlaySFX(_audioManager.DropBlockUndo);
                    gameObject.transform.position = _originalNode.transform.position;
                }
            }
            else
            {
                gameObject.transform.position = _originalNode.transform.position;
                _audioManager.PlaySFX(_audioManager.DropBlockUndo);
            }
            UpdateOffsetPosition();
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

    public static void UpdateOpacity(Block block, float value)
    {
        block._text.alpha = value;
    }

    public void SwitchToNode(Node firstNode, Node secondNode)
    {
        Node _tempNode = firstNode;
        var tempBlock = firstNode.GetBlockInNode();
        Debug.Log("First node " + firstNode.name);
        Debug.Log("Second node " + secondNode.name);

        firstNode.GetBlockInNode().transform.position = secondNode.transform.position;
        firstNode.SetBlockInNode(secondNode.GetBlockInNode());
        firstNode.GetBlockInNode().transform.SetParent(firstNode.transform);
        firstNode.GetBlockInNode()._originalNode = firstNode;

        secondNode.GetBlockInNode().transform.position = _tempNode.transform.position;
        secondNode.SetBlockInNode(tempBlock);
        secondNode.GetBlockInNode().transform.SetParent(secondNode.transform);
        secondNode.GetBlockInNode()._originalNode = secondNode;
    }
}
