using UnityEngine;
using UnityEngine.EventSystems;
using Randomm = System.Random;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ManeProject.Domain.Box;
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

    private int m_tileRow;
    private int m_tileColumn;


    // Use this for initialization

    private void Awake()
    {

    }

    async void Start()
    {

        await DBConnect.Initialize();

        var test2 = await BoxRepository.Instance.CreateBoxArray();

        var row = test2.GetLength(0);
        var column = test2.GetLength(1);

        GameObject boxObject;

        for(int r = 0; r < row; r++)
        {
            for(int c = 0; c< column; c++)
            {
                switch (test2[r,c].BoxType)
                {
                    case var _ when BoxType.Red == test2[r, c].BoxType:
                        boxObject = Instantiate(Red, new Vector3(test2[r, c].BoxPosition.X, test2[r, c].BoxPosition.Y, test2[r, c].BoxPosition.Z), new Quaternion());
                        boxObject.name = test2[r, c].BoxName.Value;
                        test2[r, c] = test2[r, c].SetGameObj(boxObject);
                        break;
                    case var _ when BoxType.Blue == test2[r, c].BoxType:
                        boxObject = Instantiate(Blue, new Vector3(test2[r, c].BoxPosition.X, test2[r, c].BoxPosition.Y, test2[r, c].BoxPosition.Z), new Quaternion());
                        boxObject.name = test2[r, c].BoxName.Value;
                        test2[r, c] = test2[r, c].SetGameObj(boxObject);
                        break;
                    case var _ when BoxType.Yellow == test2[r, c].BoxType:
                        boxObject = Instantiate(Yellow, new Vector3(test2[r, c].BoxPosition.X, test2[r, c].BoxPosition.Y, test2[r, c].BoxPosition.Z), new Quaternion());
                        boxObject.name = test2[r, c].BoxName.Value;
                        test2[r, c] = test2[r, c].SetGameObj(boxObject);
                        break;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
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

                for (int r = 0; r < test2.GetLength(0); r++)
                {
                    for (int c = 0; c < test2.GetLength(1); c++)
                    {
                        GameObject boxObject;

                        if (test2[r,c].GameObj != null)
                        {
                            test2[r, c].GameObj.transform.position = new Vector3(test2[r, c].BoxPosition.X, test2[r, c].BoxPosition.Y, test2[r, c].BoxPosition.Z);
                        }

                        if (test2[r, c].IsRegenerated && test2[r, c].GameObj == null)
                        {
                            switch (test2[r, c].BoxType)
                            {
                                case var _ when BoxType.Red == test2[r, c].BoxType:
                                    boxObject = Instantiate(Red, new Vector3(test2[r, c].BoxPosition.X, test2[r, c].BoxPosition.Y, test2[r, c].BoxPosition.Z), new Quaternion());
                                    boxObject.name = test2[r, c].BoxName.Value;
                                    test2[r, c] = test2[r, c].SetGameObj(boxObject);
                                    break;
                                case var _ when BoxType.Blue == test2[r, c].BoxType:
                                    boxObject = Instantiate(Blue, new Vector3(test2[r, c].BoxPosition.X, test2[r, c].BoxPosition.Y, test2[r, c].BoxPosition.Z), new Quaternion());
                                    boxObject.name = test2[r, c].BoxName.Value;
                                    test2[r, c] = test2[r, c].SetGameObj(boxObject);
                                    break;
                                case var _ when BoxType.Yellow == test2[r, c].BoxType:
                                    boxObject = Instantiate(Yellow, new Vector3(test2[r, c].BoxPosition.X, test2[r, c].BoxPosition.Y, test2[r, c].BoxPosition.Z), new Quaternion());
                                    boxObject.name = test2[r, c].BoxName.Value;
                                    test2[r, c] = test2[r, c].SetGameObj(boxObject);
                                    break;
                            }
                        }
                    }
                }
            }

            mouseDownSelectedObj = null;
            mouseUpSelectedObj = null;
        }
    }

}
