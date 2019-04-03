using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Mati36.CustomGUI;

namespace Mati36.CustomBuild
{
    public class CustomBuildWindow : EditorWindow
    {
        static EditorWindow w;

        BuildTargetGroup currentTargetGroup;

        [MenuItem("Custom Build/Open Tool")]
        static public void CreateWindow()
        {
            w = GetWindow(typeof(CustomBuildWindow));
            w.Show();
            w.minSize = new Vector2(300, 100);
            w.titleContent = new GUIContent("CustomBuild");
        }

        private void OnGUI()
        {
            if (w == null) w = this;

            if (isTallWindow())
                VerticalLayout();
            else
                HorizontalLayout();
        }

        private void HorizontalLayout()
        {
            GUILayout.BeginHorizontal();

            GUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.MinWidth(200), GUILayout.ExpandHeight(true));
            DrawActionsBar();
            GUILayout.EndVertical();

            GUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            DrawConfigBar();
            GUILayout.EndVertical();

            GUILayout.EndHorizontal();
        }

        private void VerticalLayout()
        {
            GUILayout.BeginVertical();

            GUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            DrawActionsBar();
            GUILayout.EndVertical();

            GUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            DrawConfigBar();
            GUILayout.EndVertical();

            GUILayout.EndVertical();
        }

        //ACTIONS
        private void DrawActionsBar()
        {
            GUILayout.Label("Acciones", EditorStyles.boldLabel);

            currentTargetGroup = BuildPipeline.GetBuildTargetGroup(EditorUserBuildSettings.activeBuildTarget);

            //WINDOWS
            if (currentTargetGroup == BuildTargetGroup.Standalone)
                CustomGUIUtility.BeginGUIColorBG(Color.green);
            GUILayout.BeginHorizontal(EditorStyles.helpBox);
            CustomGUIUtility.EndGUIColorBG();

            GUILayout.Label("PC/Mac");
            if (currentTargetGroup != BuildTargetGroup.Standalone && GUILayout.Button("Switch", EditorStyles.toolbarButton, GUILayout.Width(60)))
                EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows64);
            GUILayout.EndHorizontal();

            if (GUILayout.Button("Build Windows x32"))
            { CustomBuildPipeline.BuildWindows32(); GUIUtility.ExitGUI(); }
            if (GUILayout.Button("Build Windows x64"))
            { CustomBuildPipeline.BuildWindows64(); GUIUtility.ExitGUI(); }

            //ANDROID
            if (currentTargetGroup == BuildTargetGroup.Android)
                CustomGUIUtility.BeginGUIColorBG(Color.green);
            GUILayout.BeginHorizontal(EditorStyles.helpBox);
            CustomGUIUtility.EndGUIColorBG();

            GUILayout.Label("Android");
            if (currentTargetGroup != BuildTargetGroup.Android && GUILayout.Button("Switch", EditorStyles.toolbarButton, GUILayout.Width(60)))
                EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);

            GUILayout.EndHorizontal();
            if (GUILayout.Button("Build Android"))
            { CustomBuildPipeline.BuildAndroid(); GUIUtility.ExitGUI(); }
            if (GUILayout.Button("Pushear a Android"))
                CustomBuildPipeline.PushToAndroid();
        }

        //CONFIG
        private void DrawConfigBar()
        {
            GUILayout.Label("Configuración", EditorStyles.boldLabel);

            if (GUILayout.Button("Abrir archivo de Configuración"))
            {
                Object configAsset = CustomBuildPrefs.ConfigAsset;
                Selection.activeObject = configAsset;
                EditorGUIUtility.PingObject(configAsset);
            }
        }

        //HELPER
        private bool isTallWindow()
        {
            return w.position.width < 500;
        }
    }
}