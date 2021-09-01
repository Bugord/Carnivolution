using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using DG.Tweening;
using GameEditor;
using UnityEngine;

public class PartScript : MonoBehaviour
{
    public Stats stats;
    private ParticleSystem _particleSystem;
    private SpriteRenderer _spriteRenderer;
    private readonly Color _damagedColor = new Color(1, 0.28f, 0.28f);

    protected int PlayerLayer;
    protected int PlayerAmmunitionLayer;
    protected int EnemyLayer;
    protected int EnemyAmmunitionLayer;

    protected SoundController SoundController;

    private float _initialHealth;
    [SerializeField] protected float health; 
    protected bool IsPlayer;

    private Collider2D CurrentTrigger
    {
        set
        {
            if (value != _currentCollider)
            {
                _currentCollider = value;
                _currentPart = value.gameObject.GetComponentInParent<PartScript>();
            }
        }
        get => _currentCollider;
    }
    
    private Collider2D _currentCollider;
    private PartScript _currentPart;

    private float _damageCooldownTimer;

    protected Rigidbody2D rbd2;

    protected void Awake()
    {
        PlayerLayer = LayerMask.NameToLayer("Player");
        PlayerAmmunitionLayer = LayerMask.NameToLayer("PlayerAmmunition");
        EnemyLayer = LayerMask.NameToLayer("Enemy");
        EnemyAmmunitionLayer = LayerMask.NameToLayer("EnemyAmmunition");
        health = stats.Health;
        _initialHealth = health;
        rbd2 = GetComponent<Rigidbody2D>();
        _particleSystem = GetComponent<ParticleSystem>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        SoundController = GetComponent<SoundController>();
        IsPlayer = gameObject.layer == LayerMask.NameToLayer("Player");
    }

    private void Update()
    {
        _damageCooldownTimer += Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        CurrentTrigger = other;
        var layerToCheck = gameObject.layer == PlayerLayer ? EnemyAmmunitionLayer : PlayerAmmunitionLayer;
        if (other.gameObject.layer == layerToCheck && _damageCooldownTimer > _currentPart.stats.DamageCooldown)
        {
            GetDamage(_currentPart.stats.Damage);
            _damageCooldownTimer = 0;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        CurrentTrigger = other;
        if (_currentPart == null || !(_damageCooldownTimer < _currentPart.stats.DamageCooldown))
        {
            var layerToCheck = gameObject.layer == PlayerLayer ? EnemyAmmunitionLayer : PlayerAmmunitionLayer;
            if (other.gameObject.layer == layerToCheck)
            {
                GetDamage(_currentPart.stats.Damage);
                _damageCooldownTimer = 0;
            }
        }
    }

    private void GetDamage(float damage)
    {
        health -= damage * (gameObject.layer == PlayerLayer ? 0.5f : 1f);
        SoundController.Play(SoundManager.Instance.GetAudioClip("Hit"));
        _particleSystem.Emit(30 * (int)damage);
        DOTween.To(() => _damagedColor, x => _spriteRenderer.color = x, GetDamageColor(), 0.5f);
        if (health <= 0)
        {
            GetComponent<Detail>().DestroyDetail();
        }
    }

    private Color GetDamageColor()
    {
        return Color.Lerp(_damagedColor, Color.white, health / _initialHealth);
    }

    protected void Heal(float amount)
    {
        health += amount;
        if (health >= _initialHealth)
        {
            health = _initialHealth;
        }

        _spriteRenderer.color = GetDamageColor();
    }
}
