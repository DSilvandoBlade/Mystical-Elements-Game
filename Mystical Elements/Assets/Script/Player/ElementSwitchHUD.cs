using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ElementTree;

public class ElementSwitchHUD : MonoBehaviour
{
    private Player m_player;

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

    public void SwitchToPlasma()
    {
        m_player.SwitchElement(Element.PLASMA);
    }

    public void SwitchToFrost()
    {
        m_player.SwitchElement(Element.FROST);
    }

    public void SwitchToCharge()
    {
        m_player.SwitchElement(Element.CHARGE);
    }

    public void SwitchToFlora()
    {
        m_player.SwitchElement(Element.FLORA);
    }
}
