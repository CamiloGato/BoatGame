using UnityEngine;

namespace Boats.Components
{
    public class CameraComponent : BaseComponent
    {
        [Header("Camera")]
        [SerializeField] private Camera cam;
        [SerializeField] private Transform camTransform;

        [Header("Follow")]
        [SerializeField] private Transform target;

        [Header("Offsets")]
        [SerializeField] private Vector3 followOffset;
        [SerializeField] private Vector3 lookOffset;


        [Header("Limits")]
        [SerializeField] private Vector3 minWorldLimit;
        [SerializeField] private Vector3 maxWorldLimit;


        void Update()
        {
            FollowTarget();
        }

        void OnValidate()
        {
            FollowTarget();
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Vector3 size = maxWorldLimit - minWorldLimit;
            Vector3 center = minWorldLimit + size / 2;
            Gizmos.DrawCube(center, size);

            //Dibujado de proyeccion de la camara en el plano del jugador
            Gizmos.color = Color.magenta;
            float depht = target.position.z - camTransform.position.z;
            Vector3 minCameraPosition = cam.ViewportToWorldPoint(new Vector3(0f, 0f, depht));
            Vector3 maxCameraPosition = cam.ViewportToWorldPoint(new Vector3(1f, 1f, depht));
            Gizmos.DrawLine(minCameraPosition, maxCameraPosition);
        }

        private void FollowTarget()
        {
            if (!target || !camTransform) return;

            Vector3 nextPosition = camTransform.position = target.position + followOffset;
            nextPosition.x = Mathf.Clamp(nextPosition.x, minWorldLimit.x, maxWorldLimit.x);
            nextPosition.z = Mathf.Clamp(nextPosition.z, minWorldLimit.z, maxWorldLimit.z);
            camTransform.position = nextPosition;
            camTransform.forward = target.position + lookOffset - camTransform.position;
        }
    }
}