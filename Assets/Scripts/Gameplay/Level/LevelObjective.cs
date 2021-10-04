using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class LevelObjective : ScriptableObject
{
    public abstract bool TestCompletion();
    public abstract string[] GetDescription();
}
