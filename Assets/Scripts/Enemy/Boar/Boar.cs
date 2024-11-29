using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boar : Enemy
{
    protected override void Awake()
    {
        base.Awake();  //基于父级
        patrolState  = new BoarPatrolState();//创建实例引用
        chaseState = new BoarChaseState();
    }   
}
