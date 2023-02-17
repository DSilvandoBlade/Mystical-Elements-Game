using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    private Player m_player;
    private PlayerCombat m_combat;

    private void Start()
    {
        m_player = GetComponentInParent<Player>();
        m_combat = GetComponentInChildren<PlayerCombat>();
    }

    public void ModifyAttack(float attack)
    {
        m_combat.Attack = attack;
    }

    public void ModifyStunSwitch(int stun)
    {
        switch (stun)
        {
            case 0:
                m_combat.DoesStun = false;
                break;
            case 1:
                m_combat.DoesStun = true;
                break;
            default:
                m_combat.DoesStun = false;
                break;
        }
    }
}
