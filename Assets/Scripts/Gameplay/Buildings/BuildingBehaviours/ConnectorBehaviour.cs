using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

// Must be placed on BuildingBehaviour
public class ConnectorBehaviour : MonoBehaviour
{
    public HashSet<BuildingBehaviour> ConnectedBuildings { get; set; } = new HashSet<BuildingBehaviour>(); 
}
