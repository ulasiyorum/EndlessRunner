using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBUser
{
    // 0-> Global 1-> Monthly 2->Weekly 3->Daily

    public int[] score;
    public DateTime[] date;
    public int balance;
    public string name;
    public int[] potions;

    public int points = 0;
    public DateTime progressStart = DateTime.Now;
    public bool claimed = false;


    public DBUser(int[] sc, DateTime[] dt, int balance, string na, int[] potions, int points, DateTime progress, bool claimed)
    {
        score = sc;
        name = na;
        this.potions = potions;
        this.balance = balance;
        date = dt;

        if (progress == null)
            return;


    }

    public DBUser Init(string name)
    {
        score = new int[4];
        date = new DateTime[4];

        balance = 0;

        this.name = name;
        claimed = false;
        points = 0;
        progressStart = DateTime.Now;
        potions = new int[4];
        return this;
    }

    //This method should be called before printing leaderboard
    public void Check()
    {
        DateTime current = DateTime.Now;
        DateTime currentMonth = new DateTime(current.Year, current.Month, 1);
        DateTime currentWeek = new DateTime(current.Year, current.Month, (current.Day / 7) + 1);
        DateTime weekDate = new DateTime(date[2].Year, date[2].Month, (date[2].Day / 7) + 1);
        TimeSpan[] spans = { current - date[0], date[1] - currentMonth, weekDate - currentWeek, current - date[3] };

        if (spans[3].Days >= 1)
        {
            score[3] = 0;
            date[3] = current;
        }

        if (spans[1].Days < 0)
        {
            score[1] = 0;
            date[1] = current;
        }

        if (spans[2].Days != 0)
        {
            score[2] = 0;
            date[2] = current;
        }
    }


    public void Add(int sc, DateTime time)
    {
        for(int i = 0; i < score.Length; i++)
        {
            if(sc > score[i])
            {
                score[i] = sc;
                date[i] = time;
            }
        }
    }

    public override string ToString()
    {
        return "name: " + name + " score: " + score;
    }


}

public class DBUserComparerDaily : IComparer<DBUser>
{
    public int Compare(DBUser x, DBUser y)
    {
        return y.score[3].CompareTo(x.score[3]);
    }
}

public class DBUserComparerWeekly : IComparer<DBUser>
{
    public int Compare(DBUser x, DBUser y)
    {
        return y.score[2].CompareTo(x.score[2]);
    }
}

public class DBUserComparerMonthly : IComparer<DBUser>
{
    public int Compare(DBUser x, DBUser y)
    {
        return y.score[1].CompareTo(x.score[1]);
    }
}
public class DBUserComparerGlobal : IComparer<DBUser>
{
    public int Compare(DBUser x, DBUser y)
    {
        return y.score[0].CompareTo(x.score[0]);
    }
}
