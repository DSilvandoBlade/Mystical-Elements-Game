using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerCombat : MonoBehaviour
{
    #region Public Variables
    [HideInInspector] public float Attack
    {
        get { return m_attack; }
        set { m_attack = value; }
    }

    [HideInInspector] public bool DoesStun
    {
        get { return m_doesStun; }
        set { m_doesStun = value; }
    }
    #endregion

    #region Serialize Variables
    [Header("Attack Attributes")]
    [SerializeField] private float m_attack;
    [SerializeField] private bool m_doesStun;
    #endregion

    #region Private Variables
    private Player m_player;
    #endregion

    #region Default Functions
    private void Start()
    {
        m_player = FindObjectOfType<Player>();
    }
    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<EnemyBase>().TakeDamage(m_attack, m_doesStun, m_player.SelectedElement, false);
        }

        if (other.tag == "Activator")
        {
            other.GetComponent<ElementalRod>().Activate(m_player.SelectedElement);
        }
    }
}
