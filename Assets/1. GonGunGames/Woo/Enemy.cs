using UnityEngine;
using AllUnits;

public class Enemy : Unit
{
    protected override void Start()
    {
        base.Start();
        damage = 8f; // 적의 공격력을 8로 설정
    }
}