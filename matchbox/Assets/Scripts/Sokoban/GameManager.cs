using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private GameObject[] boxes;
    public GameObject winPanel;
    public UnityEngine.UI.Button restartButton;
    void Start()
    {
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(RestartGame);
        }
        winPanel.SetActive(false);
        boxes = GameObject.FindGameObjectsWithTag("Box");
        CheckLineMatches();
    }

    void Update()
    {

        if (GameObject.FindGameObjectsWithTag("Box").Length == 0)
        {
            ShowWinPanel();
        }
    }

    public void CheckLineMatches()
    {
        boxes = GameObject.FindGameObjectsWithTag("Box");
        HashSet<GameObject> toRemove = new HashSet<GameObject>();

        FindMatches(Vector2.left, toRemove);
        FindMatches(Vector2.down, toRemove);

        if (toRemove.Count >= 3)
        {
            foreach (GameObject box in toRemove)
            {
                Destroy(box);
            }
            Debug.Log("Boxes Removed: " + toRemove.Count);
        }
        if (GameObject.FindGameObjectsWithTag("Box").Length == 0)
        {
            ShowWinPanel();
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
            //Debug.Log(string.Join(", ", lineMatches.Select(box => box.name)));



            if (lineMatches.Count >= 3)
            {
                foreach (GameObject match in lineMatches)
                {
                    toRemove.Add(match);
                }
            }
        }
    }
    private void ShowWinPanel()
    {
        Debug.Log("Win Panel Active: " + winPanel.activeSelf);

        if (winPanel != null)
        {
            winPanel.SetActive(true);
        }
        else
        {
            Debug.Log("YOU WIN!");
        }
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
