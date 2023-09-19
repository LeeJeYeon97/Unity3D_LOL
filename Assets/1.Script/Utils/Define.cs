using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define 
{
    public enum Champ
    {
        Garen,
        Alistar,
        Jinx,
    }
    public enum Scene
    {
        UNKNOWN,
        START,
        PICK,
        GAME,

    }
    public enum MinionType
    {
        NONE,
        MELEE,
        RANGED,
        SEIGE,
    }
    public enum Team
    { 
        NONE,
        RED,
        BLUE,
    }

    public enum TurretType
    {
        NONE,
        OUTER,
        INNER,
        INHIBITOR,
        NEXUS,

    }
    public enum State
    {
        IDLE,
        MOVE,
        ATTACK,
        DIE,
        SLOW,
        SNARE,
        AIRBONE,
        KNOCKBACK,
    }
    public enum Layer
    {
        WALL = 7,
        GROUND = 8,
        RED_MINION = 9,
        BLUE_MINION = 10,
        RED_TURRET = 11,
        BLUE_TURRET = 12,
        PLAYER = 13,
        BOT = 14,
    }
    public enum Tag
    {
        PLAYER = 0,
        MINION = 1,
        TURRET,
        REDSPAWN,
        BLUESPAWN,
        BLUE_NEXUS,
        RED_NEXUS,

    }
    public enum MouseEvent
    {
        Press,
        Click,

    }
    public enum CameraMode
    {
        QuaterView,

    }
}
