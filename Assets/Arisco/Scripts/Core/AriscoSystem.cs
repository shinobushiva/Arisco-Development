using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public sealed class AriscoSystem : SingletonMonoBehaviour<AriscoSystem> {

    private static Dictionary<GameObject, Dictionary<Type, AComponent>> componentMap = new Dictionary<GameObject, Dictionary<Type, AComponent>>(); 
    private static Dictionary<Type, List<AComponent>> typeMap = new Dictionary<Type, List<AComponent>>();

    public static void Initialize(){
        print("AriscoSystem#Initialize");
        componentMap.Clear();
        mapCount = 1;
    }

    public static void Map(AAgent ag){
        GameObject go = ag.gameObject;

        int mask = 0x0; 
        AComponent[] acs = go.GetComponents<AComponent>();
        foreach (AComponent ac in acs)
        {
            Map(ag, ac);
            Type t = ac.GetType();

        }
        //r.drag = mask;
    }

    private static int mapCount;

    private static void Map(AAgent ag, AComponent comp){
        
        GameObject go = ag.gameObject;

        Dictionary<Type, AComponent> list = null;
        if (componentMap.ContainsKey(go))
        {
            list = componentMap [go];
        } else
        {
            list = new Dictionary<Type, AComponent>();
            componentMap.Add(go, list);
        }
        if(list.ContainsKey(comp.GetType())){
            print(comp.gameObject);
        }
        list.Add(comp.GetType(), comp);

        List<AComponent> compList;
        Type t = comp.GetType();
        if (typeMap.ContainsKey(t))
        {
            compList = typeMap [t];
        } else
        {
            compList = new List<AComponent>();
            typeMap.Add(t, compList);
        }
        compList.Add(comp);

    }

    public static void UnMap(GameObject go){
        componentMap.Remove(go);

        AComponent[] acs = go.GetComponents<AComponent>();
        foreach (AComponent ac in acs)
        {
            Type t = ac.GetType();
            if (typeMap.ContainsKey(t))
            {
                typeMap[t].Remove(ac);
            }
        }
    }

    public static List<AComponent> GetAComponents<T>(){
        if (typeMap.ContainsKey(typeof(T)))
        {
            //return typeMap[typeof(T)].Cast<T>().ToList();
            List<AComponent> acs = typeMap[typeof(T)];
     
            return acs;
        }
        else{
            return new List<AComponent>();
        }
    }

    public static AComponent GetAComponent(GameObject go , Type t){
        if (!componentMap.ContainsKey(go))
        {
            if (! go.GetComponent<AAgent>().initialized)
            {
                return null;
            }
            print(go.name);
            print(""+Time.time+":"+go.GetInstanceID());
            print(""+go.GetComponent<AAgent>().initialized);
            go.name = "Not Init";
            Debug.Break();
        }
        if (componentMap [go].ContainsKey(t))
        {
            return componentMap [go] [t];
        } else
        {
            return null;
        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
}
