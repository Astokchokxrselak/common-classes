using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Common.Helpers;

// The toggle manager provides an easy way to access a group of toggles.
// It uses a dictionary and uses an indexer to get and set the values of the toggles it is assigned.
namespace Common
{
    public class ToggleManager : MonoBehaviour
    {
        Dictionary<string, Toggle> toggleDict;
        public string[] togglesNames;
        public Toggle[] toggles;
        public bool this[string key]
        {
            get => toggleDict[key].isOn; // Get whether the toggle of the specific name is on or not.
            set => toggleDict[key].isOn = value; // Set the toggle of the specific name off or on.
        }
        private void Awake()
        {
            toggleDict = CollectionsHelper.ListsToDict(togglesNames, toggles);
        }
    }
}