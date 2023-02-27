using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Common.Helpers;
using Text = TMPro.TextMeshProUGUI;

// The text manager provides an easy way to manage large groups of Text objects.
// It uses a dictionary and uses an indexer to get and retrieves or sets the textx osf the objects it is assigned.
namespace Common
{
    public class TextManager : MonoBehaviour
    {
        Dictionary<string, Text> textDict;
        public string[] textNames;
        public Text[] textMeshPros;
        public string this[string key]
        {
            set => textDict[key].text = value; // Get whether the toggle of the specific name is on or not.
        }
        public Text GetText(string key)
        {
            return textDict[key]; // Set the toggle of the specific name off or on.
        }
        private void Awake()
        {
            textDict = CollectionsHelper.ListsToDict(textNames, textMeshPros);
        }
    }
}