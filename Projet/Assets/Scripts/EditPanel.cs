using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditPanel : MonoBehaviour
{
    public static EditPanel singleton;
    public Transform editPanelTransform;
    public Component currentlyEditedComponent;
    public TMPro.TMP_InputField nameInput;
    public TMPro.TMP_InputField colorInput;
    public Color chosenColor;
    public UnityEngine.UI.Toggle toggle;

    void Awake()
    {
        singleton = this;
        gameObject.SetActive(false);
    }
    public void Cancel()
    {
        currentlyEditedComponent = null;
        gameObject.SetActive(false);
    }
    public void Validate()
    {
        bool correct = true;
        if(nameInput.text == "")
        {
            ColorBlock colorBlock = new ColorBlock();
            colorBlock.normalColor = Color.red;
            nameInput.colors = colorBlock;
            correct = false;
        }

        if(StringToColor(colorInput.text) == new Color(-1f, -1f, -1f) && !toggle.isOn)
        {
            ColorBlock colorBlock = new ColorBlock();
            colorBlock.normalColor = Color.red;
            colorInput.colors = colorBlock;
            chosenColor = StringToColor(colorInput.text);
            correct  = false;
        }

        if(correct && toggle.isOn)
        {
            chosenColor = Color.HSVToRGB((Mathf.Abs(nameInput.text.GetHashCode())%512)/512f, 0.9f, 0.7f);
        }

        if(correct)
        {
            Component toModify = null;
            foreach(Component c in ComponentButtonManager.singleton.registeredComponents)
            {
                if(c == currentlyEditedComponent)
                {
                    toModify = c;
                    break;
                }
            }
            SaveManager.singleton.ChangeFileName(toModify.compName, nameInput.text);
            toModify.compName = toModify.name = nameInput.text;
            toModify.color = chosenColor;
            SaveManager.singleton.SaveComponents();
            SaveManager.singleton.LoadComponents();
            print("Component modified !");
        }
    }

    public Color StringToColor(string colorString)
    {
        Color colorColor;
        bool correct = ColorUtility.TryParseHtmlString(colorString, out colorColor);;
        if(correct)
        {
            return colorColor;
        }
        else
        {
            return new Color(-1f, -1f, -1f);
        }
    }
}
