using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppManager : MonoBehaviour
{
    public static AppManager singleton;
    public GameObject componentPrefab;
    public List<Component> components = new List<Component>();
    public List<Component> selectedComponents = new List<Component>();
    public List<Wire> wires = new List<Wire>();
    [SerializeField]
    public GameObject inputOutputPrefab;
    public GameObject leftContainer;
    public Transform leftBorderTransform;
    public List<CanvasIO> inputs = new List<CanvasIO>();
    public bool leftBorder; // true if pointer is on the left border
    [SerializeField]
    public GameObject rightContainer;
    public Transform rightBorderTransform;
    public List<CanvasIO> outputs = new List<CanvasIO>();
    public bool rightBorder; // true if pointer is on right border
    public bool leftShift;
    void Awake()
    {
        singleton = this;
        Application.targetFrameRate = 100;
    }

    void Update()
    {
        if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            leftShift = false;
        }

        if(Input.anyKey)
        {
            if(Input.GetKeyDown(KeyCode.LeftShift))
            {
                leftShift = true;
            }

            if(Input.GetKeyDown(KeyCode.H))
            {
                switch(Random.Range(0,3))
                {
                    case 0: ComponentsGraphicManager.singleton.standard = ComponentsGraphicManager.LogicGatesStandard.ANSI; break;
                    case 1: ComponentsGraphicManager.singleton.standard = ComponentsGraphicManager.LogicGatesStandard.IEC; break;
                    default: ComponentsGraphicManager.singleton.standard = ComponentsGraphicManager.LogicGatesStandard.DIN; break;
                }
                foreach(Component c in components)
                {
                    c.SetGateProperties();
                    c.UpdateCompGraphics();
                }
            }
        }
    }
    public void SetOnOffLeftBorder()
    {
        leftBorder = !leftBorder;
    }
    public void SetOnOffRightBorder()
    {
        rightBorder = !rightBorder;
    }
    public void AddInput()
    {
        CanvasIO newInput = Instantiate(inputOutputPrefab, leftBorderTransform).GetComponent<CanvasIO>();
        inputs.Add(newInput);
        int index = 0;
        foreach(CanvasIO c in inputs)
        {
            c.transform.position = new Vector2(leftBorderTransform.transform.position.x, leftBorderTransform.position.y - (leftBorderTransform.GetComponent<RectTransform>().rect.size.y/2) + (leftBorderTransform.GetComponent<RectTransform>().rect.size.y/(inputs.Count + 1))*(index+1));
            c.io.pos = new Vector2(leftBorderTransform.transform.position.x + newInput.GetComponent<RectTransform>().rect.width/2, leftBorderTransform.position.y - (leftBorderTransform.GetComponent<RectTransform>().rect.size.y/2) + (leftBorderTransform.GetComponent<RectTransform>().rect.size.y/(inputs.Count + 1))*(index+1));
            index++;
        }
    }
    public void DeleteInput()
    {

    }

    public void UpdateWires()
    {
        foreach(Wire w in wires)
        {
            w.UpdatePositions();
        }
    }
    public Component CreateComponent(Component.Type type)
    {
        Component newComp = Instantiate(componentPrefab, this.transform).GetComponent<Component>();
        newComp.type = type;
        newComp.name = newComp.type.ToString();
        components.Add(newComp);
        return newComp;
    }
    public void SelectComponent(Component c)
    {
        c.held = !c.held;
        if(c.held)
        {
            selectedComponents.Add(c);
        }
        else
        {
            selectedComponents.Remove(c);
        }
    }

    public void DeselectAllSelectedComponents()
    {
        foreach(Component c in selectedComponents)
        {
            c.held = false;
        }
        selectedComponents.Clear();
    }
}
