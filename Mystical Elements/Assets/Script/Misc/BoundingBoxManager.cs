using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundingBoxManager : MonoBehaviour
{
    public delegate void BoundingCheck();
    public static event BoundingCheck OnBoundingCheck;

    public void TriggerBoxOut()
    {
        if (OnBoundingCheck != null)
        {
            OnBoundingCheck();
        }
    }
}
