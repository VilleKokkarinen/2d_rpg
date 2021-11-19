using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

static class XPmanager
{
    public static int CalculateXP(Enemy e)
    {
        int baseXP = (Player.Instance.Level * 5) + 45;

        int easytargetlevel = CalculateGrayMobLevel(); //Player.Instance.Level - 10;

        int totalXP = 0;

        Debug.Log("ez lvl:" + easytargetlevel);

        if(e.Level >= Player.Instance.Level)
        {
            totalXP = (int)(baseXP * (1 + 0.05 * (e.Level - Player.Instance.Level) ));
        }
        else if (e.Level > easytargetlevel)
        {
            totalXP = (int)(baseXP * (1 - (Player.Instance.Level - e.Level) / ZD()));           
        }

        Debug.Log("1-p-e: " + (1 - (Player.Instance.Level - e.Level) / ZD()).ToString());
        Debug.Log("zd: " + ZD());
        Debug.Log("xp: " + totalXP);
        return totalXP;
    }

    public static int CalculateXP(Quest q)
    {
        if(Player.Instance.Level <= q.Level + 5)
        {
            return q.Xp;
        }
        if (Player.Instance.Level == q.Level + 6)
        {
            return (int)(q.Xp * 0.8 / 5) * 5;
        }
        if (Player.Instance.Level == q.Level + 7)
        {
            return (int)(q.Xp * 0.6 / 5) * 5;
        }
        if (Player.Instance.Level == q.Level + 8)
        {
            return (int)(q.Xp * 0.4 / 5) * 5;
        }
        if (Player.Instance.Level == q.Level + 9)
        {
            return (int)(q.Xp * 0.2 / 5) * 5;
        }
        if (Player.Instance.Level >= q.Level + 10)
        {
            return (int)(q.Xp * 0.1 / 5) * 5;
        }
        return 0;
    }


    private static int ZD()
    {
        if (Player.Instance.Level <= 7)
        {
            return 5;
        }
        if (Player.Instance.Level >= 8 && Player.Instance.Level <= 9)
        {
            return 6;
        }
        if (Player.Instance.Level >= 10 && Player.Instance.Level <= 11)
        {
            return 7;
        }
        if (Player.Instance.Level >= 12 && Player.Instance.Level <= 15)
        {
            return 8;
        }
        if (Player.Instance.Level >= 16 && Player.Instance.Level <= 19)
        {
            return 9;
        }
        if (Player.Instance.Level >= 20 && Player.Instance.Level <= 29)
        {
            return 11;
        }
        if (Player.Instance.Level >= 30 && Player.Instance.Level <= 39)
        {
            return 12;
        }
        if (Player.Instance.Level >= 40 && Player.Instance.Level <= 49)
        {
            return 13;
        }
        if (Player.Instance.Level >= 50 && Player.Instance.Level <= 59)
        {
            return 14;
        }
        if (Player.Instance.Level >= 60 && Player.Instance.Level <= 69)
        {
            return 15;
        }
        if (Player.Instance.Level >= 70 && Player.Instance.Level <= 79)
        {
            return 16;
        }
        if (Player.Instance.Level >= 80 && Player.Instance.Level <= 89)
        {
            return 17;
        }
        return 18;
    }

    public static int CalculateGrayMobLevel()
    {
        if(Player.Instance.Level <= 5)
        {
            return 0;
        }
        else if (Player.Instance.Level >= 6 && Player.Instance.Level <= 49)
        {
            return Player.Instance.Level - (Player.Instance.Level / 10) - 5;
        }
        else if (Player.Instance.Level == 50)
        {
            return Player.Instance.Level - 10;
        }
        else if (Player.Instance.Level >= 51 && Player.Instance.Level <= 99)
        {
            return Player.Instance.Level - (Player.Instance.Level / 5) - 1;
        }
        return Player.Instance.Level - 9;
    }
}