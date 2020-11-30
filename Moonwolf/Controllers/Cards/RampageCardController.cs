using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace SotmWorkshop.Moonwolf
{
    public class RampageCardController : MoonwolfCardController
    {
        public RampageCardController(Card card, TurnTakerController turnTakerController)
         : base(card, turnTakerController)
        {
            SpecialStringMaker.ShowNumberOfCardsInPlay(new LinqCardCriteria(c => c.IsOngoing || c.IsEnvironment, "ongoing or environment"));
        }

        public override IEnumerator Play()
        {
            //Moonwolf deals herself 1 Melee Damage.
            IEnumerator coroutine = DealDamage(CharacterCard, CharacterCard, 1, DamageType.Melee, cardSource: base.GetCardSource());
            if (base.UseUnityCoroutines)
            {
                yield return base.GameController.StartCoroutine(coroutine);
            }
            else
            {
                base.GameController.ExhaustCoroutine(coroutine);
            }
            //Destroy up to 2 Ongoing or Environment cards.
            List<DestroyCardAction> storedResults = new List<DestroyCardAction>();
            coroutine = GameController.SelectAndDestroyCards(DecisionMaker, new LinqCardCriteria((Card c) => c.IsInPlay && (c.IsOngoing || c.IsEnvironment), "ongoing or enviroment"), 2,
                requiredDecisions: 0,
                storedResultsAction: storedResults,
                cardSource: GetCardSource());
            if (base.UseUnityCoroutines)
            {
                yield return base.GameController.StartCoroutine(coroutine);
            }
            else
            {
                base.GameController.ExhaustCoroutine(coroutine);
            }

            //Moonwolf regains 1 HP for each card destroyed in this way.
            int numberOfCardsDestroyed = GetNumberOfCardsDestroyed(storedResults);
            coroutine = GameController.GainHP(CharacterCard, numberOfCardsDestroyed, cardSource: GetCardSource());
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