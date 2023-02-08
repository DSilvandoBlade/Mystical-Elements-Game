using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private float m_speed;
    private float m_setSpeed;
    private float m_turnSpeed = 1500f;

    private Rigidbody m_rigidbody;
    private Vector3 m_direction;

    private Animator m_characterAnim;
    private PlayerInput m_playerInput;

    private GravityBody m_gravityBody;

    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        m_playerInput = GetComponent<PlayerInput>();
        m_gravityBody = GetComponent<GravityBody>();
        m_characterAnim = GetComponentInChildren<Animator>();

        m_setSpeed = m_speed;
    }

    void Update()
    {
        if (m_playerInput.actions["Shift"].ReadValue<float>() == 0)
        {
            m_speed = m_setSpeed;
            m_characterAnim.SetBool("Flying", false);
        }

        else
        {
            m_speed = m_setSpeed * 2;
            m_characterAnim.SetBool("Flying", true);
        }

        m_characterAnim.SetFloat("Movement", Mathf.Abs(m_playerInput.actions["Move"].ReadValue<Vector2>().x) + Mathf.Abs(m_playerInput.actions["Move"].ReadValue<Vector2>().y));

        m_direction = new Vector3(m_playerInput.actions["Move"].ReadValue<Vector2>().x, 0f, m_playerInput.actions["Move"].ReadValue<Vector2>().y);
    }

    void FixedUpdate()
    {
        bool isRunning = m_direction.magnitude > 0.1f;

        if (isRunning)
        {
            Vector3 direction = transform.forward * m_direction.z;
            m_rigidbody.MovePosition(m_rigidbody.position + direction * (m_speed * Time.fixedDeltaTime));

            Quaternion rightDirection = Quaternion.Euler(0f, m_direction.x * (m_turnSpeed * Time.fixedDeltaTime), 0f);
            Quaternion newRotation = Quaternion.Slerp(m_rigidbody.rotation, m_rigidbody.rotation * rightDirection, Time.fixedDeltaTime * 3f); ;
            m_rigidbody.MoveRotation(newRotation);

        }
    }

    /*
    //TEMPORARY SPEED VARIABLE
    [SerializeField] private float m_speed;
    private float m_setSpeed;

    private Rigidbody m_rigidbody;
    private Animator m_characterAnim;
    private PlayerInput m_playerInput;
    private float m_smoothTime = 0.1f;
    private float m_turnSmoothVelocity;

    [SerializeField] Transform cam;

    private void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        m_characterAnim = GetComponentInChildren<Animator>();
        m_playerInput = GetComponent<PlayerInput>();
        m_setSpeed = m_speed;
    }

    private void Update()
    {
        Movement();
    }

    private void Movement()
    {
        if (m_playerInput.actions["Shift"].ReadValue<float>() == 0)
        {
            m_speed = m_setSpeed;
            m_characterAnim.SetBool("Flying", false);
        }

        else
        {
            m_speed = m_setSpeed * 2;
            m_characterAnim.SetBool("Flying", true);
        }

        float horizontal = m_playerInput.actions["Move"].ReadValue<Vector2>().x;
        float vertical = m_playerInput.actions["Move"].ReadValue<Vector2>().y;

        m_characterAnim.SetFloat("Movement", Mathf.Abs(m_playerInput.actions["Move"].ReadValue<Vector2>().x) + Mathf.Abs(m_playerInput.actions["Move"].ReadValue<Vector2>().y));

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref m_turnSmoothVelocity, m_smoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            //m_characterControl.Move(moveDir.normalized * m_speed * Time.deltaTime);

            //transform.Translate(direction * m_speed * Time.deltaTime, Space.Self);
            //m_rigidbody.velocity = direction * m_speed;

            m_rigidbody.MovePosition(m_rigidbody.position + direction * (m_speed * Time.fixedDeltaTime));
        }
    }
    */
}
