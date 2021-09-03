using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour 
{
	private CharacterController m_controller;
	private Gravity m_gravity;

	private Vector2 m_dir;

	[SerializeField]
	private float m_walkSpeed;
	[SerializeField]
	private float m_sprintSpeed;
	private Vector3 m_xVelocity;


	//TODO: air resistance?
	private float m_resistance;



	private float m_jumpHeight;

    [SerializeField]
	private float m_xVel;
	public float XVel { get => m_xVel; }


	void Start() 
	{
		m_walkSpeed = 1;
		m_sprintSpeed = 2;
		m_resistance = 0.95f;
		m_jumpHeight = 8;
		m_controller = GetComponent<CharacterController>();
		m_gravity = GetComponent<Gravity>();
	}
	

	void Update() 
	{


		m_dir.x = Input.GetAxisRaw("Horizontal");
		m_dir.y = Input.GetAxisRaw("Vertical");

		m_dir.Normalize();

		if (m_gravity.IsGrounded && Input.GetButtonDown("Jump"))
		{
			//TODO: Pre-calculate when final
			m_gravity.m_yVelocity.y = Mathf.Sqrt(m_jumpHeight * -2f * Constants.gravity);
		}

		float speed = Input.GetKey(KeyCode.LeftShift) ? m_sprintSpeed : m_walkSpeed;
			

		m_xVelocity += new Vector3(m_dir.x * speed, 0, m_dir.y * speed);
		m_xVel = m_xVelocity.magnitude;
		Vector3 moveDistance = m_xVelocity.x * transform.right + m_xVelocity.z * transform.forward;
		m_controller.Move(moveDistance * Time.deltaTime);
		m_xVelocity *= m_resistance;
	}
}
