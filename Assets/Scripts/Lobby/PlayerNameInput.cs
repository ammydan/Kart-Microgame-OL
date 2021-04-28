using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNameInput : MonoBehaviour
{
    [Header("UI")]
    //input player name
    [SerializeField] private TMP_InputField nameInputField = null;
    //submit player name
    [SerializeField] private Button continueButton = null;

    public static string DisplayName { get; private set;}

    private const string PlayerPrefsNameKey = "Player Name";

    // Start is called before the first frame update
    private void Start() => SetUpInputField();

    private void SetUpInputField()
    {
        //PlayerPrefs stores and accessed player preferences between game sessions.
        if (!PlayerPrefs.HasKey(PlayerPrefsNameKey))
        {
            return;
        }
        //history name
        string defaultName = PlayerPrefs.GetString(PlayerPrefsNameKey);

        nameInputField.text = defaultName;

        SetPlayerName(defaultName);
    }

    public void SetPlayerName(string name)
    {
        //valid name for button to continue;
        continueButton.interactable = !string.IsNullOrEmpty(name);
    }

    public void SavePlayerName()
    {
        //save the name in the PlayerPrefs
        DisplayName = nameInputField.text;

        PlayerPrefs.SetString(PlayerPrefsNameKey, DisplayName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
