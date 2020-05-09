using UnityEngine;
using System.Collections;
using System;

namespace BT.Variables
{
    [Serializable]
    public class StringReference
    {
        public bool useConstant = true;
        public string constantValue;

        public StringVariable variable;

        public StringReference() { }

        public StringReference(string value)
        {
            useConstant = true;
            constantValue = value;
        }

        public string value
        {
            get { return useConstant ? constantValue : variable.value;  }
        }

    }
}