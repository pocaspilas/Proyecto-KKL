using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Mati36/BuildTool/Config")]
public class BuildToolConfig : ScriptableObject {
    public List<SceneAsset> scenesInBuild_Android = new List<SceneAsset>();
    public List<SceneAsset> scenesInBuild_Windows = new List<SceneAsset>();
    public bool autorun, developmentMode;
}
