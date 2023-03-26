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

    [HideInInspector] public float MeleeDamage
    {
        get { return m_meleeDamage; }
        set { m_meleeDamage = value; }
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

    [Header("Melee Settings")]
    [SerializeField] private float m_meleeDamage;
    [SerializeField] private float m_meleeCooldown;
    private float m_meleeTimer;
    [SerializeField] private float m_meleeTapBuffer;
    private float m_meleeBuffer;
    [Space(10)]

    [Header("Projectile Settings")]
    [SerializeField] private GameObject m_projectile;
    [SerializeField] private Transform m_projectilePivot;
    [SerializeField] private float m_projectileSpeed;
    [SerializeField] private float m_projectileCooldown;
    private float m_projectileTimer;
    [Space(2f)]
    [SerializeField] private float m_chargeMax;
    private float m_chargeTimer;
    private bool m_isCharging;
    [Space(4f)]
    [SerializeField] private float m_projectileDamage;
    [SerializeField] private float m_projectileDamageIncreaseRate;
    private float m_projectileTrueDamage;
    [Space(2f)]
    [SerializeField] private float m_projectileSize;
    [SerializeField] private float m_projectileSizeIncreaseRate;
    private float m_projectileTrueSize;
    [Space(10)]

    [Header("Graphics")]
    [SerializeField] private Transform m_graphicsTransform;
    [SerializeField] private float m_graphicRotationSpeed;
    [SerializeField] private MeshRenderer m_hatMesh;
    [SerializeField] private SkinnedMeshRenderer m_robeMesh;
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

        //Visual
        FieldOfView();
        Animation();
        //MobileTouch();

        if (m_zDirection != 0 || m_xDirection != 0)
        {
            GraphicsRotation();
        }

        //Combat
        ProjectileCharging();
        Melee();
    }

    private void FixedUpdate()
    {
        //Moving Controls
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
        Quaternion rightDirection = Quaternion.Euler(0f, m_rDirection * (m_turnSpeed * Time.fixedDeltaTime), 0f);
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

    #region Camera Movement
    /*
    private void MobileTouch()
    {
        Debug.Log(m_playerInput.actions["Swipe"].ReadValue<float>());

        if (m_playerInput.actions["Swipe"].ReadValue<float>() != 0)
        {
            Debug.Log("DOING IT");
        }
    }*/
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

        StartScreenShake(stunPlayer);

        //TO DO: Damage Animation / Particle effect
    }

    /// <summary>
    /// Called to shake the sccreen
    /// </summary>
    public void StartScreenShake(bool stunned)
    {
        if (stunned)
        {
            CinemachineBasicMultiChannelPerlin perlin = m_camBrain.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            perlin.m_AmplitudeGain = m_camShakeIntensity * 2;
            Invoke("StopScreenShake", m_camShakeTime * 1.5f);

        }

        else
        {
            CinemachineBasicMultiChannelPerlin perlin = m_camBrain.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            perlin.m_AmplitudeGain = m_camShakeIntensity;
            Invoke("StopScreenShake", m_camShakeTime);
        }
        
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

    #region Combat Functions

    private void Melee()
    {
        m_meleeBuffer -= Time.deltaTime;

        if (m_playerInput.actions["Attack"].triggered && !m_isCharging) //is charging boolean is checked so both melee and ranged are not done at the same time
        {
            m_meleeBuffer = m_meleeTapBuffer;
        }

        if (m_meleeBuffer > 0 && m_meleeTimer <= 0)
        {
            m_characterAnim.SetTrigger("Attack");
            m_meleeTimer = m_meleeCooldown;
        }

        if (m_meleeTimer > 0)
        {
            m_meleeTimer -= Time.deltaTime;
        }
    }

    private void ProjectileCharging()
    {
        if (m_playerInput.actions["Range"].ReadValue<float>() > 0 && m_projectileTimer <= 0 && m_chargeTimer < m_chargeMax && m_meleeTimer <= 0 && m_projectileTimer <= 0) //melee cooldown is checked so both melee and ranged are not done at the same time
        {
            m_isCharging = true;

            m_projectileTrueDamage += m_projectileDamageIncreaseRate * Time.deltaTime;
            m_projectileTrueSize += m_projectileSizeIncreaseRate * Time.deltaTime;
            m_chargeTimer += Time.deltaTime;
        }

        else if ((m_playerInput.actions["Range"].WasReleasedThisFrame() && m_isCharging) || m_chargeTimer >= m_chargeMax)
        {
            Debug.Log("WAS RELEASED!");

            m_projectileTrueDamage += m_projectileDamage;
            m_projectileTrueSize += m_projectileSize;

            ProjectileShoot(m_chargeTimer > 1.5f);

            m_projectileTimer = m_projectileCooldown + (m_chargeTimer / 2);

            m_projectileTrueDamage = 0;
            m_projectileTrueSize = 0;
            m_chargeTimer = 0;
        }

        if (m_projectileTimer > 0)
        {
            m_projectileTimer -= Time.deltaTime;
        }
    }

    private void ProjectileShoot(bool stun)
    {
        GameObject projectile = GameObject.Instantiate(m_projectile, m_projectilePivot.position, m_projectilePivot.rotation);

        m_characterAnim.SetBool("LargeShot", stun);

        projectile.transform.localScale = new Vector3(m_projectileTrueSize, m_projectileTrueSize, m_projectileTrueSize);
        projectile.GetComponent<PlayerCombat>().Attack = m_projectileTrueDamage;
        projectile.GetComponent<PlayerProjectile>().Speed = m_projectileSpeed;

        Destroy(projectile, 5f);

        m_isCharging = false;
    }

    #endregion

    #region Visual Functions (Animations)
    /// <summary>
    /// Animation Handler
    /// </summary>
    private void Animation()
    {
        //Sprint Animation
        m_characterAnim.SetBool("Flying", IsSprinting());

        //Movement Animation
        m_characterAnim.SetFloat("Movement", Mathf.Abs(m_playerInput.actions["Move"].ReadValue<Vector2>().x) + Mathf.Abs(m_playerInput.actions["Move"].ReadValue<Vector2>().y));

        //Charging Animation
        m_characterAnim.SetBool("Charge", m_isCharging);
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

    public void ChangeRobeColour(Material[] mat)
    {
        m_hatMesh.materials = mat;
        m_robeMesh.materials = mat;

        Debug.Log("Material changed to " + mat);
    }

    private void GraphicsRotation()
    {
        Vector3 direction = new Vector3(m_playerInput.actions["Move"].ReadValue<Vector2>().x, 0, m_playerInput.actions["Move"].ReadValue<Vector2>().y);
        Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
        m_graphicsTransform.localRotation = Quaternion.RotateTowards(transform.localRotation, toRotation, m_graphicRotationSpeed * Time.deltaTime);
    }
    #endregion

    #region Elemental Functions

    public void SwitchElement(Element element)
    {
        m_selectedElement = element;

        //TO DO: Element Switch animation
    }

    #endregion
}