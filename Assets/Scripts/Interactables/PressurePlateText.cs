using System;
using TMPro;
using UnityEngine;

public class PressurePlateText : InteractableReceiver
{
    private TextMeshProUGUI _textMeshProUgui;
    private PressurePlate _pressurePlate;

    private void Awake()
    {
        _textMeshProUgui = GetComponent<TextMeshProUGUI>();
        if (interactableController is PressurePlate plate)
        {
            _pressurePlate = plate;
        }

        _pressurePlate.onCountChange += SetText;
        _pressurePlate.OnInteractableStateChange += ChangeState;

    }

    protected new void Start()
    {
        base.Start();
        SetText();
    }

    private void SetText()
    {
        if (interactableController is PressurePlate)
        {
            _textMeshProUgui.text = "" + _pressurePlate.currentCount + "/" + _pressurePlate.goalCount;
        }
    }

    protected override void ChangeState()
    {
        base.ChangeState();
        SetText();
    }

    private void OnDestroy()
    {
        _pressurePlate.onCountChange -= SetText;
        _pressurePlate.OnInteractableStateChange -= ChangeState;
    }
}
