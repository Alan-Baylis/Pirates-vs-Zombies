using UnityEngine;
using System.Collections;

public class ActivateSlideText : MonoBehaviour {

    public GameObject DialogBox;
    public string textFile;
    public Sprite[] icons;

    bool read = false;
    public bool isRead
    { get { return read; } }

    public void RunSlide()
    {
        if(!read)
        {
            DialogBox.GetComponent<SlideText>().ShowSlide(textFile, icons);
            read = true;
        }
    }
}
