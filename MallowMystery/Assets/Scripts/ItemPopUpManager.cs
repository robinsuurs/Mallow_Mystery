﻿using System;
using ScriptObjects;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace {
    public class ItemPopUpManager : MonoBehaviour {
        [SerializeField] private GameObject ItemPopUpScreen;
        private bool popUpShowing = false;
        
        public void showPopUp(ItemData itemData) {
            Time.timeScale = 0;
            popUpShowing = true;
            GameObject itemHolder = ItemPopUpScreen.transform.Find("ItemHolder").gameObject;
            itemHolder.transform.Find("ItemImage").gameObject.GetComponent<Image>().sprite = itemData.icon;
            itemHolder.transform.Find("ItemName").gameObject.GetComponent<TextMeshProUGUI>().text = itemData.itemName;
            itemHolder.transform.Find("ItemDescription").gameObject.GetComponent<TextMeshProUGUI>().text = itemData.description;
            ItemPopUpScreen.SetActive(true);
        }

        private void Update() {
            if (Input.GetMouseButtonDown(0) && popUpShowing) {
                ItemPopUpScreen.SetActive(false);
                Time.timeScale = 1;
                popUpShowing = false;
            }
        }
    }
}