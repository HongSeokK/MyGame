using UnityEngine;
using UnityEngine.EventSystems;
using Randomm = System.Random;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ManeProject.Domain.Box;
using ManeProject.Infrastructure.DB;
using ManeProject.Infrastructure.Repository;

namespace ManeProject.Scene.GameMain
{
    public class GameMainManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject Red;
        [SerializeField]
        private GameObject Blue;
        [SerializeField]
        private GameObject Yellow;

        private GameObject mouseDownSelectedObj;
        private GameObject mouseUpSelectedObj;

        private bool IsBoxesMoving;

        // Use this for initialization
        private void Awake()
        {
            InitScene();
        }

        /// <summary>
        /// シーンを初期化する
        /// </summary>
        async void InitScene()
        {

            await DBManager.Initialize();

            var boxInfo = await BoxRepository.Instance.CreateBoxArray();

            var row = boxInfo.GetLength(0);
            var column = boxInfo.GetLength(1);

            IsBoxesMoving = false;

            GameObject boxObject;

            for (int r = 0; r < row; r++)
            {
                for (int c = 0; c < column; c++)
                {
                    switch (boxInfo[r, c].BoxType)
                    {
                        case var _ when BoxType.Red == boxInfo[r, c].BoxType:
                            boxObject = Instantiate(Red, boxInfo[r, c].BoxPosition, new Quaternion());
                            boxObject.name = boxInfo[r, c].BoxName.Value;
                            boxInfo[r, c] = boxInfo[r, c].SetGameObj(boxObject);
                            break;
                        case var _ when BoxType.Blue == boxInfo[r, c].BoxType:
                            boxObject = Instantiate(Blue, boxInfo[r, c].BoxPosition, new Quaternion());
                            boxObject.name = boxInfo[r, c].BoxName.Value;
                            boxInfo[r, c] = boxInfo[r, c].SetGameObj(boxObject);
                            break;
                        case var _ when BoxType.Yellow == boxInfo[r, c].BoxType:
                            boxObject = Instantiate(Yellow, boxInfo[r, c].BoxPosition, new Quaternion());
                            boxObject.name = boxInfo[r, c].BoxName.Value;
                            boxInfo[r, c] = boxInfo[r, c].SetGameObj(boxObject);
                            break;
                    }
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            // タッチを確認。
            // TODO : 現在はマウスクリック判定で処理チェック中なんで、Touch 処理追加必要
            if (!IsBoxesMoving) CheckTouch();
        }

        /// <summary>
        /// タッチ処理チェック
        /// </summary>
        private void CheckTouch()
        {
            // ターチ判定
            if (Input.touchCount > 0)
            {
                for (int touch = 0; touch < Input.touchCount; touch++)
                {
                    var nTouch = Input.GetTouch(touch);

                    if (nTouch.phase == TouchPhase.Began)
                    {
                        RaycastHit hit = new RaycastHit();

                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                        if (Physics.Raycast(ray.origin, ray.direction, out hit))
                        {
                            mouseDownSelectedObj = hit.transform.gameObject;
                        }
                    }

                    if (nTouch.phase == TouchPhase.Began)
                    {
                        RaycastHit hit = new RaycastHit();

                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                        if (Physics.Raycast(ray.origin, ray.direction, out hit))
                        {
                            mouseUpSelectedObj = hit.transform.gameObject;
                        }

                        if (mouseUpSelectedObj != null && mouseDownSelectedObj != null && mouseDownSelectedObj == mouseUpSelectedObj)
                        {
                            var row = int.Parse(mouseUpSelectedObj.name.Substring(0, 1));

                            var column = int.Parse(mouseUpSelectedObj.name.Substring(1, 1));

                            var result = BoxRepository.Instance.TryDelete(row, column);

                            var BoxList = result.BoxList;

                            RearrangeBoxes(BoxList);
                        }
                        mouseDownSelectedObj = null;
                        mouseUpSelectedObj = null;
                    }
                }
            }

            // マウスクリック判定
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit = new RaycastHit();

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray.origin, ray.direction, out hit))
                {
                    mouseDownSelectedObj = hit.transform.gameObject;
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                RaycastHit hit = new RaycastHit();

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray.origin, ray.direction, out hit))
                {
                    mouseUpSelectedObj = hit.transform.gameObject;
                }

