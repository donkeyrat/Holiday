using BepInEx;
using TGCore;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Holiday
{
	[BepInPlugin("teamgrad.holiday", "Holiday", "2.1.0")]
	[BepInDependency("teamgrad.core")]
	public class HolidayLauncher : TGMod
	{
		public HolidayLauncher()
		{
			new HolidayMain();
		}

		public override void SceneManager(Scene scene, LoadSceneMode loadSceneMode)
		{
			if (scene.name == "02_Lvl3_Farmer_Snow_VC")
			{
				var secrets = new GameObject
				{
					name = "Secrets"
				};
				Instantiate(HolidayMain.holiday.LoadAsset<GameObject>("SmoreKnight_Unlock"), secrets.transform, true);
				Instantiate(HolidayMain.holiday.LoadAsset<GameObject>("SnowCannon_Unlock"), secrets.transform, true);
				Instantiate(HolidayMain.holiday.LoadAsset<GameObject>("JollyBot_Unlock"), secrets.transform, true);
				if (!HolidayMain.HasWesternMap)
				{
					Instantiate(HolidayMain.holiday.LoadAsset<GameObject>("Snowman_Unlock"), secrets.transform, true);
				}
			}
			
			if (scene.name == "WesternChristmas")
			{
				var secrets = new GameObject
				{
					name = "Secrets"
				};
				var snowman = Instantiate(HolidayMain.holiday.LoadAsset<GameObject>("Snowman_Unlock"), secrets.transform, true);
				snowman.transform.position = new Vector3(9.225f, 1.5f, 45.87f);
				snowman.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
			}
		}
	}
}
