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

    public void SetDropdown(TMP_Dropdown dropdown, List<TMP_Dropdown.OptionData> options, int index, UnityEngine.Events.UnityAction<int> onChanged)
    {
        dropdown.ClearOptions();
        dropdown.AddOptions(options);

        dropdown.value = index;
        dropdown.RefreshShownValue();

        dropdown.onValueChanged.RemoveAllListeners();
        dropdown.onValueChanged.AddListener(onChanged);
    }

    //Metodo que cambia las opciones de Resolucion en el dropdown, a su vez, actualiza el dropdown a la resolucion actual
    public (List<TMP_Dropdown.OptionData> optionDatas, int index) GetResolutionOptions()
    {
        //Lista de opciones del Dropdown, dicha data es un struct, por lo que puede tener texto y más
        List<TMP_Dropdown.OptionData> optionDatas = new List<TMP_Dropdown.OptionData>();

        //Pongo el indice a 0
        int index = 0;

        //Recorro cada resolución que pueda aguantar el monitor
        foreach (Resolution it in _resolutions)
        {
            //Creo una nueva data
            TMP_Dropdown.OptionData data = new TMP_Dropdown.OptionData();

            //A dicha data le cambio el texto por la resolución en X e Y por la resolución que le corresponde en cada iteración.
            data.text = it.width.ToString() + " x " + it.height.ToString();
            //A las opciones del Dropdown le asigno dicha data
            optionDatas.Add(data);
            //Borro ese espacio de memoria por seguridad, yk
            data = null;
            //Si la resolución de la iteración coincide con la actual y su radio de enfriamiento de frames, así cambio el index del Dropdown para luego cambiar la resolución que se ve in game
            if (it.width == Screen.currentResolution.width && it.height == Screen.currentResolution.height && Mathf.Approximately((float)it.refreshRateRatio.value, (float)Screen.currentResolution.refreshRateRatio.value))
                index = optionDatas.Count - 1;
        }
        return (optionDatas, index);
    }
    public void ChangeResolution(Resolution resolution)
    {
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreenMode, resolution.refreshRateRatio);
    }

    public void Sensitivity()
    {

    }

    public void ScreenShake()
    {

    }
}
