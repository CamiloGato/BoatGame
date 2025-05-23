using UnityEngine;

namespace Boats
{
    public class BoatPlayer : BoatController
    {
        private static readonly int SteeringValue = Animator.StringToHash("steeringValue");
        private static readonly int ForwardSpeed = Animator.StringToHash("forwardSpeed");
        private static readonly int Death = Animator.StringToHash("death");

        [Header("References")]
        [SerializeField] private Rigidbody rb;
        [SerializeField] private Animator animator;
        [SerializeField] private Collider hitBox;

        [Header("Movement")]
        public float maxSpeed = 20f;
        public float acceleration = 5f;
        public float driftFactor = 0.97f;

        [Header("Steering")]
        public float steeringSpeed = 15f;
        public float rotationSpeed = 50f;
        public float minRotationSpeed = 5f;
        public float minSpeedFactor = 0.05f;

        private float _steeringInput;
        private float _steeringValue;
        private float _currentSpeed;
        private Vector3 _velocity;
        private bool _accelerating;
        private bool _deacceleration;
        private bool _isDead;

        [SerializeField] private int life = 5;
        [SerializeField] private GameObject boatBody;
        [SerializeField] private ParticleSystem fuma;

        // Update is called once per frame
        void Update()
        {
            if(_isDead) return;
            Inputs();

            Steering(_steeringInput);

            if(_accelerating)
            {
                Accelerate();
            }

            if(_deacceleration)
            {
                AddDeacceleration();
            }

            FixVelocity();
            UpdateAnimator();
        }

        void FixedUpdate()
        {
            rb.linearVelocity = _velocity;
        }

        private void Inputs()
        {
            _steeringInput = Input.GetAxisRaw("Horizontal");

            if(Input.GetButtonDown("Accelerate"))
            {
                _accelerating = true;
            }
            else if(Input.GetButtonUp("Accelerate"))
            {
                _accelerating = false;
            }

            if(Input.GetButtonDown("Brake"))
            {
                _deacceleration = true;
            }
            else if(Input.GetButtonUp("Brake"))
            {
                _deacceleration = false;
            }
        }

        private void Accelerate()
        {
            _currentSpeed += acceleration * Time.deltaTime;
        }

        private void AddDeacceleration()
        {
            _currentSpeed -= acceleration * Time.deltaTime;
        }

        private void FixVelocity()
        {
            _currentSpeed = Mathf.Clamp(_currentSpeed, 0f, maxSpeed);
            _velocity = transform.forward * _currentSpeed;

            //
            Vector3 forwardVelocity = transform.forward * Vector3.Dot(_velocity, transform.forward);
            Vector3 rightVelocity = transform.right * Vector3.Dot(_velocity, transform.right);
            Vector3 fixedVelocity = forwardVelocity + rightVelocity * driftFactor;
            _velocity = fixedVelocity;
        }

        private void Steering(float dir)
        {
            float steeringFactor = rb.linearVelocity.magnitude * minSpeedFactor;
            steeringFactor = Mathf.Clamp01(steeringFactor);


            _steeringValue = Mathf.MoveTowards(_steeringValue, dir * steeringFactor, steeringSpeed * Time.deltaTime);
            float finalRotationSpeed = rotationSpeed * steeringFactor;
            finalRotationSpeed = Mathf.Clamp(finalRotationSpeed, minRotationSpeed, rotationSpeed);
            transform.Rotate(0f, finalRotationSpeed * dir * Time.deltaTime, 0f);
        }

        private void UpdateAnimator()
        {
            animator.SetFloat(SteeringValue, _steeringValue);
            float forwardNormalized = Mathf.Clamp01(_velocity.magnitude / maxSpeed);
            animator.SetFloat(ForwardSpeed, forwardNormalized);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("ball"))
            {
                life--;
                if (life <= 0)
                {
                    Die();
                    ShipHasBeenDestroyed();
                }
            }
        }
         private void Die()
         {
             if (life != 0) return;

             Destroy(boatBody);
             fuma.Play();
         }

        public void ShipHasBeenDestroyed()
        {
          Debug.Log("Boat has been destroyed");
          _currentSpeed = 0f;
          _velocity = Vector3.zero;
          hitBox.enabled = false;
          _isDead = true;
          animator.SetTrigger(Death);
        }
    }
}