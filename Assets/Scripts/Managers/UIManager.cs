using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    private Resolution[] _resolutions;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        _resolutions = Screen.resolutions;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Resolution[] GetResolutions() { return _resolutions; }
    public void ChangeFullScreen(bool isFullScreen)
    {
        //Cambio el modo de ventana a pantalla completa y viceversa
        if (isFullScreen) Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        else Screen.fullScreenMode = FullScreenMode.Windowed;
    }
    public void ChangeVSync(bool isVSync)
    {
        if (isVSync) QualitySettings.vSyncCount = 1;
        else QualitySettings.vSyncCount = 0;
        //TODO:Ampliarlo para poder cambiar  el VSyncCount superior a 1
    }
    //Metodo que cambia las opciones de Resolucion en el dropdown, a su vez, actualiza el dropdown a la resolucion actual
    public List<TMP_Dropdown.OptionData> GetResolutionOptions(out int index)
    {
        List<TMP_Dropdown.OptionData> optionDatas = new List<TMP_Dropdown.OptionData>();
        index = 0;
        foreach (Resolution it in _resolutions)
        {
            TMP_Dropdown.OptionData data = new TMP_Dropdown.OptionData();

            data.text = it.width.ToString() + " x " + it.height.ToString();
            optionDatas.Add(data);
            data = null;

            if (it.width == Screen.currentResolution.width && it.height == Screen.currentResolution.height && Mathf.Approximately((float)it.refreshRateRatio.value, (float)Screen.currentResolution.refreshRateRatio.value))
                index = optionDatas.Count - 1;
        }
        return optionDatas;
    }
    public void ChangeResolution(Resolution resolution)
    {
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreenMode, resolution.refreshRateRatio);
    }

    public void Sensitivity()
    {

    }

    public void Brightness()
    {

    }

    public void Quality()
    {

    }

    public void AntiAliasing()
    {

    }
}
