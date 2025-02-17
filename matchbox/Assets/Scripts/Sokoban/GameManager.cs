using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameObject[] boxes;
    private bool gameWin = false;

    void Start()
    {
        boxes = GameObject.FindGameObjectsWithTag("Box");
    }

    void Update()
    {
        if (!gameWin)
        {
            CheckLineMatches();
        }
    }

    void CheckLineMatches()
    {
        boxes = GameObject.FindGameObjectsWithTag("Box");
        HashSet<GameObject> toRemove = new HashSet<GameObject>();

        FindMatches(Vector2.right, toRemove);

        FindMatches(Vector2.up, toRemove);

        if (toRemove.Count >= 3)
        {
            foreach (GameObject box in toRemove)
            {
                Destroy(box);
            }
            Debug.Log("Boxes Removed: " + toRemove.Count);
        }
    }

    void FindMatches(Vector2 direction, HashSet<GameObject> toRemove)
    {
        float distance = 1.0f;
        foreach (GameObject box in boxes)
        {
            Vector2 startPos = box.transform.position;
            List<GameObject> lineMatches = new List<GameObject> { box };

            Vector2 currentPos = startPos + direction * distance;
            while (true)
            {
                Collider2D hit = Physics2D.OverlapBox(currentPos, new Vector2(0.1f, 0.1f), 0, LayerMask.GetMask("Box"));
                if (hit && !lineMatches.Contains(hit.gameObject))
                {
                    lineMatches.Add(hit.gameObject);
                    currentPos += direction * distance;
                }
                else
                {
                    break;
                }
            }

            if (lineMatches.Count >= 3)
            {
                foreach (GameObject match in lineMatches)
                {
                    toRemove.Add(match);
                }
            }
        }
    }
}
