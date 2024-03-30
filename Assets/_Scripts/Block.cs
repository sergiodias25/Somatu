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
    private GameManager _gameManager;
    private AudioManager _audioManager;
    private Vector3 mousePositionOffset;
    private Node _originalNode;
    private bool _isInteractible = false;
    public bool IsSelected = false;

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
            ? ColourManager.Instance.SelectedPalette().Colours[Constants.COLOR_DARK_TEXT]
            : ColourManager.Instance.SelectedPalette().Colours[Constants.COLOR_LIGHT_TEXT];
        return this;
    }

    private void OnMouseDown()
    {
        Node nodeClickedOn = GetNodeTouched();
        if (nodeClickedOn != null && _isInteractible)
        {
            FindObjectOfType<GameManager>().RemoveHints();
            if (_gameManager.SavedGameData.SettingsData.ControlMethodDrag)
            {
                UpdateOffsetPosition();
                CustomAnimation.NumberClicked(transform);
            }
            else
            {
                Block selectedBlock = FindObjectOfType<GameManager>().GetSelectedBlock();
                if (selectedBlock == null)
                {
                    FindObjectOfType<GameManager>().ResetSelectedBlock();
                    IsSelected = true;
                    nodeClickedOn.UpdateColor(
                        ColourManager.Instance.SelectedPalette().Colours[
                            Constants.COLOR_SELECTED_NODE
                        ]
                    );
                    _audioManager.PlaySFX(_audioManager.DropBlock);
                }
                else if (selectedBlock._originalNode.name == _originalNode.name)
                {
                    FindObjectOfType<GameManager>().ResetSelectedBlock();
                    selectedBlock._originalNode.UpdateColor(
                        ColourManager.Instance.SelectedPalette().Colours[
                            Constants.COLOR_NODE_NEUTRAL
                        ]
                    );
                    _audioManager.PlaySFX(_audioManager.DropBlockUndo);
                }
                else if (selectedBlock._originalNode.name != _originalNode.name)
                {
                    if (nodeClickedOn != null && nodeClickedOn.name != selectedBlock.GetNode().name)
                    {
                        selectedBlock._originalNode.UpdateColor(
                            ColourManager.Instance.SelectedPalette().Colours[
                                Constants.COLOR_NODE_NEUTRAL
                            ]
                        );
                        _gameManager.StoreUndoData(selectedBlock._originalNode, _originalNode);
                        SwitchBlocksUndo(nodeClickedOn, selectedBlock._originalNode);
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
        var cenas = _originalNode.transform.position - GetWorldMousePosition();
        cenas.z = 0;
        mousePositionOffset = cenas;
    }

    private Vector3 MoveOffsetPosition()
    {
        Vector3 position = GetWorldMousePosition() + mousePositionOffset;
        position.z = 0;
        return position;
    }

    private void OnMouseDrag()
    {
        if (_gameManager.SavedGameData.SettingsData.ControlMethodDrag && _isInteractible)
        {
            Node nodeWhereBlockIsHovering = GetNodeTouched();
            transform.position = MoveOffsetPosition();
            CustomAnimation.NumberClicked(transform);

            if (nodeWhereBlockIsHovering != null && _originalNode != nodeWhereBlockIsHovering)
            {
                _gameManager.StoreUndoData(_originalNode, nodeWhereBlockIsHovering);
                SwitchBlocks(nodeWhereBlockIsHovering);
                _originalNode = nodeWhereBlockIsHovering;
            }
        }
    }

    private void OnMouseUp()
    {
        if (_gameManager.SavedGameData.SettingsData.ControlMethodDrag)
        {
            Node nodeWhereBlockIsDropped = GetNodeTouched();
            if (nodeWhereBlockIsDropped != null && _isInteractible)
            {
                UpdateOffsetPosition();
                SwitchNodes(_originalNode.GetBlockInNode(), nodeWhereBlockIsDropped);
                if (!FindObjectOfType<GameManager>().CheckResult(true))
                {
                    _audioManager.PlaySFX(_audioManager.DropBlock);
                }
            }
            else if (nodeWhereBlockIsDropped == null || !_isInteractible)
            {
                UpdateOffsetPosition();
                CustomAnimation.NumberDropped(transform, _originalNode.transform.position);
                if (!FindObjectOfType<GameManager>().CheckResult(true))
                {
                    _audioManager.PlaySFX(_audioManager.DropBlockUndo);
                }
            }
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

    public static void SwitchNodes(Block blockMoved, Node landingNode)
    {
        CustomAnimation.NumberDropped(blockMoved.transform, landingNode.transform.position);
    }

    public async void SwitchBlocks(Node hoveredNode)
    {
        Node tempNode = hoveredNode;

        hoveredNode.GetBlockInNode().transform.SetParent(_originalNode.transform);
        hoveredNode.GetBlockInNode()._originalNode = _originalNode;
        _originalNode.SetBlockInNode(hoveredNode.GetBlockInNode());
        CustomAnimation.NumberSwitched(
            hoveredNode.GetBlockInNode().transform,
            _originalNode.transform.position
        );

        transform.SetParent(tempNode.transform);
        _originalNode = tempNode;
        hoveredNode.SetBlockInNode(this);

        await CustomAnimation.WaitForAnimation("MoveNumberBack");
    }

    public static async void SwitchBlocksUndo(Node secondNode, Node firstNode)
    {
        CustomAnimation.NumberSwitched(
            firstNode.GetBlockInNode().transform,
            secondNode.transform.position
        );
        CustomAnimation.NumberSwitched(
            secondNode.GetBlockInNode().transform,
            firstNode.transform.position
        );

        secondNode.GetBlockInNode().transform.SetParent(firstNode.transform);
        secondNode.GetBlockInNode()._originalNode = firstNode;

        firstNode.GetBlockInNode().transform.SetParent(secondNode.transform);
        firstNode.GetBlockInNode()._originalNode = secondNode;
        Block tempBlock = firstNode.GetBlockInNode();

        firstNode.SetBlockInNode(secondNode.GetBlockInNode());
        secondNode.SetBlockInNode(tempBlock);

        await CustomAnimation.WaitForAnimation("MoveNumberBack");
    }

    internal void UpdateTextColor()
    {
        _text.color = _isInteractible
            ? ColourManager.Instance.SelectedPalette().Colours[Constants.COLOR_DARK_TEXT]
            : ColourManager.Instance.SelectedPalette().Colours[Constants.COLOR_LIGHT_TEXT];
    }

    public Sequence AnimatePartialSumCorrect()
    {
        if (_text.transform != null)
            return CustomAnimation.SumIsCorrect(_text.transform, GetNode().name);
        return null;
    }

    public Sequence AnimatePuzzleCompleted()
    {
        return CustomAnimation.SumIsCorrect(
            _text.transform,
            GetNode().transform.position,
            GetNode().name
        );
    }

    public void AnimateIncorrectSolution()
    {
        CustomAnimation.SumIsIncorrect(_text.transform);
    }
}
