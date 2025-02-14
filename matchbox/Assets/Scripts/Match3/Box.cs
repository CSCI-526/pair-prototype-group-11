using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class Box : MonoBehaviour, IMatchInterface
{
    public int x_coord;
    public int y_coord;
   

    public string Color { get; set; }
    
    private Icon _icon;
    public Icon Icon
    {
        get => _icon;
        set
        {
            if(_icon==value) return;
            _icon = value;
            arrow.sprite = _icon.sprite;
        }
    }
    public Button button;
    public Image arrow;

   void Start()
    {
        button.onClick.AddListener(() =>
            {

                Board.Instance.SelectBox(this);
               

                MatchingAlgo.Index Index;
                Index.X = x_coord;
                Index.Y = y_coord;
                List<MatchingAlgo.Index> Indexes = new List<MatchingAlgo.Index>();
                List<IMatchInterface> Boxes = new List<IMatchInterface>();

                Debug.Log(MatchingAlgo.CheckMatch(Index, Board.Instance.Boxes, ref Indexes, ref Boxes));

            }
        );
    }




}
