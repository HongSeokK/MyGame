using UnityEngine;
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
    // Use this for initialization
    async void Start()
    {
        await DBConnect.Initialize();

        var ttest = DBConnect.SQLConnect.DatabasePath;

        var test = BoxRepository.Instance;

        var Random = new Randomm();


        var test2 = await test.CreateRandomType(64);

        foreach (var s in test2)
        {
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

    }
}
