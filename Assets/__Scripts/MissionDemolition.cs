using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameMode 
{
    idle,
    playing,
    levelEnd

}

public class MissionDemolition : MonoBehaviour
{
    static private MissionDemolition S;

    [Header("Set in Inspector")]
    public Text uitLevel;
    public Text uitShots;
    public Text uitBitton;
    public Vector3 castlePos;
    public GameObject[] castkes;

    [Header("Set in Dynamically")]
    public int level;
    public int levelMax;
    public int shotsTaken;
    public GameObject castle;
    public GameMode mode = GameMode.idle;
    public string showing = "Show SlingShot";

    void Start()
    {
        S = this;

        level = 0;
        levelMax = castkes.Length;
        StartLevel();
    }

    void StartLevel() 
    {
        if (castle != null) 
        {
            Destroy(castle);
        }

        GameObject[] gos = GameObject.FindGameObjectsWithTag("Projectile");
        foreach (GameObject pTemp in gos) 
        {
            Destroy(pTemp);
        }

        castle = Instantiate<GameObject>(castkes[level]);
        castle.transform.position = castlePos;
        shotsTaken = 0;

        SwitchView("Show Both");
        ProjectileLine.S.Clear();

        Goal.goalMet = false;

        UpdateGUI();

        mode = GameMode.playing;
    }

    void UpdateGUI() 
    {
        uitLevel.text = "Level:" + (level + 1) + "of" + levelMax;
        uitShots.text = "Shots Taken:" + shotsTaken; 
    }

    void Update()
    {
        UpdateGUI();
        
        if ((mode == GameMode.playing) && Goal.goalMet) 
        {
            mode = GameMode.levelEnd;
            SwitchView("Show Both");
            Invoke("NextLevel", 2f);
        }
    }

    void NextLevel() 
    {
        level++;
        if (level == levelMax) 
        {
            level = 0;
        }

        StartLevel();
    }

    public void SwitchView(string eView = "") 
    {
        if (eView == "") 
        {
            eView = uitBitton.text;
        }
        showing = eView;
        switch (showing) 
        {
            case "Show SlingShot":
                FolloCam.POI = null;
                uitBitton.text = "Shoe Castle";
                break;

            case "Show Castlr":
                FolloCam.POI = S.castle;
                uitBitton.text = "Show Both";
                break;

            case "Show Both":
                FolloCam.POI = GameObject.Find("ViewBoth");
                uitBitton.text = "Show SlingShot";
                break;

        }
    }

    public static void ShotFired() 
    {
        S.shotsTaken++;
    }

}
