using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
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
        }
        SaveManager.singleton.LoadComponents();
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