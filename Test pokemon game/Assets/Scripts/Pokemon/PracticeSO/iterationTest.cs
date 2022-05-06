using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class iterationTest : MonoBehaviour
{
    List<int> iterateNumberList = new List<int>() { 1, 2, 3, 4, 5, 6, 7 };

    private void Start()
    {
        foreach (var i in iterateNumberList.Skip(6))
        {
            Debug.Log(i);

            for (int x = 0; x < iterateNumberList.Count -1; x++)
            {
                Debug.Log(x);

                if(x == 3 )
                {
                    Debug.Log("Found " + x);
                }
            }
        }
    } 
}
