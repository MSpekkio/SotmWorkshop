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
        }

        public override IEnumerator Play()
		{
            //Moonwolf deals herself 2 Melee Damage.
			IEnumerator coroutine = base.DealDamage(base.CharacterCard, base.CharacterCard, 2, DamageType.Melee, cardSource: base.GetCardSource());
			if (base.UseUnityCoroutines)
			{
				yield return base.GameController.StartCoroutine(coroutine);
			}
			else
			{
				base.GameController.ExhaustCoroutine(coroutine);
			}
            //Search your deck for a Feral Card, and put the selected card into play or in your hand, then shuffle your deck.
            coroutine = base.SearchForCards(base.HeroTurnTakerController, true, false, 0, 1, new LinqCardCriteria(card => card.DoKeywordsContain(FeralKeyword), FeralKeyword), true, true, false, shuffleAfterwards: true);
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