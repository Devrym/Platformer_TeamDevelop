#if UNITY_EDITOR

using Animancer.Editor;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace PlatformerGame.Editor
{
    public readonly struct MixedValueFieldGUI<T>
    {
        public readonly Func<int, T> GetValue;
        public readonly Action<int, T> SetValue;
        public readonly Func<Rect, GUIContent, T, T> DrawValue;

        public MixedValueFieldGUI(
            Func<int, T> getValue,
            Action<int, T> setValue,
            Func<Rect, GUIContent, T, T> drawValue)
        {
            GetValue = getValue;
            SetValue = setValue;
            DrawValue = drawValue;
        }

        public void Draw(Rect area, GUIContent label, int count, SerializedProperty property)
        {
            var showMixedValue = EditorGUI.showMixedValue;

            T value;
            if (count == 0)
            {
                value = default;
            }
            else
            {
                value = GetValue(0);
                for (int i = 1; i < count; i++)
                {
                    if (!EqualityComparer<T>.Default.Equals(GetValue(i), value))
                    {
                        EditorGUI.showMixedValue = true;
                        break;
                    }
                }
            }

            EditorGUI.BeginChangeCheck();

            value = DrawValue(area, label, value);

            if (EditorGUI.EndChangeCheck())
            {
                property.RecordUndo();

                for (int i = 0; i < count; i++)
                    SetValue(i, value);
            }

            EditorGUI.showMixedValue = showMixedValue;
        }

    }
}

#endif
