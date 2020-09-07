using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SceneInfo = ManeProject.Common.SceneCommon.SceneInfo;

namespace ManeProject.Scene
{
    public class StartSceneManager : MonoBehaviour
    {
        /// <summary>
        /// シーンチェンジ用ボタン
        /// </summary>
        [SerializeField] private Button m_Button;

        /// <summary>
        /// メインシーンを呼び込む
        /// </summary>
        public void SceneProcess()
        {
            /// ボタンを非活性化
            m_Button.interactable = false;

            /// シーンコントローラーでシーンチェンジ（ローディングシーンを読んでくる）
            SceneChangeController.LoadScene(SceneInfo.GameMain);
        }
    }
}