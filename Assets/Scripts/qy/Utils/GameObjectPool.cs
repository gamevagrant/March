using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameObjectPool : MonoBehaviour {

	public Object target;
	
	List<Component> targetlist = new List<Component> ();
	int index = 0;


    public T getIdleTarget<T>() where T:Component
	{
		Component comp;
		for(int i = 0;i<targetlist.Count;i++)
		{
			comp = targetlist[index];
			if(!comp.gameObject.activeSelf)
			{
				index = i;
				comp.gameObject.SetActive(true);
				return comp as T;
			}
			index++;
			if(index >= targetlist.Count)
			{
				index = 0;
			}
		}

		comp = createNewTarget<T> () as Component;
        index = 0;
		return comp as T;
	}

	public List<T> getActiveTargets<T>() where T:Component
	{
		List<T> list = new List<T>();
		Component comp;
		for(int i = 0; i < targetlist.Count; i++)
		{
			comp = targetlist[i];
            if (comp.gameObject.activeSelf)
			{
				list.Add(comp as T);
			}
		}
		return list as List<T>;
	}

	T createNewTarget<T>()where T:Component
	{
		GameObject go = Instantiate (target) as GameObject;
		go.transform.SetParent (transform);
		go.transform.localPosition = Vector3.zero;
		go.transform.localRotation = Quaternion.identity;
		go.transform.localScale = Vector3.one;
		go.SetActive(true);
		T comp = go.transform.GetComponent<T> ();
		targetlist.Add (comp);
		return comp;
	}

	public void resetAllTarget()
	{

		index = 0;
		foreach(Component comp in targetlist)
		{
			comp.gameObject.SetActive(false);
		}
	}

	public void reposition()
	{

	}
}