                if (mouseUpSelectedObj != null && mouseDownSelectedObj != null && mouseDownSelectedObj == mouseUpSelectedObj)
                {
                    var row = int.Parse(mouseUpSelectedObj.name.Substring(0, 1));

                    var column = int.Parse(mouseUpSelectedObj.name.Substring(1, 1));

                    var result = BoxRepository.Instance.TryDelete(row, column);

                    var BoxList = result.BoxList;

                    RearrangeBoxes(BoxList);
                }
                mouseDownSelectedObj = null;
                mouseUpSelectedObj = null;
            }
        }

        /// <summary>
        /// 配列を再整列する
        /// </summary>
        /// <param name="BoxList"></param>
        private void RearrangeBoxes(IBoxArray[,] BoxList)
        {
            for (int r = 0; r < BoxList.GetLength(0); r++)
            {
                for (int c = 0; c < BoxList.GetLength(1); c++)
                {
                    GameObject boxObject;

                    if (BoxList[r, c].IsRegenerated && BoxList[r, c].GameObj == null)
                    {
                        var CreatePos = new Vector3(BoxList[r, c].BoxPosition.x, Common.Common.REGENERATED_Y, BoxList[r, c].BoxPosition.z);
                        switch (BoxList[r, c].BoxType)
                        {
                            case var _ when BoxType.Red == BoxList[r, c].BoxType:
                                boxObject = Instantiate(Red, CreatePos, new Quaternion());
                                boxObject.name = BoxList[r, c].BoxName.Value;
                                BoxList[r, c] = BoxList[r, c].SetGameObj(boxObject);
                                break;
                            case var _ when BoxType.Blue == BoxList[r, c].BoxType:
                                boxObject = Instantiate(Blue, CreatePos, new Quaternion());
                                boxObject.name = BoxList[r, c].BoxName.Value;
                                BoxList[r, c] = BoxList[r, c].SetGameObj(boxObject);
                                break;
                            case var _ when BoxType.Yellow == BoxList[r, c].BoxType:
                                boxObject = Instantiate(Yellow, CreatePos, new Quaternion());
                                boxObject.name = BoxList[r, c].BoxName.Value;
                                BoxList[r, c] = BoxList[r, c].SetGameObj(boxObject);
                                break;
                        }
                    }

                    if (BoxList[r, c].GameObj != null
                        && BoxList[r, c].GameObj.transform.position != BoxList[r, c].BoxPosition)
                    {
                        StartCoroutine(PositionChange(BoxList[r, c].GameObj, BoxList[r, c].BoxPosition));
                    }
                }
            }
        }

        /// <summary>
        /// ボックス移動コルティン
        /// </summary>
        /// <param name="currentPos">ゲームオブジェクトの現在位置</param>
        /// <param name="Pos">移動位置</param>
        /// <returns></returns>
        /// <remarks>
        /// コルティンチェック方式は下記のリンクを参照しました。
        /// https://answers.unity.com/questions/126783/can-i-check-if-a-coroutine-is-running.html
        /// </remarks>
        private IEnumerator PositionChange(GameObject currentPos, Vector3 Pos)
        {
            var speed = Vector2.Distance(currentPos.transform.position, Pos) / 100f;

            while (true)
            {
                currentPos.transform.position = Vector3.MoveTowards(currentPos.transform.position, Pos, speed);
                if (Vector2.Distance(Pos, currentPos.transform.position) == 0)
                {
                    break;
                }
                IsBoxesMoving = true;

                yield return null;
            }
            IsBoxesMoving = false;
        }
    }
}
