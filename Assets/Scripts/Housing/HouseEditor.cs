using UnityEditor;
using UnityEngine;

public class HouseEditor : EditorWindow
{
    HouseModule m_module;
    int m_level = 0;

    [MenuItem("Window/House Editor")]
    public static void ShowWindow()
    {
        GetWindow<HouseEditor>("House Editor");
    }

    void OnGUI()
    {
        if (m_module)
        {
            GUILayout.Label("House module selected");

            for (int i = -1; i < 2; i++)
            {
                GUILayout.BeginHorizontal(GUILayout.Width(150));

                for (int j = -1; j < 2; j++)
                {
                    //Logic to determinate if space is occupied or adjacent
                    string buttonName = "Empty";
                    if (m_module.IsOccupied(new Vector3Int(j, m_level, i)))
                        buttonName = "Taken";
                        


                    if (GUILayout.Button(buttonName))
                    { 
                        m_module.AddNewRoomModule(HouseModule.ModuleType.small, new Vector3Int(j, m_level, i));
                    }

                }

                GUILayout.EndHorizontal();
            }

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
            GUILayout.Label("Level: " + m_level.ToString());




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
                m_module = mod;
                m_level = 0;
            }
        }
        
        
    }
}
