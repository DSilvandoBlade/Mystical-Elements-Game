using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OrbiBehaviour : MonoBehaviour
{
    #region Private Variables
    private GameObject m_player;
    private EnemyBase m_base;
    private Animator m_animator;
    private NavMeshAgent m_agent;
    private float m_timer;
    private float m_attackTimer;
    private float m_distance;
    private bool m_knockedback = false;
    private bool m_dead = false;
    private Vector3 m_knockbackDirection;
    #endregion

    #region Serialized Variables
    [Header("Tracking Values")]
    [SerializeField] private float m_alertDistance;
    [SerializeField] private float m_evadeDistance;
    [SerializeField] private float m_timerForNewPath;
    [SerializeField] private bool m_mobile;


    [Header("Attack Values")]
    [SerializeField] private float m_attackMinCooldown = 1;
    [SerializeField] private float m_attackMaxCooldown = 2;
    #endregion

    #region Default Functions
    private void Start()
    {
        m_timer = m_timerForNewPath;
        m_player = FindObjectOfType<Player>().gameObject;
        m_animator = GetComponentInChildren<Animator>();
        m_agent = GetComponent<NavMeshAgent>();
        m_base = GetComponent<EnemyBase>();
    }

    private void Update()
    {
        m_distance = Vector3.Distance(m_player.transform.position, transform.position);

        if (!m_knockedback && !m_dead && m_mobile)
        {
            NavMeshMovement();
        }

        else if (m_knockedback)
        {
            m_agent.velocity = m_knockbackDirection * 8;
        }
    }
    #endregion

    #region Movement Functions
    private void NavMeshMovement()
    {
        m_animator.SetBool("Moving", !InRange() && Alerted());

        if (InRange())
        {
            Attack();
        }

        //ROTATIONS TO PLAYER
        Vector3 point = m_player.transform.position;
        Vector3 v = point - transform.position;
        Vector3 d = Vector3.Project(v, transform.up.normalized);
        Vector3 projectedPoint = point - d;

        float angle = Vector3.Angle(-transform.forward, (transform.position - projectedPoint));
        float sign = Mathf.Sign(Vector3.Dot((transform.position - projectedPoint), -transform.right));
        float finalAngle = angle * sign;

        transform.Rotate(0, finalAngle, 0);


        //Move Forward
        if (Alerted() && !InRange() && !Evade())
        {
            m_agent.SetDestination(m_player.transform.position);

            m_agent.speed = m_base.Speed * 1.5f;
            m_agent.angularSpeed = 0; //Keeps the enemy facing forwad rther than spinning
        }

        else if (Evade())
        {
            m_agent.speed = m_base.Speed * 1.5f;
            m_agent.angularSpeed = 0; //Keeps the enemy facing forwad rther than spinning
        }
    }

    private void Attack()
    {
        if (m_attackTimer <= 0)
        {
            m_animator.SetTrigger("Attack");
            m_attackTimer = Random.Range(m_attackMinCooldown, m_attackMaxCooldown);
        }

        else
        {
            m_attackTimer -= Time.deltaTime;
        }
    }

    /*
    private void Movement()
    {
        if (Alerted())
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
        if (m_mobile && !InRange() && !m_animator.GetCurrentAnimatorStateInfo(0).IsName("Stun"))
        {
            float speed = m_base.Speed;

            if (Evade())
            {
                speed = -m_base.Speed * 1.75f;
            }

            transform.Translate(Vector3.forward * Time.deltaTime * speed);
            m_animator.SetBool("Moving", true);
        }

        else if (m_mobile && InRange())
        {
            if (!m_animator.GetCurrentAnimatorStateInfo(0).IsName("Stun"))
            {
                if (m_attackTimer <= 0)
                {
                    m_animator.SetTrigger("Attack");
                    m_attackTimer = Random.Range(m_attackMinCooldown, m_attackMaxCooldown);
                }

                else
                {
                    m_attackTimer -= Time.deltaTime;
                }
            }
        }

        m_timer -= 1 * Time.deltaTime;
    }*/

    private bool Alerted()
    {
        if (m_distance < m_alertDistance)
        {
            return true;
        }

        return false;
    }

    private bool InRange()
    {
        if (m_distance < m_agent.stoppingDistance && m_distance > m_evadeDistance)
        {
            return true;
        }

        return false;
    }

    private bool Evade()
    {
        if (m_distance < m_evadeDistance)
        {
            return true;
        }

        return false;
    }
    #endregion

    public void Freeze(bool isFrozen)
    {
        m_mobile = !isFrozen;

        if (isFrozen)
        {
            m_animator.speed = 0f;
        }

        else
        {
            m_animator.speed = 1f;
        }
    }
}