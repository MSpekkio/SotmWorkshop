using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace SotmWorkshop.Moonwolf
{
    public class BloodSacrificeCardController : MoonwolfCardController
    {
        public BloodSacrificeCardController(Card card, TurnTakerController turnTakerController)
         : base(card, turnTakerController)
        {
        }

        public override IEnumerator Play()
		{
            List<DealDamageAction> storedResult = new List<DealDamageAction>();
            //Moonwolf deals herself 2 Melee Damage, then adds 3 Tokens to the card Pull of the Moon.
            IEnumerator coroutine = base.GameController.DealDamageToSelf(this.DecisionMaker, c => c == this.CharacterCard, 2, DamageType.Melee, cardSource: base.GetCardSource());
			if (base.UseUnityCoroutines)
			{
				yield return base.GameController.StartCoroutine(coroutine);
			}
			else
			{
				base.GameController.ExhaustCoroutine(coroutine);
			}
            coroutine = base.GameController.AddTokensToPool(base.PullOfTheMoon, 3, base.GetCardSource());
            if (base.UseUnityCoroutines)
            {
                yield return base.GameController.StartCoroutine(coroutine);
            }
            else
            {
                base.GameController.ExhaustCoroutine(coroutine);
            }

            //You may play a card.
            coroutine = base.SelectAndPlayCardFromHand(this.DecisionMaker);
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
    }
}