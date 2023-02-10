using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlaceholder : MonoBehaviour
{
    [SerializeField] private GameObject m_player;
    [SerializeField] private GameObject m_planet;

    // Update is called once per frame
    void Update()
    {
        //SMOOTH

        //POSITION
        transform.position = Vector3.Lerp(transform.position, m_player.transform.position, 0.1f);

        Vector3 gravDirection = (transform.position - m_planet.transform.position).normalized;

        //ROTATION
        Quaternion toRotation = Quaternion.FromToRotation(transform.up, gravDirection) * transform.rotation;
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, 0.1f);

    }


    public void NewPlanet(GameObject newPlanet)
    {
        m_planet = newPlanet;
    }

}