using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutterTool : MonoBehaviour
{
    [SerializeField]
    private Transform m_camera;

    [SerializeField]
    private LayerMask m_treeMask;

    [SerializeField]
    private float m_range;

    void Start()
    {
        m_range = 15;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            RaycastHit hit;
            Debug.DrawRay(m_camera.position, m_camera.forward * m_range, Color.white, 40, false);
            if (Physics.Raycast(m_camera.position, m_camera.forward, out hit, m_range, m_treeMask))
            {
                Debug.Log("HITTADE");
                hit.transform.gameObject.SetActive(false);
            }
        }
    }
}
