using System;
using System.Collections;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace SotmWorkshop.Moonwolf
{
    public abstract class MoonwolfCardController : CardController
    {
        protected static readonly string PullOfTheMoonIdentifier = "PullOfTheMoonPool";
        protected static readonly string FeralKeyword = "feral";

        protected MoonwolfCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        protected TokenPool PullOfTheMoon
        {
            get
            {
                return base.CharacterCard.FindTokenPool(PullOfTheMoonIdentifier);
            }
        }

        protected IEnumerator SendMessageAboutInsufficientTokens(int numberRemoved, string suffix)
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
    }
}
