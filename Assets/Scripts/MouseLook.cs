using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour 
{

	private float m_mouseRotX;
	public float m_mouseSensitivity;
	private float m_maxY;
	private float m_minY;
	public GameObject m_camera;
	public Transform m_playerTransform;
	private PlayerScript m_playerScript;
	public int m_targetFrameRate;
	private CharacterAnimator m_animator;


	// Use this for initialization
	void Start() 
	{
		m_mouseSensitivity = 1;
		m_minY = -70;
		m_maxY = 70;
		m_targetFrameRate = 60;
		m_playerScript = GetComponent<PlayerScript>();
        m_animator = GetComponent<CharacterAnimator>();
		Application.targetFrameRate = m_targetFrameRate;
	}

	// Update is called once per frame
	void Update() 
	{
		//Lock mouse to screen
		if (Input.GetKeyUp(KeyCode.Tab))
		{
			if (Cursor.lockState == CursorLockMode.Locked)
				Cursor.lockState = CursorLockMode.None;

			else
				Cursor.lockState = CursorLockMode.Locked;
		}

		//refresh
		if (Input.GetKeyUp(KeyCode.F5))
			Application.targetFrameRate = m_targetFrameRate;

		if (Cursor.lockState == CursorLockMode.Locked)
		{

			m_mouseRotX -= Input.GetAxis("Mouse Y") * m_mouseSensitivity;
			float mouseY = Input.GetAxisRaw("Mouse X") * m_mouseSensitivity;

			m_mouseRotX = Mathf.Clamp(m_mouseRotX, m_minY, m_maxY);

			m_camera.transform.localRotation = Quaternion.Euler(m_mouseRotX, 0, 0);

			if (!m_playerScript.InVehicle)
			{
				m_playerTransform.Rotate(Vector3.up * mouseY);
				m_animator.setRotationModifier(mouseY);
			}

			else
				m_animator.setRotationModifier(0);
		}
	}
}
