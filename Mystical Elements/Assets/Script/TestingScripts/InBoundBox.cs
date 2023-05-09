using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InBoundBox : MonoBehaviour
{
    [HideInInspector] public bool PlayerInBox
    {
        get { return m_playerInBox; }
        set { m_playerInBox = value; }
    }

    [SerializeField] private Transform m_spawnTransform;

    private GradeSystem m_gradeSystem;
    private BoundingBoxManager m_boundBoxManager;
    private Player m_player;
    private bool m_playerInBox;

    // Start is called before the first frame update
    private void OnEnable()
    {

    }

    private void Start()
    {
        m_gradeSystem = FindObjectOfType<GradeSystem>();
        m_player = FindObjectOfType<Player>();
        m_boundBoxManager = FindObjectOfType<BoundingBoxManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {

        }
    }
}
