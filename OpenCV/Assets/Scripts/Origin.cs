using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Origin : MonoBehaviour
{
    public List<Sprite> sprites = new List<Sprite>();

    public Image inputImage; 
    public Image outputImage;
    private Sprite originImage;

    int index = 0;

    private void Start()
    {
        originImage = inputImage.sprite;
    }

    public void ResultToInput()
    {
        inputImage.sprite = outputImage.sprite;
        outputImage.sprite = null;
    }

    public void SetOriginImage()
    {
        inputImage.sprite = originImage;
        outputImage.sprite = null;
    }

    public void ChangeInput()
    {
        index = (index + 1 >= sprites.Count) ? 0 : index+1;
        inputImage.sprite = sprites[index];
    }
}
