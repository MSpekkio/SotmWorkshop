using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace sotm_moonwolf
{
    public class ChannelTheMoonCardController : MoonwolfCardController
    {
        public ChannelTheMoonCardController(Card card, TurnTakerController turnTakerController)
         : base(card, turnTakerController)
        {
        }

        public override void AddTriggers()
        {
            base.AddRedirectDamageTrigger(dd => dd.Target.IsHero && dd.Target != base.CharacterCard, () => base.CharacterCard, false);
            base.AddTrigger<DealDamageAction>(dd => dd.Target == base.CharacterCard, DealDamageReponse, TriggerType.WouldBeDealtDamage, TriggerTiming.Before);
            base.AddStartOfTurnTrigger(tt => tt == base.TurnTaker, p => base.GameController.DestroyCard(this.DecisionMaker, base.Card, cardSource: base.GetCardSource()), TriggerType.DestroySelf);
        }

        private IEnumerator DealDamageReponse(DealDamageAction dealDamage)
        {
            List<RemoveTokensFromPoolAction> storedResults = new List<RemoveTokensFromPoolAction>();
            IEnumerator coroutine = base.GameController.RemoveTokensFromPool(this.PullOfTheMoon, dealDamage.Amount, storedResults, optional: true, cardSource: base.GetCardSource());
            if (base.UseUnityCoroutines)
            {
                yield return base.GameController.StartCoroutine(coroutine);
            }
            else
            {
                base.GameController.ExhaustCoroutine(coroutine);
            }
            if (base.DidRemoveTokens(storedResults, dealDamage.Amount))
            {
                coroutine = this.CancelAction(dealDamage, isPreventEffect: true);
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