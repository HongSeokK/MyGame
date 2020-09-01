using UnityEngine;
using UnityEngine.EventSystems;
using Randomm = System.Random;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ManeProject.Domain.Box;
using ManeProject.Common;
using ManeProject.Infrastructure.DB;
using ManeProject.Infrastructure.Repository;


public class DBTest : MonoBehaviour
{
    [SerializeField]
    private GameObject Red;
    [SerializeField]
    private GameObject Blue;
    [SerializeField]
    private GameObject Yellow;

    private GameObject mouseDownSelectedObj;
    private GameObject mouseUpSelectedObj;

    // Use this for initialization

    private void Awake()
    {
        InitScene();
    }

    async void InitScene()
    {

        await DBConnect.Initialize();

        var boxInfo = await BoxRepository.Instance.CreateBoxArray();

        var row = boxInfo.GetLength(0);
        var column = boxInfo.GetLength(1);

        GameObject boxObject;

        for(int r = 0; r < row; r++)
        {
            for(int c = 0; c< column; c++)
            {
                switch (boxInfo[r,c].BoxType)
                {
                    case var _ when BoxType.Red == boxInfo[r, c].BoxType:
                        boxObject = Instantiate(Red, new Vector3(boxInfo[r, c].BoxPosition.X, boxInfo[r, c].BoxPosition.Y, boxInfo[r, c].BoxPosition.Z), new Quaternion());
                        boxObject.name = boxInfo[r, c].BoxName.Value;
                        boxInfo[r, c] = boxInfo[r, c].SetGameObj(boxObject);
                        break;
                    case var _ when BoxType.Blue == boxInfo[r, c].BoxType:
                        boxObject = Instantiate(Blue, new Vector3(boxInfo[r, c].BoxPosition.X, boxInfo[r, c].BoxPosition.Y, boxInfo[r, c].BoxPosition.Z), new Quaternion());
                        boxObject.name = boxInfo[r, c].BoxName.Value;
                        boxInfo[r, c] = boxInfo[r, c].SetGameObj(boxObject);
                        break;
                    case var _ when BoxType.Yellow == boxInfo[r, c].BoxType:
                        boxObject = Instantiate(Yellow, new Vector3(boxInfo[r, c].BoxPosition.X, boxInfo[r, c].BoxPosition.Y, boxInfo[r, c].BoxPosition.Z), new Quaternion());
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
        CheckTouch();
    }

    private void CheckTouch()
    {
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

                var test = BoxRepository.Instance.TryDelete(row, column);

                var test2 = test.BoxList;

                Debug.Log(mouseUpSelectedObj.name);

                for (int r = 0; r < test2.GetLength(0); r++)
                {
                    for (int c = 0; c < test2.GetLength(1); c++)
                    {
                        GameObject boxObject;

                        if (test2[r, c].IsRegenerated && test2[r, c].GameObj == null)
                        {
                            switch (test2[r, c].BoxType)
                            {
                                case var _ when BoxType.Red == test2[r, c].BoxType:
                                    boxObject = Instantiate(Red, new Vector3(test2[r, c].BoxPosition.X, Common.REGENERATED_Y, test2[r, c].BoxPosition.Z), new Quaternion());
                                    boxObject.name = test2[r, c].BoxName.Value;
                                    test2[r, c] = test2[r, c].SetGameObj(boxObject);
                                    break;
                                case var _ when BoxType.Blue == test2[r, c].BoxType:
                                    boxObject = Instantiate(Blue, new Vector3(test2[r, c].BoxPosition.X, Common.REGENERATED_Y, test2[r, c].BoxPosition.Z), new Quaternion());
                                    boxObject.name = test2[r, c].BoxName.Value;
                                    test2[r, c] = test2[r, c].SetGameObj(boxObject);
                                    break;
                                case var _ when BoxType.Yellow == test2[r, c].BoxType:
                                    boxObject = Instantiate(Yellow, new Vector3(test2[r, c].BoxPosition.X, Common.REGENERATED_Y, test2[r, c].BoxPosition.Z), new Quaternion());
                                    boxObject.name = test2[r, c].BoxName.Value;
                                    test2[r, c] = test2[r, c].SetGameObj(boxObject);
                                    break;
                            }
                        }

                        if (test2[r, c].GameObj != null
                            && test2[r, c].GameObj.transform.position != new Vector3(test2[r, c].BoxPosition.X, test2[r, c].BoxPosition.Y, test2[r, c].BoxPosition.Z))
                        {
                            Debug.Log(";;");
                            StartCoroutine(PositionChange(test2[r, c].GameObj, new Vector3(test2[r, c].BoxPosition.X, test2[r, c].BoxPosition.Y, test2[r, c].BoxPosition.Z)));
                        }
                    }
                }
            }
            mouseDownSelectedObj = null;
            mouseUpSelectedObj = null;
        }
    }


    private IEnumerator PositionChange(GameObject currentPos, Vector3 Pos)
    {
        var dir = (Pos - currentPos.transform.position).normalized;

        Debug.Log(Pos);

        var speed = Vector2.Distance(currentPos.transform.position, Pos) / 100f;
        while (true)
        {
            ///currentPos.transform.position += dir * speed;
            currentPos.transform.position = Vector3.MoveTowards(currentPos.transform.position, Pos, speed);
            if (Vector2.Distance(Pos, currentPos.transform.position) == 0)
            {
                Debug.Log("breaked");
                break;
            }

            yield return null;
        }
    }
}
