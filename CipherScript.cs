using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Text;
using UnityEngine;


public class CipherScript
{

	bool enableLogging = false;
	string loggingTag = "{Cipher Output}";

	/// <summary>
	/// Use this if original logging tag should stay the same if logging is enabled.
	/// </summary>
	/// <param name="enableLogging">Enable logging in Unity for outputs.</param>
	public CipherScript(bool enableLogging)
	{
		this.enableLogging = enableLogging;
	}

	/// <summary>
	/// Use this if logging tag needs to be different if logging is enabled.
	/// </summary>
	/// <param name="enableLogging">Enable logging in Unity for outputs.</param>
	/// <param name="loggingTag">A customized logging tag.</param>
	public CipherScript(bool enableLogging, string loggingTag)
	{
		this.enableLogging = enableLogging;
		this.loggingTag = loggingTag;
	}


	/*
	  * Atbash Cipher is a very simple cipher to encrypt and decrypt.
	  * 
	  * To Encrypt:
	  * Take 2 sets of the alphabet and reverse the second set...
	  * ABCDEFGHIJKLMNOPQRSTUVWXYZ
	  * ZYXWVUTSRQPONMLKJIHGFEDCBA
	  * 
	  * Take a message like "HELLO" for example and find where each letter is in the first set and use the letter found in the same position in the second set...      
	  * 
	  * H -> S
	  * E -> V
	  * L -> O
	  * L -> O
	  * O -> L
	  * 
	  * Message "HELLO" encrypted into "SVOOL"
	  * 
	  * To Decrypt:
	  * Take the same 2 sets of alphabet from above but reverse only the first set...
	  * ZYXWVUTSRQPONMLKJIHGFEDCBA
	  * ABCDEFGHIJKLMNOPQRSTUVWXYZ
	  * 
	  * Take the message "SVOOL" for example and find where each letter is in the first set and use the letter found in the same position in the second set...
	  * 
	  * S -> H
	  * V -> E
	  * O -> L
	  * O -> L
	  * L -> O
	  * 
	  * Message "SVOOL" decrypted into "HELLO"
	  * 
	  */

	/// <summary>
	/// Encrypts the message given using the rules of Atbash Ciphering. Can be Case-Insensitive and Sensitive.
	/// </summary>
	/// <param name="messageToEncrypt">The message being encrypted.</param>
	/// <param name="caseInsensitive">Whether the message should be fully capitialized (true) or keep each state of the letter the same (false).</param>
	/// <returns>
	/// The message given encrypted with Atbash Ciphering.
	/// <para>Example of Case-Insensitive: Hello, Case-Insensitive -> SVOOL.</para>
	/// <para>Example of Case-Sensitive: HeLlO, Case-Sensitive -> SvOoL.</para>
	/// </returns>
	public string EncryptMessageUsingAtbash(string messageToEncrypt, bool caseInsensitive)
	{

		List<char[]> alphabets = new List<char[]>();
		alphabets.Add(Enumerable.Range(0, 26).Select(x => (char)(x + 'A')).ToArray()); // Alphabet all upper-case
		alphabets.Add(alphabets[0].Reverse().ToArray()); // Reversed alphabet all upper-case
		alphabets.Add(Enumerable.Range(0, 26).Select(x => (char)(x + 'a')).ToArray()); // Alphabet all lower-case
		alphabets.Add(alphabets[2].Reverse().ToArray()); // Reversed alphabet all lower-case

		StringBuilder sb = new StringBuilder();

		foreach (char c in messageToEncrypt.Replace(" ", ""))
		{
			if (!caseInsensitive)
			{
				if (char.IsLower(c))
				{
					sb.Append(alphabets[3][Array.IndexOf(alphabets[2], c)]);
					continue;
				}
				sb.Append(alphabets[1][Array.IndexOf(alphabets[0], c)]);
				continue;
			}
			sb.Append(alphabets[1][Array.IndexOf(alphabets[0], char.ToUpper(c))]);
		}
		if (enableLogging) Debug.LogFormat("{0} Encryping \"{1}\" using Atbash. The message to be returned will be {2}. The encrypted output is \"{3}\".", loggingTag, messageToEncrypt, caseInsensitive ? "case-insensitive" : "case-sensitive", sb.ToString());
		return sb.ToString();
	}

