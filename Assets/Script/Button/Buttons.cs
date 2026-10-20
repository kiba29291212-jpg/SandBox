using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Buttons : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject studentCodePanel;
    [SerializeField] private TMP_InputField studentCodeInput;
    [SerializeField] private TMP_Text statusText;

    [Header("MSV")]
    [SerializeField] private List<string> validStudentCodes = new List<string>();

    [Header("Scenes")]
    [SerializeField] private string gameSceneName = "Game Scene";

    private void Start()
    {
        if (studentCodePanel != null)
            studentCodePanel.SetActive(false);

        if (statusText != null)
            statusText.text = "";
    }

    public void OnPlayButtonClicked()
    {
        if (studentCodePanel == null)
        {
            return;
        }

        studentCodePanel.SetActive(true);

        if (studentCodeInput != null)
        {
            studentCodeInput.text = "";
            studentCodeInput.ActivateInputField();
        }

        if (statusText != null)
            statusText.text = "";
    }

    public void OnConfirmButtonClicked()
    {
        if (studentCodeInput == null)
        {
            return;
        }

        string enteredCode = studentCodeInput.text.Trim();

        if (string.IsNullOrEmpty(enteredCode))
        {
            ShowStatus("Vui long nhap ma sinh vien!");
            return;
        }

        bool isValid = false;

        foreach (string code in validStudentCodes)
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
            studentCodeInput.Select();
            studentCodeInput.ActivateInputField();
            return;
        }

        if (string.IsNullOrWhiteSpace(gameSceneName))
        {
            Debug.LogError("Ten scene game khong hop le!");
            Debug.LogError("Em biết a sẽ check nên e code tay, chỉ là tab hơi nhiều thui hẹ hẹ =))))");
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
