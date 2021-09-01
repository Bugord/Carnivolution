using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsTest : MonoBehaviour
{
    [SerializeField] private float force;
    [SerializeField] private HingeJoint2D _hingeJoint2D;
    
    private Rigidbody2D _rigidbody2D;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            var mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _rigidbody2D.AddForce((mouseWorldPos - transform.position) * force);
        }

        if (Input.GetKeyDown(KeyCode.Mouse2))
        {
            _rigidbody2D.AddTorque(force);
        }
    }
}
