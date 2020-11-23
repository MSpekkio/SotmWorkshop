using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace SotmWorkshop.Moonwolf
{
    public class TasteForBloodCardController : MoonwolfCardController
    {
        public TasteForBloodCardController(Card card, TurnTakerController turnTakerController)
         : base(card, turnTakerController)
        {
        }

        public override IEnumerator Play()
		{
            List<DealDamageAction> storedResult = new List<DealDamageAction>();
            //Moonwolf deals 1 Target 4 Melee damage.
			IEnumerator coroutine = base.GameController.SelectTargetsAndDealDamage(this.DecisionMaker, new DamageSource(base.GameController, base.CharacterCard), 4, DamageType.Melee, 1, false, 0,
                                        storedResultsDamage: storedResult,
                                        cardSource: base.GetCardSource());
			if (base.UseUnityCoroutines)
			{
				yield return base.GameController.StartCoroutine(coroutine);
			}
			else
			{
				base.GameController.ExhaustCoroutine(coroutine);
			}
            //If the Target took damage this way, Moonwolf regains 2 HP.
            if (storedResult.Any(dealDamage => dealDamage.DidDealDamage))
            {
                coroutine = base.GameController.GainHP(base.CharacterCard, 2, cardSource: base.GetCardSource());
                if (base.UseUnityCoroutines)
                {
                    yield return base.GameController.StartCoroutine(coroutine);
                }
                else
                {
                    base.GameController.ExhaustCoroutine(coroutine);
                }
            }
			yield break;
		}
    }
}