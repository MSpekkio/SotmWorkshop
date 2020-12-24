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
        public MoonwolfCharacterCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override IEnumerator UseIncapacitatedAbility(int index)
        {
            switch (index)
            {
                case 0:
                    {
                        //* Choose 1 Target to deal itself 1 Melee Damage, then regain 2 HP.
                        List<SelectTargetDecision> results = new List<SelectTargetDecision>();
                        IEnumerator coroutine = GameController.SelectTargetAndStoreResults(DecisionMaker, GameController.GetAllCards(), results,
                                                    additionalCriteria: c => c.IsTarget && c.IsInPlay && GameController.IsCardVisibleToCardSource(c, GetCardSource()),
                                                    damageType: DamageType.Melee, damageAmount: c => 1, selectionType: SelectionType.SelectTargetFriendly, cardSource: GetCardSource());
                        if (base.UseUnityCoroutines)
                        {
                            yield return base.GameController.StartCoroutine(coroutine);
                        }
                        else
                        {
                            base.GameController.ExhaustCoroutine(coroutine);
                        }
                        var selected = results.FirstOrDefault()?.SelectedCard;
                        if (selected != null)
                        {
                            coroutine = DealDamage(selected, selected, 1, DamageType.Melee, cardSource: GetCardSource());
                            if (base.UseUnityCoroutines)
                            {
                                yield return base.GameController.StartCoroutine(coroutine);
                            }
                            else
                            {
                                base.GameController.ExhaustCoroutine(coroutine);
                            }
                            coroutine = GameController.GainHP(selected, 2, cardSource: GetCardSource());
                            if (base.UseUnityCoroutines)
                            {
                                yield return base.GameController.StartCoroutine(coroutine);
                            }
                            else
                            {
                                base.GameController.ExhaustCoroutine(coroutine);
                            }
                        }
                        break;
                    }
                case 1:
                    {
                        //* The environment deals 1 Hero Character Card 2 Radiant Damage, then that Hero’s player may draw a card and play a card.
                        List<SelectTargetDecision> results = new List<SelectTargetDecision>();
                        IEnumerator coroutine = GameController.SelectTargetAndStoreResults(DecisionMaker, GameController.GetAllCards(), results,
                                                    additionalCriteria: c => c.IsHeroCharacterCard && c.IsInPlay && !c.IsIncapacitatedOrOutOfGame && GameController.IsCardVisibleToCardSource(c, GetCardSource()),
                                                    damageSource: null,
                                                    damageType: DamageType.Radiant, damageAmount: c => 2, selectionType: SelectionType.SelectTargetFriendly, cardSource: GetCardSource());
                        if (base.UseUnityCoroutines)
                        {
                            yield return base.GameController.StartCoroutine(coroutine);
                        }
                        else
                        {
                            base.GameController.ExhaustCoroutine(coroutine);
                        }
                        var selected = results.FirstOrDefault()?.SelectedCard;
                        if (selected != null)
                        {
                            var htt = selected.Owner.ToHero();
                            coroutine = GameController.DealDamageToTarget(new DamageSource(GameController, FindEnvironment().TurnTaker), selected, 2, DamageType.Radiant, cardSource: GetCardSource());
                            if (base.UseUnityCoroutines)
                            {
                                yield return base.GameController.StartCoroutine(coroutine);
                            }
                            else
                            {
                                base.GameController.ExhaustCoroutine(coroutine);
                            }
                            coroutine = DrawCard(htt, optional: true);
                            if (base.UseUnityCoroutines)
                            {
                                yield return base.GameController.StartCoroutine(coroutine);
                            }
                            else
                            {
                                base.GameController.ExhaustCoroutine(coroutine);
                            }
                            coroutine = GameController.SelectAndPlayCardFromHand(FindHeroTurnTakerController(htt), true, cardSource: GetCardSource());
                            if (base.UseUnityCoroutines)
                            {
                                yield return base.GameController.StartCoroutine(coroutine);
                            }
                            else
                            {
                                base.GameController.ExhaustCoroutine(coroutine);
                            }
                        }
                        break;
                    }
                case 2:
                    {
                        //* Destroy an Environment Target.
                        IEnumerator coroutine = base.GameController.SelectAndDestroyCard(base.HeroTurnTakerController,
                                                    new LinqCardCriteria(card => card.IsEnvironmentTarget && GameController.IsCardVisibleToCardSource(card, GetCardSource()), "environment"), true,
                                                    cardSource: base.GetCardSource());
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
            int targets = GetPowerNumeral(0, 1);
            int damages = GetPowerNumeral(1, 2);
            int selfDamage = GetPowerNumeral(2, 1);

            //Moonwolf deals 1 Target 2 Melee Damage,
            IEnumerator coroutine = GameController.SelectTargetsAndDealDamage(DecisionMaker, new DamageSource(GameController, CharacterCard), damages, DamageType.Melee, targets, false, targets, cardSource: GetCardSource());
            if (base.UseUnityCoroutines)
            {
                yield return base.GameController.StartCoroutine(coroutine);
            }
            else
            {
                base.GameController.ExhaustCoroutine(coroutine);
            }
            //...then herself 1 Melee Damage.
            coroutine = DealDamage(CharacterCard, CharacterCard, selfDamage, DamageType.Melee, cardSource: GetCardSource());
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