using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ElementTree;

public class PlayerProjectile : MonoBehaviour
{
    #region Public Variables
    [HideInInspector] public float Speed
    {
        get { return m_speed; }
        set { m_speed = value; }
    }
    #endregion

    #region Private Variables
    private Player m_player;
    private ElementManager m_elementManager;
    private VFXManager m_vfxManager;
    private Element m_projectileElement;
    private MeshRenderer m_renderer;
    private Transform m_closestEnemy;
    private Rigidbody m_rigidbody;
    private float m_speed;
    #endregion

    #region Default Functions
    private void Start()
    {
        m_player = FindObjectOfType<Player>();
        m_elementManager = FindObjectOfType<ElementManager>();
        m_renderer = GetComponent<MeshRenderer>();
        m_vfxManager = FindObjectOfType<VFXManager>();
        m_closestEnemy = Direction(FindObjectsOfType<EnemyBase>());
        m_rigidbody = GetComponent<Rigidbody>();

        m_projectileElement = m_player.SelectedElement;
       
        m_vfxManager.SummonHitEffect(transform.position, m_projectileElement);
        m_renderer.material = m_elementManager.GetRespectedElementalMaterial(m_projectileElement);
    }

    private void Update()
    {
        if (m_closestEnemy != null)
        {
            Quaternion toRot = Quaternion.LookRotation(-transform.position - -m_closestEnemy.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRot, 5 * Time.deltaTime);
        }

        m_rigidbody.velocity = transform.forward * m_speed;
    }
    #endregion

    #region Collision Functions
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            m_vfxManager.SummonHitEffect(transform.position, m_projectileElement);

            Destroy(gameObject, 0.01f);
        }
    }
    #endregion

    #region Direction Functions

    private Transform Direction(EnemyBase[] enemies)
    {
        Transform tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (EnemyBase enemy in enemies)
        {
            float dist = Vector3.Distance(enemy.transform.position, currentPos);
            if (dist < minDist)
            {
                tMin = enemy.transform;
                minDist = dist;
            }
        }
        return tMin;
    }

    #endregion
}
