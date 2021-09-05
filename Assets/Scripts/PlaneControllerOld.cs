using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneControllerOld : MonoBehaviour
{
    public float m_throttle;
    public float m_rollSpeed;
    public float m_pitchSpeed;

    [SerializeField] private float m_currentThrottle;

    //[SerializeField] private float m_airResistance;

    [SerializeField] private Vector3 m_pitchYawRoll;

    [SerializeField] private Vector3 m_accelleration;

    private PlayerScript m_playerScript;
    private Rigidbody m_rb;

    // Start is called before the first frame update
    void Start()
    {
        m_throttle = 150;
        m_rollSpeed = 100;
        m_pitchSpeed = 100;
        m_currentThrottle = 0;
        //m_airResistance = 0.005f;
        m_playerScript = GameObject.FindWithTag("Player").GetComponent<PlayerScript>();
        m_rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            m_playerScript.exitDriverSeat(new Vector3(0, 150, 0));
            enabled = false;
            m_playerScript.enabled = true;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            m_currentThrottle += m_throttle * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.LeftControl))
        {
            m_currentThrottle -= m_throttle * Time.deltaTime;
        }

        m_currentThrottle = Mathf.Max(m_currentThrottle, 0);

        //m_accelleration += 
        //m_accelleration *= Mathf.Pow(m_airResistance, Time.deltaTime);

        //TODO:Space to look around
        m_pitchYawRoll.x = Input.GetAxisRaw("Vertical") * m_pitchSpeed;
        m_pitchYawRoll.z = Input.GetAxisRaw("Horizontal") * m_rollSpeed * -1;

        m_rb.AddForce(m_currentThrottle * transform.forward * Time.deltaTime);
        m_rb.AddTorque(m_pitchYawRoll.z * transform.forward * Time.deltaTime);
        m_rb.AddTorque(m_pitchYawRoll.x * transform.right * Time.deltaTime);
        
        //transform.position += m_accelleration * Time.deltaTime;
        //transform.Rotate(m_pitchYawRoll * Time.deltaTime);

    }
}
