using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.WSA;

public class GenerateMap : MonoBehaviour
{
    #region Singleton
    public static GenerateMap instance;
    private void Awake()
    {
        if (instance != null)
            instance = this;
    }
    #endregion

    public Islands_ISO data;
    public LineRenderer roadPrefeb;
    public Transform roadParent;
    public Transform[] islandTransforms;
    public int islandNum;

    // Start is called before the first frame update
    void Start()
    {
        GenerateConnections();
        islandNum = islandTransforms.Length;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateConnections()
    {
        foreach (var connection in data.connections)
        {
            var road = Instantiate(roadPrefeb, roadParent);
            road.SetPosition(0, islandTransforms[connection.from].position);
            road.SetPosition(1, islandTransforms[connection.to].position);

            islandTransforms[connection.from].GetComponent<Island>().linkIslands.Add(islandTransforms[connection.to].GetComponent<Island>());
            islandTransforms[connection.to].GetComponent<Island>().linkIslands.Add(islandTransforms[connection.from].GetComponent<Island>());
        }
    }

}
