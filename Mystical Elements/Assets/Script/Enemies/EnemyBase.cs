using System.Collections;
using System.Collections.Generic;
using ElementTree;
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
    [SerializeField] private Element m_bodyElement;

    [Header("Death References")]
    [SerializeField] private GameObject m_ragDoll;
    [SerializeField] private bool m_isChildObject;
    [SerializeField] private GameObject m_parent;
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
    public void TakeDamage(float damageToRecieve, bool stunEnemy, Element elementType)
    {
        float res = ElementClass.GetFetchResistance(m_bodyElement, elementType);

        m_health = Mathf.Clamp(m_health - (damageToRecieve * res), 0f, m_maxHealth);

        VFXManager vfxManager = FindObjectOfType<VFXManager>();
        vfxManager.SummonHitEffect(transform.position, elementType);
        vfxManager.SummonFloatingText(transform.position, ((int)(damageToRecieve * res)).ToString(), vfxManager.GetElementColour(elementType));

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
            m_rigidbody.velocity = Vector3.zero; //Limits velocity before adding force to avoid stack
            m_rigidbody.AddForce(dir * -50);
        }
    }

    private void Death()
    {
        m_player.StartScreenShake(false);
        Vector3 dir = m_player.transform.position - transform.position;
        GameObject ragdoll = GameObject.Instantiate(m_ragDoll, transform.position, transform.rotation);
        ragdoll.GetComponent<Rigidbody>().AddForce(dir * -5000);
        Destroy(ragdoll, 5f);

        if (m_isChildObject)
        {
            Destroy(m_parent);
        }
        Destroy(gameObject);
    }
    #endregion
}
