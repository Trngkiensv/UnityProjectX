using UnityEngine;
using UnityEngine.InputSystem;

public class TableInteraction : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject tableUI;

    [Header("Player")]
    [SerializeField] private PlayerMovement8Way playerMovement;

    private bool playerInRange;
    private bool uiOpen;

    private void Update()
    {
        if (playerInRange && !uiOpen)
        {
            if (Keyboard.current != null &&
                Keyboard.current.eKey.wasPressedThisFrame)
            {
                OpenUI();
            }
        }

        if (uiOpen)
        {
            if (Keyboard.current != null &&
                Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                CloseUI();
            }
        }
    }

    private void OpenUI()
    {
        uiOpen = true;

        tableUI.SetActive(true);

        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void CloseUI()
    {
        uiOpen = false;

        tableUI.SetActive(false);

        if (playerMovement != null)
        {
            playerMovement.enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}