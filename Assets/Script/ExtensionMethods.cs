using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{
    // GameObject擴充  GetComponentsInChildren不包含自己
    public static List<T> GetCompsInChildrenNoRoot<T>(this GameObject obj) where T : Component {
        List<T> tList = new List<T>();
        var all = obj.GetComponentsInChildren<T>();
        foreach (T comp in all) {
            if (comp.gameObject.GetInstanceID() != obj.GetInstanceID()) {
                tList.Add(comp);
            }
		}
        //foreach (Transform child in obj.transform.root) {
        //    T[] scripts = child.GetComponentsInChildren<T>();
        //    if (scripts != null) {
        //        foreach (T sc in scripts)
        //            tList.Add(sc);
        //    }
        //}
        return tList;
    }
}
