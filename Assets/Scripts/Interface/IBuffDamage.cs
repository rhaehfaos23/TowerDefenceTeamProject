using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuffDamage
{
    void AddDamageBuff(float amount);
    void DeleteDamageBuff(float amount);
}
