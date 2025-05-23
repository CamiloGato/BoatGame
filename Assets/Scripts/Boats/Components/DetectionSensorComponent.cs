using UnityEngine;

namespace Boats.Components
{
    public class DetectionSensorComponent : BaseComponent
    {
        [Header("Detection Area")]
        [SerializeField] private LayerMask targetLayers;
        [SerializeField] private Transform target;
        [SerializeField] private float detectionRadius = 3f;
        [SerializeField] private Transform detectionCenterPoint;
        [SerializeField] private bool executeDetectionOnEditor;

        [Header("Color")]
        [SerializeField] private Color gizmoDetectionColor;
        [SerializeField] private Color gizmoDefaultColor;

        private void OnDrawGizmos()
        {
            if (!detectionCenterPoint)
            {
                detectionCenterPoint = transform;
            }

            if (executeDetectionOnEditor)
            {
                if (!target){}
            }

            Gizmos.color = executeDetectionOnEditor && target ? gizmoDetectionColor : gizmoDefaultColor;
            Gizmos.DrawWireSphere(detectionCenterPoint.position, detectionRadius);
        }

        public bool Detect (out Transform targetTransform)
        {
            targetTransform = null;

            Collider[] targetsDetected = new Collider[10];

            int size = Physics.OverlapSphereNonAlloc(detectionCenterPoint.position, detectionRadius, targetsDetected, targetLayers);

            if (size == 0)
            {
                return  false;
            }

            float minDistance = Vector3.Distance(detectionCenterPoint.position, targetsDetected[0].transform.position);
            target = targetsDetected[0].transform;

            for (int i = 1; i < targetsDetected.Length; i++)
            {
                float distance = Vector3.Distance(detectionCenterPoint.position, targetsDetected[i].transform.position);
                if (minDistance > distance)
                {
                    target = targetsDetected[i].transform;
                    minDistance = distance;
                }
            }

            targetTransform = target;
            return  true;
        }

    }
}