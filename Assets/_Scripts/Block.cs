using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.EventSystems;

public class Block : MonoBehaviour
{
    public int Value;

    [SerializeField]
    private TextMeshPro _text;

    [SerializeField]
    private SpriteRenderer _sprite;
    private GameManager _gameManager;
    private AudioManager _audioManager;
    private Vector3 mousePositionOffset;
    private Node _originalNode;
    private bool _isInteractible = false;
    public bool IsSelected = false;
    private Node _lastHoveredNode;

    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _audioManager = FindObjectOfType<AudioManager>();
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
        if (GetNodeTouched() != null && _isInteractible)
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
                        SwitchNodes(_originalNode, selectedBlock._originalNode);
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
        Node nodeWhereBlockIsHovering = GetNodeTouched();
        if (
            nodeWhereBlockIsHovering != null
            && _isInteractible
            && Constants.SelectedControlMethod == Constants.ControlMethod.Drag
        )
        {
            transform.position = GetWorldMousePosition() + mousePositionOffset;
            if (
                nodeWhereBlockIsHovering != null
                && nodeWhereBlockIsHovering.name != _originalNode.name
            )
            {
                _gameManager.ResetAllBlocksOpacity();
                UpdateOpacity(nodeWhereBlockIsHovering.GetBlockInNode(), 0.2f);
                _lastHoveredNode = nodeWhereBlockIsHovering;
            }
        }
        else if (nodeWhereBlockIsHovering == null && _lastHoveredNode != null)
        {
            UpdateOpacity(_lastHoveredNode.GetBlockInNode(), 1f);
            _lastHoveredNode = null;
        }
        else if (_lastHoveredNode != null)
        {
            UpdateOpacity(_lastHoveredNode.GetBlockInNode(), 1f);
        }
    }

    private void OnMouseUp()
    {
        if (
            GetNodeTouched() != null
            && _isInteractible
            && Constants.SelectedControlMethod == Constants.ControlMethod.Drag
        )
        {
            Node nodeWhereBlockIsDropped = GetNodeTouched();
            if (nodeWhereBlockIsDropped != null)
            {
                UpdateOpacity(nodeWhereBlockIsDropped.GetBlockInNode(), 1f);

                if (nodeWhereBlockIsDropped != _originalNode)
                {
                    _gameManager.StoreUndoData(_originalNode, nodeWhereBlockIsDropped);
                    SwitchNodes(_originalNode, nodeWhereBlockIsDropped);
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
        else if (GetNodeTouched() == null)
        {
            gameObject.transform.position = _originalNode.transform.position;
        }
    }

    private static Node GetNodeTouched()
    {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (
            Physics.Raycast(camRay.origin, camRay.direction, out hitInfo)
            && hitInfo.collider.GetComponent<Node>() != null
        )
        {
            if (
                hitInfo.collider.GetComponent<Node>().GetBlockInNode()._isInteractible
                && !EventSystem.current.IsPointerOverGameObject()
            )
            {
                return hitInfo.collider.GetComponent<Node>();
            }
        }
        return null;
    }

    private Vector3 GetWorldMousePosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public void DisableInteraction()
    {
        _isInteractible = false;
    }

    public bool IsInteractable()
    {
        return _isInteractible;
    }

    public static void UpdateOpacity(Block block, float value)
    {
        block._text.alpha = value;
    }

    public static void SwitchNodes(Node firstNode, Node secondNode)
    {
        Node _tempNode = firstNode;
        var tempBlock = firstNode.GetBlockInNode();

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
