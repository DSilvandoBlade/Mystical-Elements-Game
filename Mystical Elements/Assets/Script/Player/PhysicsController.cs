using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PhysicsController : MonoBehaviour
{
    private Rigidbody m_rigidbody;
    private PlayerInput m_playerInput;
    private CapsuleCollider m_collider;
    private GravityBody m_gravityBody;
    private Animator m_characterAnim;
    private Vector3 m_direction;

    

    [Header("Motion")]
    [SerializeField] private float m_maxSpeed;
    private float m_setMaxSpeed;
    [SerializeField] private float m_acceleration;
    [SerializeField] private float m_turnSpeed;
    [SerializeField] private Transform m_orientation;
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
        m_rigidbody = GetComponent<Rigidbody>();
        m_playerInput = GetComponent<PlayerInput>();
        m_collider = GetComponent<CapsuleCollider>();
        m_gravityBody = GetComponent<GravityBody>();
        m_characterAnim = GetComponentInChildren<Animator>();

        m_setMaxSpeed = m_maxSpeed;
    }

    private void Update()
    {
        m_direction = new Vector3(m_playerInput.actions["Move"].ReadValue<Vector2>().x, 0f, m_playerInput.actions["Move"].ReadValue<Vector2>().y);

        Animation();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        Vector3 direction = transform.forward * m_direction.z;
        m_rigidbody.MovePosition(m_rigidbody.position + direction * (m_maxSpeed * Time.fixedDeltaTime));

        float turnSpeed = m_turnSpeed;

        if (m_direction.z < 0)
        {
            turnSpeed = -turnSpeed;
        }

        Quaternion rightDirection = Quaternion.Euler(0f, m_direction.x * (m_turnSpeed * Time.fixedDeltaTime), 0f);
        Quaternion newRotation = Quaternion.Slerp(m_rigidbody.rotation, m_rigidbody.rotation * rightDirection, Time.fixedDeltaTime * 3f);
        m_rigidbody.MoveRotation(newRotation);
    }

    private void Animation()
    {
        if (m_playerInput.actions["Shift"].ReadValue<float>() == 0)
        {
            m_maxSpeed = m_setMaxSpeed;
            m_characterAnim.SetBool("Flying", false);
        }

        else
        {
            m_maxSpeed = m_setMaxSpeed * 2;
            m_characterAnim.SetBool("Flying", true);
        }

        m_characterAnim.SetFloat("Movement", Mathf.Abs(m_playerInput.actions["Move"].ReadValue<Vector2>().x) + Mathf.Abs(m_playerInput.actions["Move"].ReadValue<Vector2>().y));
    }
}
