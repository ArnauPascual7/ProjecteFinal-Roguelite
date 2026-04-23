using UnityEngine;

public class ProjectileWeaponRuntimeState : RangedWeaponRuntimeState
{
    public int currentMagazine;
    public bool reloading;
    public Coroutine reloadCoroutine;
}
