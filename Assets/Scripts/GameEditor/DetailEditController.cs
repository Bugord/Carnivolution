using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameEditor
{
    public class DetailEditController : MonoBehaviour
    {
        [SerializeField] private List<Detail> details;
        [SerializeField] private Detail _heartDetail;
        private void OnEnable()
        {
            GlobalEventManager.DetailAdded += OnDetailAdded;
        }


        private void OnDetailAdded(Detail detail)
        {
            details.Add(detail);
        }
        
    }
}