using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using Random = UnityEngine.Random;

namespace GameEditor
{
    public class DetailConnection : MonoBehaviour
    {
        [SerializeField] private List<Sprite> sprites;

        private Detail _firstDetail;
        private Detail _secondDetail;
        private HingeJoint2D _hingeJoint2D;
        private SpringJoint2D _springJoint2D;
        private SliderJoint2D _sliderJoint2D;

        private void Awake()
        {
            _hingeJoint2D = GetComponent<HingeJoint2D>();
            if (_hingeJoint2D.connectedBody != null)
            {
                _firstDetail = _hingeJoint2D.connectedBody.GetComponent<Detail>();
                _firstDetail.DetailDeleted += OnDetailRemoved;
            }

            _sliderJoint2D = GetComponent<SliderJoint2D>();
            if (_sliderJoint2D.connectedBody != null)
            {
                _secondDetail = _sliderJoint2D.connectedBody.GetComponent<Detail>();
                _secondDetail.DetailDeleted += OnDetailRemoved;
            }
        }

        public void Initialize(Detail firstDetail, Detail secondDetail)
        {
            GetComponent<SpriteRenderer>().sprite = sprites[Random.Range(0, sprites.Count)];

            _firstDetail = firstDetail;
            _secondDetail = secondDetail;

            _firstDetail.DetailDeleted += OnDetailRemoved;
            _secondDetail.DetailDeleted += OnDetailRemoved;

            _hingeJoint2D = GetComponent<HingeJoint2D>();
            _hingeJoint2D.connectedBody = _firstDetail.GetComponent<Rigidbody2D>();

            _springJoint2D = GetComponent<SpringJoint2D>();
            _sliderJoint2D = GetComponent<SliderJoint2D>();
            _springJoint2D.connectedBody = _secondDetail.GetComponent<Rigidbody2D>();
            _sliderJoint2D.connectedBody = _secondDetail.GetComponent<Rigidbody2D>();
        }

        private void OnDetailRemoved()
        {
            _firstDetail.DetailDeleted -= OnDetailRemoved;
            _secondDetail.DetailDeleted -= OnDetailRemoved;
            Destroy(gameObject);
        }
    }
}