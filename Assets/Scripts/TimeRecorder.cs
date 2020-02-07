using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

/// <summary>
/// 一个用以记录某次函数运行时间的工具
/// 使用单例类设计，同时使用编辑器扩展可以方便的查看时间
/// </summary>
public class TimeRecorder : MonoBehaviour
{
    /// <summary>
    /// 时间记录单位
    /// </summary>
    public enum RecordUnit
    {
        [Tooltip("记录的时间单位为毫秒")]
        Millisecond,
        [Tooltip("记录的时间为秒")]
        Second,
    }
    /// <summary>
    /// 表示一次记录的数据结构
    /// </summary>
    public class RecordData
    {
        public string ActionName;
        public double Time;
        public RecordUnit Unit;
        public int Depth;
    }

    private static TimeRecorder _Instance;
    public static TimeRecorder Instance { get { return _Instance; } }

    private TimeRecorder()
    {
        _Instance = this;

        _CurrentRecordDepth = -1;
        _RecordList = new List<RecordData>();
    }


    /// <summary>
    /// 当前调用深度
    /// </summary>
    private int _CurrentRecordDepth;
    /// <summary>
    /// 保存当前记录的RecordData
    /// </summary>
    private List<RecordData> _RecordList;

    public RecordData[] Reocrds
    {
        get
        {
            if (_RecordList == null) return new RecordData[0];
            return _RecordList.ToArray();
        }
    }

    /// <summary>
    /// 记录一次行为的执行时间
    /// </summary>
    /// <param name="actionName">行为的名字</param>
    /// <param name="action">行为</param>
    /// <param name="unit">时间单位</param>
    public void Record(string actionName, Action action, RecordUnit unit = RecordUnit.Millisecond)
    {
        _CurrentRecordDepth++;

        RecordData record = new RecordData
        {
            ActionName = actionName,
            Unit = unit,
            Depth = _CurrentRecordDepth
        };
        _RecordList.Add(record);

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        // 因为 action中不一定是正确的
        // 所以应该包含在try-catch当中，并把异常抛回调用方
        try { action(); }
        catch (Exception e) { throw e; }
        finally
        {
            stopwatch.Stop();

            long tick = stopwatch.ElapsedTicks;
            record.Time = unit == RecordUnit.Millisecond ?
                    (double)tick / TimeSpan.TicksPerMillisecond :
                    (double)tick / TimeSpan.TicksPerSecond;

            _CurrentRecordDepth--;
        }
    }
}