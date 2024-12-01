using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceList : MonoBehaviour
{
    List<ResourceGrid> resourceGrids = new();

    private void Start()
    {
        foreach (Transform child in transform)
        {
            ResourceGrid resourceGrid = child.GetComponent<ResourceGrid>();
            if(resourceGrid)
                resourceGrids.Add(resourceGrid);
        }
        
        EventCenter.Instance.AddEvent(EventName.GetResource,UpdateGrid);
        UpdateGrid();
    }

    void UpdateGrid()
    {
        foreach (var grid in resourceGrids)
        {
            grid.Show();
        }
    }
}
