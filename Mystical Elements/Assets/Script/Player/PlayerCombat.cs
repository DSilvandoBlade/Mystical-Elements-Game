using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Attack Attributes")]
    [SerializeField] private float m_attack;
    [SerializeField] private bool m_doesStun;
    [Space(10)]

    [Header("VFX")]
    [SerializeField] private GameObject m_hitVFX;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            //GameObject vfx = GameObject.Instantiate(m_hitVFX, other.transform.position, transform.rotation);
            //Destroy(vfx, 2f);

            other.GetComponent<EnemyBase>().TakeDamage(m_attack, m_doesStun);
        }
    }
}
