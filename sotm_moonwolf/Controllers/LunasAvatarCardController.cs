using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace sotm_moonwolf
{
    public class LunasAvatarCardController : MoonwolfCardController
    {
        public LunasAvatarCardController(Card card, TurnTakerController turnTakerController)
         : base(card, turnTakerController)
        {
        }

        public override void AddTriggers()
		{
            base.AddIncreaseDamageTrigger(dealDamage => dealDamage.DamageType == DamageType.Melee && dealDamage.DamageSource.IsSameCard(base.CharacterCard), 2);

			base.AddStartOfTurnTrigger(tt => tt == base.TurnTaker, p => this.RemoveTokensOrDestroyThisCardResponse(p), TriggerType.DestroySelf);
		}

        private IEnumerator RemoveTokensOrDestroyThisCardResponse(PhaseChangeAction phaseChange)
		{
            List<RemoveTokensFromPoolAction> storedResults = new List<RemoveTokensFromPoolAction>();
            IEnumerator coroutine = base.GameController.RemoveTokensFromPool(this.PullOfTheMoon, 3, storedResults, optional:true, cardSource: base.GetCardSource());
            if (base.UseUnityCoroutines)
			{
				yield return base.GameController.StartCoroutine(coroutine);
			}
			else
			{
				base.GameController.ExhaustCoroutine(coroutine);
			}
            if (!base.DidRemoveTokens(storedResults, 3))
            {
                coroutine = base.GameController.DestroyCard(this.DecisionMaker, base.Card, cardSource: base.GetCardSource());
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