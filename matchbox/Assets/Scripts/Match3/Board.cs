using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public sealed class Board : MonoBehaviour
{

    public static Board Instance {get; private set;}

    public Row[] rows;
    public Box[,] Boxes{get; private set;}
    

    public int Width => Boxes.GetLength(0);
    public int Height => Boxes.GetLength(1);

    



    private void Awake() => Instance = this;
    
    private void Start()
    {
        Boxes = new Box[rows.Max(row => row.boxes.Length), rows.Length];
        Debug.Log(Boxes.Length);
        for (var j=0; j< Height; j++)
        {
            for (var i=0; i< Width; i++)
            {

                var box = rows[j].boxes[i];

                box.x_coord = i;
                box.y_coord = j;

                box.Icon = IconDb.Icons[Random.Range(0, IconDb.Icons.Length)];
                Boxes[i,j] = box;


            }
        }



        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
