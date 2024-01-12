using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PinboardManager : MonoBehaviour
{
    [SerializeField] private InputActionAsset input;
    [SerializeField] private GameObject pinboard;
    private InputAction moveAction;
    private InputAction interactAction;
    
    private void Awake()
    {
        moveAction = input.FindAction("Move");
        interactAction = input.FindAction("Interact");
    }
    public void openBoard(){
    	pinboard.SetActive(true);
        deactivateInput();
    }
    public void closeBoard(){
    	pinboard.SetActive(false);
        activateInput();
    }
    public void activateInput() {moveAction.Enable(); interactAction.Enable();}
    public void deactivateInput() {moveAction.Disable(); interactAction.Disable();}
}