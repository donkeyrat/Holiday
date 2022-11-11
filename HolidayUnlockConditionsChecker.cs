using UnityEngine;
using System.Collections.Generic;

namespace Holiday
{
    public class HolidayUnlockConditionsChecker : MonoBehaviour
    {
        public void Start()
        {
            conditions = HolidayMain.holiday.LoadAsset<SecretUnlockConditions>("HolidayUnlockConditions");
        }

		public void Update()
        {
            if (!ServiceLocator.GetService<ISaveLoaderService>().HasUnlockedSecret("SECRET_HOLIDAYCAMPAIGN"))
            {
                foreach (var condition in conditions.m_unlockConditions[0].m_conditionUnlocks)
                {
                    if (ServiceLocator.GetService<ISaveLoaderService>().HasUnlockedSecret(condition) && !unlocked.Contains(condition))
                    {
                        unlocked.Add(condition);
                    }
                }
                conditions.CheckUnlockCondition(unlocked);
            }
            else
            {
                Destroy(gameObject);
            }
        }


		public SecretUnlockConditions conditions;

        public List<string> unlocked = new List<string>();
    }
}
