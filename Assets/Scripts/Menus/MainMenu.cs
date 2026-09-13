using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] TMP_Dropdown _resolutionDropdown;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       UIManager.Instance.SetDropdown(_resolutionDropdown, UIManager.Instance.GetResolutionOptions().optionDatas, UIManager.Instance.GetResolutionOptions().index, OnResolutionChanged);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
        Application.Quit();
    }

    private void OnResolutionChanged(int index)
    {
        UIManager.Instance.ChangeResolution(UIManager.Instance.GetResolutions()[index]);
    }
}
