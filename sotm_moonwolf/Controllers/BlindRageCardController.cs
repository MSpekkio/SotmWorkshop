using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace sotm_moonwolf
{
    public class BlindRageCardController : MoonwolfCardController
    {
        public BlindRageCardController(Card card, TurnTakerController turnTakerController)
         : base(card, turnTakerController)
        {
        }

        public override IEnumerator Play()
		{
            List<DealDamageAction> storedResult = new List<DealDamageAction>();
            //Moonwolf deals up to 3 Targets 2 Melee Damage.
			IEnumerator coroutine = base.GameController.SelectTargetsAndDealDamage(this.DecisionMaker, new DamageSource(base.GameController, base.CharacterCard), 2, DamageType.Melee, 3, false, 0,
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
            //If at least one of the Targets damaged by this card was a Hero Target, then Moonwolf deals up to 2 Targets 2 Melee Damage.
            if (storedResult.Any(dealDamage => dealDamage.Target.IsHero))
            {
                coroutine = base.GameController.SelectTargetsAndDealDamage(this.DecisionMaker, new DamageSource(base.GameController, base.CharacterCard), 2, DamageType.Melee, 2, false, 0,
                                        cardSource: base.GetCardSource());
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