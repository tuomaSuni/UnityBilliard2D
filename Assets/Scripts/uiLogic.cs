using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uiLogic : MonoBehaviour
{
    [SerializeField] private uiManager uim;
    [SerializeField] private GameObject settings;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !settings.activeSelf) uim.ActivateMenuPanel();
    }
}
