using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameEditor
{
    public class DetailConstructorManager : MonoBehaviour
    {
        public static DetailConstructorManager Instance;
        public static Action PickupHovered;
        public static Action PickupHoverError;
        public static Action PickupHoverSuccess;
        public static Action PickupHoverLeave;
        public static Action PickupPicked;
        public static Action PickupPlaced;

        [SerializeField] private GameObject detailConnectionPrefab;

        private void Awake()
        {
            Instance = this;
        }

        public void CreateConnection(Detail firstDetail, Detail secondDetail)
        {
            var spawnedDetailConnection = Instantiate(
                detailConnectionPrefab,
                Vector2.Lerp(firstDetail.transform.position, secondDetail.transform.position, .5f),
                Quaternion.identity,
                GameManager.Instance.playerParentTransform);

            spawnedDetailConnection.GetComponent<DetailConnection>().Initialize(firstDetail, secondDetail);
        }

        public void DeleteDetail(Detail detail)
        {
            var allDetails = detail.ParentHeartDetail.AllDetails;
            var connectedDetails = new List<Detail>();
            connectedDetails = detail.ParentHeartDetail.GetRecursiveDetails(connectedDetails);

            var detailsToRemove = allDetails.Except(connectedDetails).ToList();
            
            connectedDetails.ForEach(x => x.DisconnectDetailsToRemove(detailsToRemove));
            foreach (var detailToRemove in detailsToRemove)
            {
                detailToRemove.RemoveDetail();
            }
        }
    }
}