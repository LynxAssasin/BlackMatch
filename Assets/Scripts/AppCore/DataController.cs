using UnityEngine;
namespace Game.Data
{
    public class DataController : MonoBehaviour
    {
        private const string LEVEL_KEY = "Level";
        private const string ALL_STARS = "AllStars";

        public void Init()
        {
            CommandKeeper.Instance.SaveStars += SaveStars;
            CommandKeeper.Instance.GetStars += GetStars;
            CommandKeeper.Instance.GetAllStars += GetAllStars; 
        }

        public void SaveStars(int level, int stars)
        {
            PlayerPrefs.SetInt(LEVEL_KEY + level.ToString(), stars);
            SetAllStars(stars);
        }

        private void SetAllStars(int stars)
        {
            if (!PlayerPrefs.HasKey(ALL_STARS))
            {
                PlayerPrefs.SetInt(ALL_STARS, stars);
            }
            else
            {
                PlayerPrefs.SetInt(ALL_STARS, stars + PlayerPrefs.GetInt(ALL_STARS));
            }
        }

        public int GetStars(int level)
        {
            if (PlayerPrefs.HasKey(LEVEL_KEY + level.ToString()))
            {
               return PlayerPrefs.GetInt(LEVEL_KEY + level.ToString());
            }
            else
            {
                return -1;  
            }
        }

        public int GetAllStars()
        {
            if (!PlayerPrefs.HasKey(ALL_STARS))
            {
                return 0; 
            }
            else
            {
                return PlayerPrefs.GetInt(ALL_STARS);
            }
        }

        private void OnDestroy()
        {
            CommandKeeper.Instance.SaveStars -= SaveStars;
            CommandKeeper.Instance.GetStars -= GetStars;
            CommandKeeper.Instance.GetAllStars -= GetAllStars;
        }
    }
}