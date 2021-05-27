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
    public Vector2 mousePos;
    public Vector2 oldMousePos;
    public bool rightBorder; // true if pointer is on right border
    public bool leftShift;
    void Awake()
    {
        singleton = this;
        Application.targetFrameRate = 100;
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
           oldMousePos = mousePos; 
        }
        
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if(!leftShift)
        {
            Camera.main.orthographicSize -= Input.mouseScrollDelta.y * (Camera.main.orthographicSize/10);
            UpdateCanvasIO();
        }

        if(Input.GetMouseButton(1))
        {
            Camera.main.transform.position += (Vector3)(oldMousePos - mousePos);
            UpdateCanvasIO();
        }

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
    public void UpdateCanvasIO()
    {
        int index = 0;
        foreach(CanvasIO c in inputs)
        {
            c.transform.position = new Vector2(leftBorderTransform.transform.position.x, leftBorderTransform.position.y - (leftBorderTransform.GetComponent<RectTransform>().rect.size.y/2) + (leftBorderTransform.GetComponent<RectTransform>().rect.size.y/(inputs.Count + 1))*(index+1));
            c.io.pos = new Vector2(c.transform.position.x + (leftBorderTransform.GetComponent<RectTransform>().rect.size.x/2), c.transform.position.y);
            c.io.pos = Camera.main.ScreenToWorldPoint(c.io.pos);
            c.UpdateColor();
            index++;
        }
        index = 0;
        foreach(CanvasIO c in outputs)
        {
            c.transform.position = new Vector2(rightBorderTransform.transform.position.x, rightBorderTransform.position.y - (rightBorderTransform.GetComponent<RectTransform>().rect.size.y/2) + (rightBorderTransform.GetComponent<RectTransform>().rect.size.y/(outputs.Count + 1))*(index+1));
            c.io.pos = new Vector2(c.transform.position.x - (rightBorderTransform.GetComponent<RectTransform>().rect.size.x/2), c.transform.position.y);
            c.io.pos = Camera.main.ScreenToWorldPoint(c.io.pos);
            c.UpdateColor();
            index++;
        }
        
        UpdateWires();
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
            c.io.pos = new Vector2(c.transform.position.x + (leftBorderTransform.GetComponent<RectTransform>().rect.size.x/2), c.transform.position.y);
            c.io.pos = Camera.main.ScreenToWorldPoint(c.io.pos);
            c.io.input = false;
            index++;
        }
    }
    public void DeleteInput()
    {
        CanvasIO toDelete = inputs[inputs.Count-1];
        toDelete.io.FlushIO();
        inputs.Remove(toDelete);
        Destroy(toDelete.gameObject);
        int index = 0;
        foreach(CanvasIO c in inputs)
        {
            c.transform.position = new Vector2(leftBorderTransform.transform.position.x, leftBorderTransform.position.y - (leftBorderTransform.GetComponent<RectTransform>().rect.size.y/2) + (leftBorderTransform.GetComponent<RectTransform>().rect.size.y/(inputs.Count + 1))*(index+1));
            c.io.pos = new Vector2(c.transform.position.x + (leftBorderTransform.GetComponent<RectTransform>().rect.size.x/2), c.transform.position.y);
            c.io.pos = Camera.main.ScreenToWorldPoint(c.io.pos);
            index++;
        }
    }
    public void AddOutput()
    {
        CanvasIO newOutput = Instantiate(inputOutputPrefab, rightBorderTransform).GetComponent<CanvasIO>();
        outputs.Add(newOutput);
        int index = 0;
        foreach(CanvasIO c in outputs)
        {
            c.transform.position = new Vector2(rightBorderTransform.transform.position.x, rightBorderTransform.position.y - (rightBorderTransform.GetComponent<RectTransform>().rect.size.y/2) + (rightBorderTransform.GetComponent<RectTransform>().rect.size.y/(outputs.Count + 1))*(index+1));
            c.io.pos = new Vector2(c.transform.position.x - (rightBorderTransform.GetComponent<RectTransform>().rect.size.x/2), c.transform.position.y);
            c.io.pos = Camera.main.ScreenToWorldPoint(c.io.pos);
            c.io.input = true;
            c.ChangeState();
            index++;
        }
    }
    public void DeleteOutput()
    {
        CanvasIO toDelete = outputs[outputs.Count-1];
        toDelete.io.FlushIO();
        outputs.Remove(toDelete);
        Destroy(toDelete.gameObject);
        int index = 0;
        foreach(CanvasIO c in outputs)
        {
            c.transform.position = new Vector2(rightBorderTransform.transform.position.x, rightBorderTransform.position.y - (rightBorderTransform.GetComponent<RectTransform>().rect.size.y/2) + (rightBorderTransform.GetComponent<RectTransform>().rect.size.y/(outputs.Count + 1))*(index+1));
            c.io.pos = new Vector2(c.transform.position.x - (rightBorderTransform.GetComponent<RectTransform>().rect.size.x/2), c.transform.position.y);
            c.io.pos = Camera.main.ScreenToWorldPoint(c.io.pos);
            index++;
        }
    }
    public void UpdateWires()
    {
        foreach(Wire w in wires)
        {
            w.UpdatePositions();
            w.UpdateSize();
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
