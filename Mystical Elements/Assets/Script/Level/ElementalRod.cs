using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ElementTree;

public class ElementalRod : MonoBehaviour
{
    [HideInInspector] public bool Activated
    {
        get { return m_activated; }
    }

    private bool m_activated = false;
    private FinishPlatform m_finishPlatform;
    private Animator m_animator;

    [SerializeField] private Element m_activationElement;

    private void Start()
    {
        m_animator = GetComponentInChildren<Animator>();
        m_finishPlatform = FindObjectOfType<FinishPlatform>();
    }

    public void Activate(Element element)
    {
        if (element == m_activationElement && !m_activated)
        {
            Debug.Log("Activated Rod!");
            m_activated = true;
            m_finishPlatform.CheckRodActives();
            m_animator.SetTrigger("Activate");
        }
    }
}
