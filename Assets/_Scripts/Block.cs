using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using CandyCabinets.Components.Colour;
using Assets.Scripts.CustomAnimation;
using DG.Tweening;

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
            _text.fontSize = 5.5f;
            _text.ForceMeshUpdate();
        }
        _originalNode = node;
        transform.SetParent(node.transform);
        _text.color = interactible
            ? ColourManager.Instance.SelectedPalette().Colours[8]
            : ColourManager.Instance.SelectedPalette().Colours[1];
        return this;
    }

    private void OnMouseDown()
    {
        Node nodeTouched = GetNodeTouched();
        if (nodeTouched != null && _isInteractible)
        {
            FindObjectOfType<GameManager>().RemoveHints();
            if (_gameManager.SavedGameData.SettingsData.ControlMethodDrag)
            {
                UpdateOffsetPosition();
                _originalNode = nodeTouched;
                CustomAnimation.NumberClicked(transform);
            }
            else
            {
                Node nodeClickedOn = nodeTouched;
                Block selectedBlock = FindObjectOfType<GameManager>().GetSelectedBlock();
                if (selectedBlock == null)
                {
                    FindObjectOfType<GameManager>().ResetSelectedBlock();
                    IsSelected = true;
                    nodeClickedOn.UpdateColor(ColourManager.Instance.SelectedPalette().Colours[5]);
                    _audioManager.PlaySFX(_audioManager.DropBlock);
                }
                else if (selectedBlock._originalNode.name == _originalNode.name)
                {
                    FindObjectOfType<GameManager>().ResetSelectedBlock();
                    selectedBlock._originalNode.UpdateColor(
                        ColourManager.Instance.SelectedPalette().Colours[2]
                    );
                    _audioManager.PlaySFX(_audioManager.DropBlockUndo);
                }
                else if (selectedBlock._originalNode.name != _originalNode.name)
                {
                    if (nodeClickedOn != null && nodeClickedOn.name != selectedBlock.GetNode().name)
                    {
                        var tempNode = selectedBlock._originalNode;

                        selectedBlock._originalNode.UpdateColor(
                            ColourManager.Instance.SelectedPalette().Colours[2]
                        );
                        _gameManager.StoreUndoData(tempNode, _originalNode);
                        SwitchNodes(_originalNode, selectedBlock._originalNode);
                        selectedBlock.IsSelected = false;
                        _audioManager.PlaySFX(_audioManager.DropBlock);

                        FindObjectOfType<GameManager>().ResetSelectedBlock();
                        FindObjectOfType<GameManager>().CheckResult(true);
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
            && _gameManager.SavedGameData.SettingsData.ControlMethodDrag
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
        Node nodeTouched = GetNodeTouched();
        if (
            nodeTouched != null
            && _isInteractible
            && _gameManager.SavedGameData.SettingsData.ControlMethodDrag
        )
        {
            Node nodeWhereBlockIsDropped = nodeTouched;
            if (nodeWhereBlockIsDropped != null)
            {
                UpdateOpacity(nodeWhereBlockIsDropped.GetBlockInNode(), 1f);

                if (nodeWhereBlockIsDropped != _originalNode)
                {
                    _gameManager.StoreUndoData(_originalNode, nodeWhereBlockIsDropped);
                    SwitchNodes(_originalNode, nodeWhereBlockIsDropped);
                    _audioManager.PlaySFX(_audioManager.DropBlock);
                    FindObjectOfType<GameManager>().CheckResult(true);
                }
                else
                {
                    _audioManager.PlaySFX(_audioManager.DropBlockUndo);
                    CustomAnimation.NumberDropped(
                        gameObject.transform,
                        _originalNode.transform.position
                    );
                }
            }
            else
            {
                CustomAnimation.NumberDropped(transform, _originalNode.transform.position);
                _audioManager.PlaySFX(_audioManager.DropBlockUndo);
            }

            UpdateOffsetPosition();
        }
        else if (nodeTouched == null)
        {
            CustomAnimation.NumberDropped(transform, _originalNode.transform.position);
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

        CustomAnimation.NumberDropped(
            firstNode.GetBlockInNode().transform,
            secondNode.GetBlockInNode().transform.position
        );
        firstNode.SetBlockInNode(secondNode.GetBlockInNode());
        firstNode.GetBlockInNode().transform.SetParent(firstNode.transform);
        firstNode.GetBlockInNode()._originalNode = firstNode;

        CustomAnimation.NumberSwitched(
            secondNode.GetBlockInNode().transform,
            _tempNode.transform.position
        );
        secondNode.SetBlockInNode(tempBlock);
        secondNode.GetBlockInNode().transform.SetParent(secondNode.transform);
        secondNode.GetBlockInNode()._originalNode = secondNode;
    }

    internal void UpdateTextColor()
    {
        _text.color = _isInteractible
            ? ColourManager.Instance.SelectedPalette().Colours[7]
            : ColourManager.Instance.SelectedPalette().Colours[1];
    }

    public Sequence AnimatePartialSumCorrect()
    {
        return CustomAnimation.SumIsCorrect(_text.transform);
    }

    public Sequence AnimatePuzzleCompleted()
    {
        return CustomAnimation.SumIsCorrect(_text.transform);
    }

    public void AnimateIncorrectSolution()
    {
        CustomAnimation.SumIsIncorrect(_text.transform);
    }
}
