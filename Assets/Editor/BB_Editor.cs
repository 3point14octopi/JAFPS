using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace JAFBillboard
{
    [CustomEditor(typeof(Billboard)), CanEditMultipleObjects]
    public class BB_Editor : Editor
    {
        
        public override void OnInspectorGUI()
        {
            Billboard bboard = target as Billboard;

            serializedObject.Update();

            if (bboard.renderStyle == EBillboardState.Static)
            {
                DrawPropertiesExcluding(serializedObject, "rControl");
            }
            else
            {
                DrawPropertiesExcluding(serializedObject, "upper", "mid", "lower", "NumberOfDirections");
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}


