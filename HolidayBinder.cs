using System.Collections;
using UnityEngine;

namespace Holiday
{
    public class HolidayBinder : MonoBehaviour
    {

        public static void UnitGlad()
        {
            if (!instance)
            {
                instance = new GameObject
                {
                    hideFlags = HideFlags.HideAndDontSave
                }.AddComponent<HolidayBinder>();
            }
            instance.StartCoroutine(StartUnitgradLate());
        }

        private static IEnumerator StartUnitgradLate()
        {
            yield return new WaitUntil(() => FindObjectOfType<ServiceLocator>() != null);
            yield return new WaitUntil(() => ServiceLocator.GetService<ISaveLoaderService>() != null);
            new HolidayMain();
            yield break;
        }

        private static HolidayBinder instance;
    }
}