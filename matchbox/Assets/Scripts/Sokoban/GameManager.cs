using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameObject[] boxes;
    private GameObject[] targets;
    private bool gameWin = false;

    void Start()
    {
        boxes = GameObject.FindGameObjectsWithTag("Box");
        targets = GameObject.FindGameObjectsWithTag("Target");
    }

    void Update()
    {
        if (!gameWin)
        {
            check();
        }
    }

    void check()
    {
        int matchCount = 0;

        foreach (GameObject target in targets)
        {
            foreach (GameObject box in boxes)
            {
                if (Vector2.Distance(box.transform.position, target.transform.position) < 0.1f)
                {
                    matchCount++;
                    break;
                }
            }
        }

        if (matchCount == targets.Length)
        {
            gameWin = true;
            Debug.Log("You Win");
        }
    }
}