﻿using System;
using System.Collections.Generic;
using Dialogue.RunTime;
using UnityEngine;
using UnityEngine.Serialization;

namespace Dialogue.Runtime
{
    [Serializable]
    public class DialogueNodeData {
        public string nodeGuid;
        public string dialogueText;
        public Vector2 position;
        public string SpeakerName;
        public string SpeakerSpriteLeft;
        public string SpeakerSpriteRight;
        public List<ItemPortCombi> ItemPortCombis;
    }
}