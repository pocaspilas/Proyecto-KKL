using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine.SceneManagement;
using System.IO;
using System.Linq;

namespace Mati36.CustomBuild
{
    static public class CustomBuildPipeline
    {
        //
        //METHODS
        //
        static public void BuildAndroid()
        {
            BuildPlayerOptions options = new BuildPlayerOptions();
            BuildToolConfig currentConfig = CustomBuildPrefs.ConfigAsset;
            options.scenes = currentConfig.scenesInBuild_Android.Select(x => AssetDatabase.GetAssetOrScenePath(x)).ToArray();
            options.locationPathName = "Builds/Android/" + CustomBuildPrefs.AppName + ".apk";
            options.target = BuildTarget.Android;
            options.options = currentConfig.autorun ? options.options | BuildOptions.AutoRunPlayer : options.options;
            options.options = currentConfig.developmentMode ? options.options | BuildOptions.Development : options.options;
            BuildWithOptions(options);
        }

        static public void BuildWindows64()
        {
            BuildPlayerOptions options = new BuildPlayerOptions();
            BuildToolConfig currentConfig = CustomBuildPrefs.ConfigAsset;
            options.scenes = currentConfig.scenesInBuild_Windows.Select(x => AssetDatabase.GetAssetOrScenePath(x)).ToArray();
            options.locationPathName = "Builds/Windows/" + CustomBuildPrefs.AppName + ".exe";
            options.target = BuildTarget.StandaloneWindows64;
            options.options = currentConfig.autorun ? options.options | BuildOptions.AutoRunPlayer : options.options;
            options.options = currentConfig.developmentMode ? options.options | BuildOptions.Development : options.options;
            BuildWithOptions(options);
        }

        static public void BuildWindows32()
        {
            BuildPlayerOptions options = new BuildPlayerOptions();
            BuildToolConfig currentConfig = CustomBuildPrefs.ConfigAsset;
            options.scenes = currentConfig.scenesInBuild_Windows.Select(x => AssetDatabase.GetAssetOrScenePath(x)).ToArray();
            options.locationPathName = "Builds/Windows/" + CustomBuildPrefs.AppName + ".exe";
            options.target = BuildTarget.StandaloneWindows;
            options.options = currentConfig.autorun ? options.options | BuildOptions.AutoRunPlayer : options.options;
            options.options = currentConfig.developmentMode ? options.options | BuildOptions.Development : options.options;
            BuildWithOptions(options);
        }

        static private void BuildWithOptions(BuildPlayerOptions options)
        {
            string resultLog;
#if UNITY_2018
        resultLog = BuildPipeline.BuildPlayer(options).summary.result.ToString();
#else
            resultLog = BuildPipeline.BuildPlayer(options);
#endif
            Debug.Log(resultLog == "" ? "Build completado con éxito para " + options.target.ToString() : resultLog);
        }

        static public void PushToAndroid()
        {
            string strCmdText;
            string dataPath = Path.GetFullPath(Application.dataPath + @"\..\Builds\Android\" + CustomBuildPrefs.AppName + ".apk");
            if (!File.Exists(dataPath))
            { EditorUtility.DisplayDialog("Error", "No se encontró el archivo " + dataPath + ". Debe buildear para Android para poder pushear.", "Ok"); return; }
            if (!File.Exists(CustomBuildPrefs.CurrentSDKPath + "/platform-tools/adb.exe"))
            { EditorUtility.DisplayDialog("Error", "No se encontró el SDK de Android. Vaya a Preferences/Custom Build y setee la ruta correcta.", "Ok"); return; }
            strCmdText = "/c " + CustomBuildPrefs.CurrentSDKPath + "/platform-tools/adb.exe install -r " + "\"" + dataPath + "\"";
            System.Diagnostics.Process.Start("CMD.exe", strCmdText);
        }
    }
}