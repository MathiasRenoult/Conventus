using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
/// <summary>
/// This class it the one managing app files. It saves and loads components when needed.
/// </summary>
public class SaveManager : MonoBehaviour
{
    public static SaveManager singleton;
    public string path;

    void Awake()
    {
        singleton = this;
    }

    void Start()
    {
        path = Application.persistentDataPath + "/Components";
        if(!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
            AppManager.singleton.CreateBasicGates();
            print("Gates created !");
        }
        SaveComponents();
        LoadComponents();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.S) && AppManager.singleton.leftShift)
        {
            SaveComponents();
        }

        if(Input.GetKeyDown(KeyCode.L) && AppManager.singleton.leftShift)
        {
            LoadComponents();
        }
    }
    /// <summary>
    /// Takes a string corresponding to the old file we wante to modify and changes it with the new wanted name.
    /// </summary>
    /// <param name="oldFileName">Old file name, without extension</param>
    /// <param name="newFileName">New file name, without extension</param>
    public void ChangeFileName(string oldFileName, string newFileName)
    {
        foreach (string file in System.IO.Directory.GetFiles(path))
        {
            if(Path.GetFileNameWithoutExtension(file) == oldFileName)
            {
                oldFileName = file;
                newFileName = Directory.GetParent(oldFileName) + "/" + newFileName + ".json";
            }
        }
        print("old : " + oldFileName);
        print("new : " + newFileName);
        File.Move(oldFileName, newFileName);
        LoadComponents();
        print("File changed name !");
    }
    /// <summary>
    /// This function deletes a specified component in the files.
    /// </summary>
    /// <param name="fileName">Name of the component file to delete</param>
    public void DeleteFile(string fileName)
    {
        foreach (string file in System.IO.Directory.GetFiles(path))
        {
            if(Path.GetFileNameWithoutExtension(file) == fileName)
            {
                File.Delete(file);
                LoadComponents();
                print("File deleted !");
                break;
            }
        }
    }
    /// <summary>
    /// Deletes a specified component (in the code and the save file).
    /// </summary>
    /// <param name="component">Reference to the component to delete</param>
    public void DeleteComponent(Component component)
    {
        ComponentButtonManager.singleton.registeredComponents.Remove(component);
        Destroy(component.gameObject);
        DeleteFile(component.name);
    }
    /// <summary>
    /// Save current components to the HDD as json files.
    /// </summary>
    public void SaveComponents()
    {
        FileStream stream;
        string json = "";
        foreach(Component c in ComponentButtonManager.singleton.registeredComponents)
        {
            json = JsonUtility.ToJson(new ComponentData(c), true);
            stream = new FileStream(path + "/" + c.name + ".json", FileMode.OpenOrCreate);
            using(var file = new StreamWriter(stream))
            {
                file.Flush();
                file.Write(json);
            }
            stream.Close();
            print("Saved !");
        }
    }
    /// <summary>
    /// Loads all saved components into the RAM.
    /// </summary>
    public void LoadComponents()
    {
        if(ComponentButtonManager.singleton.registeredComponents != null)
        {
            ComponentButtonManager.singleton.registeredComponents.Clear();
        }
        
        foreach (Transform child in AppManager.singleton.transform) 
        {
            GameObject.Destroy(child.gameObject);
        }
        foreach (string file in System.IO.Directory.GetFiles(path))
        {
            string json = File.ReadAllText(file);
            ComponentButtonManager.singleton.registeredComponents.Add(AppManager.singleton.CreateComponent(JsonUtility.FromJson<ComponentData>(json), Path.GetFileNameWithoutExtension(file)));
            ComponentButtonManager.singleton.registeredComponents[ComponentButtonManager.singleton.registeredComponents.Count - 1].gameObject.SetActive(false);
            print("Loaded !");
        }

        ComponentMenuManager.singleton.InstantiateRegisteredComponents();

        ComponentButtonManager.singleton.RefreshButtons();
    }
}
