using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace SotmWorkshop.Moonwolf
{
    public class DrawOutTheBeastCardController : MoonwolfCardController
    {
        public DrawOutTheBeastCardController(Card card, TurnTakerController turnTakerController)
         : base(card, turnTakerController)
        {
        }

        public override IEnumerator Play()
        {
            IEnumerator coroutine;
            //Remove up to 5 tokens from Pull of the Moon.
            if (PullOfTheMoon.CurrentValue > 0)
            {
                int max = PullOfTheMoon.CurrentValue < 5 ? PullOfTheMoon.CurrentValue : 5;
                List<SelectNumberDecision> selectNumber = new List<SelectNumberDecision>();
                coroutine = GameController.SelectNumber(DecisionMaker, SelectionType.RemoveTokens, 0, max, cardSource: GetCardSource());
                if (this.UseUnityCoroutines)
                {
                    yield return this.GameController.StartCoroutine(coroutine);
                }
                else
                {
                    this.GameController.ExhaustCoroutine(coroutine);
                }
                int amount = selectNumber.FirstOrDefault()?.SelectedNumber ?? 0;
                if (amount > 0)
                {
                    coroutine = GameController.RemoveTokensFromPool(PullOfTheMoon, amount, cardSource: GetCardSource());
                    if (this.UseUnityCoroutines)
                    {
                        yield return this.GameController.StartCoroutine(coroutine);
                    }
                    else
                    {
                        this.GameController.ExhaustCoroutine(coroutine);
                    }

                    //Reveal X cards where the X is the number of tokens removed, put one card into play and the remaining cards into the trash.
                    coroutine = RevealCards_SelectSome_MoveThem_DiscardTheRest(DecisionMaker, TurnTakerController, TurnTaker.Deck, card => true, amount, 1, false, true, true, "cards");
                    if (this.UseUnityCoroutines)
                    {
                        yield return this.GameController.StartCoroutine(coroutine);
                    }
                    else
                    {
                        this.GameController.ExhaustCoroutine(coroutine);
                    }
                }
            }
            else
            {
                coroutine = this.GameController.SendMessageAction("There are no tokens in " + PullOfTheMoon.Name + " to remove.", Priority.High, GetCardSource(), null, true);
                if (this.UseUnityCoroutines)
                {
                    yield return this.GameController.StartCoroutine(coroutine);
                }
                else
                {
                    this.GameController.ExhaustCoroutine(coroutine);
                }
            }
            yield break;
        }
    }
}