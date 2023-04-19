using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class CamBoundingBox : MonoBehaviour
{
    [HideInInspector] public CinemachineVirtualCamera VirtualCamera
    {
        get { return m_virtualCamera; }
        set { m_virtualCamera = value; }
    }

    [HideInInspector] public bool CameraSwitchOn
    {
        get { return m_cameraSwitchOn; }
        set { m_cameraSwitchOn = value; }
    }

    [HideInInspector] public int Priority
    {
        get { return m_priority; }
        set { m_priority = value; }
    }

    [SerializeField] private bool m_cameraSwitchOn;
    private CinemachineVirtualCamera m_virtualCamera;
    private CamController m_camController;

    [SerializeField] private int m_priority;

    private void Start()
    {
        m_camController = FindObjectOfType<CamController>();
        m_virtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0.75f, 0, 0, 0.1f);
        Gizmos.DrawCube(transform.position, transform.localScale);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0, 0, 0.2f);
        Gizmos.DrawCube(transform.position, transform.localScale);
    }
#endif

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            m_cameraSwitchOn = true;
            m_camController.CheckPriorities();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            m_cameraSwitchOn = false;
            m_camController.CheckPriorities();
        }
    }
}
