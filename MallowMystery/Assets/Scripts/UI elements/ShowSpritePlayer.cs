using System;
using System.Collections;
using System.Collections.Generic;
using ExampleEventScriptAble;
using ScriptObjects;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShowSpritePlayer : MonoBehaviour {

    [SerializeField] private GameObject playerCanvas;
    [SerializeField] private GameObject sprite;
    [SerializeField] private GameEventChannel interactListener;
    private bool showSprite = false;
    private Camera mainCam;

    private void OnEnable() {
        mainCam = Camera.main;
    }

    public void setSpriteRotation() {
        if (!SceneManager.GetActiveScene().name.Equals("DetectiveRoom")) {
            playerCanvas.transform.rotation = Quaternion.LookRotation(playerCanvas.transform.position - Camera.main.transform.position);
        }
    }

    private void showInteractableSprite() {
        showSprite = !showSprite;
        playerCanvas.SetActive(showSprite);
    }

    public void Update() {
        if ((interactListener.GetListenersList().Count != 0 && !showSprite) || (interactListener.GetListenersList().Count == 0 && showSprite)) {
            setSpriteRotation();
            showInteractableSprite();
        }

        if (showSprite) {
            Vector3 loc = new Vector3(transform.position.x, transform.position.y + 4, transform.position.z);
            sprite.transform.position = mainCam.WorldToScreenPoint(loc);
        }
    }
}
