using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PhysicsController : MonoBehaviour
{
    private Rigidbody m_rigidBody;
    private PlayerInput m_playerInput;
    private CapsuleCollider m_collider;
    private GravityBody m_gravityBody;
    private Vector3 m_direction;

    [Header("Motion")]
    [SerializeField] private float m_maxSpeed;
    [SerializeField] private float m_turnSpeed = 1500f;
    [SerializeField] private float m_acceleration;
    [Space(10)]

    [Header("Jump")]
    [SerializeField] private float m_jumpUpVelocity;
    [SerializeField] private float m_terminalVelocity;
    [SerializeField] private float m_jumpDuration;
    [SerializeField] private float m_coyoteTime;
    [SerializeField] private float m_jumpBufferingTime;
    [Space(10)]

    [Header("Graphics")]
    [SerializeField] private Transform m_graphicsTransform;
    [Space(10)]

    [Header("Raycast")]
    private RaycastHit m_rayHit;
    private bool m_rayDidHit;
    public float UpwardForce;
    [SerializeField] private float rayLength;
    public float RideHeight;
    public float RideSpringStrength;
    public float RideSpringDamper;


    // Start is called before the first frame update
    private void Start()
    {
        m_rigidBody = GetComponent<Rigidbody>();
        m_playerInput = GetComponent<PlayerInput>();
        m_collider = GetComponent<CapsuleCollider>();
        m_gravityBody = GetComponent<GravityBody>();
    }

    private void Update()
    {
        m_direction = new Vector3(m_playerInput.actions["Move"].ReadValue<Vector2>().x, 0f, m_playerInput.actions["Move"].ReadValue<Vector2>().y);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (m_direction.z > 0.1)
        {
            Vector3 direction = transform.forward * m_direction.z;
            m_rigidBody.MovePosition(m_rigidBody.position + direction * (m_maxSpeed * Time.fixedDeltaTime));

            Quaternion rightDirection = Quaternion.Euler(0f, m_direction.x * (m_turnSpeed * Time.fixedDeltaTime), 0f);
            Quaternion newRotation = Quaternion.Slerp(m_rigidBody.rotation, m_rigidBody.rotation * rightDirection, Time.fixedDeltaTime * 3f); ;
            m_rigidBody.MoveRotation(newRotation);
        }

        else
        {
            Vector3 forwardDirection = transform.forward * m_direction.z;
            Vector3 sidewardDirection = transform.right * m_direction.x;
            m_rigidBody.MovePosition(m_rigidBody.position + (forwardDirection + sidewardDirection) * (m_maxSpeed * Time.deltaTime));

            //Quaternion toRotation = Quaternion.LookRotation((forwardDirection + sidewardDirection));
            //m_graphicsTransform.rotation = Quaternion.RotateTowards(m_graphicsTransform.rotation, toRotation, 2 * Time.deltaTime);
        }
    }
}
