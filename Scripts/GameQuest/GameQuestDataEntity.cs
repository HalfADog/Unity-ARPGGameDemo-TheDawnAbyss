using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 一个完整的任务 包含若干个子任务
/// </summary>
[Serializable]
public class GameQuestDataEntity
{
    public int id;//按照id大小排序 id越小就越先需要完成 id为0表示此任务为支线任务
    public QuestPriority questPriority;
    public string name;
    public string location;
    public string description;
    public string subQuest;
    public List<string> subQeustIds = new List<string>();
}
/// <summary>
/// 子任务
/// </summary>
[Serializable]
public class GameSubQuestDataEntity 
{
    public string id;
    public QuestType questType;
    public string name;
    public QuestTargetType targetType;
    public string targetName;
    public int value;
}

public enum QuestPriority 
{
    Major,//主线
    Minor//支线
}
public enum QuestType 
{
    Collect,//收集
    Investigate,//调查（指和NPC沟通获取情报）
    Defeat,//击败
    Arrive//到达
}
public enum QuestTargetType 
{
    NPC,
    Enemy,
    Prop,
    Scene
}