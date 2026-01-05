// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

namespace Bodoconsult.App.Abstractions.Extensions;


/// <summary>
/// Extension methods for integer values
/// </summary>
/// <remarks>Source: https://gist.github.com/ajcomeau/fdd46f8ee87f30944e3524d2d8181c36
/// https://www.comeausoftware.com/programming/convert-roman-arabic-numerals-csharp/</remarks>
public static class IntExtensions
{

    // Declare dictionary to hold Arabic numbers and Roman equivalents.
    // Sorted dictionary used to ensure order of entries. In this case,
    // the dictionary will be sorted starting with the last entry where the key is 1.
    private static readonly SortedDictionary<int, char> DyArabicToRoman = new()
    {
        { 1000, 'M' },
        { 500, 'D' },
        { 100, 'C' },
        { 50, 'L' },
        { 10, 'X' },
        { 5, 'V' },
        { 1, 'I' }
    };

    // Declare dictionary to hold Roman numbers and Arabic equivalents.
    private static readonly SortedDictionary<char, int> DyRomanToArabic = new()
    {
        { 'M', 1000 },
        { 'D', 500 },
        { 'C', 100 },
        { 'L', 50 },
        { 'X', 10 },
        { 'V', 5 },
        { 'I', 1 },
        { ' ', 0 }
    };

    /// <summary>
    /// Convert a roman number to an arabic int number. Works between 0 and 3999
    /// </summary>
    /// <param name="romanNumeral">Roman number string like XII. Blanks in the string are removed!</param>
    /// <returns>Arabic number</returns>
    public static int RomanToArabic(this string romanNumeral)
    {
        var stringPlace = 0;
        var returnValue = 0;
        var consecutiveCount = 0;

        try
        {
            // Convert to uppercase as Chars are case-sensitive.
            romanNumeral = romanNumeral.Replace(" ", string.Empty).ToUpper();

            //if (romanNumeral.Contains(' '))
            //{
            //    throw new ArgumentOutOfRangeException("Please enter a single Roman numeral - no spaces.", new Exception());
            //}

            // FIRST LOOK FOR INVALID ROMAN NUMERAL CONSTRUCTIONS.
            // THIS COULD BE DONE AS FRONT-END VALIDATION RATHER THAN IN THE CLASS.
            var notationValue = 0;
            var secondLetterValue = 0;
            for (var x = 0; x < romanNumeral.Length; x++)
            {
                // Parse the Roman numeral and compare each character to the last.
                var thisChar = romanNumeral[x];
                var lastChar = x > 0 ? romanNumeral[x - 1] : ' ';
                consecutiveCount = thisChar == lastChar ? consecutiveCount + 1 : 1;

                if (DyRomanToArabic.TryGetValue(thisChar, out notationValue) && DyRomanToArabic.TryGetValue(lastChar, out secondLetterValue))
                {
                    var subNumber = notationValue.ToString()[0] == '1' ? notationValue / 10 : notationValue / 5;

                    // If there's more than one consecutive V, L or D.
                    if (consecutiveCount > 1 && notationValue.ToString()[0] == '5')
                    {
                        throw new ArgumentOutOfRangeException("Invalid Roman numeral - invalid letter repetitions.", new Exception());
                    }

                    // If there's more than three of any character.
                    if (consecutiveCount > 3)
                    {
                        throw new ArgumentOutOfRangeException("Invalid Roman numeral - specific characters cannot appear in groups of more than three.", new Exception());
                    }

                    // If there's any other invalid combination of characters.
                    // If there's a character before this one, it must be a valid subtractive value or it must be greater than or equal to the current character.
                    if (subNumber > 0 && secondLetterValue > 0 && secondLetterValue != subNumber &&
                        secondLetterValue < notationValue)
                    {
                        throw new ArgumentOutOfRangeException("Invalid Roman numeral - possible error in subtractive combinations.", new Exception());
                    }
                }
                else
                {
                    throw new Exception("Invalid character found.");
                }

            }

            // ONCE THE STRING IS KNOWN TO BE VALID, parse the string to evaluate individual letters.
            // Using a While loop here to have more control over the movement through the string.
            while (stringPlace < romanNumeral.Length)
            {
                // Get the first letter and increment the place.
                var currentRomanNotation = romanNumeral.Substring(stringPlace, 1);
                stringPlace++;

                // Get the value of the first letter.
                if (!DyRomanToArabic.TryGetValue(currentRomanNotation[0], out notationValue))
                {
                    continue;
                }
                // If there's another letter to the right, get that one.
                if (stringPlace <= romanNumeral.Length - 1 &&
                    DyRomanToArabic.TryGetValue(romanNumeral[stringPlace], out secondLetterValue))
                {
                    // If the value of the second letter is less than the first, then use
                    // subtractive notation (i.e. CM = 900, IX = 9) as long as the second letter 
                    // is valid in that place. 
                    if (secondLetterValue > notationValue)
                    {
                        currentRomanNotation += romanNumeral[stringPlace];
                        stringPlace++;
                        notationValue = secondLetterValue - notationValue;
                    }
                }

                returnValue += notationValue;
            }

            return returnValue;
        }
        catch (Exception ex)
        {
            throw new ArgumentException($"Conversion of Roman numeral {romanNumeral} to arabic number failed", ex);
        }
    }


