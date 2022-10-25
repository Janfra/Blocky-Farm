using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : Shooting
{
    #region Variables

    /// <summary>
    /// Aim position
    /// </summary>
    [SerializeField] private Transform aim;

    /// <summary>
    /// Sets if the bullet can be shot
    /// </summary>
    private bool isReadyToShot => Time.time > timeTillShot;

    /// <summary>
    /// Sets when the bullet can be shot again
    /// </summary>
    public float timeTillShot { get; private set; }

    /// <summary>
    /// Current amount of bullets available
    /// </summary>
    private int ammo;

    #endregion

    private void Start()
    {
        ammo = bullet.MaxAmmo;
    }

    void Update()
    {
        if (isReadyToShot && Input.GetMouseButton(0))
        {
            Shot();
            //Debug.Log(timeTillShot - Time.time);
        }
    }

    #region Shooting Logic

    /// <summary>
    /// Sets ammo to max and player cant shoot till reload time is done.
    /// </summary>
    protected override void Reload()
    {
        timeTillShot = Time.time + bullet.ReloadTime;
        ammo = bullet.MaxAmmo;
    }

    /// <summary>
    /// Spawns bullet and checks the ammo.
    /// </summary>
    protected override void Shot()
    {
        SpawnBulletAt(aim);
        CheckAmmo();
    }

    /// <summary>
    /// Takes ammo away, sets new time until next shot is available and checks if reload is neccessary. 
    /// </summary>
    private void CheckAmmo()
    {
        ammo--;
        if(ammo <= 0 && bullet.ReloadTime != 0)
        {
            Reload();
            // Do reload visual stuff
        } 
        else
        {
            timeTillShot = Time.time + bullet.TimePerShot;
            // Do UI stuff
        }
    }

    #endregion
}
