using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// This class is used on items we see in the component menu. Each item has his own script.
/// </summary>
public class ComponentMenuItem : MonoBehaviour
{
    public Button editButton;
    public Button deleteButton;
    public TMPro.TextMeshProUGUI inputText;
    public TMPro.TextMeshProUGUI outputText;
    public TMPro.TextMeshProUGUI nameText;
    public Color color;
    public Image background;
    public Component linkedComponent;
    /// <summary>
    /// Instantiates the component referenced in the "linkedComponent" attribute.
    /// </summary>
    public void InstantiateComponent()
    {
        Component instComp = Instantiate(linkedComponent);
        instComp.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width/2, Screen.height/2, 0f));
        AppManager.singleton.components.Add(instComp);
        instComp.gameObject.SetActive(true);
        instComp.Move(instComp.transform.position, true);
    }
    /// <summary>
    /// Closes the component menu.
    /// </summary>
    public void ClosePanel()
    {
        ComponentMenuManager.singleton.componentMenuPanel.gameObject.SetActive(false);
    } 
    /// <summary>
    /// Triggered by the "cross button" on the item. Deletes the item and the associated component in the app files.
    /// </summary>
    public void DeleteItem()
    {
        SaveManager.singleton.DeleteComponent(linkedComponent);
    }
    /// <summary>
    /// Triggered by the "edit button" on the item. Opens an edit window allowing to change the name and the color of
    /// the saves component.
    /// </summary>
    public void EditItem()
    {
        EditPanel.singleton.gameObject.SetActive(!EditPanel.singleton.gameObject.activeSelf);
        EditPanel.singleton.currentlyEditedComponent = linkedComponent;
    }
}
