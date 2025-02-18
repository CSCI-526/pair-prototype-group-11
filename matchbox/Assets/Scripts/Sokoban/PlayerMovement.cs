using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{


    public LayerMask obstacleLayer;

    void Start()
    {
        obstacleLayer = LayerMask.GetMask("Box","Wall");
    }

    void Update()
    {
        if (Board.arrowColor== "0") Move(Vector2.down);// yellow
        if (Board.arrowColor== "1") Move(Vector2.left);//green
        if (Board.arrowColor== "2") Move(Vector2.right);//red
        if (Board.arrowColor== "3") Move(Vector2.up);//blue
    }

    public void Move(Vector2 direction)
    {
        Debug.Log(direction);
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
                }
            }
            if (hit.CompareTag("Wall"))
            {


            }
        }
        else
        {
            transform.position = targetPosition;
        }
    }
}