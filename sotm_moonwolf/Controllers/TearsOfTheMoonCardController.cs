using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace sotm_moonwolf
{
    public class TearsOfTheMoonCardController : MoonwolfCardController
    {
        public TearsOfTheMoonCardController(Card card, TurnTakerController turnTakerController)
         : base(card, turnTakerController)
        {
        }

        public override void AddTriggers()
		{
            //base.AddIncreaseDamageTrigger(dealDamage => dealDamage.DamageType == DamageType.Melee && dealDamage.DamageSource.IsSameCard(base.CharacterCard), 2);

			base.AddStartOfTurnTrigger(tt => tt == base.TurnTaker, p => base.GameController.GainHP(this.CharacterCard, 1), TriggerType.GainHP);

			base.AddPreventDamageTrigger(dd => dd.DidDealDamage && dd.Target == this.CharacterCard, PreventDamageRespose);
		}

        private IEnumerator PreventDamageRespose(DealDamageAction dealDamage)
		{
            List<RemoveTokensFromPoolAction> storedResults = new List<RemoveTokensFromPoolAction>();
            IEnumerator coroutine = base.GameController.RemoveTokensFromPool(this.PullOfTheMoon, 2, storedResults, optional:true, cardSource: base.GetCardSource());
            if (base.UseUnityCoroutines)
			{
				yield return base.GameController.StartCoroutine(coroutine);
			}
			else
			{
				base.GameController.ExhaustCoroutine(coroutine);
			}
            if (base.DidRemoveTokens(storedResults, 2))
            {
				coroutine = base.GameController.GainHP(this.CharacterCard, 1, cardSource: base.GetCardSource());
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