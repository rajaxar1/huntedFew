using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunUI : MonoBehaviour {

    public void SelectWeapon(string weaponName)
    {
        foreach (Transform weapon in transform)
        {
            if (weapon.name == weaponName)
            {
                Debug.Log(weapon.name);
                weapon.gameObject.SetActive(true);
            }
            else weapon.gameObject.SetActive(false);

        }
    }
}
