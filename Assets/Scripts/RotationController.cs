using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationController : MonoBehaviour
{
    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void Rotate(Vector2 direction, float rotationSpeed)
    {
        var deltaAngle = Mathf.DeltaAngle(_rb.rotation % 360, GetAngle(direction));
        _rb.AddTorque(deltaAngle * rotationSpeed);
    }
    
    private float GetAngle(Vector2 dir)
    {
        return Quaternion.LookRotation(Vector3.forward, dir).eulerAngles.z;
    }
}
