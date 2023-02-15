using UnityEngine;

public class RotateOvertime : MonoBehaviour
{
    [SerializeField] private float m_rotationSpeed;

    void Update()
    {
        transform.Rotate(Vector3.up * (m_rotationSpeed * Time.deltaTime));
    }
}
