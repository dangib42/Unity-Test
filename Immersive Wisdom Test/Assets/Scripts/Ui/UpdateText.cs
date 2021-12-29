using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpdateText : MonoBehaviour
{
    public ColorValue curCV = ColorValue.Red;

    private TMP_InputField redText;
    private TMP_InputField greenText;
    private TMP_InputField blueText;
    private TMP_InputField alphaText;
    private TMP_InputField hexText;

    private int curRed = 0, curGreen = 0, curBlue = 0, curAlpha = 0;

    private Image imgR, imgG, imgB;

    private Color newColor;
    public Slider curSlider;
    // Start is called before the first frame update
    void Start()
    {
        redText = GameObject.FindGameObjectWithTag("RedText").GetComponent<TMP_InputField>();
        imgR = GameObject.FindGameObjectWithTag("RedBackground").GetComponent<Image>();

        greenText = GameObject.FindGameObjectWithTag("GreenText").GetComponent<TMP_InputField>();
        imgG = GameObject.FindGameObjectWithTag("GreenBackground").GetComponent<Image>();

        blueText = GameObject.FindGameObjectWithTag("BlueText").GetComponent<TMP_InputField>();
        imgB = GameObject.FindGameObjectWithTag("BlueBackground").GetComponent<Image>();

        alphaText = GameObject.FindGameObjectWithTag("AlphaText").GetComponent<TMP_InputField>();
        
        hexText = GameObject.FindGameObjectWithTag("HexCode").GetComponent<TMP_InputField>();
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            TMP_InputField curField;
            bool isAlph = false;
            if (curCV == ColorValue.Red)
            {
                curField = redText;
                
            }
            else if (curCV == ColorValue.Green)
            {
                curField = greenText;
            }
            else if (curCV == ColorValue.Blue)
            {
                curField = blueText;
            }
            else
            {
                curField = alphaText;
                isAlph = true;
            }

            int curVal = 0;
            int.TryParse(curField.text, out curVal);
            if (curVal > 255 && !isAlph)
            {
                curField.text = "255";
                curVal = 255;
            }
            else if (curVal > 100 && isAlph)
            {
                curField.text = "100";
                curVal = 100;
            }
            else if (curVal < 0)
            {
                curField.text = "0";
                curVal = 0;
            }

            curSlider.value = curVal;

        }
    }

    public void SetSliderValue(float sliderValue)
    {

        if (curCV == ColorValue.Red)
        {
            redText.text = sliderValue.ToString();
        }
        else if (curCV == ColorValue.Green)
        {
            greenText.text = sliderValue.ToString();
        }
        else if (curCV == ColorValue.Blue)
        {
            blueText.text = sliderValue.ToString();
        }
        else
            alphaText.text = sliderValue.ToString();


        UpdateColors();
    }

    public void UpdateColors()
    {
        int.TryParse(redText.text, out curRed);
        int.TryParse(greenText.text, out curGreen);
        int.TryParse(blueText.text, out curBlue);
        int.TryParse(alphaText.text, out curAlpha);

        string hex = curRed.ToString("X2")
            + curGreen.ToString("X2")
            + curBlue.ToString("X2");

        hexText.text = hex;

        hex = "#" + hex + curAlpha.ToString("X2");
        float redVal = (float)curRed / 255f;
        float greenVal = (float)curGreen / 255f;
        float blueVal = (float)curBlue / 255f;
        float alphaVal = (float)curAlpha / 100f;


        newColor = new Color(redVal, greenVal, blueVal, alphaVal);
        Camera.main.backgroundColor = newColor;

        Material matR = Instantiate(imgR.material);
        Material matG = Instantiate(imgG.material);
        Material matB = Instantiate(imgB.material);

        matR.SetColor("_Color", new Color(0, greenVal, blueVal)); // left color
        matG.SetColor("_Color", new Color(redVal, 0, blueVal));
        matB.SetColor("_Color", new Color(redVal, greenVal, 0));

        matR.SetColor("_Color2", new Color(1, greenVal, blueVal)); // right color
        matG.SetColor("_Color2", new Color(redVal, 1, blueVal));
        matB.SetColor("_Color2", new Color(redVal, greenVal, 1));

        imgR.material = matR;
        imgG.material = matG;
        imgB.material = matB;
    }

    public enum ColorValue
    {
        Red,
        Green,
        Blue,
        Opacity
    }
}
