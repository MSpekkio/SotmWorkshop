using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace sotm_moonwolf
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
            if (base.PullOfTheMoon.CurrentValue > 0)
			{
                int max = base.PullOfTheMoon.CurrentValue < 5 ? base.PullOfTheMoon.CurrentValue : 5;
				List<SelectNumberDecision> selectNumber = new List<SelectNumberDecision>();
				coroutine = this.GameController.SelectNumber(this.DecisionMaker, SelectionType.RemoveTokens, 0, max, cardSource: this.GetCardSource());
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
                    coroutine = this.GameController.RemoveTokensFromPool(base.PullOfTheMoon, amount, cardSource: this.GetCardSource());
                    if (this.UseUnityCoroutines)
                    {
                        yield return this.GameController.StartCoroutine(coroutine);
                    }
                    else
                    {
                        this.GameController.ExhaustCoroutine(coroutine);
                    }

                    //Reveal X cards where the X is the number of tokens removed, put one card into play and the remaining cards into the trash.
                    coroutine = base.RevealCards_SelectSome_MoveThem_DiscardTheRest(base.DecisionMaker, base.TurnTakerController, base.TurnTaker.Deck, card => true, amount, 1, false, true, true, "cards");
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
				coroutine = this.GameController.SendMessageAction("There are no tokens in " + base.PullOfTheMoon.Name + " to remove.", Priority.High, this.GetCardSource(), null, true);
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