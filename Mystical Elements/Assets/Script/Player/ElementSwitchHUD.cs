using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using ElementTree;

public class ElementSwitchHUD : MonoBehaviour
{
    private Player m_player;
    private PlayerInput m_inputSystem;

    [SerializeField] private Animator m_anim;

    private void Start()
    {
        m_player = FindObjectOfType<Player>();
    }

    public void ShowElementTree(bool tree)
    {
        if (tree)
        {
            m_anim.Play("Open");
        }

        else
        {
            m_anim.Play("Close");
        }
    }
}
