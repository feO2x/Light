using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Light.Serialization.Json.LowLevelWriting.KeyNormalization
{
    public interface IJsonKeyNormalizer
    {
        string Normalize(string key);
        bool ForceNormalizeKey { get; }
        bool ForceNotToNormalizeKey { get; }
    }
}
