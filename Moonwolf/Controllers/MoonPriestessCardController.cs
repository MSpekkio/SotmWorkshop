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
        public MoonPriestessCardController(Card card, TurnTakerController turnTakerController)
         : base(card, turnTakerController)
        {
        }

        public override IEnumerator UsePower(int index = 0)
        {
            IEnumerator coroutine = base.GameController.AddTokensToPool(base.PullOfTheMoon, 2, cardSource: base.GetCardSource());
            if (base.UseUnityCoroutines)
            {
                yield return base.GameController.StartCoroutine(coroutine);
            }
            else
            {
                base.GameController.ExhaustCoroutine(coroutine);
            }
            List<GainHPAction> storedResult = new List<GainHPAction>();
            coroutine = base.GameController.SelectAndGainHP(this.DecisionMaker, 2,
                additionalCriteria: c => c.IsHeroCharacterCard,
                requiredDecisions: 1,
                storedResults: storedResult,
                cardSource: base.GetCardSource());
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
                coroutine = base.DrawCard(result.HpGainer.Owner.ToHero());
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