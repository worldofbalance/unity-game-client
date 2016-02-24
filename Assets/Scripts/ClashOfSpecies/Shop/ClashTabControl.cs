using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class ClashTabControlEntry {
	[SerializeField]
	private GameObject panel = null;
	public GameObject Panel { get { return panel; } }
	
	[SerializeField]
	private Button tab = null;
	public Button Tab { get { return tab; } }
}

public class ClashTabControl : MonoBehaviour {
	[SerializeField]
	private List<ClashTabControlEntry> entries = null;

	// Use this for initialization
	void Start () {
		foreach (ClashTabControlEntry entry in entries) {
			AddButtonListener(entry);
		}
		
		if (entries.Count > 0) {
			SelectTab(entries[0]);
		}
	}

	public void AddEntry(ClashTabControlEntry entry) {
		entries.Add(entry);
	}
	
	private void AddButtonListener(ClashTabControlEntry entry) {
		entry.Tab.onClick.AddListener(() => SelectTab(entry));
	}

	private void SelectTab(ClashTabControlEntry selectedEntry) {
		foreach (ClashTabControlEntry entry in entries) {
			bool isSelected = entry == selectedEntry;
			
			entry.Tab.interactable = !isSelected;
			entry.Panel.SetActive(isSelected);
		}
	}
}
