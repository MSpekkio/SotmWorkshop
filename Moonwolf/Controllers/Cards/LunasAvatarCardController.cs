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
        public LunasAvatarCardController(Card card, TurnTakerController turnTakerController)
         : base(card, turnTakerController)
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
            List<RemoveTokensFromPoolAction> storedResults = new List<RemoveTokensFromPoolAction>();
            IEnumerator coroutine = GameController.RemoveTokensFromPool(PullOfTheMoon, 3, storedResults, optional: true, gameAction: phaseChange, cardSource: GetCardSource());
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
                coroutine = SendMessageAboutInsufficientTokens(numberOfTokens, $"{Card.Title} will be destroyed.");
                if (base.UseUnityCoroutines)
                {
                    yield return base.GameController.StartCoroutine(coroutine);
                }
                else
                {
                    base.GameController.ExhaustCoroutine(coroutine);
                }

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
            yield break;
        }
    }
}