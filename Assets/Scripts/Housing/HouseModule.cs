using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseModule : MonoBehaviour
{
    public enum ModuleType {small = 0 };

    [SerializeField] List<Transform> m_rooms;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddNewRoomModule(ModuleType type, Vector3Int location)
    {
        RefreshRoomList();
        if (m_rooms.Count == 0)
        {
            GameObject roomGO = new GameObject();
            roomGO.AddComponent<RoomModule>();
            roomGO.transform.parent = transform;
            RoomModule room = roomGO.GetComponent<RoomModule>();
            room.m_location = new Vector3Int(0, 0, 0);
            m_rooms.Add(roomGO.transform);
        }
    }

    public void RefreshRoomList()
    {
        m_rooms.Clear();
        m_rooms = getAllChildren();
    }

    private List<Transform> getAllChildren()
    {
        List<Transform> children = new List<Transform>();

        foreach (Transform child in transform)
            children.Add(child);

        return children;
    }
}
