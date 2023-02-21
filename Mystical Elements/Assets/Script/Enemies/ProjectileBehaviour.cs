using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    #region Private Variables
    private EnemyAttack m_enemyAttack;
    #endregion

    #region Default Functions
    private void Start()
    {
        m_enemyAttack = GetComponent<EnemyAttack>();
    }
    #endregion

    #region Collision Functions
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" || other.tag == "Ground")
        {
            FindObjectOfType<VFXManager>().SummonHitEffect(transform.position, m_enemyAttack.ElementType);

            Destroy(gameObject, 0.01f);
        }
    }
    #endregion
}
