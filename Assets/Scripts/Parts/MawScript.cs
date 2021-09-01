 using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 using UnityEngine.Serialization;

 public class MawScript : PartScript
{
    [SerializeField] private Animator animator;
    private static readonly int BiteTrigger = Animator.StringToHash("Bite");

    private void OnCollisionEnter2D(Collision2D other)
    {
        var layerToCheck = gameObject.layer == PlayerLayer ? EnemyLayer : PlayerLayer;
        if (other.gameObject.layer == layerToCheck)
        {
            animator.SetTrigger(BiteTrigger);
        }
        
        if (other.gameObject.CompareTag("Food"))
        {
            SoundController.Play(SoundManager.Instance.GetAudioClip("Hit"));
            animator.SetTrigger(BiteTrigger);
            if (IsPlayer)
            {
                Heal(1);
                GameManager.Instance.progressManager.AddScore(1);
            }

            Destroy(other.gameObject, 0.5f);
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        var layerToCheck = gameObject.layer == PlayerLayer ? EnemyLayer : PlayerLayer;
        if (other.gameObject.layer == layerToCheck)
        {
            animator.SetTrigger(BiteTrigger);
        }
    }
}
