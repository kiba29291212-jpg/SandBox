using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using static UnityEngine.Debug;

public class OpenChest : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private Camera playerCamera;

    [Header("Tuong tac")]
    [SerializeField] private float interactDistance = 4f;
    [SerializeField] private LayerMask chestLayer;

    [Header("D26 Pass")]
    [SerializeField] private List<string> D26PASS = new List<string>();

    [Header("Scenes")]
    [SerializeField] private string passSceneName = "Pass";
    [SerializeField] private string failSceneName = "NoPass";

    private PlayerInputActions input;

    private void Awake()
    {
        input = new PlayerInputActions();
    }

    private void OnEnable()
    {
        if (input == null)
            input = new PlayerInputActions();

        input.Player.Interact.performed += OnInteract;
        input.Player.Enable();
    }

    private void OnDisable()
    {
        if (input == null)
            return;

        input.Player.Interact.performed -= OnInteract;
        input.Player.Disable();
    }

    private void OnDestroy()
    {
        input?.Dispose();
        input = null;
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        if (playerCamera == null)
        {
            return;
        }

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance))
        {
            OpenChest chest = hit.collider.GetComponentInParent<OpenChest>();

            if (chest == this)
            {
                CheckMSV();
            }
        }

    }

    private void CheckMSV()
    {
        bool hasPass = D26PASS.Exists(code => !string.IsNullOrWhiteSpace(code) && code.Trim() == D26PassGCC.D26MSV.Trim());

        if (hasPass)
        {
            SceneManager.LoadScene(passSceneName);
        }
        else
        {
            SceneManager.LoadScene(failSceneName);
        }
    }
}
