using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour 
{
	private CharacterController m_controller;

	private Vector2 m_dir;

	[SerializeField]
	private float m_walkSpeed;
	[SerializeField]
	private float m_sprintSpeed;
	private Vector3 m_xVelocity;
	private Vector3 m_yVelocity;

	//TODO: air resistance?
	private float m_resistance;

	private float m_gravity;

	private float m_jumpHeight;

	//Ground detection
	[SerializeField]
	private Transform m_groundCheck;

	private float m_groundRadius;

	[SerializeField]
	private LayerMask m_groundMask;

	//[SerializeField] //For testing
    private bool m_isGrounded;
    public bool IsGrounded { get => m_isGrounded; }

    [SerializeField]
	private float m_xVel;
	public float XVel { get => m_xVel; }


	void Start() 
	{
		m_walkSpeed = 1;
		m_sprintSpeed = 2;
		m_resistance = 0.95f;
		m_gravity = -100f;
		m_groundRadius = 0.5f;
		m_jumpHeight = 8;
		m_controller = GetComponent<CharacterController>();
	}
	

	void Update() 
	{
		m_isGrounded = Physics.CheckSphere(m_groundCheck.position, m_groundRadius, m_groundMask);

		//Not 0 since the detection happens preemptively
		if (m_isGrounded && m_yVelocity.y < 0)
			m_yVelocity.y = -50;

		m_dir.x = Input.GetAxisRaw("Horizontal");
		m_dir.y = Input.GetAxisRaw("Vertical");

		m_dir.Normalize();

		if (m_isGrounded && Input.GetButtonDown("Jump"))
		{
			//TODO: Pre-calculate when final
			m_yVelocity.y = Mathf.Sqrt(m_jumpHeight * -2f * m_gravity);
		}

		float speed = Input.GetKey(KeyCode.LeftShift) ? m_sprintSpeed : m_walkSpeed;
			

		m_xVelocity += new Vector3(m_dir.x * speed, 0, m_dir.y * speed);
		m_xVel = m_xVelocity.magnitude;
		Vector3 moveDistance = m_xVelocity.x * transform.right + m_xVelocity.z * transform.forward;
		m_controller.Move(moveDistance * Time.deltaTime);
		m_xVelocity *= m_resistance;

		m_yVelocity.y += m_gravity * Time.deltaTime;
		m_controller.Move(m_yVelocity * Time.deltaTime);
	}
}
