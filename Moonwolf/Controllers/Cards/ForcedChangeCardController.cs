using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace SotmWorkshop.Moonwolf
{
    public class ForcedChangeCardController : MoonwolfCardController
    {
        public ForcedChangeCardController(Card card, TurnTakerController turnTakerController)
         : base(card, turnTakerController)
        {
            SpecialStringMaker.ShowNumberOfCardsAtLocation(TurnTaker.Deck, new LinqCardCriteria(c => IsFeral(c), FeralKeyword));
        }

        public override IEnumerator Play()
        {
            //Moonwolf deals herself 2 Melee Damage.
            IEnumerator coroutine = DealDamage(CharacterCard, CharacterCard, 2, DamageType.Melee, cardSource: GetCardSource());
            if (base.UseUnityCoroutines)
            {
                yield return base.GameController.StartCoroutine(coroutine);
            }
            else
            {
                base.GameController.ExhaustCoroutine(coroutine);
            }
            //Search your deck for a Feral Card, and put the selected card into play or in your hand, then shuffle your deck.
            coroutine = SearchForCards(DecisionMaker, true, false, 0, 1, new LinqCardCriteria(card => IsFeral(card), FeralKeyword), true, true, false, shuffleAfterwards: true);
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