	/// <summary>
	/// Decrypts a message using the rules of Atbash Ciphering. Can be Case-Insensitive and Sensitive.
	/// </summary>
	/// <param name="messageToDecrypt">The message to be decrypted.</param>
	/// <param name="caseInsensitive">Whether the message should be fully capitialized (true) or keep each state of the letter the same (false).</param>
	/// <returns>
	/// The message given decrypted with Atbash Ciphering.
	/// <para>Example of Case-Insensitive: Svool, Case-Insensitive -> HELLO.</para>
	/// <para>Example of Case-Sensitive: SvOoL, Case-Sensitive -> HeLlO.</para>
	/// </returns>
	public string DecryptMessageUsingAtbash(string messageToDecrypt, bool caseInsensitive)
	{

		List<char[]> alphabets = new List<char[]>();
		alphabets.Add(Enumerable.Range(0, 26).Select(x => (char)(x + 'A')).ToArray()); // Alphabet all upper-case
		alphabets.Add(alphabets[0].Reverse().ToArray()); // Reversed alphabet all upper-case
		alphabets.Add(Enumerable.Range(0, 26).Select(x => (char)(x + 'a')).ToArray()); // Alphabet all lower-case
		alphabets.Add(alphabets[2].Reverse().ToArray()); // Reversed alphabet all lower-case

		StringBuilder sb = new StringBuilder();

		foreach (char c in messageToDecrypt.Replace(" ", ""))
		{
			if (!caseInsensitive)
			{
				if (char.IsLower(c))
				{
					sb.Append(alphabets[2][Array.IndexOf(alphabets[3], c)]);
					continue;
				}
				sb.Append(alphabets[0][Array.IndexOf(alphabets[1], c)]);
				continue;
			}
			sb.Append(alphabets[0][Array.IndexOf(alphabets[1], char.ToUpper(c))]);
		}
		if (enableLogging) Debug.LogFormat("{0} Decrypting \"{1}\" using Atbash. The message to be returned will be {2}. The decrypted output is \"{3}\".", loggingTag, messageToDecrypt, caseInsensitive ? "case-insensitive" : "case-sensitive", sb.ToString());
		return sb.ToString();
	}

	// -----

	/*
	 * ROT13 is very similar to Atbash Cipher and Caesar Cipher.
	 * Except ROT13 uses a fixed second alphabet (also know as a subsitution key): NOPQRSTUVWXYZABCDEFGHIJKLM
	 * 
	 * To Encrypt:
	 * 
	 * Take the alphabet and the subsitution key:
	 * ABCDEFGHIJKLMNOPQRSTUVWXYZ
	 * NOPQRSTUVWXYZABCDEFGHIJKLM
	 * 
	 * Take the message "HELLO" and find each position in the original alphabet and look at the same position in the subsitution key to get the corresponding letter.
	 * 
	 * H -> U
	 * E -> R
	 * L -> Y
	 * L -> Y
	 * O -> C
	 * 
	 * Message "HELLO" encrypted into "URYYC"
	 * 
	 * To Decrypt:
	 * Take alphabet and the subsitution key and swap their positions:
	 * NOPQRSTUVWXYZABCDEFGHIJKLM
	 * ABCDEFGHIJKLMNOPQRSTUVWXYZ
	 * 
	 * Take the message "URYYC" and find each position in the subsitution key and look at the same position in the original alphabet to get the corresponding letter.
	 * 
	 * U -> H
	 * R -> E
	 * Y -> L
	 * Y -> L
	 * C -> O
	 * 
	 * Message "URYYC" decrypted into "HELLO"
	 * 
	 */

	/// <summary>
	/// Encrypts a message using the rules of ROT13 Ciphering. Can be Case-Insensitive and Sensitive.
	/// </summary>
	/// <param name="messageToEncrypt">The message to be encrypted.</param>
	/// <param name="caseInsensitive">Whether the message should be fully capitialized (true) or keep each state of the letter the same (false).</param>
	/// <returns>
	/// The message given encrypted with ROT13 Ciphering.
	/// <para>Example of Case-Insensitive: Hello, Case-Insensitive -> URYYB.</para>
	/// <para>Example of Case-Sensitive: HeLlO, Case-Sensitive -> UrYyB.</para>
	/// </returns>
	public string EncryptMessageUsingROT13(string messageToEncrypt, bool caseInsensitive)
	{
		List<char[]> alphabets = new List<char[]>();
		alphabets.Add(Enumerable.Range(0, 26).Select(x => (char)(x + 'A')).ToArray());// Alphabet all upper-case
		alphabets.Add("NOPQRSTUVWXYZABCDEFGHIJKLM".ToCharArray()); // Alphabet starting from 'N' all upper-case
		alphabets.Add(Enumerable.Range(0, 26).Select(x => (char)(x + 'a')).ToArray()); // Alphabet all lower-case
		alphabets.Add("NOPQRSTUVWXYZABCDEFGHIJKLM".ToLower().ToCharArray()); // Alphabet starting from 'N' all lower-case

		StringBuilder sb = new StringBuilder();

		foreach (char c in messageToEncrypt)
		{
			if (!caseInsensitive)
			{
				if (char.IsLower(c))
				{
					sb.Append(alphabets[3][Array.IndexOf(alphabets[2], c)]);
					continue;
				}
				sb.Append(alphabets[1][Array.IndexOf(alphabets[0], c)]);
				continue;
			}
			sb.Append(alphabets[1][Array.IndexOf(alphabets[0], char.ToUpper(c))]);
		}
		if (enableLogging) Debug.LogFormat("{0} Encrypting \"{1}\" using ROT13. The message to be returned will be {2}. The encrypted output is \"{3}\".", loggingTag, messageToEncrypt, caseInsensitive ? "case-insensitive" : "case-sensitive", sb.ToString());
		return sb.ToString();
	}

