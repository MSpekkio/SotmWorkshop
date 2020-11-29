using System;
using System.Collections;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace SotmWorkshop.Moonwolf
{
    public abstract class MoonwolfCardController : CardController
    {
        protected static readonly string PullOfTheMoonIdentifier = "PullOfTheMoon";
        protected static readonly string FeralKeyword = "feral";

        protected MoonwolfCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        protected TokenPool PullOfTheMoon
        {
            get
            {
                return HeroTurnTaker.FindCard(PullOfTheMoonIdentifier).FindTokenPool(PullOfTheMoonIdentifier);
            }
        }

        protected IEnumerator SendMessageAboutInsufficientTokensRemoved(int numberRemoved, string suffix)
        {
            string str = "There are no tokens to remove";
            if (numberRemoved == 1)
            {
                str = "Only one token was removed";
            }
            else if (numberRemoved > 1)
            {
                str = string.Format("Only {0} tokens were removed", numberRemoved);
            }
            string message = str + ", so " + suffix;
            IEnumerator coroutine = base.GameController.SendMessageAction(message, Priority.Medium, base.GetCardSource());
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

        protected IEnumerator SendMessageAboutInsufficientTokensRequired(int numberRequired, string suffix)
        {
            string str = $"{CharacterCard.Title} does not have ";
            if (numberRequired == 1)
            {
                str += "1 token to remove, so ";
            }
            else
            {
                str += numberRequired.ToString() + " tokens to remove, so ";
            }

            str += suffix;
            IEnumerator coroutine = base.GameController.SendMessageAction(str, Priority.Medium, base.GetCardSource());
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
