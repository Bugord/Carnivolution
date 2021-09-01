using System;
using System.Collections;
using System.Collections.Generic;
using GameEditor;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField] private List<Texture2D> initialListSprites;
    [SerializeField] private Texture2D pickupHoverSprite;
    [SerializeField] private Texture2D pickupDragSprite;
    [SerializeField] private Texture2D pickupDragErrorSprite;
    [SerializeField] private Texture2D pickupDragSuccessSprite;

    private readonly Vector2 _hotSpot = Vector2.zero;
    private readonly CursorMode _cursorMode = CursorMode.Auto;

    private bool _isDragging;
    private bool _isHovering;
    private float _playerDashTimer;
    private float _playerDashCooldown;
    private bool _dashed;

    private int _currentFrame;
    private void Awake()
    {
        _currentFrame = initialListSprites.Count - 1;
        Cursor.SetCursor(initialListSprites[_currentFrame], _hotSpot, _cursorMode);
    }

    private void OnEnable()
    {
        DetailConstructorManager.PickupHovered += OnPickupHovered;
        DetailConstructorManager.PickupHoverLeave += OnPickupHoverEnd;
        DetailConstructorManager.PickupPicked += OnPickupPicked;
        DetailConstructorManager.PickupPlaced += OnPickupPlaced;
        DetailConstructorManager.PickupHoverError += OnPickupHoverError;
        DetailConstructorManager.PickupHoverSuccess += OnPickupHoverSuccess;
        
        PlayerController.Dashed += OnPlayerDashed;
    }

    private void OnPlayerDashed(float cooldown)
    {
        _dashed = true;
        _playerDashTimer = 0;
        _playerDashCooldown = cooldown;
    }

    private void Update()
    {
        if (_dashed)
        {
            _playerDashTimer += Time.deltaTime;
            if (_playerDashTimer >= _playerDashCooldown)
            {
                _playerDashTimer = _playerDashCooldown;
                _dashed = false;
            }

            _currentFrame = (int) Mathf.Lerp(0, initialListSprites.Count - 1, _playerDashTimer / _playerDashCooldown);
            if (!_isHovering)
            {
                Cursor.SetCursor(initialListSprites[_currentFrame], _hotSpot, _cursorMode);
            }
        }
    }

    private void OnPickupHoverSuccess()
    {
        Cursor.SetCursor(pickupDragSuccessSprite, _hotSpot, _cursorMode);
    }

    private void OnPickupHoverError()
    {
        Cursor.SetCursor(pickupDragErrorSprite, _hotSpot, _cursorMode);
    }

    private void OnPickupPlaced()
    {
        _isDragging = false;
        _isHovering = false;
        Cursor.SetCursor(initialListSprites[_currentFrame], _hotSpot, _cursorMode);
    }

    private void OnDisable()
    {
        DetailConstructorManager.PickupHovered -= OnPickupHovered;
        DetailConstructorManager.PickupHoverLeave -= OnPickupHoverEnd;
        DetailConstructorManager.PickupPicked -= OnPickupPicked;
        DetailConstructorManager.PickupPlaced -= OnPickupPlaced;
        DetailConstructorManager.PickupHoverError -= OnPickupHoverError;
        DetailConstructorManager.PickupHoverSuccess -= OnPickupHoverSuccess;
        
        PlayerController.Dashed -= OnPlayerDashed;
    }

    private void OnPickupPicked()
    {
        _isDragging = true;
        Cursor.SetCursor(pickupDragSprite, _hotSpot, _cursorMode);
    }

    private void OnPickupHoverEnd()
    {
        if (!_isDragging)
        {
            _isHovering = false;
            Cursor.SetCursor(initialListSprites[_currentFrame], _hotSpot, _cursorMode);
        }
    }

    private void OnPickupHovered()
    {
        if (!_isDragging)
        {
            _isHovering = true;
            Cursor.SetCursor(pickupHoverSprite, _hotSpot, _cursorMode);
        }
    }
}