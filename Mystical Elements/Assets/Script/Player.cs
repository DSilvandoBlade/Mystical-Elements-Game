using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private PlayerInput m_playerInput;

    public GameObject Planet;
    public GameObject PlayerPlaceholder;

    public float speed = 4;
    public float JumpHeight = 1.2f;

    float gravity = 100;
    bool OnGround = false;

    float distanceToGround;
    Vector3 Groundnormal;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        m_playerInput = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {

        //MOVEMENT

        float x = m_playerInput.actions["Move"].ReadValue<Vector2>().x * Time.deltaTime * speed;
        float y = m_playerInput.actions["Rot"].ReadValue<float>();
        float z = m_playerInput.actions["Move"].ReadValue<Vector2>().y * Time.deltaTime * speed;

        transform.Translate(x, 0, z);

        //Local Rotation
        transform.Rotate(0, y * (150 * Time.deltaTime), 0);

        //Jump

        if (m_playerInput.actions["Jump"].triggered)
        {
            rb.AddForce(transform.up * 40000 * JumpHeight * Time.deltaTime);
        }

        /*
        //GroundControl

        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(transform.position, -transform.up, out hit, 10))
        {

            distanceToGround = hit.distance;
            Groundnormal = hit.normal;

            if (distanceToGround <= 0.2f)
            {
                OnGround = true;
            }
            else
            {
                OnGround = false;
            }


        }


        //GRAVITY and ROTATION

        Vector3 gravDirection = (transform.position - Planet.transform.position).normalized;

        if (OnGround == false)
        {
            rb.AddForce(gravDirection * -gravity);

        }

        //

        Quaternion toRotation = Quaternion.FromToRotation(transform.up, Groundnormal) * transform.rotation;
        transform.rotation = toRotation;*/
    }


    //CHANGE PLANET

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.transform != Planet.transform)
        {

            Planet = collision.transform.gameObject;

            Vector3 gravDirection = (transform.position - Planet.transform.position).normalized;

            Quaternion toRotation = Quaternion.FromToRotation(transform.up, gravDirection) * transform.rotation;
            transform.rotation = toRotation;

            rb.velocity = Vector3.zero;
            rb.AddForce(gravDirection * gravity);


            PlayerPlaceholder.GetComponent<PlayerPlaceholder>().NewPlanet(Planet);

        }
    }


}