using BepInEx;

namespace Holiday
{
	[BepInPlugin("teamgrad.holiday", "Holiday", "2.0.1")]
	public class HolidayLauncher : BaseUnityPlugin
	{
		public HolidayLauncher()
		{
			HolidayBinder.UnitGlad();
		}
	}
}
