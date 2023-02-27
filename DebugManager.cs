using System.Reflection;

using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using Text = TMPro.TextMeshProUGUI;
namespace Common.Testing
{
    public class DebugManager : MonoBehaviour
    {
        private readonly struct StatisticsDisplay
        {
            private readonly string var;
            private readonly object obj;
            private readonly Text name;
            private readonly Text type;
            private readonly Text value;
            private readonly MemberInfo info;
            private readonly MemberTypes memberType;
            public StatisticsDisplay(string var, object obj, Text name, Text type, Text value, MemberInfo info, MemberTypes memberType)
            {
                this.var = var;
                this.obj = obj;
                this.name = name;
                this.type = type;
                this.value = value;
                this.info = info;
                this.memberType = memberType;
            }
            public void Update()
            {
                switch (memberType)
                {
                    case MemberTypes.Field:
                        var field = (info as FieldInfo).GetValue(obj);
                        name.text = var;
                        type.text = field.GetType().ToString();
                        value.text = field.ToString();
                        break;
                    case MemberTypes.Property:
                        var property = (info as PropertyInfo).GetValue(obj);
                        name.text = var;
                        type.text = property.GetType().ToString();
                        value.text = property.ToString();
                        break;
                    default:
                        throw new System.NotSupportedException();
                }
            }
        }
        private void Update()
        {
            foreach (var display in displays.Values)
            {
                display.Update();
            }
        }
        static DebugManager instance;
        public GameObject statisticTab;
        private void Awake()
        {
            stats = new();
            displays = new();
            instance = this;
        }
        static void BuildStatisticsDisplay(string var, MemberInfo member, MemberTypes memberType)
        {
            Text nameDisplay = null, typeDisplay = null, valueDisplay = null;
            switch (memberType)
            {
                case MemberTypes.Field:
                    {
                        var field = member as FieldInfo;
                        var value = field.GetValue(stats[var]);

                        var display = Instantiate(instance.statisticTab, instance.transform, false);
                        display.hideFlags = HideFlags.HideInHierarchy;
                        var displayTransform = display.transform;
                        nameDisplay = displayTransform.Find("NameDisplay").GetComponentInChildren<Text>();
                        nameDisplay.text = var;

                        typeDisplay = displayTransform.Find("TypeDisplay").GetComponentInChildren<Text>();
                        var type = value.GetType();
                        typeDisplay.text = type.ToString();

                        valueDisplay = displayTransform.GetComponentInChildren<Text>();
                        valueDisplay.text = value.ToString();
                        break;
                    }
                case MemberTypes.Property:
                    {
                        var property = member as PropertyInfo;
                        var value = property.GetValue(stats[var]);

                        var display = Instantiate(instance.statisticTab, instance.transform, false);
                        display.hideFlags = HideFlags.HideInHierarchy;
                        var displayTransform = display.transform;
                        nameDisplay = displayTransform.Find("NameDisplay").GetComponentInChildren<Text>();
                        nameDisplay.text = var;

                        typeDisplay = displayTransform.Find("TypeDisplay").GetComponentInChildren<Text>();
                        var type = value.GetType();
                        typeDisplay.text = type.ToString();

                        valueDisplay = displayTransform.GetComponentInChildren<Text>();
                        valueDisplay.text = value.ToString();
                        break;
                    }
            }
            displays[var] = new StatisticsDisplay(var, stats[var], nameDisplay, typeDisplay, valueDisplay, member, memberType);
        }

        public static Dictionary<string, object> stats;
        private static Dictionary<string, StatisticsDisplay> displays;
        public static void AddStat(string stat, object obj, MemberTypes type, BindingFlags flags = BindingFlags.Default)
        {
            stats[stat] = obj;
            var info = obj.GetType().GetMember(stat, type, flags);
            foreach (var infoE in info) 
            {
                BuildStatisticsDisplay(stat, infoE, type);
            }
        }
    }
}