	/// <summary>
	/// Decrypts a message using the rules of ROT13 Ciphering. Can be Case-Insensitive and Sensitive.
	/// </summary>
	/// <param name="messageToDecrypt">The message to be decrypted.</param>
	/// <param name="caseInsensitive">Whether the message should be fully capitialized (true) or keep each state of the letter the same (false).</param>
	/// <returns>
	/// The message given decrypted with ROT13 Ciphering.
	/// <para>Example of Case-Insensitive: Uryyb, Case-Insensitive -> HELLO.</para>
	/// <para>Example of Case-Sensitive: UrYyB, Case-Sensitive -> HeLlO.</para>
	/// </returns>
	public string DecryptMessageUsingROT13(string messageToDecrypt, bool caseInsensitive)
	{
		List<char[]> alphabets = new List<char[]>();
		alphabets.Add(Enumerable.Range(0, 26).Select(x => (char)(x + 'A')).ToArray());// Alphabet all upper-case
		alphabets.Add("NOPQRSTUVWXYZABCDEFGHIJKLM".ToCharArray()); // Alphabet starting from 'N' all upper-case
		alphabets.Add(Enumerable.Range(0, 26).Select(x => (char)(x + 'a')).ToArray()); // Alphabet all lower-case
		alphabets.Add("NOPQRSTUVWXYZABCDEFGHIJKLM".ToLower().ToCharArray()); // Alphabet starting from 'N' all lower-case

		StringBuilder sb = new StringBuilder();

		foreach (char c in messageToDecrypt)
		{
			if (!caseInsensitive)
			{
				if (char.IsLower(c))
				{
					sb.Append(alphabets[2][Array.IndexOf(alphabets[3], c)]);
					continue;
				}
				sb.Append(alphabets[0][Array.IndexOf(alphabets[1], c)]);
				continue;
			}
			sb.Append(alphabets[0][Array.IndexOf(alphabets[1], char.ToUpper(c))]);
		}
		if (enableLogging) Debug.LogFormat("{0} Decrypting \"{1}\" using ROT13. The message to be returned will be {2}. The decrypted output is \"{3}\".", loggingTag, messageToDecrypt, caseInsensitive ? "case-insensitive" : "case-sensitive", sb.ToString());
		return sb.ToString();
	}

	// -----

	/*
	 * Caesar Cipher is just an updated form of ROT13 allowing a key to be what ever it wants. 
	 * But, of course a modulo will be applied to such numbers so that it can't happen for an error.
	 * 
	 * To Encrypt:
	 * You may use two sets of the alphabet as I will in this example to easily be able to get the corresponding letters.
	 * ABCDEFGHIJKLMNOPQRSTUVWXYZ
	 * ABCDEFGHIJKLMNOPQRSTUVWXYZ
	 * 
	 * When encrypting with Caesar, there is normally a form of "key" that is involved. This number can be what ever it wants but it will reduced to a range of 0-25 (or modulo 26).
	 * Lets say our shift key is +8, this means that the second set of the alphabet is shifted left 8 times.
	 * ABCDEFGHIJKLMNOPQRSTUVWXYZ
	 * IJKLMNOPQRSTUVWXYZABCDEFGH (Now shifted +8)
	 * 
	 * Now take your message "HELLO" and encrypt it by finding it in the top set, then looking at the same position in the bottom set to get the corresponding letter.
	 * 
	 * H -> P
	 * E -> M
	 * L -> T
	 * L -> T
	 * O -> W
	 * 
	 * Message "HELLO" encrypted into "PMTTW" using the shift key +8.
	 * 
	 * To Decrypt:
	 * As the same in encrypting, you may use two sets of the alphabet as I will here.
	 * ABCDEFGHIJKLMNOPQRSTUVWXYZ
	 * ABCDEFGHIJKLMNOPQRSTUVWXYZ 
	 * 
	 * Now, sometimes you won't always have the "key", just like how a message that has been encrypted in Caesar. 
	 * A good way to take a look at message, try looking for duplicate letters that appear, as well keep in mind the length of the word.
	 * 
	 * Lets take the message "RIDZWQOHS ZSHHSFG OFS TIB HC RSQFMDH" and decrypt it.
	 * Take down any duplicate letters and start shifting them around till they seem like they work with the length of the word.
	 * If that doesn't work, perhaps it won't be best to waste time and start shifting each word separately by a certain key until you get to a word or something that makes sense.
	 * 
	 * RIDZWQOHS -> DUPLICATE using key -14
	 * 
	 * Now you can see that the key being used is +14 to encrypt the word, so that means we have to shift by -14 on each of the encrypted letters to decrypt them.
	 * You can also plug-in letters that are in the first word into other words.
	 * 
	 * "duplicate letteFG aFe TuB tC decFMpt" -> You can see now that a message starts to almost completely unfold because of all the duplicate letters we found.
	 * 
	 * Message "RIDZWQOHS ZSHHSFG OFS TIB HC RSQFMDH" decrypted into "DUPLICATE LETTERS ARE FUN TO DECRYPT" with the shift key -14.
	 * 
	 */

