using Runner.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Runner.UI
{
    public class ScoreDisplay : MonoBehaviour
    {
        private Text _text;

        void Awake()
        {
            _text = GetComponent<Text>();
        }

        void Start()
        {
            UpdateText();
        }

        void LateUpdate()
        {
            UpdateText();
        }

        private void UpdateText()
        {
            _text.text = ((int)(GameManager.CurrentScore)).ToString();
        }
    }
}
