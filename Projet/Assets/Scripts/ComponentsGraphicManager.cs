using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VectorGraphics;

public class ComponentsGraphicManager : MonoBehaviour
{
    public enum LogicGatesStandard {ANSI, IEC, DIN};
    public static ComponentsGraphicManager singleton;
    public List<Sprite> spritesIEC = new List<Sprite>();
    public List<Sprite> spritesANSI = new List<Sprite>();
    public List<Sprite> spritesDIN = new List<Sprite>();
    public Sprite defaultComponent;
    public LogicGatesStandard standard;

    void Awake()
    {
        singleton = this;
    }
}