	/// <summary>
	/// Encrypt a message using the rules of Caesar Cipher. Can be Case-Insensitive and Sensitive.
	/// </summary>
	/// <param name="messageToEncrypt">The message to be encrypted.</param>
	/// <param name="caesarKey">A positive value integer that will be used to shift the letters around.</param>
	/// <param name="subtractKey">Determines if the key should be subtracted from the letters instead of added to the letters.</param>
	/// <param name="caseInsensitive">Whether the message should be fully capitialized (true) or keep each state of the letter the same (false).</param>
	/// <returns>
	/// The message given encrypted with Caesar Ciphering.
	/// <para>Example of Case-Insensitive: Hello, Case-Insensitive -> PMTTW.</para>
	/// <para>Example of Case-Sensitive: HeLlO, Case-Sensitive -> PmTtW.</para>
	/// </returns>
	public string EncryptMessageUsingCaesar(string messageToEncrypt, int caesarKey, bool subtractKey, bool caseInsensitive)
	{
		if (caesarKey < 0) { Debug.LogErrorFormat("{0} Unable to encrypt the message using Caesar, an error has occured: The Caesar Key cannot be a negative value. Please set the \"subtractKey\" variable to true.", loggingTag); return null; }
		List<char[]> alphabets = new List<char[]>();
		alphabets.Add(Enumerable.Range(0, 26).Select(x => (char)(x + 'A')).ToArray());
		alphabets.Add(alphabets[0].Select(x => char.ToLower(x)).ToArray());

		StringBuilder sb = new StringBuilder();

		foreach (char c in messageToEncrypt)
		{
			int index = 0;

			if (!caseInsensitive)
			{
				if (char.IsLower(c))
				{
					index = Array.IndexOf(alphabets[1], c);
					if (subtractKey)
					{
						index = Modulo(index - caesarKey, 26);
						sb.Append(alphabets[1][index]);
						continue;
					}
					index = Modulo(index + caesarKey, 26);
					sb.Append(alphabets[1][index]);
				}
				else
				{
					index = Array.IndexOf(alphabets[0], c);
					if (subtractKey)
					{
						index = Modulo(index - caesarKey, 26);
						sb.Append(alphabets[0][index]);
						continue;
					}
					index = Modulo(index + caesarKey, 26);
					sb.Append(alphabets[0][index]);
				}

			}

			index = Array.IndexOf(alphabets[0], char.ToUpper(c));

			if (subtractKey)
			{
				index = Modulo(index - caesarKey, 26);
				sb.Append(alphabets[0][index]);
				continue;
			}
			index = Modulo(index + caesarKey, 26);
			sb.Append(alphabets[0][index]);
			continue;

		}
		if (enableLogging) Debug.LogFormat("{0} Encrypting \"{1}\" using Caesar. Each letter will be shifted by {2}. The message to be returned will be {3}. The encrypted output is \"{4}\".", loggingTag, messageToEncrypt, subtractKey ? "-" + caesarKey : "+" + caesarKey, caseInsensitive ? "case-insensitive" : "case-sensitive", sb.ToString());
		return sb.ToString();
	}

