using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GameEditor;
using UnityEngine;

public class StoreUI : MonoBehaviour
{
    private RectTransform _rectTransform;

    public static Action StoreOpened;
    public static Action StoreClosed;
    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        GlobalEventManager.DetailAdded += OnDetailAdded;
        ProgressManager.ScoreToGainDetailReached += OpenStore;
    }

    private void OnDetailAdded(Detail detail)
    {
        CloseStore();
    }

    private void OnDisable()
    {
        GlobalEventManager.DetailAdded -= OnDetailAdded;
        ProgressManager.ScoreToGainDetailReached -= OpenStore;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            OpenStore();
        }
        else if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            CloseStore();
        }
    }

    void OpenStore()
    {
        DOTween.To(() => _rectTransform.anchoredPosition, 
            x => _rectTransform.anchoredPosition = x,
            Vector2.up * 100f,
            0.5f)
            .OnComplete(() => StoreOpened?.Invoke());
    }

    void CloseStore()
    {
        DOTween.To(() => _rectTransform.anchoredPosition, 
            x => _rectTransform.anchoredPosition = x,
            Vector2.up * -50f,
            0.5f)
            .OnComplete(() => StoreClosed?.Invoke());
    }
}