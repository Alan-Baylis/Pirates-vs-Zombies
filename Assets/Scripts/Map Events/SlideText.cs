using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;


public class SlideText : MonoBehaviour {


    public Image imgCanvas;
    public Text textCanvas;


    private Sprite[] icons;
    private string[] lines;
    private int slideNumber = 0;


    public void ShowSlide(string textFile, Sprite[] iconCollection)
    {
        icons = iconCollection;
        lines = System.IO.File.ReadAllLines(textFile);

        if (lines == null)
            DebugHandler.printError("SlideText", "No such file was found!");

        gameObject.SetActive(true);
        NextSlide();
    }


    public void HideSlide()
    {
        gameObject.SetActive(false);
    }



    public void NextSlide()
    {
        textCanvas.text = "";
        
        imgCanvas.sprite = icons[(int)char.GetNumericValue(lines[slideNumber][0])];
        
        // Skip the number in the beginning
        for (int i = 1; i < lines[slideNumber].Length; i++)
            textCanvas.text += lines[slideNumber][i];

        slideNumber++;
    }

    public void NextSlideClickEvent()
    {
        if (slideNumber >= lines.Length)
        {
            HideSlide();
            slideNumber = 0;
        }
        else
            NextSlide();
    }





    

}
