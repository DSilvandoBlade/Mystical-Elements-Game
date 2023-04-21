using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class OutOfBoundsBox : MonoBehaviour
{
    [SerializeField] private Transform m_spawnTransform;

    private GradeSystem m_gradeSystem;
    private Player m_player;

    // Start is called before the first frame update
    private void Start()
    {
        m_gradeSystem = FindObjectOfType<GradeSystem>();
        m_player = FindObjectOfType<Player>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            m_gradeSystem.TimesFellOff++;
            m_player.OutOfBound(m_spawnTransform);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0.75f, 0, 0, 0.1f);
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawCube(Vector3.zero, Vector3.one);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0, 0, 0.2f);
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawCube(Vector3.zero, Vector3.one);
    }
#endif
}
