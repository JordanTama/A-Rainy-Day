using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitButton : MonoBehaviour
{
    private MainUIController _mainUiController;
    
    private void Awake()
    {
        _mainUiController = GetComponentInParent<MainUIController>();
    }
    
    public void Quit()
    {
        _mainUiController.PlayMenuButtonAudio();
        Application.Quit();
    }
}