	/// <summary>
	/// Decrypt a message using the rules of Caesar Cipher. Can be Case-Insensitive and Sensitive.
	/// </summary>
	/// <param name="messageToDecrypt">The message to be decrypted.</param>
	/// <param name="caesarKey">A positive value integer that will be used to shift the letters around.</param>
	/// <param name="subtractKey">Determines if the key should be subtracted from the letters instead of added to the letters.</param>
	/// <param name="caseInsensitive">Whether the message should be fully capitialized (true) or keep each state of the letter the same (false).</param>
	/// <returns>
	/// The message given decrypted with Caesar Ciphering.
	/// <para>Example of Case-Insensitive: Pmttw, Case-Insensitive -> HELLO.</para>
	/// <para>Example of Case-Sensitive: PmTtW, Case-Sensitive -> HeLlO.</para>
	/// </returns>
	public string DecryptMessageUsingCaesar(string messageToDecrypt, int caesarKey, bool subtractKey, bool caseInsensitive)
	{
		if (caesarKey < 0) { Debug.LogErrorFormat("{0} Unable to decrypt the message using Caesar, an error has occured: The Caesar Key cannot be a negative value. Please set the \"subtractKey\" variable to true.", loggingTag); return null; }
		List<char[]> alphabets = new List<char[]>();
		alphabets.Add(Enumerable.Range(0, 26).Select(x => (char)(x + 'A')).ToArray());
		alphabets.Add(alphabets[0].Select(x => char.ToLower(x)).ToArray());

		StringBuilder sb = new StringBuilder();

		foreach (char c in messageToDecrypt)
		{
			int index = 0;

			if (!caseInsensitive)
			{
				if (char.IsLower(c))
				{
					index = Array.IndexOf(alphabets[1], c);
					if (subtractKey)
					{
						index = Modulo(index - caesarKey, 26);
						sb.Append(alphabets[1][index]);
						continue;
					}
					index = Modulo(index + caesarKey, 26);
					sb.Append(alphabets[1][index]);
				}
				else
				{
					index = Array.IndexOf(alphabets[0], c);
					if (subtractKey)
					{
						index = Modulo(index - caesarKey, 26);
						sb.Append(alphabets[0][index]);
						continue;
					}
					index = Modulo(index + caesarKey, 26);
					sb.Append(alphabets[0][index]);
				}

			}

			index = Array.IndexOf(alphabets[0], char.ToUpper(c));

			if (subtractKey)
			{
				index = Modulo(index - caesarKey, 26);
				sb.Append(alphabets[0][index]);
				continue;
			}
			index = Modulo(index + caesarKey, 26);
			sb.Append(alphabets[0][index]);
			continue;

		}
		if (enableLogging) Debug.LogFormat("{0} Decrypting \"{1}\" using Caesar. Each letter will be shifted by {2}. The message to be returned will be {3}. The decrypted output is \"{4}\".", loggingTag, messageToDecrypt, subtractKey ? "-" + caesarKey : "+" + caesarKey, caseInsensitive ? "case-insensitive" : "case-sensitive", sb.ToString());
		return sb.ToString();
	}

	// -----

