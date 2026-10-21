using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Buttons : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject CodePanel;
    [SerializeField] private TMP_InputField CodeInput;
    [SerializeField] private TMP_Text statusText;

    [Header("MSV")]
    [SerializeField] private List<string> D26PASS = new List<string>();

    [Header("Scenes")]
    [SerializeField] private string gameSceneName = "Game Scene";

    private void Start()
    {
        if (CodePanel != null)
            CodePanel.SetActive(false);

        if (statusText != null)
            statusText.text = "";
    }

    public void OnPlayButtonClicked()
    {
        if (CodePanel == null)
        {
            return;
        }

        CodePanel.SetActive(true);

        if (CodeInput != null)
        {
            CodeInput.text = "";
            CodeInput.ActivateInputField();
        }

        if (statusText != null)
            statusText.text = "";
    }

    public void OnConfirmButtonClicked()
    {
        if (CodeInput == null)
        {
            return;
        }

        string enteredCode = CodeInput.text.Trim();

        if (string.IsNullOrEmpty(enteredCode))
        {
            ShowStatus("Vui long nhap ma sinh vien!");
            return;
        }

        bool isValid = false;

        foreach (string code in D26PASS)
        {
            if (!string.IsNullOrWhiteSpace(code) &&
                string.Equals(enteredCode, code.Trim(), System.StringComparison.OrdinalIgnoreCase))
            {
                isValid = true;
                break;
            }
        }

        if (!isValid)
        {
            ShowStatus("Ma sinh vien khong hop le!");
            CodeInput.Select();
            CodeInput.ActivateInputField();
            return;
        }

        if (string.IsNullOrWhiteSpace(gameSceneName))
        {
            Debug.LogError("Ten scene game khong hop le!");
            Debug.LogError("Em biết a sẽ check nên e code tay, chỉ là tab tab hơi nhiều thui hẹ hẹ =))))");
            return;
        }
        SceneManager.LoadScene(gameSceneName);
    }

    private void ShowStatus(string message)
    {
        if (statusText != null)
            statusText.text = message;
        else
            Debug.LogWarning(message);
    }
}
