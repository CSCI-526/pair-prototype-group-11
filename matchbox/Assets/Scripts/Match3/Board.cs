using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;

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

                //Debug.Log("Box: " + i + " " + j);

                do
                { int index = Random.Range(0, IconDb.Icons.Length);
                
                   box.Icon = IconDb.Icons[index];
                   box.Color = index.ToString();
                } while (HasImmediateMatch(i, j, box.Icon));

                Boxes[i,j] = box;


            }
        }

        
    }

    private bool HasImmediateMatch(int x, int y, Icon icon)
    {
        return CheckMatch(x, y, icon, 1, 0) || // check horizontal
            CheckMatch(x, y, icon, 0, 1);  // check vertical
    }

    private int CountMatches(int x, int y, Icon icon, int dx, int dy, int count = 0)
    {
        int curr_x = x + dx;
        int curr_y = y + dy;

        if (curr_x < 0 || curr_x >= no_of_rows || curr_y < 0 || curr_y >= no_of_cols || Boxes[curr_x, curr_y]?.Icon != icon)// end search if any are true
            return count;
        
        return CountMatches(curr_x, curr_y, icon, dx, dy, count + 1); //recursively search for matches
    }

    private bool CheckMatch(int x, int y, Icon icon, int dx, int dy)
    {
      
        int forward = CountMatches(x, y, icon, dx, dy);
        int backward = CountMatches(x, y, icon, -dx, -dy);

        //including the current box
        int totalCount = backward + forward + 1;

        return totalCount >= 3; //returns true if there are 3 or more matches
        //total count can be used for powerup if needed
    }


    private Box _selectedBox1, _selectedBox2, tempBox2;
    public async void SelectBox(Box box)
    {
        if (_selectedBox1 == null && _selectedBox1!= box)
        {
            _selectedBox1= box;
            return;
        }

        // checking the box is already selected
        if (_selectedBox2 == null && box!= _selectedBox1 )
        {
            tempBox2 = box;
            //checking neighbouring condition of box
            int x_diff = Mathf.Abs(_selectedBox1.x_coord - tempBox2.x_coord);
            int y_diff = Mathf.Abs(_selectedBox1.y_coord - tempBox2.y_coord);

            if (x_diff + y_diff != 1)
            {
                _selectedBox1 = tempBox2;
                return;
            }
            
            _selectedBox2= box;
           
        }


        if (_selectedBox1 == null || _selectedBox2 == null) return;

    
        Debug.Log("Box1: " + _selectedBox1.x_coord + " " + _selectedBox1.y_coord 
        + " Box2: " + _selectedBox2.x_coord + " " + _selectedBox2.y_coord);
     
        await SwapBoxes(_selectedBox1, _selectedBox2);
        _selectedBox1 = _selectedBox2 = null;
       
    }


    public async Task SwapBoxes(Box box1, Box box2)
{

    BoxSwapMechanic(box1, box2); // initial swap
    MatchingAlgo.Index Index;
    Index.X = box1.x_coord;
    Index.Y = box1.y_coord;
    List<MatchingAlgo.Index> Indexes = new List<MatchingAlgo.Index>();
    List<IMatchInterface> Boxes1 = new List<IMatchInterface>();

    bool box1_Match= MatchingAlgo.CheckMatch(Index, Board.Instance.Boxes, ref Indexes, ref Boxes1);

    MatchingAlgo.Index Index2;
    Index2.X = box2.x_coord;
    Index2.Y = box2.y_coord;
    List<MatchingAlgo.Index> Indexes2 = new List<MatchingAlgo.Index>();
    List<IMatchInterface> Boxes2 = new List<IMatchInterface>();
    bool box2_Match= MatchingAlgo.CheckMatch(Index2, Board.Instance.Boxes, ref Indexes2, ref Boxes2);
    Debug.Log("Box1 Match: " + box1_Match + " Box2 Match: " + box2_Match);
    
    if (!box1_Match && !box2_Match)
    {
        await Task.Delay(500);
        await BoxSwapMechanic(box1, box2);// swaps back if no match
    }

    if (Indexes.Count >= 3 || Indexes2.Count >= 3)
    {
        await Task.Delay(500);
        PopulateDestroyedBoxes(Indexes, Indexes2);
    }




    await Task.Delay(10); 
}

    public async Task BoxSwapMechanic(Box box1, Box box2)
    {
    
    
    var arrow1 = box1.arrow;
    var arrow2 = box2.arrow;

    box1.arrow = arrow2;
    box2.arrow = arrow1;

    var arrow1Transform = arrow1.transform;
    var arrow2Transform = arrow2.transform;

  
    var tempPosition = arrow1Transform.position;
    arrow1Transform.position = arrow2Transform.position;
    arrow2Transform.position = tempPosition;

    arrow1Transform.SetParent(box2.transform);
    arrow2Transform.SetParent(box1.transform);

    var tempColor = box1.Color;
    box1.Color = box2.Color;
    box2.Color = tempColor;


    var tempIcon = box1.Icon;
    box1.Icon = box2.Icon;
    box2.Icon = tempIcon;


    Debug.Log("Swapped Boxes: " + box1.x_coord + " " + box1.y_coord +" color"+ box1.Color + " " + box2.x_coord + " " + box2.y_coord + "color"+ box2.Color);
    




    }




public async void PopulateDestroyedBoxes(List<MatchingAlgo.Index> Indexes1, List<MatchingAlgo.Index> Indexes2)
{
    

    var allIndexes= Indexes1.Concat(Indexes2).Distinct().ToList();
    allIndexes = allIndexes.OrderByDescending(index => index.X).ToList();

    foreach (var index in allIndexes)
    {
        int x = index.X;
        int y = index.Y;

       
        Boxes[x, y].Color= null;
    }

    for (int y = 0; y < no_of_cols; y++)
    {
        bool checkFlag= true;
        
        while(checkFlag){
            checkFlag= false;
            for (int x = no_of_rows - 1; x >= 0; x--)
            {
                if (Boxes[x, y].Color == null)
                {
                    // Shift boxes  down
                    for (int k = x; k > 0; k--)
                    {
                        if (Boxes[k - 1, y] != null)
                        {
                            
                            await BoxSwapMechanic(rows[k].boxes[y], rows[k - 1].boxes[y]);
            
                            checkFlag= true;
                        }
                    }
                    // Generate a new box at the top
                    Boxes[0, y] = GenerateNewBox(0, y);
                  
                    
                    
                }
            }
        }
    }
}

private Box GenerateNewBox(int i, int j)
{

    var newBox = rows[i].boxes[j];

    newBox.x_coord = i;
    newBox.y_coord = j;

    Debug.Log("Created New Box: " + i + " " + j);

    int index = Random.Range(0, IconDb.Icons.Length);
    
    newBox.Icon = IconDb.Icons[index];
    newBox.Color = index.ToString();

    return newBox;
}


   


    void Update()
    {
        
    }
}