#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(Magazine))]
public class MagazineEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        EditorGUILayout.Space(10);

        Magazine mag = (Magazine)target;

        if (GUILayout.Button("Add Test Bullet"))
        {
            mag.AddTestBullet();
        }

        if (GUILayout.Button("Remove Bullet"))
        {
            mag.RemoveBullet();
        }
    }
}
#endif