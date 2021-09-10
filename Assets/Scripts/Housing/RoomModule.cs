using System.Collections;
using System;
using UnityEngine;
using System.Collections.Generic;

public class RoomModule : MonoBehaviour
{
    public static readonly string[] wallTypes = { "wall", "window" };
    public enum WallType { wall = 0, window = 1 };
    public enum Sides { left, right, front, back, top, bottom };

    [Serializable]
    public class Side
    {
        private GameObject go;
        private Vector3 rotation;
        [SerializeField] private WallType walltype;
        [SerializeField] private Sides side;
        public string name;

        public WallType Walltype { get => walltype;}
        public GameObject Go { get => go; }

        public Side(Sides whatSide, string name = "DefaultSideName", Vector3 rotation = default(Vector3))
        {
            this.name = name;
            this.go = null;
            this.walltype = WallType.wall;
            this.rotation = rotation;
            this.side = whatSide;
        }

        public void SetSide(WallType walltype, Transform parent)
        {
            this.walltype = walltype;
            if (go)
                Destroy(go);

            switch (walltype)
            {
                case WallType.wall:
                    if (side == Sides.left || side == Sides.right)
                        go = Instantiate(Resources.Load<GameObject>("House Prefabs/Wall"), parent);

                    else if(side == Sides.front || side == Sides.back)
                        go = Instantiate(Resources.Load<GameObject>("House Prefabs/Long Wall"), parent);
                    break;
                case WallType.window:
                    if (side == Sides.left || side == Sides.right)
                        go = Instantiate(Resources.Load<GameObject>("House Prefabs/Window"), parent);

                    else if (side == Sides.front || side == Sides.back)
                        go = Instantiate(Resources.Load<GameObject>("House Prefabs/Long Window"), parent);
                    break;
            }
            go.name = name;
            go.transform.Rotate(rotation);
        }
    }


    public Vector3Int m_location;
    [SerializeField] Side m_right;
    [SerializeField] Side m_left;
    [SerializeField] Side m_front;
    [SerializeField] Side m_back;

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

        m_right = new Side(Sides.right, "Right", new Vector3(0, 180, 0));
        m_right.SetSide(WallType.wall, transform);

        m_left = new Side(Sides.left, "Left");
        m_left.SetSide(WallType.wall, transform);

        m_front = new Side(Sides.front, "Front", new Vector3(0, 180, 0));
        m_front.SetSide(WallType.wall, transform);

        m_back = new Side(Sides.back, "Back");
        m_back.SetSide(WallType.wall, transform);
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
                return m_front;
            case Sides.back:
                return m_back;
            case Sides.top:
                break;
            case Sides.bottom:
                break;
            default:
                break;
        }

        return new Side(Sides.top);
    }

    public List<Side> GetAllSides()
    {
        List<Side> sides = new List<Side>();

        sides.Add(m_left);
        sides.Add(m_right);
        sides.Add(m_front);
        sides.Add(m_back);

        return sides;
    }
}
