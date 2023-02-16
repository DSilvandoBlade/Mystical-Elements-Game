using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyAttack : MonoBehaviour
{
    [Header("Attack Attributes")]
    [SerializeField] private float m_attack;
    [SerializeField] private bool m_doesStun;
    [Space(10)]

    [Header("VFX")]
    [SerializeField] private GameObject m_hitVFX;
    [SerializeField] private GameObject m_floatingText;
    [SerializeField] private float m_floatingTextOffset;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameObject vfx = GameObject.Instantiate(m_hitVFX, other.transform.position, transform.rotation);
            Destroy(vfx, 1f);

            GameObject text = GameObject.Instantiate(m_floatingText, new Vector3(other.transform.position.x, other.transform.position.y + m_floatingTextOffset, other.transform.position.z), transform.rotation);
            text.GetComponentInChildren<TextMeshProUGUI>().text = ((int)m_attack).ToString();
            text.GetComponentInChildren<TextMeshProUGUI>().color = Color.white; //Will be changed depending on element
            Destroy(text, 1f);

            other.GetComponent<Player>().DamagePlayer(m_attack, m_doesStun);
        }
    }
}
