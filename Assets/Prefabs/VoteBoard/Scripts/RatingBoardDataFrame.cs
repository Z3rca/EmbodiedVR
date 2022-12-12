using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatingBoardDataFrame : EventArgs
{
    public bool Approved;
    public List<int> Ratings;
    public List<double> RatingTimeStamps;
    public double ChoiceTimeStamp;
    public int Choice; // is zero if no vote was pressed
    public double TimeStampEnd; // todo assign
}
