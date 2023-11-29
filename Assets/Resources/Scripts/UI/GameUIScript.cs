using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameUIScript : MonoBehaviour
{
    private EventSystem eventSystem;

    [SerializeField] private GameObject Menu;
    [SerializeField] private GameObject sousaUI;
    [SerializeField] private Button MenuButton;
    // Start is called before the first frame update
    void Start()
    {
        Menu.SetActive(false);
        sousaUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MenuUI()
    {
        Menu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(MenuButton.gameObject);
    }
    public void MenuButtonDown()
    {
        sousaUI.SetActive(true);
    }
}
