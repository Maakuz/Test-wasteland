using System.Collections;
using System;
using UnityEngine;
using System.Collections.Generic;

public class RoomModule : MonoBehaviour
{
    [Serializable]
    public class Side
    {
        private GameObject go;
        private Vector3 rotation;
        [SerializeField] private WallType walltype;
        public string name;

        public WallType Walltype { get => walltype;}
        public GameObject Go { get => go; }

        public Side(string name = "DefaultSideName", Vector3 rotation = default(Vector3))
        {
            this.name = name;
            this.go = null;
            this.walltype = WallType.wall;
            this.rotation = rotation;
        }

        public void SetSide(WallType walltype, Transform parent)
        {
            this.walltype = walltype;
            if (go)
                Destroy(go);

            switch (walltype)
            {
                case WallType.wall:
                    go = Instantiate(Resources.Load<GameObject>("House Prefabs/Wall"), parent);
                    break;
                case WallType.window:
                    go = Instantiate(Resources.Load<GameObject>("House Prefabs/Window"), parent);
                    break;
            }
            go.name = name;
            go.transform.Rotate(rotation);
        }
    }
    public static readonly string[] wallTypes = { "wall", "window" };
    public enum WallType { wall = 0, window = 1 };
    public enum Sides {left, right, front, back, top, bottom };

    public Vector3Int m_location;
    [SerializeField] Side m_right;
    [SerializeField] Side m_left;

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

        m_right = new Side("Right", new Vector3(0, 180, 0));
        m_right.SetSide(WallType.wall, transform);

        m_left = new Side("Left");
        m_left.SetSide(WallType.wall, transform);
    }

    public Side GetSide(Sides whatSide)
    {
        switch (whatSide)
        {
            case Sides.left:
                return m_left;
            case Sides.right:
                return m_right;
            case Sides.front:
                break;
            case Sides.back:
                break;
            case Sides.top:
                break;
            case Sides.bottom:
                break;
            default:
                break;
        }

        return new Side();
    }

    public List<Side> GetAllSides()
    {
        List<Side> sides = new List<Side>();

        sides.Add(m_left);
        sides.Add(m_right);

        return sides;
    }
}
