using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace SotmWorkshop.Moonwolf
{
    public class MoonlitInterludeCardController : MoonwolfCardController
    {
        public MoonlitInterludeCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override IEnumerator Play()
        {
            LinqCardCriteria cardCriteria = new LinqCardCriteria(card => card.IsHeroCharacterCard && card.Owner != HeroTurnTaker, "hero character card");
            List<SelectCardDecision> storedResults = new List<SelectCardDecision>();

            IEnumerator selection = base.GameController.SelectCardAndStoreResults(DecisionMaker, SelectionType.GainHP,
                cardCriteria, storedResults, false, cardSource: GetCardSource());
            if (base.UseUnityCoroutines)
            {
                yield return base.GameController.StartCoroutine(selection);
            }
            else
            {
                base.GameController.ExhaustCoroutine(selection);
            }

            if (storedResults.Count > 0)
            {
                //Moonwolf and another Hero character card regain 2 HP,
                Card selected = storedResults.First().SelectedCard;
                TurnTaker selectedTurnTaker = selected.Owner;
                IEnumerator coroutine1 = GameController.GainHP(selected, 2, cardSource: GetCardSource());
                IEnumerator coroutine2 = GameController.GainHP(CharacterCard, 2, cardSource: GetCardSource());
                if (base.UseUnityCoroutines)
                {
                    yield return base.GameController.StartCoroutine(coroutine1);
                    yield return base.GameController.StartCoroutine(coroutine2);
                }
                else
                {
                    base.GameController.ExhaustCoroutine(coroutine1);
                    base.GameController.ExhaustCoroutine(coroutine2);
                }

                //then choose one of these Hero's players to play a card.
                selection = GameController.SelectHeroToPlayCard(DecisionMaker,
                                additionalCriteria: new LinqTurnTakerCriteria(tt => tt == HeroTurnTaker || tt == selectedTurnTaker, "a pleasent evening"),
                                cardSource: GetCardSource());
                if (base.UseUnityCoroutines)
                {
                    yield return base.GameController.StartCoroutine(selection);
                }
                else
                {
                    base.GameController.ExhaustCoroutine(selection);
                }
            }
        }
    }
}