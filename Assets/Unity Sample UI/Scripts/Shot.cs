using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour {
    public void CutToShot () {
        Camera.main.transform.localPosition = this.transform.position;
        Camera.main.transform.localRotation = this.transform.rotation;
    }
}
