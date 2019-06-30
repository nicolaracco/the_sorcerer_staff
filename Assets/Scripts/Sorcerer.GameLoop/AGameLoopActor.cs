using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sorcerer.GameLoop
{
    public abstract class AGameLoopActor : MonoBehaviour, IComparable<AGameLoopActor>
    {
        public int turnOrder = 0;

        public int CompareTo(AGameLoopActor other)
        {
            return turnOrder.CompareTo(other.turnOrder);
        }

        public abstract IEnumerator WaitForAction();
    }
}