using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    #region Serialized Variables
    [Header("Attributes")]
    [SerializeField] private float m_speed;
    [SerializeField] private float m_maxHealth;
    private float m_health;
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
        m_health = m_maxHealth;
    }
    #endregion

    #region Health Functions
    public void TakeDamage(float damageToRecieve, bool stunEnemy)
    {
        m_health = Mathf.Clamp(m_health - damageToRecieve, m_maxHealth, 0f);

        //call floating text number

        if (m_health <= 0)
        {
            Death();
        }

        if (stunEnemy)
        {
            //hurt animation & stun
        }
    }

    private void Death()
    {
        Destroy(this.gameObject);
    }
    #endregion
}
