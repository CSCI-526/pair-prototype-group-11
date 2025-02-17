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
            }
        );
    }




}