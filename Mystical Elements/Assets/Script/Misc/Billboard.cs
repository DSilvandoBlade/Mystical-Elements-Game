using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    void Update()
    {
        transform.localRotation = Quaternion.LookRotation(transform.localPosition - Camera.main.transform.position);
    }
}
