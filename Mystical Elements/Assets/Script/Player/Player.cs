using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using ElementTree;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    #region Public Variables
    [HideInInspector] public Element SelectedElement
    {
        get { return m_selectedElement; }
        set { m_selectedElement = value; }
    }
    #endregion

    #region Private Variables
    private Rigidbody m_rigidbody;
    private PlayerInput m_playerInput;
    private CapsuleCollider m_collider;
    private GravityBody m_gravityBody;
    private Animator m_characterAnim;
    private Vector3 m_direction;

    private float m_xDirection;
    private float m_zDirection;
    private float m_rDirection;
    #endregion

    #region Serialized Variables
    [Header("Attributes")]
    private float m_health;
    [SerializeField] private float m_maxHealth;
    [SerializeField] private Element m_selectedElement;
    [Space(10)]

    [Header("Motion")]
    [SerializeField] private float m_maxSpeed;
    private float m_setMaxSpeed;
    [SerializeField] private float m_turnSpeed;
    [Space(10)]

    [Header("Ground")]
    [SerializeField] private Transform m_groundCheckObj;
    [SerializeField] private LayerMask m_groundLayerMask;
    [SerializeField] private PhysicMaterial slipperyMat;
    [SerializeField] private PhysicMaterial stickyMat;
    [Space(10)]

    [Header("Jump")]
    [SerializeField] private float m_jumpUpVelocity;
    [SerializeField] private float m_coyoteTime;
    private float m_coyoteTimer;
    [SerializeField] private float m_jumpBufferingTime;
    private float m_jumpBufferingTimer;
    [Space(10)]

    [Header("Graphics")]
    [SerializeField] private Transform m_graphicsTransform;
    [SerializeField] private float m_graphicRotationSpeed;
    [Space(10)]

    [Header("HUD References")]
    [SerializeField] private Image m_healthBar;
    [SerializeField] private Image m_energyBar;
    [Space(10)]

    [Header("Cam")]
    [SerializeField] private CinemachineVirtualCamera m_camBrain;
    [SerializeField] private float m_camShakeIntensity;
    [SerializeField] private float m_camShakeTime;
    [SerializeField] private float m_fieldOfViewIncrease;
    #endregion

    #region Default Functions (Start & Update)
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        m_playerInput = GetComponent<PlayerInput>();
        m_collider = GetComponent<CapsuleCollider>();
        m_gravityBody = GetComponent<GravityBody>();
        m_characterAnim = GetComponentInChildren<Animator>();

        m_setMaxSpeed = m_maxSpeed;
        m_health = m_maxHealth;

        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = 30;
    }

    private void Update()
    {
        m_xDirection = m_playerInput.actions["Move"].ReadValue<Vector2>().x * Time.deltaTime * m_maxSpeed;
        m_zDirection = m_playerInput.actions["Move"].ReadValue<Vector2>().y * Time.deltaTime * m_maxSpeed;
        m_rDirection = m_playerInput.actions["Rot"].ReadValue<float>();

        FieldOfView();
        Animation();

        if (m_zDirection != 0 || m_xDirection != 0)
        {
            GraphicsRotation();
        }
    }

    private void FixedUpdate()
    {
        Movement();
        Jump();
    }
    #endregion

    #region Player Movement Control Functions (Walking & Jumping)
    /// <summary>
    /// Movement & Rotation functionality
    /// </summary>
    private void Movement()
    {
        //Speed Multiply on sprint check
        float speedMultiply = 1f;
        if (IsSprinting())
        {
            speedMultiply = 1.5f;
        }

        //Movement
        Vector3 direction = transform.forward * m_zDirection + transform.right * m_xDirection;
        m_rigidbody.MovePosition(m_rigidbody.position + direction * ((m_maxSpeed * speedMultiply) * Time.fixedDeltaTime));

        //Local Rotation
        Quaternion rightDirection = Quaternion.Euler(0f, m_rDirection * (100 * m_turnSpeed * Time.fixedDeltaTime), 0f);
        Quaternion newRotation = Quaternion.Slerp(m_rigidbody.rotation, m_rigidbody.rotation * rightDirection, Time.fixedDeltaTime * 3f); ;
        m_rigidbody.MoveRotation(newRotation);
    }

    /// <summary>
    /// Jump functionality
    /// </summary>
    private void Jump()
    {
        if (m_playerInput.actions["Jump"].triggered)
        {
            m_jumpBufferingTimer = m_jumpBufferingTime;
            Debug.Log("JumpInputted");
        }

        m_jumpBufferingTimer -= Time.deltaTime;

        if (!IsGrounded())
        {
            m_coyoteTimer -= Time.deltaTime;
        }

        if (m_coyoteTimer > 0 && JumpAvaliable() && m_jumpBufferingTimer > 0)
        {
            m_rigidbody.AddForce(transform.up * 1000 * m_jumpUpVelocity * Time.deltaTime);
            Debug.Log("Jumped");
        }
    }

    /// <summary>
    /// Function checks if the player has an avaliable jump
    /// </summary>
    private bool JumpAvaliable()
    {
        return IsGrounded() || (!IsGrounded() && m_coyoteTime <= 0);
    }

    /// <summary>
    /// Function checks if the player is on the ground as well as changing mats
    /// </summary>
    private bool IsGrounded()
    {
        if (!Physics.CheckSphere(m_groundCheckObj.position, 0.25f, m_groundLayerMask))
        {
            m_collider.sharedMaterial = slipperyMat;
            return false;
        }

        if (m_jumpBufferingTime > 0)
        {
            m_collider.sharedMaterial = stickyMat;
        }

        m_coyoteTimer = m_coyoteTime;
        return true;
    }

    /// <summary>
    /// Function checks if the player is holding the sprint button
    /// </summary>
    private bool IsSprinting()
    {
        return (m_playerInput.actions["Shift"].ReadValue<float>() != 0);
    }
    #endregion

    #region Health Functions (Add / Remove)

    /// <summary>
    /// Function that is called when the player recieves healing
    /// </summary>
    /// <param name="healthToAdd"> Amount of healing recieved </param>
    public void HealPlayer(float healthToAdd)
    {
        m_health = Mathf.Clamp(m_health + healthToAdd, m_maxHealth, 0f);

        m_healthBar.fillAmount = m_health / m_maxHealth;
        //TO DO: Healing Animation / Particle effect
    }

    /// <summary>
    /// Function that is called when the player is damaged
    /// </summary>
    /// <param name="healthToRemove"> Amount of damage recieved </param>
    /// <param name="stunPlayer"> Boolean that checkes iff the attack can stun the player </param>
    public void DamagePlayer(float healthToRemove, bool stunPlayer, Element elementType)
    {
        m_health = Mathf.Clamp(m_health - healthToRemove, 0f, m_maxHealth);

        m_healthBar.fillAmount = m_health / m_maxHealth;

        //Debug.Log("Damage: " + healthToRemove + " Stun: " + stunPlayer + " Current Health: " + m_health);

        VFXManager vfxManager = FindObjectOfType<VFXManager>();
        vfxManager.SummonHitEffect(transform.position, elementType);
        vfxManager.SummonFloatingText(transform.position, ((int)healthToRemove).ToString(), vfxManager.GetElementColour(elementType));

        if (m_health <= 0)
        {
            Death();
            return;
        }

        if (stunPlayer)
        {
            CinemachineBasicMultiChannelPerlin perlin = m_camBrain.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            perlin.m_AmplitudeGain = m_camShakeIntensity * 2;
            Invoke("StopScreenShake", m_camShakeTime * 1.5f);

            //Call Stun Player
            //NOTE: Have a check for if the stun function is already being played so it doesn't stack
        }

        else
        {
            CinemachineBasicMultiChannelPerlin perlin = m_camBrain.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            perlin.m_AmplitudeGain = m_camShakeIntensity;
            Invoke("StopScreenShake", m_camShakeTime);
        }

        //TO DO: Damage Animation / Particle effect
    }

    /// <summary>
    /// Called to stop the cinemachine camera from shaking
    /// </summary>
    private void StopScreenShake()
    {
        CinemachineBasicMultiChannelPerlin perlin = m_camBrain.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        perlin.m_AmplitudeGain = 0;
    }

    /// <summary>
    /// Function that kills off the player and ends the game
    /// </summary>
    private void Death()
    {
        //TO DO: Death animation that will show progress
    }

    #endregion

    #region Visual Variables (Animations)
    /// <summary>
    /// Animation Handler
    /// </summary>
    private void Animation()
    {
        //Sprint Anim
        m_characterAnim.SetBool("Flying", IsSprinting());

        //Movement Anim
        m_characterAnim.SetFloat("Movement", Mathf.Abs(m_playerInput.actions["Move"].ReadValue<Vector2>().x) + Mathf.Abs(m_playerInput.actions["Move"].ReadValue<Vector2>().y));

        //Basic Attack Anim
        if (m_playerInput.actions["Attack"].triggered)
        {
            m_characterAnim.SetTrigger("Attack");
        }
    }

    private void FieldOfView()
    {
        float fov = 40;

        if (IsSprinting())
        {
            fov += m_fieldOfViewIncrease;
        }

        else if (m_playerInput.actions["Move"].ReadValue<Vector2>() != Vector2.zero)
        {
            fov += m_fieldOfViewIncrease / 4;
        }

        m_camBrain.m_Lens.FieldOfView = Mathf.SmoothStep(m_camBrain.m_Lens.FieldOfView, fov, 0.5f); ;
        
    }

    private void GraphicsRotation()
    {
        Vector3 direction = new Vector3(m_xDirection, 0, m_zDirection);
        Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
        m_graphicsTransform.localRotation = Quaternion.RotateTowards(transform.localRotation, toRotation, m_graphicRotationSpeed * Time.deltaTime);
    }
    #endregion
}