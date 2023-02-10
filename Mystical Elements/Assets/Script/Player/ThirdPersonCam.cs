using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonCam : MonoBehaviour
{
    [Header("OtherRef")]
    [SerializeField] private PlayerInput m_playerInput;
    [SerializeField] private Transform m_orientation;
    [SerializeField] private Transform m_player;
    [SerializeField] private Transform m_playerGraphics;
    [SerializeField] private Rigidbody m_rigidbody;
    [SerializeField] private float m_turnSpeed;

    private void Start()
    {
        Application.targetFrameRate = 30;
    }

    private void Update()
    {
        //For rotating the m_orientation object
        Vector3 viewDir = m_player.position - new Vector3(transform.localPosition.x, m_player.position.y, transform.localPosition.z);
        m_orientation.forward = viewDir.normalized;

        //For rotating the player
        float m_horizontalInput = m_playerInput.actions["Move"].ReadValue<Vector2>().x;
        float m_verticalInput = m_playerInput.actions["Move"].ReadValue<Vector2>().y;
        Vector3 inputDir = (m_orientation.forward * m_verticalInput + m_orientation.right * m_horizontalInput);
        //Vector3 inputDir = m_orientation.TransformDirection(Vector3.forward) * m_verticalInput + m_orientation.TransformDirection(Vector3.forward) * m_horizontalInput;

        if (inputDir != Vector3.zero)
        {
            m_playerGraphics.forward = Vector3.Slerp(m_playerGraphics.forward, inputDir.normalized, Time.deltaTime * m_turnSpeed);
            //m_playerGraphics.up = m_player.TransformDirection(Vector3.up);
        }
    }
}
