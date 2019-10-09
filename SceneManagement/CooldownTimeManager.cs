using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CooldownTimeManager : MonoBehaviour
{
    public Transform LoadingBar;
    public Transform ItemImage;
    //public Transform TextLoading;

    [SerializeField] private float mCurrentAmount;
    [SerializeField] private float mSpeed;

    [SerializeField] private RopeSystemPrac mRopeSystemPrac;
    [SerializeField] private JetpackSystem mJetpackSystem;

    // TODO: Caching 2 variables for performance.

    private Image hookItemImage;
    private Image hookLoadingBarGauge;

    private Image jetpackItemImage;
    private Image jetpackILoadingBarGauge;

    // Start is called before the first frame update
    void Awake()
    {
        hookItemImage = ItemImage.GetComponent<Image>();
        hookLoadingBarGauge = LoadingBar.GetComponent<Image>();
        
        jetpackItemImage = ItemImage.GetComponent<Image>();
        jetpackILoadingBarGauge = LoadingBar.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        // bool tempBool = false;  // comment it out after set the boolean type in PlayerMovement or RopeSystem..
        ShowHookCooldown();
        ShowJetpackCooldown();

    }

    private void ShowJetpackCooldown()
    {
        if (gameObject.name == "JetpackCooldownCircle")
        {
            mCurrentAmount = mJetpackSystem.JetpackBoostGauge / mJetpackSystem.MaxJetpackBoostGauge;
            jetpackILoadingBarGauge.fillAmount = mCurrentAmount;
            if (mJetpackSystem.BJetpackReady == false)
            {
                Color32 newColor = new Color32(0xD1, 0xD1, 0xD1, 0xFF);
                // mCurrentAmount = mJetpackSystem.JetpackBoostGauge;
                //mCurrentAmount += mSpeed * Time.deltaTime;

                jetpackItemImage.color = Color.red;
                jetpackILoadingBarGauge.color = newColor;
                // jetpackILoadingBarGauge.fillAmount = mCurrentAmount;

                // D1D1D1
                //LoadingBar.gameObject.SetActive(true);
            }
            else
            {
                //LoadingBar.gameObject.SetActive(false);
                // Color32 newColor = new Color32(0xD1, 0xD1, 0xD1, 0xFF);
                // LoadingBar.GetComponent<Image>().color = newColor;
                jetpackItemImage.color = Color.white;
            }
        }
    }

    private void ShowHookCooldown()
    {
        // TODO: check if calling from AnchorHook UI.
        if (gameObject.name == "AnchorHookCooldownCircle")
        {
            mCurrentAmount = mRopeSystemPrac.CooldownTime / mRopeSystemPrac.MaxCooldownTime;
            if (mRopeSystemPrac.BRopeReady == false)
            {
                Color32 newColor = new Color32(0xD1, 0xD1, 0xD1, 0xFF);

                //mCurrentAmount += mSpeed * Time.deltaTime;

                hookItemImage.color = Color.red;
                hookLoadingBarGauge.color = Color.red;
                hookLoadingBarGauge.fillAmount = mCurrentAmount;

                // D1D1D1
                //LoadingBar.gameObject.SetActive(true);
            }
            else
            {
                //LoadingBar.gameObject.SetActive(false);
                Color32 newColor = new Color32(0xD1, 0xD1, 0xD1, 0xFF);
                hookLoadingBarGauge.color = newColor;
                hookItemImage.color = Color.white;
            }
        }

        
    }
}
