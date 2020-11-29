using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace SotmWorkshop.Moonwolf
{
    public class HowlAtTheMoonCardController : MoonwolfCardController
    {
        public HowlAtTheMoonCardController(Card card, TurnTakerController turnTakerController)
         : base(card, turnTakerController)
        {
            SpecialStringMaker.ShowTokenPool(PullOfTheMoon);
        }

        public override IEnumerator Play()
        {
            IEnumerator coroutine;
            if (PullOfTheMoon.CurrentValue == 0)
            {
                coroutine = SendMessageAboutInsufficientTokensRemoved(0, "Moonwolf cannot heal.");
                if (base.UseUnityCoroutines)
                {
                    yield return base.GameController.StartCoroutine(coroutine);
                }
                else
                {
                    base.GameController.ExhaustCoroutine(coroutine);
                }
            }
            else
            {
                //Remove 5 Tokens from the card Pull of the Moon.
                List<RemoveTokensFromPoolAction> storedResults = new List<RemoveTokensFromPoolAction>();
                coroutine = GameController.RemoveTokensFromPool(PullOfTheMoon, 5, storedResults, optional: true, cardSource: GetCardSource());
                if (base.UseUnityCoroutines)
                {
                    yield return base.GameController.StartCoroutine(coroutine);
                }
                else
                {
                    base.GameController.ExhaustCoroutine(coroutine);
                }

                // If you do, each Hero Target regains 1 HP.
                int numberOfTokensRemoved = GetNumberOfTokensRemoved(storedResults);
                if (numberOfTokensRemoved == 5)
                {
                    coroutine = GameController.GainHP(this.DecisionMaker, card => card.IsHero, 1, cardSource: GetCardSource());
                }
                else
                {
                    coroutine = SendMessageAboutInsufficientTokensRemoved(numberOfTokensRemoved, "Moonwolf cannot heal.");
                }

                if (base.UseUnityCoroutines)
                {
                    yield return base.GameController.StartCoroutine(coroutine);
                }
                else
                {
                    base.GameController.ExhaustCoroutine(coroutine);
                }
            }

            //Increase all Damage dealt by Hero Targets by 1 until the start of your next turn.
            IncreaseDamageStatusEffect increaseDamageStatusEffect = new IncreaseDamageStatusEffect(1);
            increaseDamageStatusEffect.SourceCriteria.IsHero = true;
            increaseDamageStatusEffect.SourceCriteria.IsTarget = true;
            increaseDamageStatusEffect.UntilStartOfNextTurn(TurnTaker);
            increaseDamageStatusEffect.CardSource = Card;
            coroutine = AddStatusEffect(increaseDamageStatusEffect);
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