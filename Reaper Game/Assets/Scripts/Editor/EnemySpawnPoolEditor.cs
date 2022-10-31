using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Reaper.Enemy;

namespace Reaper.Environment
{
    [CustomEditor(typeof(EnemySpawnPool))]
    public class EnemySpawnPoolEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            EnemySpawnPool pool = (EnemySpawnPool)target;
            int length = EditorGUILayout.DelayedIntField("Number of Enemy Types", pool.Count);
            while (length > pool.Count)
            {
                pool.Add(null, 0);
            }
            if (length < pool.Count)
            {
                pool.RemoveRange(length, pool.Count - length);
            }
            if (length == 0)
                return;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Enemy Type");
            EditorGUILayout.LabelField("Weight");
            EditorGUILayout.EndHorizontal();
            for (int i = 0; i < length; i++)
            {
                EditorGUILayout.BeginHorizontal();
                (EnemyInfo, int) item = pool[i];
                item.Item1 = (EnemyInfo)EditorGUILayout.ObjectField(item.Item1, typeof(EnemyInfo), false);
                item.Item2 = EditorGUILayout.IntField(item.Item2);
                pool[i] = item;
                EditorGUILayout.EndHorizontal();
            }
            if(GUI.changed)
                EditorUtility.SetDirty(target);
        }
    }
}
