using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour 
{
	private CharacterController m_controller;

	private Vector2 m_dir;

	[SerializeField] private float m_walkSpeed;
	[SerializeField] private float m_sprintSpeed;
	[SerializeField] private float m_zeroToMaxWalk;
	[SerializeField] private float m_zeroToMaxSprint;
	[SerializeField] private float m_maxToZero;
	[SerializeField] private float m_accelRateSprint;
	[SerializeField] private float m_accelRateWalk;
	[SerializeField] private float m_stepOffset;
	private float m_decelRate;

	[SerializeField] bool m_recalculateRates;

	public float MaxSpeed { get => m_sprintSpeed; }
	public float MaxStrafeSpeed { get => m_walkSpeed; }

	private Vector3 m_xVelocity;



	private float m_jumpHeight;

    [SerializeField]
	private float m_forwardVel;
	public float ForwardVel { get => m_forwardVel; }

	[SerializeField]
	private float m_strafeVel;
	public float StrafeVel { get => m_strafeVel; }

	private bool m_inVehicle;
	public bool InVehicle { get => m_inVehicle; set => m_inVehicle = value; }

    #region Gravity
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

	#endregion

	void Start() 
	{
		m_walkSpeed = 25;
		m_sprintSpeed = 50;
		m_zeroToMaxSprint = 1f;
		m_zeroToMaxWalk = 0.3f;
		m_maxToZero = 0.5f;
		m_stepOffset = 1f;

		recalculateRates();

		m_jumpHeight = 8;
		m_controller = GetComponent<CharacterController>();
		m_inVehicle = false;

		m_groundRadius = 0.5f;

		m_recalculateRates = false;
	}
	

	void Update() 
	{
		if (m_recalculateRates)
			recalculateRates();

		m_dir.x = Input.GetAxisRaw("Horizontal");
		m_dir.y = Input.GetAxisRaw("Vertical");

		m_dir.Normalize();

		if (IsGrounded && Input.GetButtonDown("Jump"))
		{
			//TODO: Pre-calculate when final
			m_yVelocity.y = Mathf.Sqrt(m_jumpHeight * -2f * Constants.gravity);
		}

		if (m_yVelocity.y > 0)
			m_controller.stepOffset = 0;

		else
			m_controller.stepOffset = m_stepOffset;

		bool sprinting = Input.GetKey(KeyCode.LeftShift);
		float rate = sprinting ? m_accelRateSprint : m_accelRateWalk;
		float max = sprinting ? m_sprintSpeed : m_walkSpeed;
		
		m_xVelocity += new Vector3(m_dir.x * m_accelRateWalk * Time.deltaTime, 0, m_dir.y * rate * Time.deltaTime);
		m_xVelocity.z = Mathf.Min(m_xVelocity.z, max);
		m_xVelocity.x = Mathf.Min(m_xVelocity.x, m_walkSpeed);
		m_xVelocity.z = Mathf.Max(m_xVelocity.z, -max);
		m_xVelocity.x = Mathf.Max(m_xVelocity.x, -m_walkSpeed);


		if (Mathf.Abs(m_dir.y) < 0.001)
		{
			m_xVelocity.z = Mathf.MoveTowards(m_xVelocity.z, 0, m_decelRate * Time.deltaTime);
		}

		if (Mathf.Abs(m_dir.x) < 0.001)
		{
			m_xVelocity.x = Mathf.MoveTowards(m_xVelocity.x, 0, m_decelRate * Time.deltaTime);
		}

		m_forwardVel = m_xVelocity.z;
		m_strafeVel = m_xVelocity.x;

		Vector3 moveDistance = m_xVelocity.x * transform.right + m_xVelocity.z * transform.forward;
		m_controller.Move(moveDistance * Time.deltaTime);

		updateGravity();
	}

	void updateGravity()
	{
		if (m_yVelocity.y < 0f)
		{
			m_isGrounded = Physics.CheckSphere(m_groundCheck.position, m_groundRadius, m_groundMask);

			//Not 0 since the detection happens preemptively
			if (m_isGrounded)
				m_yVelocity.y = -25;
		}

		m_yVelocity.y += Constants.gravity * Time.deltaTime;
		if (m_controller)
			m_controller.Move(m_yVelocity * Time.deltaTime);
		else
			transform.Translate(m_yVelocity * Time.deltaTime);
	}

	public void SetCollission(bool value)
	{
		m_controller.detectCollisions = value;
	}

	public void ClearMovement()
	{
		m_forwardVel = 0;
		m_strafeVel = 0;
	}

	public void enterDriverSeat(Transform seat)
	{
		SetCollission(false);
		ClearMovement();
		transform.position = seat.position;
		transform.rotation = seat.rotation;
		transform.parent = seat;
		m_inVehicle = true;
	}

	public void exitDriverSeat(Vector3 exitLocation, Vector3 rotation)
	{
		SetCollission(true);
		transform.position = exitLocation;
		transform.rotation = Quaternion.Euler(rotation);
		transform.parent = null;
		m_inVehicle = false;
	}

	private void recalculateRates()
	{
		m_recalculateRates = false;

		m_accelRateSprint = m_sprintSpeed / m_zeroToMaxSprint;
		m_accelRateWalk = m_walkSpeed / m_zeroToMaxWalk;
		m_decelRate = m_sprintSpeed / m_maxToZero;

	}
}
