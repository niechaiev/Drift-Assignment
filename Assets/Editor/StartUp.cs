using UnityEngine;
using UnityEditor;
 
[InitializeOnLoad]
public class StartUp {
 
    #if UNITY_EDITOR
 
    static StartUp() {
 
        PlayerSettings.keystorePass = "111111";
        PlayerSettings.keyaliasPass = "111111";
    }
 
    #endif
}