	/*
	 * Affine is a little bit more advanced then Atbash, ROT13 and Caesar, but can still be easy to encrypt/decrypt if you know the equations.
	 * 
	 * For more indepth information then what is here you can always check these links below:
	 * http://practicalcryptography.com/ciphers/classical-era/affine/
	 * https://en.wikipedia.org/wiki/Affine_cipher#:~:text=The%20affine%20cipher%20is%20a,converted%20back%20to%20a%20letter.
	 * https://crypto.interactive-maths.com/affine-cipher.html
	 * 
	 * To Encrypt:
	 * Now this could get confusing to understand since this cipher requires a little bit more math then others.
	 * 
	 * Throughout this example 'a' will be the shift amount, 'b' will be the alphabet offset and 'm' will be the size of the alphabet.
	 * 
	 * Let 'a' = 5, Let 'b' = 5, Let 'm' = 26 (The size of the upper-case alphabet).
	 * 
	 * Lets setup our first alphabet set.
	 * ABCDEFGHIJKLMNOPQRSTUVWXYZ
	 * 
	 * Now, we can create a cipher alphabet (optional). 
	 * When we are encrypting a letter from a message, we use a specific equation:
	 * '(ax + b) mod m', where 'x' is the alphabetical position (A0Z25) of the message letter
	 * 
	 * So, with this lets create our alphabet:
	 * ABCDEFGHIJKLMOPQRSTUVWXYZ
	 * 
	 * A -> [5(0) + 5] mod 26 -> 5 -> F | So, we have our first letter, whenever we see 'A' we know that turns into an 'F'.
	 * B -> [5(1) + 5] mod 26 -> 10 -> K | Here's the second, wherever there is 'B' that becomes 'K'.
	 * etc...
	 * 
	 * Completing all of the calculations for each letter, you'll get a cipher alphabet:
	 * FKPUZEJOTYDINSXCHMRWBGLQVA
	 * 
	 * Now, we have both of our alphabets.
	 * ABCDEFGHIJKLMNOPQRSTUVWXYZ
	 * FKPUZEJOTYDINSXCHMRWBGLQVA
	 * 
	 * Take the message "CIPHER TIME" and encrypt it.
	 * 
	 * C -> P
	 * I -> T
	 * P -> C
	 * H -> O
	 * E -> Z
	 * R -> M
	 * 
	 * T -> W
	 * I -> T
	 * M -> N
	 * E -> Z
	 * 
	 * Message "CIPHER TIME" encrypted is "PTCOZM WTNZ"
	 * 
	 * To Decrypt:
	 * 
	 * We're going to use a new equation when we're decrypting:
	 * 
	 * 'c(x - b) mod m', where 'c' is the modular multiplicative inverse of 'a'
	 * In otherwords, where 'a × c = 1 mod m', where c can be any number that multiplied by a and modulo by m (which is the length of the alphabet being used) equals to 1. Otherwise, its invalid.
	 * 
	 * Using the example from above, lets decrypt "PTCOZM WTNZ".
	 * Where 'a' = 5, 'b' = 5 and 'm' = 26.
	 * 
	 * Lets start plugging in information into the equation for our first letter.
	 * 
	 * c(16 - 5) mod 26
	 * 
	 * Now, lets figure out what the value of 'c' is by finding the modular multiplicative inverse of 'a'.
	 * 
	 * 5c = 1 mod 26, 'c' cannot be anything that allows the modulo to not be into play unless 'a' = 1. So this rules out 0-5 immediately.
	 * 
	 * So, now we will go higher until we find a number that when multiplied by 'a' and modulo 'm' is equal to 1.
	 * 
	 * c = 6 -> 5(6) = 30 mod 26 = 4 != 1
	 * c = 7 -> 5(7) = 35 mod 26 = 9 != 1
	 * At this point, you can see a pattern start to occur the next few numbers will be 14, 19, 24. And after this we can modulo at least twice into the number.
	 * 
	 * c = 11 -> 5(11) = 55 mod 52 = 3 != 1
	 * c = 12 -> 5(12) = 60 mod 52 = 8 != 1
	 * Yet another pattern, and each time we come past the modulo and increase it we see that the starting number decreases every 6 instances of 'c'.
	 * 
	 * So if c = 6 and the answer is 4. that means that once we reach 21, the equation will equal to 1.
	 * This means the modular multiplicative inverse of '5' is '21'.
	 * 
	 * 
	 * 
	 */

	/// <summary>
	/// Encrypts a message using the rules of Affine Cipher. Can be Case-Insensitive and Sensitive.
	/// </summary>
	/// <param name="messageToEncrypt">The message to encrypt.</param>
	/// <param name="shiftAmount">This is will be life a Caesar Shift Key but; there are some restrictions to certain numbers. <para>Possible numbers are 1, 3, 5, 7, 9, 11, 15, 17, 19, 21, 23 and 25.</para></param>
	/// <param name="alphabetOffset">This will be for how you want to alphabet to be shifted <i>after</i> shift amount.</param>
	/// <param name="caseInsensitive">Whether the message should be fully capitialized (true) or keep each state of the letter the same (false).</param>
	/// <returns>
	/// The message given encrypted with Affine.
	/// <para>Example of Case-Insensitive: Hello, Case-Insensitive -> OZIIX.</para>
	/// <para>Example of Case-Sensitive: HeLlO, Case-Sensitive -> OzIiX.</para>
	/// </returns>
	public string EncryptMessageUsingAffine(string messageToEncrypt, int shiftAmount, int alphabetOffset, bool caseInsensitive)
	{
		int[] properAValues = new int[] { 1, 3, 5, 7, 9, 11, 15, 17, 19, 21, 23, 25 };
		if (!properAValues.Any(x => x == shiftAmount)) { Debug.LogErrorFormat("{0} Unable to encrypt message using Affine, an error has occured: The \"shift amount\" cannot be {1}. Please change to one of the following: {2}.", loggingTag, shiftAmount, properAValues.Join(", ")); }
		char[] alphabet = Enumerable.Range(0, 26).Select(x => (char)(x + 'A')).ToArray();
		string tempMessage = messageToEncrypt.ToUpper();
		List<int> messageAlphaPositions = tempMessage.Select(x => Array.IndexOf(alphabet, x)).ToList();

		char[] cipherAlphabet = new char[26];

		for (int i = 0; i < 26; i++)
		{
			cipherAlphabet[i] = alphabet[((shiftAmount * i) + alphabetOffset) % alphabet.Length];
		}

		StringBuilder sb = new StringBuilder();

		for (int i = 0; i < messageToEncrypt.Length; i++)
		{
			if (!caseInsensitive)
			{
				if (char.IsLower(messageToEncrypt[i]))
				{
					sb.Append(char.ToLower(cipherAlphabet[messageAlphaPositions[i]]));
					continue;
				}
				sb.Append(cipherAlphabet[messageAlphaPositions[i]]);
				continue;
			}
			sb.Append(cipherAlphabet[messageAlphaPositions[i]]);
		}
		if (enableLogging) Debug.LogFormat("{0} Encrypting \"{1}\" using Affine where a = {2} and b = {3}. The cipher alphabet that has been created is {4}. The message to be returned will be {5}. The encrypted output is \"{6}\".",
			loggingTag, messageToEncrypt, shiftAmount, alphabetOffset, cipherAlphabet.Join(""), caseInsensitive ? "case-insensitive" : "case-sensitive", sb.ToString());
		return sb.ToString();
	}

