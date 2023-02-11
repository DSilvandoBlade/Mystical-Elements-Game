using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Rigidbody m_rigidbody;
    private PlayerInput m_playerInput;
    private CapsuleCollider m_collider;
    private GravityBody m_gravityBody;
    private Animator m_characterAnim;
    private Vector3 m_direction;

    private float m_xDirection;
    private float m_zDirection;
    private float m_rDirection;

    [Header("Motion")]
    [SerializeField] private float m_maxSpeed;
    private float m_setMaxSpeed;
    [SerializeField] private float m_turnSpeed;
    
    [Space(10)]

    [Header("Jump")]
    [SerializeField] private GameObject groundCheckObj;
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private float m_jumpUpVelocity;
    [SerializeField] private float m_coyoteTime;
    [SerializeField] private float m_jumpBufferingTime;
    [Space(10)]

    [Header("Graphics")]
    [SerializeField] private Transform m_graphicsTransform;
    [SerializeField] private float m_graphicRotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        m_playerInput = GetComponent<PlayerInput>();
        m_collider = GetComponent<CapsuleCollider>();
        m_gravityBody = GetComponent<GravityBody>();
        m_characterAnim = GetComponentInChildren<Animator>();

        m_setMaxSpeed = m_maxSpeed;

        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = 30;
    }

    private void Update()
    {
        m_xDirection = m_playerInput.actions["Move"].ReadValue<Vector2>().x * Time.deltaTime * m_maxSpeed;
        m_zDirection = m_playerInput.actions["Move"].ReadValue<Vector2>().y * Time.deltaTime * m_maxSpeed;
        m_rDirection = m_playerInput.actions["Rot"].ReadValue<float>();

        Animation();

        if (m_zDirection != 0 || m_xDirection != 0)
        {
            GraphicsRotation();
        }
    }

    private void FixedUpdate()
    {
        //Movement
        Vector3 direction = transform.forward * m_zDirection + transform.right * m_xDirection;
        m_rigidbody.MovePosition(m_rigidbody.position + direction * (m_maxSpeed * Time.fixedDeltaTime));

        //Local Rotation
        Quaternion rightDirection = Quaternion.Euler(0f, m_rDirection * (100 * m_turnSpeed * Time.fixedDeltaTime), 0f);
        Quaternion newRotation = Quaternion.Slerp(m_rigidbody.rotation, m_rigidbody.rotation * rightDirection, Time.fixedDeltaTime * 3f); ;
        m_rigidbody.MoveRotation(newRotation);

        //Jump
        if (m_playerInput.actions["Jump"].triggered)
        {
            m_rigidbody.AddForce(transform.up * 1000 * m_jumpUpVelocity * Time.deltaTime);
        }
    }

    private void Animation()
    {
        //Sprint Anim
        if (m_playerInput.actions["Shift"].ReadValue<float>() == 0)
        {
            m_maxSpeed = m_setMaxSpeed;
            m_characterAnim.SetBool("Flying", false);
        }

        else
        {
            m_maxSpeed = m_setMaxSpeed * 1.5f;
            m_characterAnim.SetBool("Flying", true);
        }

        //Movement Anim
        m_characterAnim.SetFloat("Movement", Mathf.Abs(m_playerInput.actions["Move"].ReadValue<Vector2>().x) + Mathf.Abs(m_playerInput.actions["Move"].ReadValue<Vector2>().y));
    }

    private void GraphicsRotation()
    {
        Vector3 direction = new Vector3(m_xDirection, 0, m_zDirection);
        Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
        m_graphicsTransform.localRotation = Quaternion.RotateTowards(transform.localRotation, toRotation, m_graphicRotationSpeed * Time.deltaTime);
    }
}