using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            _text.enabled = !_text.enabled;
        }
    }
}
