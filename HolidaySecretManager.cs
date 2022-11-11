using UnityEngine;
using UnityEngine.SceneManagement;

namespace Holiday
{
    public class HolidaySecretManager : MonoBehaviour
    {
        public HolidaySecretManager()
        {
            SceneManager.sceneLoaded += SceneLoaded;
        }

        public void SceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            if (scene.name == "00_Simulation_Day_VC")
            {
                var secrets = new GameObject()
                {
                    name = "Secrets"
                };
                //Instantiate(Main.holiday.LoadAsset<GameObject>("Santa_Unlock"), secrets.transform, true);
            }
        }

        public bool doneStealing;
    }
}
