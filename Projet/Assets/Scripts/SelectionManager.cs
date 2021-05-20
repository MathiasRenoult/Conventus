using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager singleton;
    public Vector2 mousePos;
    public Vector2 oldMousePos;
    public GameObject selectRectPrefab;
    public LineRenderer selectRectLine;
    public BoxCollider2D selectRectCollider;
    public BoxCollider2D canvasCollider;
    public bool selecting;
    public bool onTrash;
    public bool snapToGrid = true;
    public float gridSize = 0.1f;
    void Awake()
    {
        singleton = this;
    }
    // Update is called once per frame
    void LateUpdate() // LateUpdate cause it's more fluid for movements
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if(!WireTool.singleton.selectDot.gameObject.activeSelf)
        {
            if(Input.GetMouseButtonDown(0))
            {
                RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
                if(hit.collider != null && hit.collider.gameObject.GetComponent<Component>() != null)
                {
                    Component c = hit.collider.gameObject.GetComponent<Component>();
                    c.heldPoint = mousePos - (Vector2)c.transform.position;
                    AppManager.singleton.DeselectAllSelectedComponents();
                    AppManager.singleton.SelectComponent(c);
                }
            }
            if(Input.GetMouseButtonUp(0))
            {
                RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
                if(hit.collider != null && hit.collider.gameObject.GetComponent<Component>() != null && hit.collider.gameObject.GetComponent<Component>().held == true)
                {
                    Component c = hit.collider.gameObject.GetComponent<Component>();
                    c.heldPoint = mousePos - (Vector2)c.transform.position;
                    AppManager.singleton.DeselectAllSelectedComponents();
                    if(onTrash)
                    {
                        Destroy(c.gameObject);
                        print("Destroyed !");
                    }
                }
            }
            if(Input.GetMouseButton(0))
            {
                foreach(Component c in AppManager.singleton.selectedComponents)
                {
                    c.Move(SelectionManager.singleton.mousePos, snapToGrid);
                }
            }  
        }
    }

    public void SetOnTrash()
    {
        onTrash = !onTrash;
    }
}
