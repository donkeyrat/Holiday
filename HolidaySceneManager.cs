using UnityEngine;
using UnityEngine.SceneManagement;

namespace Holiday
{
    public class HolidaySceneManager : MonoBehaviour
    {
        public HolidaySceneManager()
        {
            SceneManager.sceneLoaded += SceneLoaded;
        }

        public void SceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            if (scene.name == "02_Lvl3_Farmer_Snow_VC")
            {
                var secrets = new GameObject()
                {
                    name = "Secrets"
                };
                Instantiate(HolidayMain.holiday.LoadAsset<GameObject>("SmoreKnight_Unlock"), secrets.transform, true);
                Instantiate(HolidayMain.holiday.LoadAsset<GameObject>("SnowCannon_Unlock"), secrets.transform, true);
                Instantiate(HolidayMain.holiday.LoadAsset<GameObject>("JollyBot_Unlock"), secrets.transform, true);
            }
        }
    }
}
