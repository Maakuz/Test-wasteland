using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomModule : MonoBehaviour
{
    public Vector3Int m_location;
    GameObject m_right;
    GameObject m_left;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NewRoom(HouseModule.ModuleType type, Vector3Int location)
    {
        m_location = location;

        m_right = Instantiate(Resources.Load<GameObject>("House Prefabs/Wall"), transform);
        m_right.transform.Rotate(new Vector3(0, 180, 0));
        m_right.name = "Right";

        m_left = Instantiate(Resources.Load<GameObject>("House Prefabs/Wall"), transform);
        m_left.name = "Left";
    }
}
