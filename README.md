# UnityTimerRecorder
A simple way to record time in Unity

---

# 这是什么/它能做什么

这是一个基于Unity的小工具，你能够通过它记录任意一段代码的运行时间。

### 效果图

如下图所失，它们分别展示了：

- 一次任务的记录
- 嵌套任务的记录（记录中嵌套记录）
- 递归任务的记录（套娃记录）

![image-20200207153738788](https://github.com/UnknownArkish/UnityTimerRecorder/blob/master/Images/image-20200207153738788.png)

### 为什么要使用它

- 简单、安全的使用方式，你只需要增加一行代码即可。几乎不破坏代码结构，更不会影响执行结果；
- 若存在嵌套记录，它会自动处理好，你无需额外担心；
- 通过编辑器扩展方便地查看结果，无需通过调试、Log等方式查看结果。

---

# 我如何使用它

## 放在你的Unity工程下

1. 将 Assets/Scripts/TimeRecorder.cs文件放在你的工程目录中放置脚本的文件夹下；
2. 将Assets/Editor/TimeRecorderEditor.cs文件放置在你的工程目录的Assets/Editor文件夹下（可以不直接处于Editor文件夹下，但要被Editor包含）
3. 场景中创建一个物体，挂上 <kbd>TimeRecorder.cs</kbd> 脚本

## 使用示例

TimeRecorder只有一个函数用以记录时间<kbd>Record</kbd>:

```csharp
public void Record(
	string actionName, 
	Action action, 
	RecordUnit unit = RecordUnit.Millisecond
}
```

- <kbd>string</kbd> <kbd>actionName</kbd>: 行为的名字。起一个容易辨认的名字，它会出现在编辑器扩展上；
- <kbd>Action</kbd> <kbd>action</kbd>: 行为。你想要记录的某段代码，你可以将它们包装为 无参数无返回值的 lambda 表达式，或者作为临时本地函数传入；
- <kbd>RecordUnit</kbd> <kbd>unit</kbd> <kbd>Option</kbd>: 时间单位，可选参数。你可以选择以毫秒或者秒作为单位进行记录，默认为毫秒。

---

注意

- TimeRecorder采用单例模式，因此你需要用过<kbd>TimeRecorder.Instance</kbd>来获取单例实例；
- 以下示例均可在<kbd>Assets/Scenes/TestRecorderSample.cs</kbd>中找到，你可以查看源码来学会使用方法，但是下面的示例会有更详细易懂的讲解。

### 简单记录

大多数情况下，你只需要将需要记录的一段代码打包成一个lambda表达式作为参数传入即可。例如有以下代码：

```csharp
// 任务A
DoTaskA();
// 任务B
DoTaskB();
```

如果想要记录<kbd>任务A</kbd>的运行时间，那么只需更改为以下代码即可：

```csharp
TimeRecorder.Instance.Record( 
    "任务A", 
    ()=>{
    // 任务A
    	DoTaskA();
    }
);
// 任务B
DoTaskB();
```

---

以下是更加具体的例子，假设有两个任务，一个任务几乎不耗时，另外一个任务耗时严重（这里使用循环模拟繁重任务）：

```csharp
    // 模拟一项简单的任务，几乎不消耗时间
    private void SimpleFunc() { }
    // 模拟一项繁重的任务，消耗很多时间
    private void HeavyFunc()
    {
        int count = 1000000;
        for (int i = 0; i < count; i++) { // Do heavy task... }
    }
```

它们在示例代码中的调用如下：

```csharp
    private void TestSimpleFunc()
    {
        SimpleFunc();
    }
    private void TestHeavyFunc()
    {
        HeavyFunc();
    }
```

如果我想知道SimpleFunc（或者HeavyFunc）的执行时间，那么只需要更改为如下所示的代码即可：

```csharp
    private void TestSimpleFunc()
    {
        TimeRecorder.Instance.Record("TestSimpleFunc", () =>
        {
            SimpleFunc();
        });
    }
    private void TestHeavyFunc()
    {
        TimeRecorder.Instance.Record("TestHeavyFunc", () =>
        {
            HeavyFunc();
        });
    }
```

### 查看记录

通过<kbd>TimeRecorder.Instance.Record</kbd>的记录结果，可以在挂载了<kbd>TimeRecorder.cs</kbd>脚本的<kbd>Inspector</kbd>窗口上查看。

如下图所示，是调用若干次<kbd>TestSimpleFunc</kbd>和<kbd>TestHeavyFunc</kbd>后，在<kbd>Inspector</kbd>上得到的结果：

<img src="https://img-blog.csdnimg.cn/20200207025155485.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L0Fya2lzaA==,size_16,color_FFFFFF,t_7#pic_center" alt="single" width="350" height="350" />

### 嵌套记录/递归记录

和简单记录一样，你只需要在想要记录的某段代码处，调用<kbd>TimeRecorder.Instance.Record</kbd>即可。

- 如果出现了嵌套记录，TimeRecorder会自动处理好它们，你无需额外担心其他事情;
- 递归记录本质上是嵌套记录，所以他仍然能正常的工作



为了模拟嵌套记录，示例代码中使用了下面的代码：

（<kbd>TestEmbeddedFunc</kbd>中嵌套了简单记录中的<kbd>TestSimpleFunc</kbd>和<kbd>TestHeavyFunc</kbd>）

```csharp
    // 测试嵌套记录
    private void TestEmbeddedFunc()
    {
        TimeRecorder.Instance.Record("EmbeddedFunc", () =>
        {
            TestSimpleFunc();
            TestHeavyFunc();
        });
    }
```

运行结果如下图所示，同样的你可以在<kbd>Inspector</kbd>上查看得到：

<img src="https://img-blog.csdnimg.cn/2020020703000292.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L0Fya2lzaA==,size_16,color_FFFFFF,t_70#pic_center" alt="embedded" width="350" height="350" />

- 可以很直观的发现<kbd>TestSimpleFunc</kbd>和<kbd>TestHeavyFunc</kbd>是<kbd>TestEmbeddedFunc</kbd>的子行为，以及它们各自的时间

为了模拟递归记录，示例代码中使用了下面的代码：

```csharp
    // 测试递归记录，参数是递归次数
    private void TestRecursiveFunc(int recursiveTime)
    {
        if (recursiveTime < 1) return;
        TimeRecorder.Instance.Record(
	        string.Format("TestRecursiveFunc_{0}", recursiveTime), 
	        () =>
	        {
	            HeavyFunc();
	            TestRecursiveFunc(recursiveTime - 1);
	        }
        );
    }
```

运行结果如下图所示（这里递归次数为5）：

<img src="https://img-blog.csdnimg.cn/2020020703090555.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L0Fya2lzaA==,size_16,color_FFFFFF,t_70#pic_center" alt="Recursive" width="350" height="350" />
