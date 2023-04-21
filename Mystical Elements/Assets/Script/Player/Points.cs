using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Points : MonoBehaviour
{
    [SerializeField] private int m_points;
    [SerializeField] private float m_healAmount;
    [SerializeField] private GameObject m_vfx;

    private LevelManager m_manager;
    private Player m_player;

    private void Start()
    {
        m_manager = FindObjectOfType<LevelManager>();
        m_player = FindObjectOfType<Player>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            m_manager.AddPoints(m_points);
            m_player.HealPlayer(m_healAmount);
            GameObject vfx = GameObject.Instantiate(m_vfx, transform.position, transform.rotation);
            Destroy(vfx, 0.6f);
            Destroy(gameObject);
        }
    }
}
