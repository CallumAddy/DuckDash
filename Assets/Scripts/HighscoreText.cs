using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class HighscoreText : MonoBehaviour
{

    Text highscore;
    // Start is called before the first frame update
    void Start()
    {
        highscore = GetComponent<Text>();
        highscore.text = "Highscore: " + PlayerPrefs.GetInt("Highscore").ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
