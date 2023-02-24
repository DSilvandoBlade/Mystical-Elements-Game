using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnButton : MonoBehaviour
{
    [SerializeField] private GameObject[] m_enemy;
    [SerializeField] private Transform m_spawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameObject enemy = GameObject.Instantiate(m_enemy[Random.Range(0, m_enemy.Length)], m_spawnPoint.position, m_spawnPoint.rotation);
        }
    }
}
