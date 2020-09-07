using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SceneInfo = ManeProject.Common.SceneCommon.SceneInfo;
using ManeProject.Infrastructure.Repository;

namespace ManeProject.Scene
{
    public class EndSceneManager : MonoBehaviour
    {
        /// <summary>
        /// シーンチェンジ用ボタン
        /// </summary>
        [SerializeField] private Button m_Button;

        /// <summary>
        /// シーンチェンジ用ボタン
        /// </summary>
        [SerializeField] private Text m_ScoreText;

        private void Awake()
        {
            var score = ScoreRepository.Instance.GetScore();

            m_ScoreText.text = score.m_NowScore.ToString();
        }

        /// <summary>
        /// メインシーンを呼び込む
        /// </summary>
        public void SceneProcess()
        {
            /// ボタンを非活性化
            m_Button.interactable = false;

            /// シーンコントローラーでシーンチェンジ（ローディングシーンを読んでくる）
            SceneChangeController.LoadScene(SceneInfo.StartScene);
        }
    }
}