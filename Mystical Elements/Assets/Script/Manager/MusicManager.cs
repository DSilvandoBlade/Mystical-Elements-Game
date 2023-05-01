using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioClip m_musicClip;
    [SerializeField] private AudioClip m_completeClip;

    private AudioSource m_source;

    private void Start()
    {
        m_source = GetComponent<AudioSource>();
        m_source.clip = m_musicClip;
        m_source.Play();
        m_source.loop = true;
    }

    public void Death()
    {
        m_source.Stop();
    }

    public void Complete()
    {
        m_source.Stop();
        m_source.clip = m_completeClip;
        m_source.loop = false;
        m_source.Play();
    }
}
