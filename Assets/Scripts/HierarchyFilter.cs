using UnityEditor;
using UnityEngine;

//[InitializeOnLoad]
public class HierarchyFilter
{
    //[MenuItem("GameObject/MyMenu/Do Something", false, 0)]
    //static void Test()
    //{
    //}
    static Texture2D m_texCollider;

    static HierarchyFilter()
    {
        m_texCollider = AssetDatabase.LoadAssetAtPath("Assets/EditorIcons/colission.png", typeof(Texture2D)) as Texture2D;
        EditorApplication.hierarchyWindowItemOnGUI += HierarchyItemCallback;
    }

    static void HierarchyItemCallback(int instanceID, Rect selectionRect)
    {
        Rect r = new Rect(selectionRect);
        r.x = r.width - 20;
        r.width = 20;

        GameObject go = EditorUtility.InstanceIDToObject(instanceID) as GameObject;

        if (go.GetComponent<Collider>())
            GUI.Label(r, m_texCollider);
    }
}
