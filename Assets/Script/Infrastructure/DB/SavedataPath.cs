using System;
using UnityEngine;
namespace ManeProject.Infrastructure.DB
{
    public static class SavedataPath
    {
        /// <summary>
        /// セーブデータパスを探す。
        /// </summary>
        /// <returns></returns>
        public static string GetSecureDataPath()
        {  
#if !UNITY_EDITOR && UNITY_ANDROID
        using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        using (var currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
        using (var getFilesDir = currentActivity.Call<AndroidJavaObject>("getFilesDir"))
        {
            string secureDataPathForAndroid = getFilesDir.Call<string>("getCanonicalPath");
            return secureDataPathForAndroid;
        }
#elif !UNITY_EDITOR && UNITY_IOS
            UnityEngine.iOS.Device.SetNoBackupFlag(Application.persistentDataPath);
#endif
            return Application.persistentDataPath;
        }
    }
}