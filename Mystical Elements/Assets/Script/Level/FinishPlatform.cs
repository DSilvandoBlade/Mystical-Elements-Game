using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishPlatform : MonoBehaviour
{
    [SerializeField] private ElementalRod[] m_rods;
    [SerializeField] private GameObject m_ui;

    private bool m_levelFinishActivate = false;
    private EnemyBase[] m_allEnemies;
    private Animator m_animator;

    private void Start()
    {
        m_animator = GetComponent<Animator>();
        m_allEnemies = FindObjectsOfType<EnemyBase>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && m_levelFinishActivate)
        {
            Debug.Log("Game Finished");
            m_animator.SetTrigger("Win");
            
            foreach (EnemyBase enemy in m_allEnemies)
            {
                Destroy(enemy.gameObject);
            }

            FindObjectOfType<Player>().gameObject.SetActive(false);
            m_ui.SetActive(false);
        }
    }

    public void CheckRodActives()
    {
        if (AllRodsActive())
        {
            m_levelFinishActivate = true;
        }
    }

    private bool AllRodsActive()
    {
        foreach (ElementalRod rod in m_rods)
        {
            if (!rod.Activated)
            {
                return false;
            }
        }

        m_animator.SetTrigger("Ready");
        return true;
    }
}