    /// <summary>
    /// Convert arabic integer value to roman number string. Works between 0 and 3999
    /// </summary>
    /// <param name="inputNumber">Arabic number to convert to Roman numeral</param>
    /// <returns>Roman numeral</returns>
    /// <exception cref="ArgumentOutOfRangeException">Input values must be between 1 and 3999.</exception>
    public static string ArabicToRoman(this int inputNumber)
    {
        var returnValue = string.Empty;

        try
        {
            // The class currently does not process numbers over 3999.
            if (inputNumber > 3999 || inputNumber < 1)
            {
                throw new ArgumentOutOfRangeException("Input values must be between 1 and 3999.", new Exception());
            }

            // Start at the end of the dictionary. Sorted dictionary orders by the key so 1000 is at the end.
            // Get Arabic number, Roman character and the subtractive level under it.
            var dictionaryElement = DyArabicToRoman.Count - 1;
            var arabicNumber = DyArabicToRoman.ElementAt(dictionaryElement).Key;     // Holds arabic number and "9" place under it i.e. 1000 and 900, 10 and 9
            var romanChar = DyArabicToRoman.ElementAt(dictionaryElement).Value;
            var arabicSubLevel = arabicNumber - (arabicNumber.ToString()[0] == '1' ? arabicNumber / 10 : arabicNumber / 5);     // Holds arabic number and "9" place under it i.e. 1000 and 900, 10 and 9

            // InputNumber will be continually reduced as the Roman numeral is built.
            while (inputNumber is > 0 and < 4000)
            {
                if (inputNumber >= arabicNumber) // If the number remains above the current test.
                {
                    // If the current Roman numeral ends with three of the current Roman character,
                    // and the current Arabic number starts with 1, remove the three characters and
                    // add the subtractive notation (i.e. III to IV and XXXVIII to XXXIX)
                    if (returnValue.EndsWith(new string(romanChar, 3)) && arabicNumber.ToString()[0] == '1')
                    {
                        returnValue = returnValue[..^3];
                        returnValue += romanChar;
                        returnValue += DyArabicToRoman.ElementAt(dictionaryElement + 1).Value;
                    }
                    else // Otherwise, just add another character.
                    {
                        returnValue += DyArabicToRoman.ElementAt(dictionaryElement).Value;
                    }

                    // Subtract the amount that has been added to the Roman numeral.
                    inputNumber -= arabicNumber;
                }
                else if (inputNumber >= arabicSubLevel)
                {
                    // If the number is less than the current level but greater than the sublevel
                    // (i.e. less than 1000 but 900 or greater), add the appropriate letters.

                    if (arabicNumber.ToString()[0] == '1')
                    {
                        returnValue += DyArabicToRoman.ElementAt(dictionaryElement - 2).Value;
                    }
                    else
                    {
                        returnValue += DyArabicToRoman.ElementAt(dictionaryElement - 1).Value;
                    }

                    returnValue += DyArabicToRoman.ElementAt(dictionaryElement).Value;

                    // Subtract the amount that has been added to the Roman numeral.
                    inputNumber -= arabicSubLevel;
                }
                else
                {
                    // Otherwise, move forward in the dictionary and get the new values.
                    dictionaryElement--;
                    arabicNumber = DyArabicToRoman.ElementAt(dictionaryElement).Key;
                    romanChar = DyArabicToRoman.ElementAt(dictionaryElement).Value;
                    arabicSubLevel = arabicNumber - (arabicNumber.ToString()[0] == '1' ? arabicNumber / 10 : arabicNumber / 5);
                }
            }


            return returnValue;
        }
        catch (Exception ex)
        {
            throw new ArgumentException($"Conversion of {inputNumber} to Roman numeral failed", ex);
        }
    }

    /// <summary>
    /// To upper latin. Use this method for page number conversions
    /// </summary>
    /// <param name="value">Value to convert to upper latin</param>
    /// <returns>Upper lation for the given value</returns>
    public static string ToUpperLatin(this int value)
    {
        return Convert.ToChar(64 + value).ToString();
    }

    /// <summary>
    /// To lower latin. Use this method for page number conversions
    /// </summary>
    /// <param name="value">Value to convert to upper latin</param>
    /// <returns>Lower lation for the given value</returns>
    public static string ToLowerLatin(this int value)
    {
        return Convert.ToChar(96 + value).ToString();
    }
}
