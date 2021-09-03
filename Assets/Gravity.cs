using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    public Vector3 m_yVelocity;


    //[SerializeField] //For testing
    private bool m_isGrounded;
    public bool IsGrounded { get => m_isGrounded; }
    

    //Ground detection
    [SerializeField]
    private Transform m_groundCheck;

    private float m_groundRadius;

    [SerializeField]
    private LayerMask m_groundMask;

    private CharacterController m_controller;


    // Start is called before the first frame update
    void Start()
    {
		m_groundRadius = 0.5f;
        m_controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        m_isGrounded = Physics.CheckSphere(m_groundCheck.position, m_groundRadius, m_groundMask);

        //Not 0 since the detection happens preemptively
        if (m_isGrounded && m_yVelocity.y < 0)
            m_yVelocity.y = -25;

        m_yVelocity.y += Constants.gravity * Time.deltaTime;
        if (m_controller)
            m_controller.Move(m_yVelocity * Time.deltaTime);
        else
            transform.Translate(m_yVelocity * Time.deltaTime);
    }
}
