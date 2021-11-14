using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable] public class ELIVRDataFrame
{
   public Vector3 CombinedvalidationErrorOffset;
   public Vector3 LeftEyeValidationErrorOffset;
   public Vector3 RightEyeValidationOffset;
   public double applicationStartTimestamp;
   public double ExperimentStartTimestamp;
   public double ExperimentFinishTimeStamp;
   public string participantID;
   public string Condition;
   public string Order;
   public List<StationDataFrame> _stationDataFrames;
}
