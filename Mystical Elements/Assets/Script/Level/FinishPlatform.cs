using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishPlatform : MonoBehaviour
{
    [SerializeField] private ElementalRod[] m_rods;

    private bool m_levelFinishActivate = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && m_levelFinishActivate)
        {
            Debug.Log("Game Finished");
            Debug.Break();
        }
    }

    public void CheckRodActives()
    {
        if (AllRodsActive())
        {
            m_levelFinishActivate = true;
        }
    }

    private bool AllRodsActive()
    {
        foreach (ElementalRod rod in m_rods)
        {
            if (!rod.Activated)
            {
                return false;
            }
        }

        return true;
    }
}
