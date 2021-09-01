using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeScript : MonoBehaviour
{
    private int _playerLayer;
    private int _playerAmmunitionLayer;
    private int _enemyLayer;
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rb2d;
    private BoxCollider2D _collider;
    

    private void Awake()
    {
        _playerLayer = LayerMask.NameToLayer("Player");
        _playerAmmunitionLayer = LayerMask.NameToLayer("PlayerAmmunition");
        _enemyLayer = LayerMask.NameToLayer("Enemy");
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rb2d = GetComponent<Rigidbody2D>();
        _collider = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var layerToCheck = gameObject.layer == _playerAmmunitionLayer ? _enemyLayer : _playerLayer;
        if (other.gameObject.layer == layerToCheck)
        {
            StartCoroutine(DisableSpike());
        }
    }

    private IEnumerator DisableSpike()
    {
        _rb2d.velocity = Vector2.zero;
        _spriteRenderer.enabled = false;
        _collider.enabled = false;
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
        _spriteRenderer.enabled = true;
        _collider.enabled = true;
    }
}
