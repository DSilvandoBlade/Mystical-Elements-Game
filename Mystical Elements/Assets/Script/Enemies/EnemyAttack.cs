using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ElementTree;
using TMPro;

public class EnemyAttack : MonoBehaviour
{
    [Header("Attack Attributes")]
    [SerializeField] private float m_attack;
    [SerializeField] private bool m_doesStun;
    [SerializeField] private Element m_elementType;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<Player>().DamagePlayer(m_attack, m_doesStun, m_elementType);
        }
    }
}
