using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameEditor
{
    public class PickupDetail : MonoBehaviour
    {
        [SerializeField] private GameObject detailPrefab;
        
        private SpriteRenderer _spriteRenderer;
        private List<Collider2D> _hoveredSlotsColliders2D;
        private int _hoveredDetailsCount;
        private bool _isPickedUp;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _hoveredSlotsColliders2D = new List<Collider2D>();
        }

        private void Update()
        {
            if (!_isPickedUp)
                return;

            if (Input.GetKeyDown(KeyCode.R))
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, 1);
            }

            if (Input.GetKey(KeyCode.Q))
            {
                transform.Rotate(0, 0, Time.deltaTime * 130f);
            }
            
            if (Input.GetKey(KeyCode.E))
            {
                transform.Rotate(0, 0, Time.deltaTime * -130f);
            }
        }

        private void OnMouseEnter()
        {
            DetailConstructorManager.PickupHovered?.Invoke();
        }

        private void OnMouseExit()
        {
            DetailConstructorManager.PickupHoverLeave?.Invoke();
        }

        private void OnMouseDown()
        {
            GlobalEventManager.EditModeToggled?.Invoke(true);
            _isPickedUp = true;
            DetailConstructorManager.PickupPicked?.Invoke();
        }

        private void OnMouseDrag()
        {
            transform.position = (Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        private void OnMouseUp()
        {
            GlobalEventManager.EditModeToggled?.Invoke(false);
            _isPickedUp = false;

            if (_hoveredDetailsCount == 0 && _hoveredSlotsColliders2D.Any())
            {
                var detailsToConnect = _hoveredSlotsColliders2D.Select(x => x.GetComponent<ConnectionSlot>().GetParentDetail())
                    .Distinct();

                var detail = Instantiate(detailPrefab, transform.position, transform.rotation, GameManager.Instance.playerParentTransform).GetComponent<Detail>();
                detail.transform.localScale = new Vector3(Mathf.Sign(transform.localScale.x), Mathf.Sign(transform.localScale.y), 1);
                
                foreach (var detailToConnect in detailsToConnect)
                {
                    detail.ConnectToDetail(detailToConnect);
                }
                
                GlobalEventManager.DetailAdded(detail);


                Destroy(gameObject);
            }
            else
            {
                Return();
            }
            
            DetailConstructorManager.PickupPlaced?.Invoke();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("ConnectionSlot"))
            {
                _hoveredSlotsColliders2D.Add(other);
            }
            else if (other.CompareTag("Detail"))
            {
                _hoveredDetailsCount++;
            }
            RecalculateColor(); 
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("ConnectionSlot"))
            {
                _hoveredSlotsColliders2D.Remove(other);
            } else if (other.CompareTag("Detail"))
            {
                _hoveredDetailsCount--;
            }
            RecalculateColor();
        }
        
        private void RecalculateColor()
        {
            if(!_isPickedUp)
                return;
            
            if (_hoveredDetailsCount == 0)
            {
                if (_hoveredSlotsColliders2D.Any())
                {
                    DetailConstructorManager.PickupHoverSuccess?.Invoke();
                }
                else
                {
                    DetailConstructorManager.PickupPicked?.Invoke();
                }
            }
            else
            {
                DetailConstructorManager.PickupHoverError?.Invoke();
            }
            
            _spriteRenderer.color = _hoveredDetailsCount == 0 ? (_hoveredSlotsColliders2D.Any() ? Color.cyan : Color.white) : Color.red;
        }

        private void Return()
        {
            transform.localPosition = Vector3.zero;
            transform.rotation = Quaternion.identity;
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), Mathf.Abs(transform.localScale.y), 1);
            _spriteRenderer.color = Color.white;
        }
    }
}