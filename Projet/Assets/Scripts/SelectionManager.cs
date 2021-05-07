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
    public bool selecting;
    public bool snapToGrid = true;
    public float gridSize = 0.1f;
    void Awake()
    {
        singleton = this;
    }
    // Update is called once per frame
    void LateUpdate()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if(Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
            if(hit.collider != null && hit.collider.gameObject.GetComponent<Component>() != null)
            {
                Component c = hit.collider.gameObject.GetComponent<Component>();
                c.heldPoint = mousePos - (Vector2)c.transform.position;
                AppManager.singleton.SelectComponent(c);
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
