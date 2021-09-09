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

    //Returns null if occupied
    public RoomModule AddNewRoomModule(ModuleType type, Vector3Int location)
    {
        RefreshRoomList();
        if (m_rooms.Count == 0)
        {
            GameObject roomGO = Instantiate(Resources.Load<GameObject>("House Prefabs/Room"), transform);
            RoomModule room = roomGO.GetComponent<RoomModule>();
            room.NewRoom(type, new Vector3Int(0, 0, 0));
            m_rooms.Add(roomGO.transform);
            return room;
        }

        else 
        {
            if (!GetOccupant(location))
            {
                GameObject roomGO = Instantiate(Resources.Load<GameObject>("House Prefabs/Room"), transform);
                roomGO.transform.localPosition = HelperFunctions.Mul(roomGO.GetComponent<MeshFilter>().sharedMesh.bounds.size, location);
                RoomModule room = roomGO.GetComponent<RoomModule>();
                room.NewRoom(type, location);
                m_rooms.Add(roomGO.transform);
                return room;
            }
        }

        return null;
    }

    public RoomModule GetOccupant(Vector3Int location)
    {
        RefreshRoomList();

        foreach (Transform room in m_rooms)
        {
            RoomModule module = room.GetComponent<RoomModule>();
            if (module.m_location == location)
                return module;
        }

        return null;
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
