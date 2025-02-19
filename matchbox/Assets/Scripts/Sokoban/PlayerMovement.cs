using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



public class PlayerMovement : MonoBehaviour
{

    public LayerMask obstacleLayer;
    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        obstacleLayer = LayerMask.GetMask("Box","Wall");

    }

    void Update()
    {
        
        /*
        if (Board.arrowColor== "0") Move(Vector2.down);// yellow
        if (Board.arrowColor== "1") Move(Vector2.left);//green
        if (Board.arrowColor== "2") Move(Vector2.right);//red
        if (Board.arrowColor== "3") Move(Vector2.up);//blue
        */
        

        if (Input.GetKeyDown(KeyCode.W)) Move(Vector2.up);
        if (Input.GetKeyDown(KeyCode.A)) Move(Vector2.left);
        if (Input.GetKeyDown(KeyCode.S)) Move(Vector2.down);
        if (Input.GetKeyDown(KeyCode.D)) Move(Vector2.right);
    
    }

    public void Move(Vector2 direction)
    {
        Vector2 targetPosition = (Vector2)transform.position + direction;
        Collider2D hit = Physics2D.OverlapBox(targetPosition, new Vector2(0.1f, 0.1f), 0f, obstacleLayer);
        if (hit)
        {
            if (hit.CompareTag("Box"))
            {
                Vector2 boxTargetPosition = targetPosition + direction;
                Collider2D boxHit = Physics2D.OverlapBox(boxTargetPosition, new Vector2(0.1f, 0.1f), 0f, obstacleLayer);
                if (!boxHit)
                {
                    hit.transform.position = boxTargetPosition;
                    transform.position = targetPosition;
                    Invoke(nameof(TriggerMatchCheck), 0.1f);
                    return;
                }
            }
        }
        else
        {
            transform.position = targetPosition;
        }
        Invoke(nameof(TriggerMatchCheck), 0.1f);
    }

    private void TriggerMatchCheck()
    {
        gameManager.CheckLineMatches();
    }

}