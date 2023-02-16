using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    #region Private Variables
    private Animator m_animator;
    private Player m_player;
    private Rigidbody m_rigidbody;
    #endregion

    #region Serialized Variables
    [Header("Attributes")]
    [SerializeField] private float m_speed;
    [SerializeField] private float m_maxHealth;
    private float m_health;

    [Header("Death References")]
    [SerializeField] private GameObject m_ragDoll;
    #endregion

    #region Public Variables
    [HideInInspector] public float Speed
    {
        get { return m_speed; }
        set { m_speed = value; }
    }
    #endregion

    #region Default Functions
    void Start()
    {
        m_animator = GetComponentInChildren<Animator>();
        m_player = FindObjectOfType<Player>();
        m_rigidbody = GetComponent<Rigidbody>();

        m_health = m_maxHealth;
    }
    #endregion

    #region Health Functions
    public void TakeDamage(float damageToRecieve, bool stunEnemy)
    {
        m_health = Mathf.Clamp(m_health - damageToRecieve, 0f, m_maxHealth);

        Debug.Log("Damage: " + damageToRecieve + " Stun: " + stunEnemy + " Current Health: " + m_health);

        //call floating text number

        if (m_health <= 0)
        {
            Death();
        }

        if (stunEnemy)
        {
            m_animator.SetTrigger("Stun");
            Vector3 dir = m_player.transform.position - transform.position;
            m_rigidbody.AddForce(dir * -50);
        }
    }

    private void Death()
    {
        Vector3 dir = m_player.transform.position - transform.position;
        GameObject ragdoll = GameObject.Instantiate(m_ragDoll, transform.position, transform.rotation);
        ragdoll.GetComponent<Rigidbody>().AddForce(dir * -5000);
        Destroy(ragdoll, 5f);
        Destroy(this.gameObject);
    }
    #endregion
}
