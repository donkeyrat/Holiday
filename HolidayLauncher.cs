using BepInEx;
using TGCore;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Holiday
{
	[BepInPlugin("teamgrad.holiday", "Holiday", "2.0.4")]
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
			}
		}
	}
}
