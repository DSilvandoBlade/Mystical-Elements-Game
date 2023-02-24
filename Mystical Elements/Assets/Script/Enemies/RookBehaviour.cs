using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class RookBehaviour : MonoBehaviour
{
    #region Private Variables
    private Player m_player;
    private Animator m_animator;
    private float m_timer;
    private bool m_mobile = true;
    #endregion

    #region Serialized Variables
    [Header("Tracking Values")]
    [SerializeField] private float m_alertDistance;
    [Space(10)]

    [Header("Projectile References")]
    [SerializeField] private Transform m_projectilePoint;
    [SerializeField] private GameObject m_projectileAsset;
    [Space(10)]

    [Header("Projectile Settings")]
    [SerializeField] private float m_projectileSpeed;
    [SerializeField] private float m_projectileMinCooldown;
    [SerializeField] private float m_projectileMaxCooldown;
    [Space(10)]

    [Header("Rig References")]
    [SerializeField] private Rig m_iKRig;
    #endregion

    private void Start()
    {
        m_player = FindObjectOfType<Player>();
        m_animator = GetComponentInParent<Animator>();

        m_timer = Random.Range(m_projectileMinCooldown, m_projectileMaxCooldown);
    }

    private void Update()
    {
        if (m_mobile)
        {
            if (Alerted())
            {
                m_iKRig.weight = Mathf.SmoothStep(m_iKRig.weight, 1f, 10f * Time.deltaTime);
                Shoot();
            }

            else
            {
                m_iKRig.weight = Mathf.SmoothStep(m_iKRig.weight, 0.15f, 10f * Time.deltaTime);
                m_timer = m_projectileMinCooldown;
            }
        }
    }

    private void Shoot()
    {
        if (m_timer <= 0)
        {
            m_animator.SetTrigger("Shoot");
            GameObject projectile = GameObject.Instantiate(m_projectileAsset, m_projectilePoint.position, m_projectilePoint.rotation);
            projectile.GetComponent<Rigidbody>().velocity = projectile.transform.up * m_projectileSpeed;
            Destroy(projectile, 4f);
            m_timer = Random.Range(m_projectileMinCooldown, m_projectileMaxCooldown);
        }

        else
        {
            m_timer -= Time.deltaTime;
        }
    }

    private bool Alerted()
    {
        float distance = Vector3.Distance(m_player.transform.position, m_projectilePoint.position);

        if (distance < m_alertDistance)
        {
            return true;
        }

        return false;
    }

    public void Freeze(bool isFrozen)
    {
        m_mobile = !isFrozen;

        if (isFrozen)
        {
            m_animator.speed = 0f;
        }

        else
        {
            m_animator.speed = 1f;
        }
    }
}