	/// <summary>
	/// Decrypts a message using the rules of Affine Cipher. Can be Case-Insensitive and Sensitive.
	/// </summary>
	/// <param name="messageToDecrypt">The message to decrypt.</param>
	/// <param name="shiftAmount">This is will be life a Caesar Shift Key but; there are some restrictions to certain numbers. <para>Possible numbers are 1, 3, 5, 7, 9, 11, 15, 17, 19, 21, 23 and 25.</para></param>
	/// <param name="alphabetOffset">This will be for how you want to alphabet to be shifted <i>after</i> shift amount.</param>
	/// <param name="caseInsensitive">Whether the message should be fully capitialized (true) or keep each state of the letter the same (false).</param>
	/// <returns>
	/// The message given decrypted with Affine.
	/// <para>Example of Case-Insensitive: Hello, Case-Insensitive -> OZIIX.</para>
	/// <para>Example of Case-Sensitive: HeLlO, Case-Sensitive -> OzIiX.</para>
	/// </returns>
	public string DecryptMessageUsingAffine(string messageToDecrypt, int shiftAmount, int alphabetOffset, bool caseInsensitive)
	{

		int[] properAValues = new int[] { 1, 3, 5, 7, 9, 11, 15, 17, 19, 21, 23, 25 };
		if (!properAValues.Any(x => x == shiftAmount)) { Debug.LogErrorFormat("{0} Unable to encrypt message using Affine, an error has occured: The \"shift amount\" cannot be {1}. Please change to one of the following: {2}.", loggingTag, shiftAmount, properAValues.Join(", ")); }
		char[] alphabet = Enumerable.Range(0, 26).Select(x => (char)(x + 'A')).ToArray();
		string tempMessage = messageToDecrypt.ToUpper();

		char[] cipherAlphabet = new char[26];

		for (int i = 0; i < 26; i++)
		{
			cipherAlphabet[i] = alphabet[((shiftAmount * i) + alphabetOffset) % alphabet.Length];
		}

		List<int> messageAlphaPositions = tempMessage.Select(x => Array.IndexOf(cipherAlphabet, x)).ToList();

		StringBuilder sb = new StringBuilder();

		for (int i = 0; i < messageToDecrypt.Length; i++)
		{
			if (!caseInsensitive)
			{
				if (char.IsLower(messageToDecrypt[i]))
				{
					sb.Append(char.ToLower(alphabet[messageAlphaPositions[i]]));
					continue;
				}
				sb.Append(alphabet[messageAlphaPositions[i]]);
				continue;
			}
			sb.Append(alphabet[messageAlphaPositions[i]]);
		}
		if (enableLogging) Debug.LogFormat("{0} Decrypting \"{1}\" using Affine where a = {2} and b = {3}. The cipher alphabet that has been created is {4}. The message to be returned will be {5}. The decrypted output is \"{6}\".",
			loggingTag, messageToDecrypt, shiftAmount, alphabetOffset, cipherAlphabet.Join(""), caseInsensitive ? "case-insensitive" : "case-sensitive", sb.ToString());
		return sb.ToString();
	} // Will come back to do actual method.

	// -----

