using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Island_ISO", menuName = "Island / Island_ISO")]
public class Islands_ISO : ScriptableObject
{
    public List<LinkedConnections> connections = new List<LinkedConnections> ();

    

}


[System.Serializable]
public class LinkedConnections
{
    public int from;
    public int to;
}