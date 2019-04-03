using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Mati36.CustomBuild
{
    static public class CustomBuildPrefs
    {
        public const string PATHPREF = "CUSTOMBUILDSDKPATH/";

        static public string currentSDKPath;

        [PreferenceItem("Custom Build")]
        static public void CustomBuildPrefGUI()
        {
            GUILayout.Label("Android SDK path", EditorStyles.boldLabel);

            if (SDKPathIsSet && currentSDKPath != CurrentSDKPath)
                currentSDKPath = CurrentSDKPath;

            GUILayout.Label(currentSDKPath, EditorStyles.helpBox);

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Examine"))
            {
                string filePath;

                if (currentSDKPath != "")
                {
                    string openAtPath = Application.dataPath.Replace("Assets", "") + currentSDKPath;
                    openAtPath = Path.GetDirectoryName(openAtPath);
                    filePath = EditorUtility.OpenFolderPanel("Select Android SDK folder", openAtPath, "");
                }
                else
                    filePath = EditorUtility.OpenFolderPanel("Select Android SDK folder", Application.dataPath, "");

                if (filePath == "") return;
                currentSDKPath = filePath;
                SaveSDKPath(currentSDKPath);
            }

            GUILayout.EndHorizontal();
        }

        //
        //PROPERTIES
        //
        private static BuildToolConfig _configAsset;
        public static BuildToolConfig ConfigAsset
        {
            get
            {
                if (_configAsset == null)
                    _configAsset = (BuildToolConfig)Resources.Load("BuildConfig");

                return _configAsset;
            }
        }

        static private string _appName;
        static public string AppName
        {
            get
            {
                if (_appName == null)
                {
                    string fullPath = Application.dataPath;
                    fullPath = fullPath.Split(new string[1] { "Assets" }, System.StringSplitOptions.None)[0];
                    string[] splitPath = fullPath.Split('/');
                    _appName = splitPath[splitPath.Length - 2];
                }
                return _appName;
            }
            set { _appName = value; }
        }

        //METHODS
        static public void SaveSDKPath(string newPath)
        {
            EditorPrefs.SetString(PATHPREF, newPath);
        }

        static public string CurrentSDKPath
        {
            get { return EditorPrefs.GetString(PATHPREF); }
        }

        static public bool SDKPathIsSet
        {
            get { return EditorPrefs.HasKey(PATHPREF); }
        }
    }
}