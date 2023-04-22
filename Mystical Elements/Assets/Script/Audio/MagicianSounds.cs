using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicianSounds : MonoBehaviour
{
    [SerializeField] private SoundClips[] m_soundClips;
    
    private AudioSource m_audioSource;

    private void Start()
    {
        m_audioSource = GetComponent<AudioSource>();
    }

    public void PlaySoundClip(string soundName)
    {
        foreach (SoundClips clips in m_soundClips)
        {
            if (clips.AudioName == soundName)
            {
                m_audioSource.PlayOneShot(clips.PlayRandomClip());
                break;
            }
        }
    }
}
