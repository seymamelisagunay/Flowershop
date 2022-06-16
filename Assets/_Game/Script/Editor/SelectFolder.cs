using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SelectFolder : Editor
{
    [MenuItem("OB/Select ScriptableObject Folder #g")]
    public static void SelectScriptableObjectFolder()
    {
        UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath("Assets/_Game/ScriptableObject", typeof(UnityEngine.Object));
        Selection.activeObject = obj;
        EditorGUIUtility.PingObject(obj);
    }
    [MenuItem("OB/Select Scene Folder #s")]
    public static void SelectSceneFolder()
    {
        UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath("Assets/_Game/Scenes", typeof(UnityEngine.Object));
        Selection.activeObject = obj;
        EditorGUIUtility.PingObject(obj);
    }
    [MenuItem("OB/Select Script Folder #x")]
    public static void SelectScriptFolder()
    {
        UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath("Assets/_Game/Script", typeof(UnityEngine.Object));
        Selection.activeObject = obj;
        EditorGUIUtility.PingObject(obj);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
