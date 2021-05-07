using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentsGraphicManager : MonoBehaviour
{
    public static ComponentsGraphicManager singleton;
    public List<Sprite> spritesIEC = new List<Sprite>();
    public List<Sprite> spritesANSI = new List<Sprite>();

    void Awake()
    {
        singleton = this;
    }
}
