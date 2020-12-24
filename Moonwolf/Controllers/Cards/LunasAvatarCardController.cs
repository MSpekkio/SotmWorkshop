using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace SotmWorkshop.Moonwolf
{
    public class LunasAvatarCardController : MoonwolfCardController
    {
        public LunasAvatarCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
            SpecialStringMaker.ShowTokenPool(PullOfTheMoon);
        }

        public override void AddTriggers()
        {
            base.AddIncreaseDamageTrigger(dealDamage => !dealDamage.Target.IsHero && dealDamage.DamageType == DamageType.Melee && dealDamage.DamageSource.IsSameCard(CharacterCard), 2);

            base.AddStartOfTurnTrigger(tt => tt == TurnTaker, p => RemoveTokensOrDestroyThisCardResponse(p), TriggerType.DestroySelf);
        }

        private IEnumerator RemoveTokensOrDestroyThisCardResponse(PhaseChangeAction phaseChange)
        {
            IEnumerator coroutine;
            bool destroySelf = true;
            if (PullOfTheMoon.CurrentValue >= 3)
            {
                List<RemoveTokensFromPoolAction> storedResults = new List<RemoveTokensFromPoolAction>();
                coroutine = GameController.RemoveTokensFromPool(PullOfTheMoon, 3, storedResults, optional: true, gameAction: phaseChange, cardSource: GetCardSource());
                if (base.UseUnityCoroutines)
                {
                    yield return base.GameController.StartCoroutine(coroutine);
                }
                else
                {
                    base.GameController.ExhaustCoroutine(coroutine);
                }
                int numberOfTokens = GetNumberOfTokensRemoved(storedResults);
                if (numberOfTokens != 3)
                {
                    coroutine = SendMessageAboutInsufficientTokensRemoved(numberOfTokens, $"{Card.Title} will be destroyed.");
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
                    destroySelf = false;
                }
            }
            else
            {
                coroutine = SendMessageAboutInsufficientTokensRequired(3, $"{Card.Title} will be destroyed.");
                if (base.UseUnityCoroutines)
                {
                    yield return base.GameController.StartCoroutine(coroutine);
                }
                else
                {
                    base.GameController.ExhaustCoroutine(coroutine);
                }
            }
            if (destroySelf)
            {
                coroutine = GameController.DestroyCard(DecisionMaker, Card, cardSource: GetCardSource());
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
    }
}