using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Handelabra;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace SotmWorkshop.Moonwolf
{
    public class DrawOutTheBeastCardController : MoonwolfCardController
    {
        public DrawOutTheBeastCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
            SpecialStringMaker.ShowTokenPool(PullOfTheMoon);
        }

        public override IEnumerator Play()
        {
            IEnumerator coroutine;
            //Remove up to 5 tokens from Pull of the Moon.
            if (PullOfTheMoon.CurrentValue > 0)
            {
                int max = PullOfTheMoon.CurrentValue < 5 ? PullOfTheMoon.CurrentValue : 5;
                List<SelectNumberDecision> selectNumber = new List<SelectNumberDecision>();
                coroutine = GameController.SelectNumber(DecisionMaker, SelectionType.RemoveTokens, 0, max, storedResults: selectNumber, cardSource: GetCardSource());
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
                    coroutine = RevealCards_PlayOne_DiscardTheRest(amount);
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
        }

        protected IEnumerator RevealCards_PlayOne_DiscardTheRest(int numberOfMatchesToReveal)
        {
            List<Card> revealedCards = new List<Card>();
            IEnumerator coroutine = GameController.RevealCards(DecisionMaker, TurnTaker.Deck, numberOfMatchesToReveal, revealedCards, cardSource: GetCardSource());
            if (UseUnityCoroutines)
            {
                yield return GameController.StartCoroutine(coroutine);
            }
            else
            {
                GameController.ExhaustCoroutine(coroutine);
            }
            if (revealedCards.Any())
            {
                List<SelectCardDecision> storedResults = new List<SelectCardDecision>();
                coroutine = GameController.PlayCards(DecisionMaker, (Card c) => revealedCards.Contains(c), false, true, 1, 1, storedResults: storedResults, cardSource: GetCardSource());
                if (UseUnityCoroutines)
                {
                    yield return GameController.StartCoroutine(coroutine);
                }
                else
                {
                    GameController.ExhaustCoroutine(coroutine);
                }
                if (DidSelectCard(storedResults))
                {
                    revealedCards.Remove(GetSelectedCard(storedResults));
                }
                else
                {
                    Log.Warning("[" + Card.Title + "]: A card was unable to be selected from a series of revealed cards: " + revealedCards.ToCommaList() + ".");
                }
                coroutine = GameController.MoveCards(TurnTakerController, revealedCards, TurnTaker.Trash, isDiscard: true, cardSource: GetCardSource());
                if (UseUnityCoroutines)
                {
                    yield return GameController.StartCoroutine(coroutine);
                }
                else
                {
                    GameController.ExhaustCoroutine(coroutine);
                }
            }
            else
            {
                coroutine = GameController.SendMessageAction("There were no cards in " + TurnTaker.Deck.GetFriendlyName() + " to reveal.", Priority.High, GetCardSource());
                if (UseUnityCoroutines)
                {
                    yield return GameController.StartCoroutine(coroutine);
                }
                else
                {
                    GameController.ExhaustCoroutine(coroutine);
                }
            }
        }
    }
}