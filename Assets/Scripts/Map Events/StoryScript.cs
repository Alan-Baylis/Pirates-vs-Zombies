using UnityEngine;
using System.Collections;

public class StoryScript : MonoBehaviour {

    public string changeScene = "";
    public GameObject nextButton;


    private bool slideOver = false;
    public bool isSlideOver
    { get { return slideOver; } }
    
    private int currentSlide = 0;

    void Start()
    {
        if(!nextButton)
            DebugHandler.printError("StoryScript", "Next button is missing");
        else
        {
            // Hides everything
            StopSlideShow();
        }
    }


    public void StartSlideShow()
    {
        nextButton.SetActive(true);
        ShowSlide();
    }

    public void StopSlideShow()
    {
        nextButton.SetActive(false);
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(false);
    }

    public void NextSlide()
    {
        currentSlide++;
        ShowSlide();
    }

    void ShowSlide()
    {
        if (currentSlide > 0 && currentSlide < transform.childCount)
            transform.GetChild(currentSlide - 1).gameObject.SetActive(false);
        else if (currentSlide >= transform.childCount)
        {
            if (changeScene != "")
            {
                Application.LoadLevel(changeScene);
                return;
            }


            StopSlideShow();
            slideOver = true;
        }
                
        transform.GetChild(currentSlide).gameObject.SetActive(true);
    }


}
