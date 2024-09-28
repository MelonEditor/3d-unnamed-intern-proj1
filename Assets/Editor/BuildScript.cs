using UnityEditor; // This is required for editor-related functionalities
using UnityEngine;

public class BuildScript
{
    [MenuItem("Build/Build Project")] // This adds the option in the "Build" menu in Unity
    public static void BuildGame()
    {
        // Specify the scenes you want to include in the build
        string[] scenes = { "Assets/Scenes/Level-1.unity"};

        // Specify the build path and target platform
        string buildPath = "/home/patri/Projects/3d-unnamed-intern-proj1/Build/Linux/NewGame"; // Replace with your desired path

        // Build the project
        BuildPipeline.BuildPlayer(scenes, buildPath, BuildTarget.StandaloneLinux64, BuildOptions.None);

    }
}
