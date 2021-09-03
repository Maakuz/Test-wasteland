using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneController : MonoBehaviour
{

    public float m_forwardSpeed;
    public float m_strafeSpeed;
    public float m_hoverSpeed;

    [SerializeField]
    private Vector3 m_yawPitchRoll;

    [SerializeField]
    private Vector3 m_accelleration;


    // Start is called before the first frame update
    void Start()
    {
        m_forwardSpeed = 10;
        m_strafeSpeed= 5;
        m_hoverSpeed = 3;
    }

    // Update is called once per frame
    void Update()
    {
        //TODO:Space to look around
        m_yawPitchRoll.x = Input.GetAxisRaw("Vertical") * m_forwardSpeed;
        m_yawPitchRoll.y = Input.GetAxis("Mouse Y") * m_hoverSpeed;
        m_yawPitchRoll.z = Input.GetAxisRaw("Horizontal") * m_strafeSpeed;

        transform.position += transform.forward * m_yawPitchRoll.x * Time.deltaTime;
        transform.position += transform.right * m_yawPitchRoll.y * Time.deltaTime;
        transform.position += transform.up * m_yawPitchRoll.z * Time.deltaTime;

    }
}
