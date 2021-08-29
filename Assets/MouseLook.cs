using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour {

	float m_mouseRotX;
	public float m_mouseSensitivity;
	public float m_maxY;
	public float m_minY;
	public GameObject m_camera;
	public Transform m_playerTransform;

	// Use this for initialization
	void Start () {
		m_mouseSensitivity = 100;
		m_minY = -80;
		m_maxY = 80;
	}

	// Update is called once per frame
	void Update () {
		if (Cursor.lockState == CursorLockMode.Locked)
		{

			m_mouseRotX -= Input.GetAxis("Mouse Y") * m_mouseSensitivity * Time.deltaTime;
			float mouseY = Input.GetAxisRaw("Mouse X") * m_mouseSensitivity * Time.deltaTime;

			m_mouseRotX = Mathf.Clamp(m_mouseRotX, m_minY, m_maxY);

			m_camera.transform.localRotation = Quaternion.Euler(m_mouseRotX, 0, 0);
			m_playerTransform.Rotate(Vector3.up * mouseY);
		}
	}
}
