using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace SotmWorkshop.Moonwolf
{
    public class ChannelTheMoonCardController : MoonwolfCardController
    {
        public ChannelTheMoonCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override void AddTriggers()
        {
            base.AddRedirectDamageTrigger(dd => dd.Target.IsHero && dd.Target != CharacterCard, () => CharacterCard, false);
            base.AddTrigger<DealDamageAction>(dd => dd.Target == CharacterCard, DealDamageReponse, TriggerType.WouldBeDealtDamage, TriggerTiming.Before);
            base.AddStartOfTurnTrigger(tt => tt == TurnTaker, p => GameController.DestroyCard(DecisionMaker, Card, cardSource: GetCardSource()), TriggerType.DestroySelf);
        }

        private IEnumerator DealDamageReponse(DealDamageAction dealDamage)
        {
            IEnumerator coroutine;
            if (PullOfTheMoon.CurrentValue >= dealDamage.Amount)
            {
                List<RemoveTokensFromPoolAction> storedResults = new List<RemoveTokensFromPoolAction>();
                coroutine = GameController.RemoveTokensFromPool(PullOfTheMoon, dealDamage.Amount, storedResults, optional: true, cardSource: GetCardSource());
                if (base.UseUnityCoroutines)
                {
                    yield return base.GameController.StartCoroutine(coroutine);
                }
                else
                {
                    base.GameController.ExhaustCoroutine(coroutine);
                }
                if (DidRemoveTokens(storedResults, dealDamage.Amount))
                {
                    coroutine = CancelAction(dealDamage, isPreventEffect: true);
                    if (base.UseUnityCoroutines)
                    {
                        yield return base.GameController.StartCoroutine(coroutine);
                    }
                    else
                    {
                        base.GameController.ExhaustCoroutine(coroutine);
                    }
                }
            }
            else
            {
                coroutine = SendMessageAboutInsufficientTokensRequired(dealDamage.Amount, "damage cannot be prevented.");
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