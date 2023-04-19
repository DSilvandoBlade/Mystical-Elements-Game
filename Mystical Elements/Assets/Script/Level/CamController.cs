using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamController : MonoBehaviour
{
    private CamBoundingBox[] m_cinemachineCams;
    private void Start()
    {
        m_cinemachineCams = FindObjectsOfType<CamBoundingBox>();
    }

    public void CheckPriorities()
    {
        List<CamBoundingBox> enabledCams = new List<CamBoundingBox>();

        foreach (CamBoundingBox c in m_cinemachineCams)
        {
            if (c.CameraSwitchOn)
            {
                enabledCams.Add(c);
            }
        }

        if (enabledCams.Count == 0)
        {
            Debug.LogWarning("No Cameras have been selected, Character must have not been in a bounding box.");
            return;
        }

        int maxInt = 0;
        CamBoundingBox camBoundingBox = new CamBoundingBox();

        foreach (CamBoundingBox b in enabledCams)
        {
            b.VirtualCamera.Priority = 0;

            if (b.Priority > maxInt)
            {
                maxInt = b.Priority;
                camBoundingBox = b;
            }
        }

        camBoundingBox.VirtualCamera.Priority = 10;
    }
}
