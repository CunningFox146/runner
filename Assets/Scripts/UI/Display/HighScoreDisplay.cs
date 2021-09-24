using Runner.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Runner.UI
{
    public class HighScoreDisplay : MonoBehaviour
    {
        void Start()
        {
            GetComponent<Text>().text = SaveManager.CurrentSave.highScore.ToString();
        }
    }
}