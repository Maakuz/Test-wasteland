using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaneController : MonoBehaviour
{
    [SerializeField] List<AeroSurface> m_controllableSurfaces;
    [SerializeField] List<WheelCollider> m_wheels;
    [SerializeField] float m_rollSensitivity = 0.2f;
    [SerializeField] float m_pitchSensitivity = 0.2f;
    [SerializeField] float m_yawSensitivity = 0.2f;


    [SerializeField] float m_flap;
    [SerializeField] float m_pitch;
    [SerializeField] float m_roll;
    [SerializeField] float m_yaw;

    [SerializeField] Text displayText = null;

    float m_thrustPercentage;
    float m_brakesTorque;

    AircraftPhysics m_physics;
    private PlayerScript m_playerScript;
    Rigidbody m_rb;

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
            setControlSurfaceAngles();
    }

    // Start is called before the first frame update
    void Start()
    {
        m_physics = GetComponent<AircraftPhysics>();
        m_rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        m_pitch = Input.GetAxis("Vertical");
        m_roll = Input.GetAxis("Horizontal");
        m_yaw = Input.GetAxis("Yaw");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_thrustPercentage = m_thrustPercentage > 0 ? 0 : 1f;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            m_flap = m_flap > 0 ? 0 : 0.3f;
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            m_brakesTorque = m_brakesTorque > 0 ? 0 : 100f;
        }

        if (Input.GetButtonDown("Cancel"))
        {
            m_playerScript.exitDriverSeat(new Vector3(0, 150, 0), Vector3.Cross(transform.right, new Vector3(0, 1, 0)));
            enabled = false;
            m_playerScript.enabled = true;
        }

        displayText.text = "V: " + ((int)m_rb.velocity.magnitude).ToString("D3") + " m/s\n";
        displayText.text += "A: " + ((int)transform.position.y).ToString("D4") + " m\n";
        displayText.text += "T: " + (int)(m_thrustPercentage * 100) + "%\n";
        displayText.text += m_brakesTorque > 0 ? "B: ON" : "B: OFF";
    }

    private void FixedUpdate()
    {
        setControlSurfaceAngles();
        m_physics.ThrustPercent = m_thrustPercentage;

        foreach (WheelCollider wheel in m_wheels)
        {
            wheel.brakeTorque = m_brakesTorque;
            wheel.motorTorque = 0.01f;
        }
    }

    public void setControlSurfaceAngles()
    {
        foreach (AeroSurface surface in m_controllableSurfaces)
        {
            if (surface == null || !surface.m_controlSurface) 
                continue;

            switch (surface.m_inputType)
            {
                case ControlInputType.Pitch:
                    surface.FlapAngle = m_pitch * m_pitchSensitivity;
                    break;
                case ControlInputType.Yaw:
                    surface.FlapAngle = m_yaw * m_yawSensitivity;
                    break;
                case ControlInputType.Roll:
                    surface.FlapAngle = m_roll * m_rollSensitivity;
                    break;
                case ControlInputType.Flap:
                    surface.FlapAngle = m_flap;
                    break;
            } 
        }
    }
}
