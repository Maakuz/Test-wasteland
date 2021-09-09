using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class HouseEditor : EditorWindow
{
    HouseModule m_house;
    RoomModule m_room;
    int m_level = 0;
    int m_offsetX;
    int m_offsetY;

    [MenuItem("Window/House Editor")]
    public static void ShowWindow()
    {
        GetWindow<HouseEditor>("House Editor");
    }

    void OnGUI()
    {
        if (m_house)
        {
            GUILayout.Label("House module selected");


            selectButtons();
            navigation();

            GUILayout.BeginArea(new Rect(200, 2, 200, 200));
            if (m_room)
            {
                List<RoomModule.Side> sides = m_room.GetAllSides();
                foreach (RoomModule.Side side in sides)
                {
                    GUILayout.Label(side.name);
                    int selection = EditorGUILayout.Popup((int)side.Walltype, RoomModule.wallTypes);
                    if (selection != (int)side.Walltype)
                    {
                        DestroyImmediate(side.Go);
                        side.SetSide((RoomModule.WallType)selection, m_room.transform);
                    }
                }


            }
            
            GUILayout.EndArea();
        }
        else
            GUILayout.Label("No house module selected");



        if (GUILayout.Button("Load selected obj"))
        {
            HouseModule mod;
            if (!Selection.activeGameObject.TryGetComponent(out mod))
                Debug.LogWarning("No HouseModule on selected item.");

            else
            {
                m_house = mod;
                m_level = 0;
                m_offsetX = 0;
                m_offsetY = 0;
            }
        }
        
        
    }

    private void selectButtons()
    {
        GUILayout.BeginHorizontal(GUILayout.Width(200));
        GUILayout.Space(50);
        GUILayout.Label((m_offsetX - 1).ToString(), GUILayout.Width(50));
        GUILayout.Label((m_offsetX).ToString(), GUILayout.Width(50));
        GUILayout.Label((m_offsetX + 1).ToString(), GUILayout.Width(50));
        GUILayout.EndHorizontal();

        for (int i = 1; i >= -1; i--)
        {
            GUILayout.BeginHorizontal(GUILayout.Width(200));

            string label = (m_offsetY + i).ToString();
            GUILayout.Label(label, GUILayout.Width(25));

            for (int j = -1; j < 2; j++)
            {
                //Logic to determinate if space is occupied or adjacent
                string buttonName = "Empty";
                if (m_house.GetOccupant(new Vector3Int(j + m_offsetX, m_level, i + m_offsetY)))
                    buttonName = "Taken";



                if (GUILayout.Button(buttonName))
                {
                    Vector3Int vec = new Vector3Int(j + m_offsetX, m_level, i + m_offsetY);
                    RoomModule occuPied = m_house.GetOccupant(vec);
                    if (occuPied) 
                    {
                        m_room = occuPied;
                    }

                    else
                    {
                        m_room = m_house.AddNewRoomModule(HouseModule.ModuleType.small, vec);
                    } 
                }

            }

            GUILayout.EndHorizontal();
        }
    }

    private void navigation()
    {
        GUILayout.Label("Level: " + m_level.ToString());

        GUILayout.BeginHorizontal(GUILayout.Width(150));
        if (GUILayout.Button("UP"))
        {
            m_level++;
        }
        if (GUILayout.Button("ZERO"))
        {
            m_level = 0;
        }
        if (GUILayout.Button("DOWN"))
        {
            m_level--;
            m_level = m_level > 0 ? m_level : 0;
        }


        GUILayout.EndHorizontal();

        if (GUILayout.Button("forward", GUILayout.Width(150)))
        {
            m_offsetY++;
        }

        GUILayout.BeginHorizontal(GUILayout.Width(150));
        if (GUILayout.Button("LEFT"))
        {
            m_offsetX--;
        }
        if (GUILayout.Button("ZERO"))
        {
            m_offsetX = 0;
            m_offsetY = 0;
        }
        if (GUILayout.Button("RIGHT"))
        {
            m_offsetX++;
        }
        GUILayout.EndHorizontal();

        if (GUILayout.Button("BACKWARD", GUILayout.Width(150)))
        {
            m_offsetY--;
        }
    }
}
