using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace SotmWorkshop.Moonwolf
{
    public class TearsOfTheMoonCardController : MoonwolfCardController
    {
        public TearsOfTheMoonCardController(Card card, TurnTakerController turnTakerController)
         : base(card, turnTakerController)
        {
            SpecialStringMaker.ShowTokenPool(PullOfTheMoon);
        }

        public override void AddTriggers()
        {
            base.AddStartOfTurnTrigger(tt => tt == base.TurnTaker, p => base.GameController.GainHP(this.CharacterCard, 1), TriggerType.GainHP);

            base.AddTrigger<DealDamageAction>(dd => dd.DidDealDamage && dd.Target == this.CharacterCard, DealDamageResponse, TriggerType.GainHP, TriggerTiming.After);
        }

        private IEnumerator DealDamageResponse(DealDamageAction dealDamage)
        {
            if (PullOfTheMoon.CurrentValue >= 2)
            {
                List<RemoveTokensFromPoolAction> storedResults = new List<RemoveTokensFromPoolAction>();
                IEnumerator coroutine = base.GameController.RemoveTokensFromPool(this.PullOfTheMoon, 2, storedResults,
                                            optional: true,
                                            gameAction: dealDamage,
                                            cardSource: GetCardSource());
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
            }
            yield break;
        }
    }
}