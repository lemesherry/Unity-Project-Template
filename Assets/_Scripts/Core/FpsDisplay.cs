using UnityEngine;
using TMPro;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using System;

[RequireComponent(typeof(TextMeshProUGUI))]
public class FpsDisplay : MonoBehaviour
{
    public UniversalRenderPipelineAsset urpAsset;
    public float updateInterval = 0.5f;
    //public bool showMedian = false;
    //public float medianLearnrate = 0.05f;
    public int targetFps = 60;
    public int targetFpsEditor = 60;

    private float accum = 0; // FPS accumulated over the interval
    private int frames = 0; // Frames drawn over the interval
    private float timeleft; // Left time for current interval
    private float currentFPS = 0;
    //private float median = 0;
    //private float average = 0;
    //public float CurrentFPS{ get { return currentFPS; } }
    //public float FPSMedian { get { return median; } }
    //public float FPSAverage { get { return average; } }
    //private int currFps;
    [SerializeField] TextMeshProUGUI fpsTxt;
    [SerializeField] TextMeshProUGUI cpuTxt;

    //private void Awake()
    //{
    //#if UNITY_EDITOR
    //        Application.targetFrameRate = targetFpsEditor;
    //#else
    //        Application.targetFrameRate = targetFps;
    //#endif
    //}
    private void Start()
    {
#if UNITY_EDITOR
        Application.targetFrameRate = targetFpsEditor;
#else
        Application.targetFrameRate = targetFps;
#endif
        //QualitySettings.vSyncCount = 2;
        //OnDemandRendering.renderFrameInterval = 2;
        //
        timeleft = updateInterval;
        //
        //InvokeRepeating(nameof(SetRenderScale), 2, 5);
        // Invoke(nameof(SetRenderScale), 5f);
    }
    private void Update()
    {
        // Timing inside the editor is not accurate. Only use in actual build.
        //#if !UNITY_EDITOR
        timeleft -= Time.deltaTime;
        accum += Time.timeScale / Time.deltaTime;
        ++frames;
        // Interval ended - update GUI text and start new interval
        if (timeleft <= 0.0)
        {
            currentFPS = accum / frames;
            //average += (Mathf.Abs(currentFPS) - average) * 0.1f;
            //median += Mathf.Sign(currentFPS - median) * Mathf.Min(average * medianLearnrate, Mathf.Abs(currentFPS - median));
            // display two fractional digits (f2 format)
            //fps = showMedian ? median : currentFPS;
            fpsTxt.text = $"FPS: {Mathf.Round(currentFPS)}";
            //fpsTxt.text = Mathf.Round(currentFPS) + " (" + Mathf.Round(1000.0f / currentFPS) + " ms)";
            cpuTxt.text = $"CPU: {Mathf.Round( 1000.0f / currentFPS )}";
            timeleft = updateInterval;
            accum = 0.0F;
            frames = 0;
        }
        //#endif
    }

    //public void ResetMedianAndAverage()
    //{
    //    median = 0;
    //    average = 0;
    //}
    private void SetRenderScale() {
        //Debug.Log("Device fresh rate ===>" + Screen.currentResolution.refreshRate);
        //if (currentFPS > 55)
        //{
        //    Debug.Log("Frame rate > 55 ===>");
        //    urpAsset.renderScale = 1;
        //}
        //else if (currentFPS <= 55 && currentFPS > 50)
        //{
        //    Debug.Log("Frame rate > 50 ===>");
        //    urpAsset.renderScale = 0.8f;
        //}
        //else if (currentFPS <= 50 && currentFPS > 35)
        //{
        //    Debug.Log("Frame rate <= 50 ===>");
        //    urpAsset.renderScale = 0.7f;
        //}
        //else if (currentFPS <= 35 && currentFPS > 20)
        //{
        //    Debug.Log("Frame rate <= 30 ===>");
        //    urpAsset.renderScale = 0.65f;
        //}
        //else if (currentFPS <= 20)
        //{
        //    Debug.Log("Frame rate <= 20 ===>");
        //    urpAsset.renderScale = 0.55f;
        //}

        if (Screen.currentResolution.refreshRateRatio.value <= currentFPS)
            return;

        urpAsset.renderScale = SystemInfo.graphicsMemorySize switch {
            > 2000 when SystemInfo.processorFrequency > 5000 && SystemInfo.systemMemorySize > 5000 => 1,
            > 1500 when SystemInfo.processorFrequency > 3000 && SystemInfo.systemMemorySize > 4000 => 0.8f,
            > 1000 when SystemInfo.processorFrequency > 2000 && SystemInfo.systemMemorySize > 3000 => 0.75f,
            > 1000 when SystemInfo.processorFrequency > 1000 && SystemInfo.systemMemorySize > 2000 => 0.7f,
            _ => urpAsset.renderScale
        };
    }
    
    public void RestartScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);
}

