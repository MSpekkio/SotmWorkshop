using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace SotmWorkshop.Moonwolf
{
    public class MoonPriestessCardController : MoonwolfCardController
    {
        public MoonPriestessCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override IEnumerator UsePower(int index = 0)
        {
            //Power: Add 2 Tokens to the card Pull of the Moon, then select a Hero Character card to regain 2 HP, and that Hero's player may draw a card.

            int tokens = GetPowerNumeral(0, 2);
            int regains = GetPowerNumeral(1, 2);

            IEnumerator coroutine = GameController.AddTokensToPool(PullOfTheMoon, tokens, cardSource: GetCardSource());
            if (base.UseUnityCoroutines)
            {
                yield return base.GameController.StartCoroutine(coroutine);
            }
            else
            {
                base.GameController.ExhaustCoroutine(coroutine);
            }
            List<GainHPAction> storedResult = new List<GainHPAction>();
            coroutine = base.GameController.SelectAndGainHP(DecisionMaker, regains,
                additionalCriteria: c => c.IsHeroCharacterCard,
                requiredDecisions: 1,
                storedResults: storedResult,
                cardSource: GetCardSource());
            if (base.UseUnityCoroutines)
            {
                yield return base.GameController.StartCoroutine(coroutine);
            }
            else
            {
                base.GameController.ExhaustCoroutine(coroutine);
            }
            if (storedResult.Count > 0)
            {
                var result = storedResult.First();
                coroutine = DrawCard(result.HpGainer.Owner.ToHero());
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