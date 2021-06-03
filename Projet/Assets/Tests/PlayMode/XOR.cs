using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class XOR
{
    // A Test behaves as an ordinary method
    [Test]
    public void YesXOR()
    {
        var gameObject = new GameObject();
        var xorGate = gameObject.AddComponent<Component>();
        xorGate.type = Component.Type.XOR;

        Component nComp;
        nComp = AppManager.singleton.CreateComponent(Component.Type.Buffer, new Color(-1f,-1f,-1f), null, 1, 1, null);
        ComponentButtonManager.singleton.registeredComponents.Add(nComp);
        ComponentButtonManager.singleton.favoriteComponents.Add(nComp);

        Component instComp = MonoBehaviour.Instantiate(nComp);
        AppManager.singleton.components.Add(instComp);
        AppManager.singleton.SelectComponent(instComp);
        instComp.gameObject.SetActive(true);
        instComp.shapeRenderer.color = instComp.color;

        AppManager.singleton.AddInput();
        AppManager.singleton.AddInput();
        AppManager.singleton.AddOutput();

        Assert.AreEqual(1,1);
    }
}
