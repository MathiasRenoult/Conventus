using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Manages saved components and display them in a scrollable panel.
/// </summary>
public class ComponentMenuManager : MonoBehaviour
{
   public static ComponentMenuManager singleton;
   public GameObject componentPrefab;
   public Transform scrollListTransform;
   public Transform componentMenuButton;
   public Transform componentMenuPanel;
   public List<GameObject> registeredComponents;

   void Awake()
   {
       singleton = this;
   }
   void Start()
   {
       componentMenuPanel.gameObject.SetActive(false);
   }

    void Update()
    {
        if(componentMenuPanel.gameObject.activeSelf)
        {
            componentMenuPanel.transform.position = new Vector3(componentMenuButton.transform.position.x, componentMenuPanel.transform.position.y, 0f);
        }
    }
    /// <summary>
    /// Destroys every element in the component list.
    /// </summary>
    void DestroyList()
    {
        List<GameObject> toDestroy = new List<GameObject>();
        foreach(GameObject g in registeredComponents)
        {
            toDestroy.Add(g);
        }
        registeredComponents.Clear();
        foreach(GameObject g in toDestroy)
        {
            Destroy(g.gameObject);
        }
    }
    /// <summary>
    /// Takes every basic logic gate and creates a shortcut button on the interface for it.
    /// </summary>
    public void InstantiateRegisteredComponents()
    {
        DestroyList();
        ComponentButtonManager.singleton.favoriteComponents.Clear();
        foreach(Component c in ComponentButtonManager.singleton.registeredComponents)
        {
            if(c.type == Component.Type.Custom)
            {
                ComponentMenuItem newItem = Instantiate(componentPrefab, scrollListTransform).GetComponent<ComponentMenuItem>();
                newItem.nameText.text = c.name;
                newItem.color = Color.HSVToRGB((Mathf.Abs(c.name.GetHashCode())%512)/512f, 0.9f, 0.7f);
                newItem.background.color = newItem.color;
                newItem.inputText.text = c.CountInputsOutputs().x.ToString();
                newItem.outputText.text = c.CountInputsOutputs().y.ToString();
                newItem.linkedComponent = c;
                registeredComponents.Add(newItem.gameObject);
            }
            else
            {
                ComponentButtonManager.singleton.favoriteComponents.Add(c);
            }
        }
    }
}
