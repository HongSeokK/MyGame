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


    private GameObject MainCanvas;
    // Use this for initialization

    private void Awake()
    {
        MainCanvas = GameObject.Find("MainCanvas");
    }

    async void Start()
    {

        await DBConnect.Initialize();
        var ttest = DBConnect.SQLConnect.DatabasePath;

        var test = BoxRepository.Instance;

        var Random = new Randomm();

        var test2 =await test.CreateBoxArray(); 

        foreach (var s in test2)
        {
            if (s == null)
                Debug.Log("null");
            switch (s.BoxType)
            {
                case var _ when BoxType.Red == s.BoxType: 
                    Instantiate(Red, new Vector3(s.BoxPosition.X,s.BoxPosition.Y,s.BoxPosition.Z),new Quaternion());
                    break;
                case var _ when BoxType.Blue == s.BoxType:
                    Instantiate(Blue, new Vector3(s.BoxPosition.X, s.BoxPosition.Y, s.BoxPosition.Z), new Quaternion());
                    break;
                case var _ when BoxType.Yellow == s.BoxType:
                    Instantiate(Yellow, new Vector3(s.BoxPosition.X, s.BoxPosition.Y, s.BoxPosition.Z), new Quaternion());
                    break;
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
                Debug.Log("12345");
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
                Debug.Log("Ho!");
            }

            mouseDownSelectedObj = null;
            mouseUpSelectedObj = null;
        }
    }

}
