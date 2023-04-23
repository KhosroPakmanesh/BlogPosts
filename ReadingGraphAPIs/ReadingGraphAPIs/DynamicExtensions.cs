using Newtonsoft.Json;

namespace Application.Controllers.SocialReview.Extensions
{
    public static class DynamicExtensions
    {
        public static bool TryExtractValue<T>
            (dynamic dynamicObject, string fieldName, out T extractedValue)
        {
            try
            {
                var tokens = ExtractTokens(fieldName);

                var rawTokenValue = ExtractRawTokenValue(dynamicObject, tokens);
                if (!string.IsNullOrEmpty(rawTokenValue))
                {
                    extractedValue = JsonConvert.DeserializeObject(rawTokenValue, typeof(T));
                    return true;
                }
            }
            catch {}

            extractedValue = default!;            
            return false;
        }
        public static T ExtractValue<T> (dynamic dynamicObject, string fieldName)
        {
            var tokens = ExtractTokens(fieldName);

            var rawTokenValue = ExtractRawTokenValue(dynamicObject, tokens);
            if (!string.IsNullOrEmpty(rawTokenValue))
            {
                return JsonConvert.DeserializeObject(rawTokenValue, typeof(T));
            }

            throw new InvalidOperationException($"{fieldName} field name not found!");
        }
        public static object ExtractValue
            (dynamic dynamicObject, string fieldName, Type type)
        {
            var tokens = ExtractTokens(fieldName);

            var rawTokenValue = ExtractRawTokenValue(dynamicObject, tokens);
            if (!string.IsNullOrEmpty(rawTokenValue))
            {
                return JsonConvert.DeserializeObject(rawTokenValue, type);
            }

            throw new InvalidOperationException($"{fieldName} field name not found!");
        }
        public static bool HasValue(dynamic dynamicObject, string fieldName)
        {
            try
            {
                var tokens = ExtractTokens(fieldName);

                var rawTokenValue = ExtractRawTokenValue(dynamicObject, tokens);
                if (!string.IsNullOrEmpty(rawTokenValue))
                {
                    return true;
                }
            }
            catch { }

            return false;
        }

        private static List<string> ExtractTokens(string fieldName)
        {
            return fieldName.Split(".").ToList();
        }
        private static string ExtractRawTokenValue
            (dynamic dynamicObject, List<string> tokens)
        {
            var currentTokenValueDictionary =
                (IDictionary<string, object>) dynamicObject;

            foreach (var token in tokens)
            {               
                var tokenWithoutBrackets = GetTokenWithoutBrackets(token);

                if (!currentTokenValueDictionary.ContainsKey(tokenWithoutBrackets))
                {
                    return string.Empty;
                }

                object tokenValue;
                currentTokenValueDictionary.
                    TryGetValue(tokenWithoutBrackets, out tokenValue!);

                if (token != tokens.LastOrDefault())
                {
                    currentTokenValueDictionary = 
                        GetNextTokenValueDictionary(token, tokenValue);
                }
                else
                {
                   return GetTokenValue (token, tokenValue);
                }
            }

            return string.Empty;
        }
        private static string GetTokenWithoutBrackets(string extractedToken)
        {
            if (extractedToken.Contains('['))
            {
                var beginBracketIndex = extractedToken.IndexOf('[');
                var endBracketIndex = extractedToken.IndexOf(']');

                var indexSubString = extractedToken.Substring(beginBracketIndex
                    , endBracketIndex - beginBracketIndex + 1);

                return extractedToken.Remove(beginBracketIndex, indexSubString.Length);
            }

            return extractedToken;
        }
        private static IDictionary<string, object> GetNextTokenValueDictionary
            (string token, object tokenValue)
        {
            if (tokenValue is List<object> objectList)
            {
                var tokenListIndexValue = GetTokenListIndexValue(token);
                return (IDictionary<string, object>)objectList[tokenListIndexValue];
            }
            else
            {
                return (IDictionary<string, object>)tokenValue;
            }
        }
        private static string GetTokenValue
            (string token, object tokenValue)
        {
            var tokenListIndexValue = GetTokenListIndexValue(token);

            if (tokenValue is List<object> objectList
                && tokenListIndexValue != -1)
            {
                var currentDynamicObject = objectList[tokenListIndexValue];
                return JsonConvert.SerializeObject(currentDynamicObject);                
            }
            else
            {
                return JsonConvert.SerializeObject(tokenValue); ;
            }
        }
        private static int GetTokenListIndexValue(string token)
        {
            if (!token.Contains('['))
            {
                return -1;
            }

            var beginBracketIndex = token.IndexOf('[');
            var endBracketIndex = token.IndexOf(']');

            var indexSubString = token.Substring(beginBracketIndex
                , endBracketIndex - beginBracketIndex + 1);

            var stringIndexValue = indexSubString.Replace("[", string.Empty);
            stringIndexValue = stringIndexValue.Replace("]", string.Empty);

            return Convert.ToInt32(stringIndexValue);
        }
    }
}