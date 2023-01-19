using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;


namespace AlertedSnake.AnimationCombiner
{
    public class MainWindow : EditorWindow
    {
        private Vector2 scroll;

        // Our class with the real logic
        private Combiner combiner = new Combiner();

        private AnimationClip _targetAnimation;
        private List<AnimationClip> _sourceClips;

        public MainWindow() {
            _sourceClips = new List<AnimationClip>();
        }


        [MenuItem("Window/AlertedSnake/Animation Combiner")]
        public static void ShowWindow() {
            // Show existing window instance. If one doesn't exist, make one.
            var window = EditorWindow.GetWindow(typeof(MainWindow));
            window.titleContent = new GUIContent("Animation Combiner");
            window.Show();
        }

        private void Header() {
            GUIStyle styleTitle = new GUIStyle(GUI.skin.label);
            styleTitle.fontSize = 16;
            styleTitle.margin = new RectOffset(20, 20, 20, 20);
            EditorGUILayout.LabelField("Animation Combiner", styleTitle);
            EditorGUILayout.Space();

            // show the version
            GUIStyle styleVersion = new GUIStyle(GUI.skin.label);
            EditorGUILayout.LabelField(Version.VERSION, styleVersion);
            EditorGUILayout.Space();
        }

        private static bool DrawListField<type>(List<type> list) where type : UnityEngine.Object {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Add", EditorStyles.miniButton))
                list.Add(null);
            if (GUILayout.Button("Remove", EditorStyles.miniButton))
                if (list.Count > 0)
                    list.RemoveAt(list.Count - 1);
            GUILayout.EndHorizontal();

            for (int i = 0; i < list.Count; i++) {
                list[i] = (type)EditorGUILayout.ObjectField(list[i], typeof(type), false);
            }
            return false;
        }

        private void MainOptions() {
            
            _targetAnimation = EditorGUILayout.ObjectField(
                "Target Animation", _targetAnimation, typeof(AnimationClip), true) as AnimationClip;

            GUILayout.Label("Source Animations", EditorStyles.boldLabel);
            DrawListField<AnimationClip>(_sourceClips);
        }

        private void ApplyOptions() {
            combiner.setTarget(_targetAnimation);
            combiner.setSources(_sourceClips);

        }

        void OnGUI() {
            Header();

            scroll = EditorGUILayout.BeginScrollView(scroll);
            MainOptions();
            ApplyOptions();

            if (GUILayout.Button("Combine")) {
                combiner.Combine();
            }

            EditorGUILayout.EndScrollView();
        }
    }
}
