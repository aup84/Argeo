using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialBarProgress : MonoBehaviour {
    public Transform LoadingBar;
    public Transform txtPercent;

    // Use this for initialization

    [SerializeField] float currentAmount;
    [SerializeField] float speed = 0.01f;

    void Update () {
        if (currentAmount <100){
            currentAmount += speed * Time.deltaTime;
            txtPercent.GetComponent<Text>().text = ((int)currentAmount).ToString() + "%";
        }

        LoadingBar.GetComponent<Image>().fillAmount = currentAmount / 100;
	}
}
