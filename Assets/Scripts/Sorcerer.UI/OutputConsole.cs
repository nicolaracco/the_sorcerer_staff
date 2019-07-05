using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Sorcerer.UI
{
    [RequireComponent(typeof(TMP_Text))]
    public class OutputConsole : MonoBehaviour
    {
        private TMP_Text textArea;

        private LinkedList<string> messages = new LinkedList<string>();

        private void Awake()
        {
            textArea = GetComponent<TMP_Text>();
        }

        public void Append(string text)
        {
            messages.AddLast(text);
            if (messages.Count > 5)
                messages.RemoveFirst();
            textArea.SetText(String.Join("\n", messages));
        }
    }
}