using UnityEngine;

public class WeaponSwitching : MonoBehaviour {

    public int selectedWeapon = 0;
    public Transform ArPrefab;
    public Transform PistolPrefab;
    public Transform RocketPrefab;
    public GunUI gunUI;

    // Use this for initialization
    void Start () {
        SelectWeapon();
	}
	
	// Update is called once per frame
	void Update () {

        int previousSelectedWeapon = selectedWeapon;

		if(Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (selectedWeapon >= transform.childCount - 1) selectedWeapon = 0;
            else selectedWeapon++;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (selectedWeapon <= 0) selectedWeapon = transform.childCount - 1;
            else selectedWeapon--;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) && transform.childCount >= 1)
        {
            selectedWeapon = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && transform.childCount >= 2)
        {
            selectedWeapon = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && transform.childCount >= 3)
        {
            selectedWeapon = 2;
        }

        if (previousSelectedWeapon != selectedWeapon) SelectWeapon();
    }

    public void AddItem(string weaponString)
    {
        Transform weapon =null;
        switch (weaponString)
        {
            case "ar":
                weapon = (Transform)Instantiate(ArPrefab, transform.position, transform.rotation);
                weapon.parent = transform;
                break;
            case "pistol":
                weapon = (Transform)Instantiate(PistolPrefab, transform.position, transform.rotation);
                weapon.parent = transform;
                break;
            case "rocket":
                weapon = (Transform)Instantiate(RocketPrefab, transform.position, transform.rotation);
                weapon.parent = transform;
                break;
        }
    }

    void SelectWeapon()
    {
        int i = 0;
        foreach(Transform weapon in transform)
        {
            if (i == selectedWeapon)
            {
                Player.setActiveGun(weapon.name);
                gunUI.SelectWeapon(weapon.name);
                weapon.gameObject.SetActive(true);
            }
            else weapon.gameObject.SetActive(false);
            i++;

        }
    }
}
