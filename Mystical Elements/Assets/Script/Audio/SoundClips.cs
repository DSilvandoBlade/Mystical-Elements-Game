using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SoundClips
{
    [SerializeField] private string m_audioName;

    [SerializeField] public string AudioName
    {
        get { return m_audioName; }
        set { m_audioName = value; }
    }

    [SerializeField] private AudioClip[] m_audioList;

    public AudioClip PlayRandomClip()
    {
        return m_audioList[Random.Range(0, m_audioList.Length)];
    }
}
