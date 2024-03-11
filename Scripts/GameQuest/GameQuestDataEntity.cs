using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// һ������������ �������ɸ�������
/// </summary>
[Serializable]
public class GameQuestDataEntity
{
    public int id;//����id��С���� idԽС��Խ����Ҫ��� idΪ0��ʾ������Ϊ֧������
    public QuestPriority questPriority;
    public string name;
    public string location;
    public string description;
    public string subQuest;
    public List<string> subQeustIds = new List<string>();
}
/// <summary>
/// ������
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
    Major,//����
    Minor//֧��
}
public enum QuestType 
{
    Collect,//�ռ�
    Investigate,//���飨ָ��NPC��ͨ��ȡ�鱨��
    Defeat,//����
    Arrive//����
}
public enum QuestTargetType 
{
    NPC,
    Enemy,
    Prop,
    Scene
}