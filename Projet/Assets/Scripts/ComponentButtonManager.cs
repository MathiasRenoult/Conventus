using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class ComponentButtonManager : MonoBehaviour
{
    public Button createAndSaveButton;
    public Button componentMenuButton;
    public GameObject componentMenuPanel;
    public int itemLimit = 12;
    public float spacing = 120f;
    public GameObject componentPrefab;
    public List<ComponentButton> buttons = new List<ComponentButton>();
    public List<Component> registeredComponents;
    public List<Component> favoriteComponents;


    void Start()
    {
        Component newComp = AppManager.singleton.CreateComponent(Component.Type.Buffer);
        registeredComponents.Add(newComp);
        newComp.gameObject.SetActive(false);
        favoriteComponents.Add(newComp);
        newComp = AppManager.singleton.CreateComponent(Component.Type.NOT);
        registeredComponents.Add(newComp);
        newComp.gameObject.SetActive(false);
        favoriteComponents.Add(newComp);
        newComp = AppManager.singleton.CreateComponent(Component.Type.AND);
        registeredComponents.Add(newComp);
        newComp.gameObject.SetActive(false);
        favoriteComponents.Add(newComp);
        newComp = AppManager.singleton.CreateComponent(Component.Type.OR);
        registeredComponents.Add(newComp);
        newComp.gameObject.SetActive(false);
        favoriteComponents.Add(newComp);
        newComp = AppManager.singleton.CreateComponent(Component.Type.XOR);
        registeredComponents.Add(newComp);
        newComp.gameObject.SetActive(false);
        favoriteComponents.Add(newComp);
        newComp = AppManager.singleton.CreateComponent(Component.Type.NAND);
        registeredComponents.Add(newComp);
        newComp.gameObject.SetActive(false);
        favoriteComponents.Add(newComp);
        newComp = AppManager.singleton.CreateComponent(Component.Type.NOR);
        registeredComponents.Add(newComp);
        newComp.gameObject.SetActive(false);
        favoriteComponents.Add(newComp);
        newComp = AppManager.singleton.CreateComponent(Component.Type.XNOR);
        registeredComponents.Add(newComp);
        newComp.gameObject.SetActive(false);
        favoriteComponents.Add(newComp);

        for(int i = 0; i<favoriteComponents.Count && i<itemLimit;i++)
        {
            ComponentButton newButt = Instantiate(componentPrefab, this.transform).GetComponent<ComponentButton>();
            newButt.transform.position = new Vector3(this.transform.position.x - (this.GetComponent<RectTransform>().sizeDelta.x/2) + (i * spacing), this.transform.transform.position.y, 0f);            
            newButt.linkedComponent = favoriteComponents[i];
            newButt.SetText(favoriteComponents[i].compName);
            newButt.SetColor(Color.red);
        }

        componentMenuButton.transform.position = new Vector3(this.transform.position.x - (this.GetComponent<RectTransform>().sizeDelta.x/2) + (favoriteComponents.Count * spacing), this.transform.transform.position.y, 0f); 
    }

    public void RefreshButtons()
    {
        foreach(ComponentButton c in buttons)
        {
            c.SetText(c.linkedComponent.type.ToString());
            c.SetColor(Color.red);
        }
    }

    public void OpenCloseComponentMenu()
    {
        componentMenuPanel.SetActive(!componentMenuPanel.activeSelf);
    }
}
