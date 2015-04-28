using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Light.Serialization.Json
{
    public interface IJsonFormatter
    {
        int CurrentIndentationLevel { get; }

        void NewlineAndIncreaseIndent(IJsonWriter writer);
        void Newline(IJsonWriter writer);
        void NewlineAndDecreaseIndent(IJsonWriter writer);
        void InsertWhitespaceBetweenKeyAndValue(IJsonWriter writer);
        void ResetIndentationLevel();
    }
}
