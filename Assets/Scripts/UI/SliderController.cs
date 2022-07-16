using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    [SerializeField]
    GameObject panel;
    private void Start()
    {
        MutationRateSilder();
        ElitismRateSilder();
        CrossoverRateSilder();
        PoolSizeSilder();
        ProblemSizeSilder();
        TripNumberSlider();
        AlphaSlider();
        BetaSlider();
        NumberOfAntsSlider();
    }
    public void MutationRateSilder()
    {
        Slider slider = panel.gameObject.transform.Find("MutationRateSlider").GetComponent<Slider>();
        TextMeshProUGUI text = panel.gameObject.transform.Find("MutationRateNumber").GetComponent<TextMeshProUGUI>();
        text.SetText(slider.value.ToString("0.00"));
        
    }
    public void ElitismRateSilder()
    {
        Slider slider = panel.gameObject.transform.Find("ElitismSlider").GetComponent<Slider>();
        TextMeshProUGUI text = panel.gameObject.transform.Find("ElitismNumber").GetComponent<TextMeshProUGUI>();
        text.text = slider.value.ToString("0.00");
    }
    public void CrossoverRateSilder()
    {
        Slider slider = panel.gameObject.transform.Find("CrossoverRateSlider").GetComponent<Slider>();
        TextMeshProUGUI text = panel.gameObject.transform.Find("CrossoverRateNumber").GetComponent<TextMeshProUGUI>();
        text.text = slider.value.ToString("0.00");
    }
    public void PoolSizeSilder()
    {
        Slider slider = panel.gameObject.transform.Find("PoolSizeSlider").GetComponent<Slider>();
        TextMeshProUGUI text = panel.gameObject.transform.Find("PoolSizeNumber").GetComponent<TextMeshProUGUI>();
        text.text = slider.value.ToString();
    }
    public void ProblemSizeSilder()
    {
        Slider slider = panel.gameObject.transform.Find("ProblemSizeSlider").GetComponent<Slider>();
        TextMeshProUGUI text = panel.gameObject.transform.Find("ProblemSizeNumber").GetComponent<TextMeshProUGUI>();
        text.text = slider.value.ToString();
    }
    public void TripNumberSlider()
    {
        Slider slider = panel.gameObject.transform.Find("TripNumberSlider").GetComponent<Slider>();
        TextMeshProUGUI text = panel.gameObject.transform.Find("TripNumberNumber").GetComponent<TextMeshProUGUI>();
        text.SetText(slider.value.ToString());
    }
    public void AlphaSlider(){
        Slider slider = panel.gameObject.transform.Find("AlphaSlider").GetComponent<Slider>();
        TextMeshProUGUI text = panel.gameObject.transform.Find("AlphaNumber").GetComponent<TextMeshProUGUI>();
        text.SetText(slider.value.ToString("0.00"));
    }
    public void BetaSlider(){
        Slider slider = panel.gameObject.transform.Find("BetaSlider").GetComponent<Slider>();
        TextMeshProUGUI text = panel.gameObject.transform.Find("BetaNumber").GetComponent<TextMeshProUGUI>();
        text.SetText(slider.value.ToString("0.00"));
    }
    public void NumberOfAntsSlider(){
        Slider slider = panel.gameObject.transform.Find("NumberOfAntsSlider").GetComponent<Slider>();
        TextMeshProUGUI text = panel.gameObject.transform.Find("NumberOfAntsNumber").GetComponent<TextMeshProUGUI>();
        text.text = (slider.value.ToString());
    }
}
