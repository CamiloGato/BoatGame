using Boats.Components;
using Health;
using UnityEngine;

namespace Cannon
{
    public class CannonComponent : BaseComponent
    {
        [SerializeField] private GameObject ownerGo;

        [Header("Detection Area")]
        [SerializeField] private LayerMask targetLayers;
        [SerializeField] private float detectionRadius = 3f;
        [SerializeField] private float detectionAngle = 45f;
        [SerializeField] private Transform detectionCenterPoint;

        [Header("Shoot")]
        [SerializeField] private CannonBallComponent cannonBallPrefab;
        [SerializeField] private Transform shootingPoint;

        [SerializeField] private float shootVerticalOffset;

        [SerializeField] private float shootTimeMin = 1f;
        [SerializeField] private float shootTimeMax = 3f;
        [SerializeField] private float shootTimestamp;

        [Header("Follow")]
        [SerializeField] private float rotationSpeed = 90f;
        [SerializeField] private bool rotate360;
        [SerializeField] private Transform body;

        [Header("Debug")]
        [SerializeField] private bool executeDetectionOnEditor;
        [SerializeField] private Color gizmoDefaultColor = Color.red;
        [SerializeField] private Color gizmoDetectionColor = Color.green;
        [SerializeField] private Transform target;

        private void OnDrawGizmos()
        {
            // Comprobamos si hay asignado un detectionPoint para evitar errores en el editor
            if (!detectionCenterPoint)
            {
                detectionCenterPoint = transform; // Si no lo hay asignamos el transform del propio cañón
            }

            if (executeDetectionOnEditor)
            {
                if (!CheckIfTargetInRange()) target = null;
                if (!target) Detect();
            }

            // Dibujamos el área de detección
            Gizmos.color = (executeDetectionOnEditor && target) ? gizmoDetectionColor : gizmoDefaultColor;
            Gizmos.DrawWireSphere(detectionCenterPoint.position, detectionRadius);
            // Dibujamos los vectores que delimitan el área de detección según detectionAngle
            Quaternion leftRotation = Quaternion.Euler(0f, -detectionAngle, 0f);
            Quaternion rightRotation = Quaternion.Euler(0f, detectionAngle, 0f);
            Gizmos.DrawRay(detectionCenterPoint.position,
                leftRotation * detectionCenterPoint.forward * detectionRadius);
            Gizmos.DrawRay(detectionCenterPoint.position,
                rightRotation * detectionCenterPoint.forward * detectionRadius);

            if (target)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(detectionCenterPoint.position, target.position);
            }
        }

        void Start()
        {
            // Asegúrate de que los objetos estén correctamente asignados
            if (!cannonBallPrefab || !shootingPoint)
            {
                Debug.LogError("Cannon ball prefab or shooting point is not assigned in the Inspector!");
            }
        }

        void Update()
        {
            if (!CheckIfTargetInRange()) target = null;
            if (!target)
            {
                Detect();
                return;
            }

            FollowTarget();

            if (shootTimestamp <= Time.time)
            {
                Shoot();
                shootTimestamp = Random.Range(shootTimeMin, shootTimeMax) + Time.time;
            }
        }

        [ContextMenu("Detect")]
        private void Detect()
        {
            Collider[] targetsDetected = new Collider[10];
            int size = Physics.OverlapSphereNonAlloc(detectionCenterPoint.position, detectionRadius, targetsDetected, targetLayers);
            if (size == 0) return;

            int firstInRange = -1;
            for (int i = 0; i < size; i++)
            {
                if (CheckIfPointInRange(targetsDetected[i].transform.position))
                {
                    firstInRange = i;
                    break;
                }
            }

            if (firstInRange < 0) return;

            float minDistance = Vector3.Distance(detectionCenterPoint.position,
                targetsDetected[firstInRange].transform.position);
            target = targetsDetected[firstInRange].transform;

            for (int i = firstInRange + 1; i < targetsDetected.Length; i++)
            {
                if (CheckIfPointInRange(targetsDetected[i].transform.position)) continue;

                float distance = Vector3.Distance(detectionCenterPoint.position, targetsDetected[0].transform.position);
                if (minDistance > distance)
                {
                    minDistance = distance;
                    target = targetsDetected[i].transform;
                }
            }
        }

        private bool CheckIfPointInRange(Vector3 point)
        {
            Vector3 dir = point - detectionCenterPoint.position;
            dir.y = 0;
            dir.Normalize();

            float scalar = Vector3.Dot(detectionCenterPoint.forward, dir);
            float angle = (1 - scalar) * 90f;

            return detectionAngle >= angle;
        }

        private bool CheckIfTargetInRange()
        {
            if (target == null || !CheckIfPointInRange(target.position)) return false;

            float distance = Vector3.Distance(detectionCenterPoint.position, target.position);
            return distance < detectionRadius;
        }

        /// <summary>
        /// Ejecuta el disparo hacia el frente del cañón
        /// </summary>
        private void Shoot()
        {
            // Instanciar el proyectil con la rotación del cañón
            CannonBallComponent cannonBall =
                Instantiate(cannonBallPrefab, shootingPoint.position, shootingPoint.rotation);

            if (cannonBall.TryGetComponent(out DamageDealer damageDealer))
            {
                damageDealer.ownerGo = ownerGo;
            }

            // Aplicar la fuerza al proyectil
            // cannonBallRb.AddForce(_shootingPoint.forward * shootForce, ForceMode.Impulse);
            Vector3 targetPoint = target.position;
            targetPoint.y += shootVerticalOffset;
            cannonBall.StartMovement(30f, targetPoint);
        }

        /// <summary>
        /// Método que orienta el cañón hacia su objetivo
        /// </summary>
        private void FollowTarget()
        {
            if (!target) return;

            Vector3 direction = target.position - transform.position;
            direction.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(direction, transform.up);

            if (rotate360)
            {
                if (body)
                {
                    body.forward = transform.forward;
                }

                transform.rotation =
                    Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
            else if (body)
            {
                body.rotation =
                    Quaternion.RotateTowards(body.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }

}