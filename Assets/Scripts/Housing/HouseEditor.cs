using UnityEditor;
using UnityEngine;

public class HouseEditor : EditorWindow
{
    HouseModule m_module;

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

            GUILayout.BeginHorizontal(GUILayout.Width(150));
            GUILayout.Button("1");
            if(GUILayout.Button("2"))
            {
                m_module.AddNewRoomModule(HouseModule.ModuleType.small, new Vector3Int(0, 0, 0));
            }
            GUILayout.Button("3");
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(GUILayout.Width(150));
            GUILayout.Button("1");
            GUILayout.Button("2");
            GUILayout.Button("3");
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(GUILayout.Width(150));
            GUILayout.Button("1");
            GUILayout.Button("2");
            GUILayout.Button("3");
            GUILayout.EndHorizontal();
        }
        else
            GUILayout.Label("No house module selected");



        if (GUILayout.Button("Load selected obj"))
        {
            HouseModule mod;
            if (!Selection.activeGameObject.TryGetComponent(out mod))
                Debug.LogWarning("No HouseModule on selected item.");

            else
                m_module = mod;
        }
        
        
    }
}
