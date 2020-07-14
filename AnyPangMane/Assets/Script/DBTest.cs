using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ManeProject.Infrastructure.DB;


public class DBTest : MonoBehaviour
{
    // Use this for initialization
    async void Start()
    {
        await DBConnect.Initialize();

        var ttest = DBConnect.SQLConnect.DatabasePath;

        Debug.Log(ttest);

        var test = DBConnect.SQLConnect.Table<Array>().AsEnumerable();

        foreach(var s in test)
        {
            Debug.Log("Row = " + s.Row + ", Column = " + s.Column + ", PositionX = " + s.PositionX + ", PositionY = " + s.PositionY);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
