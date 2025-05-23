using UnityEngine;

namespace Cannon
{
    public class CannonBallComponent : MonoBehaviour
    {
        private Vector3 _initialPoint;
        private Vector3 _targetPoint;
        private float _moveSpeed;
        private bool _isMoving = false;
        private float _time;
        private float _timeCounter;
        private Vector3 _lastPosition;
        private Vector3 _velocity;

        void OnTriggerEnter(Collider other)
        {
            Destroy(gameObject);
        }

        void Update()
        {
            if(_isMoving){
                float t = _timeCounter / _time;

                transform.position = Vector3.Slerp(_initialPoint, _targetPoint, t);


                if (_timeCounter >= _time)
                {
                    _isMoving = false;
                    _velocity = (transform.position - _lastPosition).normalized * _moveSpeed;
                    return;
                }

                _timeCounter += Time.deltaTime;

                _lastPosition = transform.position;

            }
            else
            {
                transform.position += _velocity * Time.deltaTime;
            }

        }

        public void StartMovement(float speed, Vector3 targetPoint)
        {
            _moveSpeed = speed;
            _initialPoint = targetPoint;
            _targetPoint = targetPoint;
            float distance = Vector3.Distance(transform.position,_targetPoint);
            _time = distance / _moveSpeed;
            _isMoving = true;
        }
    }
}
