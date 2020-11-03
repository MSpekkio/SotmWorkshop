using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace sotm_moonwolf
{
    public class PullOfTheMoonCardController : MoonwolfCardController
    {
        public PullOfTheMoonCardController(Card card, TurnTakerController turnTakerController)
         : base(card, turnTakerController)
        {
            base.AddThisCardControllerToList(CardControllerListType.MakesIndestructible);

            base.SpecialStringMaker.ShowTokenPool(base.PullOfTheMoon);
        }

        public override void AddTriggers()
        {
            base.AddTrigger<DealDamageAction>(
                (DealDamageAction dealDamage) => dealDamage.Target == base.CharacterCard && dealDamage.DidDealDamage,
                (DealDamageAction dealDamage) => AddTokensResponse(dealDamage.Amount),
                TriggerType.DealDamage,
                TriggerTiming.After);
        }

        private IEnumerator AddTokensResponse(int amount)
        {
            IEnumerator coroutine = base.GameController.AddTokensToPool(base.PullOfTheMoon, amount, base.GetCardSource());
			if (base.UseUnityCoroutines)
			{
				yield return base.GameController.StartCoroutine(coroutine);
			}
			else
			{
				base.GameController.ExhaustCoroutine(coroutine);
			}
            yield break;
        }

        public override bool AskIfCardIsIndestructible(Card card)
		{
			return card == base.Card;
		}
    }
}