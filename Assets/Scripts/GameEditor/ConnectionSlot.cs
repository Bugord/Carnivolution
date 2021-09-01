using System;
using TMPro;
using UnityEngine;

namespace GameEditor
{
    public class ConnectionSlot : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;
        private Detail _parentDetail;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _parentDetail = GetComponentInParent<Detail>();
        }

        private void OnEnable()
        {
            GlobalEventManager.EditModeToggled += ToggleRenderer;
        }

        private void ToggleRenderer(bool enable)
        {
            // _spriteRenderer.enabled = enable;
        }

        private void OnDisable()
        {
            GlobalEventManager.EditModeToggled -= ToggleRenderer;
        }

        public Detail GetParentDetail()
        {
            return _parentDetail;
        }
    }
}