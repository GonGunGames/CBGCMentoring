using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AllUnits;

public class HitBox : Unit
{
    public float attackdamage;

  protected override void Start()
{
    base.Start();
    attackdamage = damage;  // 'damage'가 제대로 초기화되었는지 확인
}

    // Update is called once per frame
    void Update()
    {
        
    }
}
