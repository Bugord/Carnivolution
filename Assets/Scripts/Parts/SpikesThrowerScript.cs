using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SpikesThrowerScript : PartScript
{
    [SerializeField] private float cooldown;
    private float _timer;
    [SerializeField] private Rigidbody2D spike;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite normalState;
    [SerializeField] private Sprite emptyState;
    [SerializeField] private Transform startTransform;

    private new void Awake()
    {
        base.Awake();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (_timer <= cooldown)
        {
            _timer += Time.deltaTime;
            if (_timer > cooldown * 0.5f)
            {
                spriteRenderer.sprite = normalState;
            }
        }
        else
        {
            Shoot();
            _timer = 0;
            spriteRenderer.sprite = emptyState;
        }
    }

    private void Shoot()
    {
        SoundController.Play(SoundManager.Instance.GetAudioClip("Shoot"));
        spike.MovePosition(startTransform.position);
        spike.MoveRotation(rbd2.rotation);
        spike.velocity = Vector2.zero;
        spike.gameObject.SetActive(true);
        spike.AddForce(transform.up*5, ForceMode2D.Impulse);
    }
}
