using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(MovementController), typeof(RotationController), typeof(VisionController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private MovementController movementController;
    [SerializeField] private RotationController rotationController;
    [SerializeField] private VisionController visionController;

    [SerializeField] private List<PartScript> parts;

    public static Action<float> Dashed;

    private float _movementSpeed;
    private float _dashCooldown;
    private float _rotationSpeed;
    private float _visionRadius;
    
    private Camera _camera;
    private Vector2 _mousePosition;
    private Vector2 _direction;

    private bool _isDashing;
    private bool _canDash = true;
    
    private void Awake()
    {
        _camera = Camera.main;
        UpdateStats();
        visionController.UpdateVision(_visionRadius);
        GlobalEventManager.DetailAdded += detail =>
        {
            AddPart(detail.GetComponent<PartScript>());
        };
        GlobalEventManager.DetailDestroyed += detail =>
        {
            RemovePart(detail.GetComponent<PartScript>());
        };
    }

    // Update is called once per frame
    void Update()
    {
        UpdateInput();
        
        if (Input.GetKeyDown(KeyCode.Space) && _canDash)
        {
            StartCoroutine(RefreshDash());
        }
    }

    private void FixedUpdate()
    {
        if (_isDashing)
        {
            movementController.Move(transform.up * (_movementSpeed * 3));
            return;
        }
        
        if (_direction.sqrMagnitude > 0.1f)
        {
            if (Input.GetMouseButton(1))
            {
                rotationController.Rotate(_direction.normalized, _rotationSpeed);
                if (Vector2.Angle(transform.up, _direction.normalized) < 15)
                {
                    movementController.Move(transform.up * (_movementSpeed * (!_isDashing ?1 : 5)));
                }
            } 
        }
    }

    void UpdateInput()
    {
        _mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
        _direction = (_mousePosition - (Vector2)transform.position);
    }

    private IEnumerator RefreshDash()
    {
        _isDashing = true;
        _canDash = false;
        yield return new WaitForSeconds(0.3f);
        _isDashing = false;
        movementController.Move(transform.up * _movementSpeed);
        Dashed?.Invoke(_dashCooldown);
        yield return new WaitForSeconds(_dashCooldown);
        _canDash = true;
    }

    private void UpdateStats()
    {
        _movementSpeed = parts.Sum(p => p.stats.MovementSpeed);
        _rotationSpeed = parts.Sum(p => p.stats.RotationSpeed);
        _visionRadius = parts.Sum(p => p.stats.VisionRadius);
        _dashCooldown = parts.Sum(p => p.stats.DashCooldown);
        if (_dashCooldown < 0.5f)
        {
            _dashCooldown = 0.5f;
        }
    }

    public void AddPart(PartScript part)
    {
        parts.Add(part);
        UpdateStats();
        visionController.UpdateVision(_visionRadius);
    }

    public void RemovePart(PartScript part)
    {
        parts.Remove(part);
        UpdateStats();
        visionController.UpdateVision(_visionRadius);
    }
}
