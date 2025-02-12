using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public sealed class Board : MonoBehaviour
{

    public static Board Instance {get; private set;}

    public Row[] rows;
    public Box[,] Boxes{get; private set;}
    

    public int no_of_rows => Boxes.GetLength(0);
    public int no_of_cols => Boxes.GetLength(1);

    



    private void Awake() => Instance = this;
    
    void Start()
    {
        Boxes = new Box[rows.Max(row => row.boxes.Length), rows.Length];
     
        for (var i=0; i< no_of_rows; i++)
        {
            for (var j=0; j< no_of_cols; j++)
            {

                var box = rows[i].boxes[j];

                box.x_coord = i;
                box.y_coord = j;

                box.Icon = IconDb.Icons[Random.Range(0, IconDb.Icons.Length)];
                Boxes[i,j] = box;


            }
        }



        
    }

    private Box _selectedBox1, _selectedBox2;
   /* public void SelectBox(Box box)
    {
        if (_selectedBox1 == null || _selectedBox2 == null) return;

    
        Debug.Log("Selected Box1: " + _selectedBox1.x_coord + " " + _selectedBox1.y_coord);
        Debug.Log("Selected Box2: " + _selectedBox2.x_coord + " " + _selectedBox2.y_coord);
        _selectedBox1.Clear();
        _selectedBox2.Clear();

    }*/

    public void Swap(Box box1, Box box2)
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
