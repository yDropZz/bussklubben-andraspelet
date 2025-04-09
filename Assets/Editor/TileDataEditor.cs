using UnityEngine;
using UnityEditor;

public class TileDataEditorWindow : EditorWindow
{
    private TileData tileData;

    [MenuItem("Tools/TileData Editor")]
    public static void ShowWindow()
    {
        GetWindow<TileDataEditorWindow>("TileData Editor");
    }

    private Vector2 scroll;

    void OnGUI()
    {
        GUILayout.Label("TileData Editor", EditorStyles.boldLabel);

        tileData = (TileData)EditorGUILayout.ObjectField("Tile Data", tileData, typeof(TileData), false);

        if (tileData == null)
        {
            EditorGUILayout.HelpBox("Please assign a TileData asset.", MessageType.Info);
            return;
        }

        EditorGUILayout.Space();

        scroll = EditorGUILayout.BeginScrollView(scroll);

        int carCount = Mathf.Max(0, EditorGUILayout.IntField("Number of Cars", tileData.carPrefabs != null ? tileData.carPrefabs.Length : 0));
        EnsureArraySizes(carCount);

        for (int i = 0; i < carCount; i++)
        {
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField($"Car {i + 1}", EditorStyles.boldLabel);

            tileData.carPrefabs[i] = (GameObject)EditorGUILayout.ObjectField("Prefab", tileData.carPrefabs[i], typeof(GameObject), false);
            tileData.carPosition[i] = EditorGUILayout.Vector3Field("Position", tileData.carPosition[i]);
            tileData.carSpeeds[i] = EditorGUILayout.FloatField("Speed", tileData.carSpeeds[i]);

            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndScrollView();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(tileData);
        }
    }

    private void EnsureArraySizes(int size)
    {
        if (tileData.carPrefabs == null || tileData.carPrefabs.Length != size)
            System.Array.Resize(ref tileData.carPrefabs, size);
        if (tileData.carPosition == null || tileData.carPosition.Length != size)
            System.Array.Resize(ref tileData.carPosition, size);
        if (tileData.carSpeeds == null || tileData.carSpeeds.Length != size)
            System.Array.Resize(ref tileData.carSpeeds, size);
    }
}