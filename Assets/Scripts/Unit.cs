using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit
{
    public string id;
    public int move;
    public int fuel;
    public int ammo;
    public string moveType;
    public string unitType;
    public Hashtable damageTable;
    public Hashtable fuelTable;

    int realHp;
    int displayHp;
}
