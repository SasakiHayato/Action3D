using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public Player Player { get; set; } = null;

    public int CurrentExp { get; set; } = 0;
    public int NextLevelExp { get; set; } = 100;

    public int HP { get; set; }
    public int Power { get; set; }
}
