using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ElementTree;

public class ElementalReaction : MonoBehaviour
{
    #region Public Variables
    [HideInInspector] public EnemyBase Enemy
    {
        get { return m_enemy; }
        set { m_enemy = value; }
    }
    #endregion

    #region Private Variables
    private Player m_player;
    private EnemyBase m_enemy;
    private SphereCollider m_sphereCollider;
    private VFXManager m_vFXManager;
    #endregion

    #region Serialized Variables
    [Header("General Attributes")]
    [SerializeField] private Vector3 m_textOffset;

    [Header("Evaporation Attributes")]
    [SerializeField] private float m_evaporationDamage;
    [SerializeField] private float m_evaporationTickTime;
    [SerializeField] private int m_evaporationMaxTicks;
    private int m_evaporationTickAmount;
    [Space(10)]

    [Header("Growth Attributes")]
    [SerializeField] private float m_growthDamage;
    [SerializeField] private float m_growthSubDamage;
    [SerializeField] private float m_growthRate;
    [SerializeField] private float m_growthRadius;
    private List<EnemyBase> m_surroundingEnemies = new List<EnemyBase>();
    [Space(10)]

    [Header("Energize Attributes")]
    [SerializeField] private float m_energizeResistanceShred;
    [SerializeField] private float m_energizeTimer;
    [Space(10)]

    [Header("Condensate Attributes")]
    [SerializeField] private float m_condensateDamage;
    [Space(10)]

    [Header("Burst Attributes")]
    [SerializeField] private float m_burstDamage;
    [SerializeField] private float m_burstForce;
    [SerializeField] private float m_burstRadius;
    [Space(10)]

    [Header("Stick Attributes")]
    [SerializeField] private float m_stickDuration;
    #endregion

    private void Awake()
    {
        m_player = FindObjectOfType<Player>();
        m_sphereCollider = GetComponent<SphereCollider>();
        m_vFXManager = FindObjectOfType<VFXManager>();
    }

    #region Reaction Function
    public void ChooseElementalReaction(Element layer, Element incoming)
    {
        switch (layer)
        {
            case Element.PLASMA:
                switch (incoming)
                {
                    case Element.CHARGE:
                        BurstReaction();
                        break;
                    case Element.FLORA:
                        EvaporateReaction();
                        break;
                    case Element.FROST:
                        CondensateReaction();
                        break;
                    default:
                        Debug.LogError("Incoming Element could not be paired");
                        break;
                }
                break;
            case Element.CHARGE:
                switch (incoming)
                {
                    case Element.PLASMA:
                        BurstReaction();
                        break;
                    case Element.FLORA:
                        GrowthReaction();
                        break;
                    case Element.FROST:
                        EnergizeReaction();
                        break;
                    default:
                        Debug.LogError("Incoming Element could not be paired");
                        break;
                }
                break;
            case Element.FROST:
                switch (incoming)
                {
                    case Element.CHARGE:
                        EnergizeReaction();
                        break;
                    case Element.FLORA:
                        StickReaction();
                        break;
                    case Element.PLASMA:
                        CondensateReaction();
                        break;
                    default:
                        Debug.LogError("Incoming Element could not be paired");
                        break;
                }
                break;
            case Element.FLORA:
                switch (incoming)
                {
                    case Element.CHARGE:
                        GrowthReaction();
                        break;
                    case Element.PLASMA:
                        EvaporateReaction();
                        break;
                    case Element.FROST:
                        StickReaction();
                        break;
                    default:
                        Debug.LogError("Incoming Element could not be paired");
                        break;
                }
                break;
            default:
                Debug.LogError("Layer Element not found");
                break;
        }
    }
#endregion

    #region Evaporation Function
    private void EvaporateReaction()
    {
        m_evaporationTickAmount = m_evaporationMaxTicks;

        Invoke("EvaporateTick", m_evaporationTickTime);
    }

    private void EvaporateTick()
    {
        m_enemy.TakeDamage(m_evaporationDamage, false, Element.PLASMA, true);
        m_vFXManager.SummonFloatingText(m_enemy.transform.position + m_textOffset, "EVAPORATION", Color.red);
        m_evaporationTickAmount--;

        if (m_evaporationTickAmount > 0)
        {
            Invoke("EvaporateTick", m_evaporationTickTime);
        }
    }
    #endregion

    #region Growth Function
    private void GrowthReaction()
    {
        int enemyCount = 0;

        Collider[] c = Physics.OverlapSphere(transform.position, m_growthRadius, 3);

        foreach (Collider collider in c)
        {
            EnemyBase enemy = collider.gameObject.GetComponent<EnemyBase>();

            enemy.TakeDamage(m_growthSubDamage, false, Element.FLORA, true);
            m_vFXManager.SummonFloatingText(m_enemy.transform.position + m_textOffset, "GROWTH", Color.green);
            enemyCount++;
        }

        Debug.Log(enemyCount);

        m_enemy.TakeDamage(m_growthDamage + (m_growthRate * enemyCount), false, Element.DEFAULT, true);
    }
    #endregion

    #region Energize Function
    private void EnergizeReaction()
    {
        m_enemy.ResShred = m_energizeResistanceShred;
        m_vFXManager.SummonFloatingText(m_enemy.transform.position + m_textOffset, "ENERGIZE", Color.yellow);
        Invoke("RemoveResShred", m_energizeTimer);
    }

    private void RemoveResShred()
    {
        m_enemy.ResShred = 1f;
    }
    #endregion

    #region Condensate Function
    private void CondensateReaction()
    {
        Debug.LogWarning("CONDENSATE");
        m_enemy.TakeDamage(m_condensateDamage, true, Element.FROST, true);
        m_vFXManager.SummonFloatingText(m_enemy.transform.position + m_textOffset, "CONDENSATE", Color.blue);
    }
    #endregion

    private void BurstReaction()
    {
        Collider[] c = Physics.OverlapSphere(transform.position, m_burstRadius, 3);

        foreach (Collider collider in c)
        {
            EnemyBase enemy = collider.gameObject.GetComponent<EnemyBase>();

            m_vFXManager.SummonFloatingText(m_enemy.transform.position + m_textOffset, "BURST", Color.white);
            enemy.TakeDamage(m_growthSubDamage, true, Element.DEFAULT, true);
            enemy.GetComponent<Rigidbody>().AddExplosionForce(m_burstForce, transform.position, m_sphereCollider.radius);
        }
    }

    #region Stick Function
    private void StickReaction()
    {
        m_vFXManager.SummonFloatingText(m_enemy.transform.position + m_textOffset, "STICK", Color.white);
        m_enemy.Freeze(true);
        Invoke("Unstick", m_stickDuration);
    }

    private void Unstick()
    {
        m_enemy.Freeze(false);
    }
    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            m_surroundingEnemies.Add(other.GetComponent<EnemyBase>());
        }
    }
}
