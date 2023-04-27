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
    private float m_attackTimer;
    private float m_distance;
    private bool m_knockedback = false;
    private bool m_dead = false;
    private Vector3 m_knockbackDirection;
    #endregion

    #region Serialized Variables
    [Header("Tracking Values")]
    [SerializeField] private float m_alertDistance;
    [SerializeField] private float m_timerForNewPath;
    [SerializeField] private bool m_mobile;


    [Header("Attack Values")]
    [SerializeField] private float m_attackMinCooldown = 1;
    [SerializeField] private float m_attackMaxCooldown = 2;
    #endregion

    #region Default Functions
    private void Start()
    {
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
        if (Alerted() && PathPossible())
        {
            m_agent.SetDestination(m_player.transform.position);

            m_agent.speed = m_base.Speed;
            m_agent.angularSpeed = 500; //Keeps the enemy facing forwad rther than spinning
        }
    }

    private bool PathPossible()
    {
        NavMeshPath navMeshPath = new NavMeshPath();
        if (m_agent.CalculatePath(m_player.transform.position, navMeshPath))
        {
            switch (navMeshPath.status)
            {
                case NavMeshPathStatus.PathComplete:
                    return true;

                case NavMeshPathStatus.PathPartial:
                    return false;

                case NavMeshPathStatus.PathInvalid:
                    return false;
            }
        }

        return false;
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
        if (Alerted() && m_agent.remainingDistance <= m_agent.stoppingDistance)
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