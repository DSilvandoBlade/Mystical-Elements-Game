using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaukBehaviour : MonoBehaviour
{
    #region Private Variables
    private GameObject m_player;
    private EnemyBase m_base;
    private Animator m_animator;
    private float m_timer;
    private float m_distance;
    #endregion

    #region Serialized Variables
    [Header("Tracking Values")]
    [SerializeField] private float m_alertDistance;
    [SerializeField] private float m_playerDistance;
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
        m_distance = Vector3.Distance(m_player.transform.position, transform.position);

        Movement();
    }
    #endregion

    #region Movement Functions
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
            transform.Translate(Vector3.forward * Time.deltaTime * m_base.Speed);
            m_animator.SetBool("Moving", true);
            m_animator.SetBool("Attacking", false);
        }

        else if (m_mobile && InRange())
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
        if (m_distance < m_playerDistance)
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
