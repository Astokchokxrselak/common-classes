using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Common
{
    public class EndlessModeDataCounter : MonoBehaviour
    {
        Dictionary<string, Text> Counters = new Dictionary<string, Text>();
        public Text this[string s]
        {
            get => Counters[s];
        }
        public string[] counters;
        // Start is called before the first frame update
        void Start()
        {
            for (int i = 0; i < counters.Length; i++)
            {
                var name = counters[i];
                Counters[name] = GameObject.Find("Name").GetComponentInChildren<Text>();
            }
        }
        void UpdateText(string[] names, string[] texts)
        {
            for (int i = 0; i < names.Length; i++)
            {
                string name = names[i], text = texts[i];
                Counters[name].text = text;
            }
        }
    }
}