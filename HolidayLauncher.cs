using BepInEx;

namespace Holiday
{
	[BepInPlugin("teamgrad.holiday", "Holiday", "2.0.2")]
	public class HolidayLauncher : BaseUnityPlugin
	{
		public HolidayLauncher()
		{
			HolidayBinder.UnitGlad();
		}
	}
}
