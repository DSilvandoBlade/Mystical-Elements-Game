using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ElementTree;
using TMPro;

public class EnemyAttack : MonoBehaviour
{
    #region Public Variables
    [HideInInspector] public Element ElementType
    {
        get {return m_elementType;}
        set {m_elementType = value;}
    }
    #endregion

    #region Serialized Variables
    [Header("Attack Attributes")]
    [SerializeField] private float m_attack;
    [SerializeField] private bool m_doesStun;
    [SerializeField] private Element m_elementType;
    #endregion

    #region Collision Function
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<Player>().DamagePlayer(m_attack, m_doesStun, m_elementType);
        }
    }
    #endregion
}
