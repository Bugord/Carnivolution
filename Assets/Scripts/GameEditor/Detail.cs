using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameEditor
{
    public class Detail : MonoBehaviour
    {
        private Rigidbody2D _rigidbody2D;
        private Detail _parentHeartDetail;
        public Detail ParentHeartDetail => isHeart ? this : _parentHeartDetail;
        [SerializeField] private bool isHeart;
        private SpriteRenderer _spriteRenderer;
        [SerializeField] private List<Detail> _connectedDetails;

        private List<Detail> _allDetails;
        public List<Detail> AllDetails => _allDetails;

        private bool _markedToDelete;

        private bool _isConnectedToHeart;

        public Action DetailDeleted;

        private void Awake()
        {
            if (gameObject.layer == LayerMask.NameToLayer("Enemy") &&
                SceneManager.GetActiveScene().name != "EnemiesEditorScene")
            {
                DisableEnemyConnectionSlots();
            }

            _allDetails ??= new List<Detail>();
            _connectedDetails ??= new List<Detail>();

            _parentHeartDetail = transform.parent.GetComponentsInChildren<Detail>().First(x => x.isHeart);

            if (isHeart)
            {
                _allDetails.Add(this);
                _allDetails.AddRange(_connectedDetails);
            }

            _rigidbody2D = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void DisableEnemyConnectionSlots()
        {
            foreach (var go in GetComponentsInChildren<ConnectionSlot>().Select(x => x.gameObject))
            {
                go.SetActive(false);
            }
        }

        private void SetConnectedDetails()
        {
            var colliders = Physics2D.OverlapCircleAll(transform.position, 0.8f, 1 << 8);
            var details = colliders.Select(x => x.GetComponent<Detail>()).Where(x => x != null && x != this);
            Debug.Log($"{name}: {string.Join(", ", details.Select(x => x.name))}");
        }

        public void ConnectDetail(Detail otherDetail)
        {
            AddDetailToList(otherDetail);
            _connectedDetails.Add(otherDetail);
            DetailConstructorManager.Instance.CreateConnection(this, otherDetail);
        }

        public void ConnectToDetail(Detail otherDetail)
        {
            _parentHeartDetail = otherDetail.ParentHeartDetail;
            _connectedDetails.Add(otherDetail);
            otherDetail.ConnectDetail(this);
            _isConnectedToHeart = true;
        }

        public void DestroyDetail()
        {
            _markedToDelete = true;
            DetailConstructorManager.Instance.DeleteDetail(this);
        }

        public List<Detail> GetRecursiveDetails(List<Detail> addedDetails)
        {
            if (addedDetails.Contains(this) || _markedToDelete)
            {
                return addedDetails;
            }

            addedDetails.Add(this);

            return _connectedDetails.Any() && !_markedToDelete
                ? _connectedDetails.SelectMany(x => x.GetRecursiveDetails(addedDetails)).ToList()
                : addedDetails;
        }

        public void OnMouseDown()
        {
            if (!isHeart)
            {
                DestroyDetail();
            }
        }

        public void AddDetailToList(Detail detail)
        {
            if (isHeart)
            {
                if (!_allDetails.Contains(detail))
                {
                    _allDetails.Add(detail);
                }
            }
            else
            {
                _parentHeartDetail.AddDetailToList(detail);
            }
        }

        public void DisconnectDetailsToRemove(List<Detail> detailsToRemove)
        {
            if (isHeart)
            {
                _allDetails = _allDetails.Except(detailsToRemove).ToList();
            }

            _connectedDetails = _connectedDetails.Except(detailsToRemove).ToList();
        }

        public void RemoveDetail()
        {
            GlobalEventManager.DetailDestroyed?.Invoke(this);
            DetailDeleted?.Invoke();
            if (isHeart)
            {
                if (gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
                else
                {
                    transform.parent.gameObject.SetActive(false);
                    GlobalEventManager.EnemyDied?.Invoke(transform.parent.gameObject);
                    Destroy(transform.parent.gameObject, 3);
                }
            }
            else
            {
                gameObject.SetActive(false);
                Destroy(gameObject, 3);
            }
        }
    }
}