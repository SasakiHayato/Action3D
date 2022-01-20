using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public int CurrentExp { get; set; } = 0;
    public int NextLevelExp { get; set; } = 100;
    public Player Player { get; set; } = null;
}
