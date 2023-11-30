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
    [SerializeField] private Button SousaButton;
    [SerializeField] private GameObject MenuPanel;
    // Start is called before the first frame update
    void Start()
    {
        Menu.SetActive(false);
        sousaUI.SetActive(false);
        MenuPanel.SetActive(false);
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
        MenuPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(SousaButton.gameObject);
    }

    public void SousaButtonDown()
    {
        sousaUI.SetActive(true);
        MenuPanel.SetActive(false);
    }
}
