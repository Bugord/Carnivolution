using System;
using GameEditor;
using UnityEngine;

public static class GlobalEventManager
{
    public static Action<bool> EditModeToggled;
    public static Action<Detail> DetailDestroyed;
    public static Action<Detail> DetailAdded;
    public static Action<GameObject> EnemyDied;
}