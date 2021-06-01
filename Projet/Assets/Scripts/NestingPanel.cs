using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NestingPanel : MonoBehaviour
{   
    public TMPro.TMP_InputField nameInputField;
    public TMPro.TMP_InputField colorInputField;
    public Toggle randomColorToggle;
    public void EnableDisablePanel()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public void CreateComponent()
    {
        bool correct = true;
        if(nameInputField.text == "")
        {
            ColorBlock colorBlock = new ColorBlock();
            colorBlock.normalColor = Color.red;
            nameInputField.colors = colorBlock;
            correct = false;
        }

        if(StringToColor(colorInputField.text) == new Color(-1f, -1f, -1f) && !randomColorToggle.isOn)
        {
            ColorBlock colorBlock = new ColorBlock();
            colorBlock.normalColor = Color.red;
            colorInputField.colors = colorBlock;
            correct  = false;
        }
        if(correct)
        {
            print("This is correct !");
            Component newComp = AppManager.singleton.CreateComponent(Component.Type.Custom, StringToColor(colorInputField.text), nameInputField.text, AppManager.singleton.inputs.Count, AppManager.singleton.outputs.Count, AppManager.singleton.ComputeCurrentTruthTable());
            newComp.gameObject.SetActive(false);
            ComponentButtonManager.singleton.registeredComponents.Add(newComp);
            print("New component added !");
        }

        SaveManager.singleton.SaveComponents();
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
