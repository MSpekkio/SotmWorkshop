using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace SotmWorkshop.Moonwolf
{
    public class PullOfTheMoonCardController : MoonwolfCardController
    {
        public PullOfTheMoonCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
            AddThisCardControllerToList(CardControllerListType.MakesIndestructible);

            SpecialStringMaker.ShowTokenPool(PullOfTheMoon);
        }

        public override void AddTriggers()
        {
            base.AddTrigger<DealDamageAction>(
                (DealDamageAction dealDamage) => dealDamage.Target == base.CharacterCard && dealDamage.DidDealDamage,
                (DealDamageAction dealDamage) => AddTokensResponse(dealDamage.Amount),
                TriggerType.DealDamage,
                TriggerTiming.After);
        }

        private IEnumerator AddTokensResponse(int amount)
        {
            IEnumerator coroutine = GameController.AddTokensToPool(PullOfTheMoon, amount, base.GetCardSource());
            if (base.UseUnityCoroutines)
            {
                yield return base.GameController.StartCoroutine(coroutine);
            }
            else
            {
                base.GameController.ExhaustCoroutine(coroutine);
            }
        }

        public override bool AskIfCardIsIndestructible(Card card)
        {
            return card == base.Card;
        }
    }
}