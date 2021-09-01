using System;
using System.Collections.Generic;
using GameEditor;
using UnityEngine;

public class ProgressManager : MonoBehaviour
{
    [SerializeField] private List<int> scoreToGainDetail;
    public static Action ScoreToGainDetailReached;
    public static Action<float> ProgressChanged;

    private int _score;
    public int Score => _score;
    public float PercentToGainDetail => (float) _score / scoreToGainDetail[Level];

    private int _detailsCount = 0;
    private int Level => Mathf.Clamp(_detailsCount, 0, scoreToGainDetail.Count - 1);

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            AddScore(1);
        }
    }

    private void OnEnable()
    {
        GlobalEventManager.DetailAdded += OnDetailAdded;
        GlobalEventManager.DetailDestroyed += OnDetailRemoved;
    }

    private void OnDetailAdded(Detail detail)
    {
        _detailsCount++;
        _score = 0;
        ProgressChanged?.Invoke(PercentToGainDetail);
    }

    private void OnDetailRemoved(Detail detail)
    {
        if(detail.gameObject.layer != LayerMask.NameToLayer("Player"))
            return;
        
        var pastPercent = _score * 1f / scoreToGainDetail[Level];
        _detailsCount--;
        _score = Mathf.Clamp((int) (pastPercent * scoreToGainDetail[Level]), 0, scoreToGainDetail[Level] - 1);
        ProgressChanged?.Invoke(PercentToGainDetail);
    }

    public void AddScore(int score)
    {
        var prevScore = _score;
        _score += score;

        if (_score >= scoreToGainDetail[Level])
        {
            _score = scoreToGainDetail[Level];
            // _score -= scoreToGainDetail[_detailsCount];
            if (prevScore != _score)
            {
                ScoreToGainDetailReached?.Invoke();
            }
        }

        ProgressChanged?.Invoke(PercentToGainDetail);
    }
}