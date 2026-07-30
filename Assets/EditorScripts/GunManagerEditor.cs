
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(GunManager))]
public class GunManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        EditorGUILayout.Space(10);
        GunManager gunManager = (GunManager)target;
        if (GUILayout.Button("Fire Gun"))
        {
            gunManager.Fire();
        }
        if (GUILayout.Button("Rack"))
        {
            gunManager.Rack();
        }
        if(GUILayout.Button("Slide Release"))
        {
            gunManager.SlideRelease();
        }
        if(GUILayout.Button("Manual Slide Release"))
        {
            gunManager.ManualSlideRelease();
        }
    }

}

#endif
