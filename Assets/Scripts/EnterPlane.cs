using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterPlane : MonoBehaviour
{
    private PlaneController m_planeController;
    [SerializeField] GameObject m_enterPrompt;
    private GameObject m_player;
    private PlayerScript m_playerScript;
    [SerializeField] Transform m_cockpit;

    // Start is called before the first frame update
    void Start()
    {
        m_planeController = GetComponent<PlaneController>();
        m_player = GameObject.FindWithTag("Player");
        m_enterPrompt.SetActive(false);
        m_playerScript = m_player.GetComponent<PlayerScript>();
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            m_enterPrompt.SetActive(true);
            if (Input.GetButton("Confirm"))
            {
                m_planeController.enabled = true;
                m_playerScript.enabled = false;
                m_playerScript.enterDriverSeat(m_cockpit);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
            m_enterPrompt.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
