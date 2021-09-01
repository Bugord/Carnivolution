using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private Image barImage;

    private void OnEnable()
    {
        ProgressManager.ProgressChanged += OnProgressChanged;
    }

    private void OnProgressChanged(float percent)
    {
        DOTween.To(() => barImage.fillAmount, x => barImage.fillAmount = x, percent, 0.3f);
    }
}