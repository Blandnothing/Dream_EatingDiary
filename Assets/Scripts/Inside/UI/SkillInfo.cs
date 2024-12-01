using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillInfo : MonoBehaviour
{
    Dictionary<ResourceType,SkillGrid> skillGrids = new();
    [SerializeField,Header("第一个为可用颜色，第二个为使用中颜色，第三个为不可用颜色")] List<Color> skillColors = new();

    private void Awake()
    {
        skillGrids[ResourceType.Dichotomy] = transform.GetChild(0).gameObject.GetComponent<SkillGrid>();
        skillGrids[ResourceType.Attract] = transform.GetChild(1).gameObject.GetComponent<SkillGrid>();
        skillGrids[ResourceType.Accelerate] = transform.GetChild(2).gameObject.GetComponent<SkillGrid>();
    }

    private void Start()
    {
        EventCenter.Instance.AddEvent(EventName.SkillTimeChange,UpdateGrid);
        UpdateGrid();
    }

    void UpdateGrid()
    {
        Color color;
        foreach (var grid in skillGrids.Keys)
        {
            if (FunctionManager.Instance.FunctionDic[grid].isStart)
            {
                color=skillColors[1];
            }else if (FunctionManager.Instance.FunctionDic[grid].isable)
            {
                color=skillColors[0];
            }else
                color=skillColors[2];
            skillGrids[grid].Show(color, FunctionManager.Instance.TimeDic[grid].ToString());
        }
    }
}
