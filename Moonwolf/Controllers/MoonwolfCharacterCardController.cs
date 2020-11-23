using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace SotmWorkshop.Moonwolf
{
    public class MoonwolfCharacterCardController : HeroCharacterCardController
    {
        public MoonwolfCharacterCardController(Card card, TurnTakerController turnTakerController)
        : base(card, turnTakerController)
        {
        }
    
        public override IEnumerator UseIncapacitatedAbility(int index)
        {
            switch (index)
            {
                /*
            case 0:
            {
                //* Choose 1 Target to deal itself 1 Melee Damage, then regain 2 HP.
                IEnumerator coroutine = base.GameController.SelectHeroToUsePower(base.HeroTurnTakerController, false, true, false, null, null, null, true, true, base.GetCardSource(null));
                if (base.UseUnityCoroutines)
                {
                    yield return base.GameController.StartCoroutine(coroutine);
                }
                else
                {
                    base.GameController.ExhaustCoroutine(coroutine);
                }
                break;
            }
            case 1:
            {
                //* The environment deals 1 Hero Character Card 2 Radiant Damage, then that Hero’s player may draw a card and play a card.
                IEnumerator coroutine2 = base.GameController.SelectHeroAndIncreaseNextDamageDealt(base.HeroTurnTakerController, 2, 1, null, base.GetCardSource(null));
                if (base.UseUnityCoroutines)
                {
                    yield return base.GameController.StartCoroutine(coroutine2);
                }
                else
                {
                    base.GameController.ExhaustCoroutine(coroutine2);
                }
                break;
            }
            */
            case 2:
            {
                //* Destroy an Environment Target.
                IEnumerator coroutine = base.GameController.SelectAndDestroyCard(base.HeroTurnTakerController, new LinqCardCriteria(card => card.IsEnvironment, "environment"), true, cardSource: base.GetCardSource());
                if (base.UseUnityCoroutines)
                {
                    yield return base.GameController.StartCoroutine(coroutine);
                }
                else
                {
                    base.GameController.ExhaustCoroutine(coroutine);
                }
                break;
            }
            }
            yield break;
        }

        public override IEnumerator UsePower(int index = 0)
        {
            //Moonwolf deals 1 Target 2 Melee Damage,
            IEnumerator coroutine = base.GameController.SelectTargetsAndDealDamage(this.DecisionMaker, new DamageSource(base.GameController, base.Card), 2, DamageType.Melee, 1, false, 1, cardSource: base.GetCardSource());
			if (base.UseUnityCoroutines)
			{
				yield return base.GameController.StartCoroutine(coroutine);
			}
			else
			{
				base.GameController.ExhaustCoroutine(coroutine);
			}
            //...then herself 1 Melee Damage.
            coroutine = base.DealDamage(base.CharacterCard, base.CharacterCard, 1, DamageType.Melee, cardSource: base.GetCardSource());
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