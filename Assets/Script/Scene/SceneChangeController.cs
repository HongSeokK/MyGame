using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using SceneInfo = ManeProject.Common.SceneCommon.SceneInfo;

namespace ManeProject.Scene
{
    public class SceneChangeController : MonoBehaviour
    {
        /// <summary>
        /// ロードするシーン
        /// </summary>
        public static string nextScene;

        /// <summary>
        /// ローディングバー
        /// </summary>
        [SerializeField] private Slider m_progressBar;

        /// <summary>
        /// シーン初期化
        /// ローディングシーンで使うクラスなので、
        /// ローディングバーの初期化、設定されたシーンを呼び込むコルティンの実行
        /// </summary>
        private void Start()
        {
            m_progressBar.value = 0f;
            StartCoroutine(LoadScene());
        }

        /// <summary>
        /// 他のシーンでシーンをロードするための Static 関数
        /// </summary>
        /// <param name="next"></param>
        public static void LoadScene(SceneInfo next)
        {
            nextScene = $"Scenes/{next}";
            SceneManager.LoadScene($"Scenes/{SceneInfo.LoadingScene}");
        }

        /// <summary>
        /// シーンを読んでくる
        /// </summary>
        /// <returns></returns>
        private IEnumerator LoadScene()
        {
            /// 次のシーンを準備する
            var op = SceneManager.LoadSceneAsync(nextScene);
            op.allowSceneActivation = false;

            // progressbar を使うためのタイマー
            float timer = 0.0f;

            /// シーン準備が終わるまでに待つ
            /// progressbar の処理を行い、終わったらシーンを呼ぶ
            while (!op.isDone)
            {
                timer += Time.deltaTime;
                if (op.progress < 0.9f)
                {
                    m_progressBar.value = Mathf.Lerp(m_progressBar.value, op.progress, timer);
                    if (m_progressBar.value >= op.progress)
                    {
                        timer = 0f;
                    }
                }
                else
                {
                    yield return new WaitForSeconds(0.1f);
                    m_progressBar.value = Mathf.Lerp(m_progressBar.value, 1f, timer);
                    if (m_progressBar.value == 1.0f)
                    {
                        yield return new WaitForSeconds(0.5f);
                        op.allowSceneActivation = true;
                        yield break;
                    }
                }
            }
        }
    }
}