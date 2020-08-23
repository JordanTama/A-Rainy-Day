using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : IGameService
{
    private InputControls input;

    public InputAction P_LeftClick { get; private set; }

    public InputManager()
    {
        input = new InputControls();

        input.Player.Enable();
    }
}
