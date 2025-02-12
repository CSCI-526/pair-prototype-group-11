using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class IconDb
{
    public static Icon[] Icons{get; private set;}

    [RuntimeInitializeOnLoadMethod( RuntimeInitializeLoadType.BeforeSceneLoad)]private static void Initialize()=> Icons = Resources.LoadAll<Icon>("Items/");
    

}