	/// <summary>
	/// Encrypts a messsage using the rules of Railfence Cipher. Can be Case-Insensitive and Sensitive.
	/// </summary>
	/// <param name="messageToEncrypt">The message to encrypt.</param>
	/// <param name="amountOfRails">The amount of rails that the message will be encoded in.</param>
	/// <param name="caseInsensitive">Whether the message should be fully capitialized (true) or keep each state of the letter the same (false).</param>
	/// <returns>
	/// The message given encrypted with Railfence.
	/// <para>Example of Case-Insensitive: Cipher, Case-Insensitive -> CEIHRP.</para>
	/// <para>Example of Case-Sensitive: CiPhEr, Case-Sensitive -> CEihrP.</para>
	/// </returns>
	public string EncryptMessageUsingRailfence(string messageToEncrypt, int amountOfRails, bool caseInsensitive)
	{

		List<List<char>> rows = new List<List<char>>();

		for (int i = 0; i < amountOfRails; i++)
		{
			rows.Add(new List<char>());
		}

		int x = 0;
		bool oppo = false;

		foreach (char c in messageToEncrypt.Replace(" ", ""))
		{
			if (x == amountOfRails) { oppo = true; x -= 2; }

			if (oppo)
			{
				if (x == 0)
				{
					oppo = false;
				}
				else
				{
					rows[x].Add(c);
					x--;
					continue;
				}
			}
			rows[x].Add(c);
			x++;

		}

		StringBuilder sb = new StringBuilder();
		int test = 1;
		foreach (List<char> cl in rows)
		{
			foreach (char c in cl)
			{
				sb.Append(c);
			}
			test++;
		}
		if (enableLogging) Debug.LogFormat("{0} Encrypting \"{1}\" using Railfence. The message to be returned will be {2}. The encrypted output is \"{3}\".",
			loggingTag, messageToEncrypt, caseInsensitive ? "case-insensitive" : "case-sensitive", sb.ToString());
		return sb.ToString().Replace(" ", "");
	}

	/// <summary>
	/// Decrypts a messsage using the rules of Railfence Cipher. Can be Case-Insensitive and Sensitive.
	/// </summary>
	/// <param name="messageToDecrypt">The message to encrypt.</param>
	/// <param name="amountOfRails">The amount of rails that the message will be encoded in.</param>
	/// <param name="caseInsensitive">Whether the message should be fully capitialized (true) or keep each state of the letter the same (false).</param>
	/// <returns>
	/// The message given decrypted with Railfence.
	/// <para>Example of Case-Insensitive: Ceihrp, Case-Insensitive -> CIPHER.</para>
	/// <para>Example of Case-Sensitive: CeIhRp, Case-Sensitive -> CIpheR.</para>
	/// </returns>
	public string DecryptMessageUsingRailfence(string messageToDecrypt, int amountOfRails, bool caseInsensitive)
	{

		List<char[]> rows = new List<char[]>();

		string tempMessage = messageToDecrypt.Replace(" ", "");

		int cycleAmount = (amountOfRails * 2) - 2;
		int fullCycles = tempMessage.Length / cycleAmount;
		int partialCycle = tempMessage.Length % cycleAmount;

		StringBuilder sb = new StringBuilder();

		for (int i = 0; i < amountOfRails; i++)
		{
			int size = fullCycles;
			if (i > 0 && i < amountOfRails - 1)
			{
				size = fullCycles * 2;
			}
			if (i + 1 <= partialCycle) { size++; }
			rows.Add(new char[size]);
		}

		int messageIndex = 0;

		foreach (char[] ca in rows)
		{
			for (int i = 0; i < ca.Length; i++)
			{
				ca[i] = tempMessage[messageIndex];
				messageIndex++;
			}
		}

		int[] rowIndexs = new int[amountOfRails];

		int x = 0;
		bool oppo = false;

		for (int i = 0; i < tempMessage.Length; i++)
		{

			if (x == amountOfRails) { oppo = true; x -= 2; }

			if (oppo)
			{
				if (x == 0)
				{
					oppo = false;
				}
				else
				{
					sb.Append(rows[x][rowIndexs[x]]);
					rowIndexs[x]++;
					x--;
					continue;
				}
			}
			sb.Append(rows[x][rowIndexs[x]]);
			rowIndexs[x]++;
			x++;

		}
		if (enableLogging) Debug.LogFormat("{0} Decrypting \"{1}\" using Railfence. The message to be returned will be {2}. The decrypted output is \"{3}\".",
			loggingTag, messageToDecrypt, caseInsensitive ? "case-insensitive" : "case-sensitive", sb.ToString());
		return sb.ToString().Replace(" ", "");
	}

	// Extra Methods

	/// <summary>
	/// An extra modulo function that allows for negative modulos to happen. Can be made public, if needed.
	/// </summary>
	/// <param name="x">The negative/positive integer that needs the operation done on.</param>
	/// <param name="mod">The modulo being used.</param>
	/// <returns>A non-negative integer within range of the modulo.</returns>
	int Modulo(int x, int mod)
	{
		while (x < 0)
		{
			x += mod;
		}
		return x % mod;
	}

	/// <summary>
	/// Returns the modular multiplicative inverse of the number given.
	/// </summary>
	/// <param name="x">The value being used to look for the modular multiplicative inverse of.</param>
	/// <param name="mod">The integer value to use for modulo.</param>
	/// <returns>The integer that is the modular multiplicative inverse of <i>x</i></returns>
	public int GetModularMultiInverse(int x, int mod)
	{
		for (int i = 0; i <= 200; i++)
		{
			int result = (x * i) % mod;
			if (result == 1) return i;
		}
		return -1;
	}

}
