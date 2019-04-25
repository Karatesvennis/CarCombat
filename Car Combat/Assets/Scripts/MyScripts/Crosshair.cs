using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{

    public Texture2D crosshairImage;



    private void OnGUI()
    {
        float xMin = (Screen.width / 2) - (crosshairImage.width / 2);
        float yMin = Mathf.Clamp((Screen.height - Input.mousePosition.y) - (crosshairImage.height / 2), Screen.height - Screen.height, Screen.height / 2);

        GUI.DrawTexture(new Rect(xMin, yMin, crosshairImage.width, crosshairImage.height), crosshairImage);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
}
