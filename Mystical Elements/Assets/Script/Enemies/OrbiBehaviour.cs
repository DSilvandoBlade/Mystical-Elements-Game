using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbiBehaviour : MonoBehaviour
{
    #region Private Variables
    private GameObject m_player;
    private EnemyBase m_base;
    private Animator m_animator;
    private float m_timer;
    #endregion

    #region Serialized Variables
    [Header("Temp Refs")]
    [SerializeField] private GameObject m_planet;
    [Space(10)]

    [Header("Tracking Values")]
    [SerializeField] private float m_alertDistance;
    [SerializeField] private float m_playerDistance;
    [SerializeField] private float m_evadeDistance;
    [SerializeField] private float m_timerForNewPath;
    [SerializeField] private bool m_mobile;
    #endregion

    #region Default Functions
    private void Start()
    {
        m_timer = m_timerForNewPath;
        m_player = FindObjectOfType<Player>().gameObject;
        m_animator = GetComponentInChildren<Animator>();
        m_base = GetComponent<EnemyBase>();
    }

    private void Update()
    {
        Movement();
    }
    #endregion

    #region Movement Functions
    private void Movement()
    {
        //MOVEMENT CALCUL
        Vector3 spokeToActual = transform.position - m_planet.transform.position;
        Vector3 spokeToCorrect = m_player.transform.position - m_planet.transform.position;
        float angleFromCenter = Vector3.Angle(spokeToActual, spokeToCorrect);
        float distance = 2 * Mathf.PI * m_planet.GetComponent<SphereCollider>().radius * (angleFromCenter / 360);

        if (distance < m_alertDistance)
        {
            //ROTATIONS TO PLAYER
            Vector3 point = m_player.transform.position;
            Vector3 v = point - transform.position;
            Vector3 d = Vector3.Project(v, transform.up.normalized);
            Vector3 projectedPoint = point - d;

            float angle = Vector3.Angle(-transform.forward, (transform.position - projectedPoint));
            float sign = Mathf.Sign(Vector3.Dot((transform.position - projectedPoint), -transform.right));
            float finalAngle = angle * sign;

            transform.Rotate(0, finalAngle, 0);
        }

        else if (m_timer <= 0)
        {
            //ROTATIONS RANDOM
            float newY = transform.rotation.y + (Random.Range(-45, 45));
            transform.Rotate(0, newY, 0);
            m_timer = m_timerForNewPath;
        }

        //MOVEMENT
        if (m_mobile && (distance > m_playerDistance || distance < m_evadeDistance) && !m_animator.GetCurrentAnimatorStateInfo(0).IsName("Stun"))
        {
            float speed = m_base.Speed;

            if (distance < m_evadeDistance)
            {
                speed = -m_base.Speed * 2f;
            }

            transform.Translate(Vector3.forward * Time.deltaTime * speed);
            m_animator.SetBool("Moving", true);
            m_animator.SetBool("Attacking", false);
        }

        else if (m_mobile && distance < m_playerDistance)
        {
            if (m_animator.GetCurrentAnimatorStateInfo(0).IsName("Stun"))
            {
                m_animator.SetBool("Moving", false);
                m_animator.SetBool("Attacking", false);
            }

            else
            {
                m_animator.SetBool("Moving", false);
                m_animator.SetBool("Attacking", true);
            }
        }

        m_timer -= 1 * Time.deltaTime;
    }
    #endregion
}