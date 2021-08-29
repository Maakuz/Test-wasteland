using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

	private GameObject m_player;
	private Rigidbody m_rb;
	private Vector2 m_dir;
	public Vector2 m_cameraOffset;
	public float m_speed;
	public Vector2 m_acceleration;
	public float m_resistance;

	
	// Use this for initialization
	void Start() 
	{
		m_player = GameObject.Find("PlayerModel");
		m_speed = 0.5f;
		m_resistance = 0.95f;
		m_rb = m_player.GetComponent<Rigidbody>();
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

		moveCharacter();

	}

	private void moveCharacter()
	{
		m_dir.x = Input.GetAxisRaw("Horizontal");


		m_dir.y = Input.GetAxisRaw("Vertical");

		m_acceleration += m_dir * m_speed * Time.deltaTime;
		m_player.transform.Translate(new Vector3(m_acceleration.x, 0, m_acceleration.y) * Time.deltaTime);
		m_acceleration *= m_resistance;
	}

	private void controlCamera()
	{
		

}
}
