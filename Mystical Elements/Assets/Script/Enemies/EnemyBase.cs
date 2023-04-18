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
    private Element m_layerElement;
    private float m_resShred = 1f;

    private enum EnemyType
    {
        Pauk,
        Orbi,
        Rook
    }
    #endregion

    #region Serialized Variables
    [Header("Basic")]
    [SerializeField] private EnemyType m_enemyType;
    [SerializeField] private GameObject m_elementalReactions;
    [Space(10)]

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

    [HideInInspector] public float ResShred
    {
        get { return m_resShred; }
        set { m_resShred = value; }
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
    public void TakeDamage(float damageToRecieve, bool stunEnemy, Element elementType, bool reactionDamage)
    {
        float res = ElementClass.GetFetchResistance(m_bodyElement, elementType);

        m_health = Mathf.Clamp(m_health - ((damageToRecieve * m_resShred) * res), 0f, m_maxHealth);

        VFXManager vfxManager = FindObjectOfType<VFXManager>();
        vfxManager.SummonHitEffect(transform.position, elementType);
        vfxManager.SummonFloatingText(transform.position, ((int)((damageToRecieve * m_resShred) * res)).ToString(), vfxManager.GetElementColour(elementType));

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

        if (!reactionDamage)
        {
            ReactionCheck(elementType);
        }
        else
        {
            m_layerElement = Element.DEFAULT;
        }
    }

    private void ReactionCheck(Element incomingElement)
    {
        if (m_layerElement == Element.DEFAULT || m_layerElement == incomingElement)
        {
            m_layerElement = incomingElement;
            CancelInvoke("LayerElementReset");
            Invoke("LayerElementReset", 5f);
        }

        else
        {
            GameObject em = GameObject.Instantiate(m_elementalReactions, transform.position, transform.rotation);
            em.GetComponent<ElementalReaction>().Enemy = this;
            em.GetComponent<ElementalReaction>().ChooseElementalReaction(m_layerElement, incomingElement);
            LayerElementReset();
        }
    }

    public void LayerElementReset()
    {
        m_layerElement = Element.DEFAULT;
    }

    private void Death()
    {
        m_player.StartScreenShake(false);
        Vector3 dir = m_player.transform.position - transform.position;
        GameObject ragdoll = GameObject.Instantiate(m_ragDoll, transform.position, transform.rotation);
        ragdoll.GetComponent<Rigidbody>().AddForce(dir * -5000);
        Destroy(ragdoll, 5f);

        FindObjectOfType<GradeSystem>().EnemiesDefeated++;

        if (m_isChildObject)
        {
            Destroy(m_parent);
        }
        Destroy(gameObject);
    }
    #endregion

    public void Freeze(bool isFrozen)
    {
        switch (m_enemyType)
        {
            case EnemyType.Pauk:
                GetComponent<PaukBehaviour>().Freeze(isFrozen);
                break;
            case EnemyType.Orbi:
                GetComponent<OrbiBehaviour>().Freeze(isFrozen);
                break;
            case EnemyType.Rook:
                GetComponent<RookBehaviour>().Freeze(isFrozen);
                break;
            default:
                Debug.LogWarning("Behaviour could not be found to freeze");
                break;
        }
    }
}
