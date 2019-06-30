using System;
using System.Collections;
using UnityEngine;

namespace Sorcerer.GameLoop
{
    public class GameLoopManager : MonoBehaviour
    {
        /// <summary>
        /// turn delay in seconds
        /// </summary>
        public float turnDelay;

        private void Start()
        {
            StartCoroutine(TurnLoop());
        }

        private IEnumerator TurnLoop()
        {
            while (true)
            {
                yield return StartCoroutine(ExecuteTurns());
                yield return new WaitForSeconds(turnDelay);
            }
        }

        private IEnumerator ExecuteTurns()
        {
            AGameLoopActor[] actors = Resources.FindObjectsOfTypeAll<AGameLoopActor>();
            Array.Sort(actors);
            for (int i = 0; i < actors.Length; i++)
            {
                yield return actors[i].WaitForAction();
                if (i < actors.Length - 1)
                    yield return 0; // wait a frame between each actor
            }
        }
    }
}
