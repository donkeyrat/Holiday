using BepInEx;

namespace Holiday
{
	[BepInPlugin("teamgrad.holiday", "Holiday", "1.2.0")]
	public class HolidayLauncher : BaseUnityPlugin
	{
		public HolidayLauncher()
		{
			HolidayBinder.UnitGlad();
		}
	}
}
