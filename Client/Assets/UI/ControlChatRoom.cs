using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlChatRoom : MonoBehaviour
{

    public InputField chatInput;
    public Text chatText;
    public ScrollRect scrollRect;
    string username = "zzjjjj";
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Sendmessage()
    {
        if (chatInput.text != "")
        {
            string addText = "\n  " + "<color=red>" + username + "</color>: " + chatInput.text;
            chatText.text += addText;
            chatInput.text = "";
            //以下代码未理解
            chatInput.ActivateInputField();
            Canvas.ForceUpdateCanvases();      
            scrollRect.verticalNormalizedPosition = 0f; 
            Canvas.ForceUpdateCanvases();   
        }

    }
}