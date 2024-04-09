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
    float dragTime = 0f;

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
            _text.fontSize = 4.5f;
            _text.enableAutoSizing = false;
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

    private Vector3 MoveOffsetPosition()
    {
        Vector3 originalPosition = GetWorldMousePosition();
        originalPosition.z = 0;
        return originalPosition;
    }

    private void OnMouseDrag()
    {
        if (_gameManager.SavedGameData.SettingsData.ControlMethodDrag && _isInteractible)
        {
            Node nodeWhereBlockIsHovering = GetNodeTouched();
            _text.DOFade(0.66f, .25f);
            transform.position = MoveOffsetPosition();
            CustomAnimation.NumberClicked(transform);
            if (nodeWhereBlockIsHovering != null && _originalNode != nodeWhereBlockIsHovering)
            {
                dragTime += Time.deltaTime;
            }
            else
            {
                dragTime = 0f;
            }

            if (
                nodeWhereBlockIsHovering != null
                && _originalNode != nodeWhereBlockIsHovering
                && dragTime > 1f
            )
            {
                _gameManager.StoreUndoData(_originalNode, nodeWhereBlockIsHovering);
                SwitchBlocks(nodeWhereBlockIsHovering);
                _originalNode = nodeWhereBlockIsHovering;
                dragTime = 0f;
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
                CustomAnimation.NumberDropped(
                    _originalNode.GetBlockInNode().transform,
                    nodeWhereBlockIsDropped.transform.position
                );
                if (_originalNode != nodeWhereBlockIsDropped)
                {
                    _gameManager.StoreUndoData(_originalNode, nodeWhereBlockIsDropped);
                    SwitchBlocks(nodeWhereBlockIsDropped);
                    _originalNode = nodeWhereBlockIsDropped;
                }
                if (!FindObjectOfType<GameManager>().CheckResult(true))
                {
                    _audioManager.PlaySFX(_audioManager.DropBlock);
                }
            }
            else if (nodeWhereBlockIsDropped == null || !_isInteractible)
            {
                CustomAnimation.NumberDropped(transform, _originalNode.transform.position);
                if (!FindObjectOfType<GameManager>().CheckResult(true))
                {
                    _audioManager.PlaySFX(_audioManager.DropBlockUndo);
                }
            }
            _text.DOFade(1f, .25f);
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
