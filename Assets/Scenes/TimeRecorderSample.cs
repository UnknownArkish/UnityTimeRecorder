using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeRecorderSample : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            TestSimpleFunc();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            TestHeavyFunc();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            TestEmbeddedFunc();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            TestRecursiveFunc(5);
        }
    }

    /// <summary>
    /// 测试简单任务的记录
    /// </summary>
    private void TestSimpleFunc()
    {
        TimeRecorder.Instance.Record("TestSimpleFunc", () =>
        {
            SimpleFunc();
        });
    }
    /// <summary>
    /// 测试繁重任务的记录
    /// </summary>
    private void TestHeavyFunc()
    {
        TimeRecorder.Instance.Record("TestHeavyFunc", () =>
        {
            HeavyFunc();
        });
    }

    /// <summary>
    /// 测试嵌套记录
    /// </summary>
    private void TestEmbeddedFunc()
    {
        TimeRecorder.Instance.Record("EmbeddedFunc", () =>
        {
            TestSimpleFunc();
            TestHeavyFunc();
        });
    }

    /// <summary>
    /// 测试递归记录
    /// </summary>
    /// <param name="recursiveTime">递归次数</param>
    private void TestRecursiveFunc(int recursiveTime)
    {
        if (recursiveTime < 1) return;
        TimeRecorder.Instance.Record(string.Format("TestRecursiveFunc_{0}", recursiveTime), () =>
        {
            HeavyFunc();
            TestRecursiveFunc(recursiveTime - 1);
        });
    }

    /// <summary>
    /// 模拟一项简单的任务，几乎不消耗时间
    /// </summary>
    private void SimpleFunc() { }

    /// <summary>
    /// 模拟一项繁重的任务，消耗很多时间
    /// </summary>
    private void HeavyFunc()
    {
        int count = 1000000;
        for (int i = 0; i < count; i++) { }
    }
}
