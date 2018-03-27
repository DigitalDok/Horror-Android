using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {

    public List<string> PoolOfItems = new List<string>();
    public List<Sprite> PoolOfSprites = new List<Sprite>();

    public List<string> CurrentItems = new List<string>();
    public List<Sprite> CurrentSprites = new List<Sprite>();

    internal List<GameObject> UI_Items = new List<GameObject>();

	void Start ()
    {
        foreach (var item in transform.Find("InventoryPanel").Find("Items").GetComponentsInChildren<Button>())
        {
           UI_Items.Add(item.gameObject);
        }
	}
	
	public void UpdateInventory()
    {
        for (int i = 0; i < UI_Items.Count; i++)
        {
            UI_Items[i].SetActive(false);
        }

        for (int i = 0; i < CurrentItems.Count; i++)
        {
            if (CurrentItems[i] != "")
            {
                UI_Items[i].SetActive(true);
                UI_Items[i].transform.GetChild(0).GetComponent<Image>().sprite = CurrentSprites[i];
                UI_Items[i].transform.GetChild(1).GetComponent<Text>().text = CurrentItems[i];
            }
            else
            {
                UI_Items[i].SetActive(false);
            }
        }
    }

    public void AddToInventory(string Name)
    {
        CurrentItems.Add(Name);
        CurrentSprites.Add(GetSpriteByName(Name));
    }

    private Sprite GetSpriteByName(string name)
    {
        return PoolOfSprites[PoolOfItems.IndexOf(name)];
    }

    public void RemoveFromInventory(string Name)
    {
        int IND = GetIndexByName(Name);
        if(IND!=-1)
        {
            CurrentItems.RemoveAt(IND);
            CurrentSprites.RemoveAt(IND);
        }
    }

    public int GetIndexByName(string Name)
    {
        return CurrentItems.IndexOf(Name);
    }
}
