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
        }

        public override void AddTriggers()
        {
            base.AddIncreaseDamageTrigger(dealDamage => dealDamage.DamageType == DamageType.Melee && dealDamage.DamageSource.IsSameCard(CharacterCard), 2);

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
            if (!base.DidRemoveTokens(storedResults, 3))
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
            yield break;
        }
    }
}