using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaOffset : MonoBehaviour
{
    [SerializeField] private float m_scrollSpeed = 0.5f;
    private Renderer m_rend;

    void Start()
    {
        m_rend = GetComponent<Renderer>();
    }

    void Update()
    {
        float offset = Time.time * m_scrollSpeed;
        m_rend.material.mainTextureOffset = new Vector2(0, offset);
    }
}
