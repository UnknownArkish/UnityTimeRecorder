using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TimeRecorder))]
public class TimeRecorderEditor : Editor
{
    private bool _Foldout_TimeRecord = true;
    public override void OnInspectorGUI()
    {
        TimeRecorder timeRecorder = target as TimeRecorder;

        int oldIndentLevel = EditorGUI.indentLevel;

        _Foldout_TimeRecord = EditorGUILayout.Foldout(_Foldout_TimeRecord, new GUIContent("Time Records"));
        if (_Foldout_TimeRecord)
        {
            var records = timeRecorder.Reocrds;
            for (int i = 0; i < records.Length; i++)
            {
                var record = records[i];
                OnInspectorGUI_RecordData(record);
            }
        }

        EditorGUI.indentLevel = oldIndentLevel;
        Repaint();
    }

    private void OnInspectorGUI_RecordData(TimeRecorder.RecordData record)
    {
        // 使用RecordData的深度作为IndentLevel
        EditorGUI.indentLevel = record.Depth;

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(record.ActionName);
        EditorGUILayout.LabelField(
            string.Format("{0:N2} {1}",
            record.Time,
            record.Unit == TimeRecorder.RecordUnit.Millisecond ? "ms" : "s"));
        EditorGUILayout.EndHorizontal();
    }
}