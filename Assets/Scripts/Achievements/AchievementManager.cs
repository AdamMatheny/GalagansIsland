using UnityEngine;
using System.Collections.Generic;
using System;
using ManagedSteam;

namespace Assets.Scripts.Achievements
{
    [Serializable]
    public class AchievementBase
    {
        public string AchievementSteamID;
        public bool IsUnlocked;
    }

    public class AchievementManager : MonoBehaviour
    {
        public static AchievementManager instance;

        public List<AchievementBase> AchievementList;

        //Level Dependent Achievements
        private int currentLevel = -1;
        private int livesBeforeBoss;
        private ScoreManager scoreManager;

        //overheat Achievement
        public int numberOfOverheats;

        //RepairStation Dependent Stats
        [Header("Repair Station Stats")]
        public AchievementStatInt RepairShip100Times;
        public AchievementStatInt UpgradeWeapons100Times;
        public AchievementStatInt UpgradeSpeed100Times;
        public AchievementStatInt UpgradeOnlyWeapons;
        public AchievementStatInt UpgradeOnlySpeed;

        //Kill Counters
        [Header("Kill Stats")]
        public AchievementStatInt KillCounter1;
        public AchievementStatInt KillCounter2;
        public AchievementStatInt KillCounter3;
        public AchievementStatInt KillCounter4;
        public AchievementStatInt KillCounter5;

        //Other Counters
        [Header("Other Stats")]
        public AchievementStatInt BackStab;
        public AchievementStatTimer FriendOMine;
        public AchievementStatInt UpgradesCollected;
        public AchievementStatInt IDontCare;

        void Awake()
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }

        void Start()
        {
            LoadAchievements();
        }

        void Update()
        {
            HandleLevelAchievements();
        }

        private bool IsAchievementUnlocked(string achievementID)
        {
            foreach (var achiv in AchievementList)
            {
                if (achiv.AchievementSteamID == achievementID)
                {
                    return achiv.IsUnlocked;
                }
            }
            return true;
        }

        private void SetAchievementAsUnlocked(string achievementID)
        {
            foreach (var achiv in AchievementList)
            {
                if (achiv.AchievementSteamID == achievementID)
                {
                    achiv.IsUnlocked = true;
                    PlayerPrefs.SetInt("ACHIV_" + achievementID, 1);
                    return;
                }
            }
            HandleMasterOfGalangans();
        }

        private void LoadAchievements()
        {
            foreach (var achiv in AchievementList)
            {
                if (PlayerPrefs.HasKey("ACHIV_" + achiv.AchievementSteamID))
                {
                    achiv.IsUnlocked = true;
                }
                else
                {
                    achiv.IsUnlocked = false;
                }
            }
        }

        public void PostAchievement(string achievementID)
        {
            if (IsAchievementUnlocked(achievementID) == false)
            {
                Debug.Log("Achievement " + achievementID + " has been Unlocked!");
                Steamworks.SteamInterface.Stats.SetAchievement(achievementID);
                SetAchievementAsUnlocked(achievementID);
            }
        }

        private void HandleLevelAchievements()
        {
            if (currentLevel != Application.loadedLevel)
            {
                try
                {
                    scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
                }
                catch (Exception e)
                {

                }

                currentLevel = Application.loadedLevel;
                switch (currentLevel)
                {
					case 1:
						numberOfOverheats = 0;
						break;
                    case 2:
                        if (scoreManager.mP1Lives == 100)
                        {
                            PostAchievement("CantTouchThis"); // No lifes lost till lvl 1.
                            //numberOfOverheats = 0;
                        }
                        break;
                    case 6:
                        if (scoreManager.mP1Lives == 100)
                        {
                            PostAchievement("FasterThanLight"); // No lifes lost till lvl 5.
                        }
                        livesBeforeBoss = scoreManager.mP1Lives;
                        break;
                    case 7:
                        PostAchievement("A_Boss1");
                        if (livesBeforeBoss >= scoreManager.mP1Lives)
                        {
                            PostAchievement("A_Boss1F");
                        }
                        break;
                    case 12:
                        livesBeforeBoss = scoreManager.mP1Lives;
                        break;
                    case 13:
                        PostAchievement("A_Boss2");
                        if (livesBeforeBoss >= scoreManager.mP1Lives)
                        {
                            PostAchievement("A_Boss2F");
                        }
                        break;
                    case 19:
                        livesBeforeBoss = scoreManager.mP1Lives;
                        break;
                    case 20:
                        PostAchievement("A_Boss3");
                        if (livesBeforeBoss >= scoreManager.mP1Lives)
                        {
                            PostAchievement("A_Boss3F");
                        }
                        break;
                    case 24:
                        livesBeforeBoss = scoreManager.mP1Lives;
                        break;
                    case 25:
                        PostAchievement("A_Boss4");
                        if (livesBeforeBoss >= scoreManager.mP1Lives)
                        {
                            PostAchievement("A_Boss4F");
                        }
                        break;
                    case 30:
                        livesBeforeBoss = scoreManager.mP1Lives;
                        break;
                    case 32:
                        PostAchievement("A_Boss5");
                        PostAchievement("ItsOverIsntIt");
                        if (livesBeforeBoss >= scoreManager.mP1Lives)
                        {
                            PostAchievement("A_Boss5F");
                        }
                        if (numberOfOverheats == 0)
                        {
                            PostAchievement("CoolFire");
                        }
                        break;
                }
            }
        }

        private void HandleMasterOfGalangans()
        {
            int amountOfFinishedAchievements = 0;
            foreach(var achiv in AchievementList)
            {
                if (achiv.IsUnlocked) amountOfFinishedAchievements++;
            }
            
            if (amountOfFinishedAchievements >= AchievementList.Count-1)
            {
                PostAchievement("MasterofGalagan");
            }
        }
    }
}
