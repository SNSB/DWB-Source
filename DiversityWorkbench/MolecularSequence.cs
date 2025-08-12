using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiversityWorkbench
{
    public static class MolecularSequence
    {
        #region Basic tables
        /// <summary>
        /// List of amino acids with three-letter and one-letter symbols
        /// </summary>
        private static string[] _AminoAcid = { "Alanine",        "Ala", "A",
                                               "Arginine",       "Arg", "R",
                                               "Asparagine",     "Asn", "N",
                                               "Aspartic acid",  "Asp", "D",
                                               "Cysteine",       "Cys", "C",
                                               "Glutamic acid",  "Glu", "E",
                                               "Glutamine",      "Gln", "Q",
                                               "Glycine",        "Gly", "G",
                                               "Histidine",      "His", "H",
                                               "Isoleucine",     "Ile", "I",
                                               "Leucine",        "Leu", "L",
                                               "Lysine",         "Lys", "K",
                                               "Methionine",     "Met", "M",
                                               "Phenylalanine",  "Phe", "F",
                                               "Proline",        "Pro", "P",
                                               "Serine",         "Ser", "S",
                                               "Threonine",      "Thr", "T",
                                               "Tryptophan",     "Trp", "W",
                                               "Tyrosine",       "Tyr", "Y",
                                               "Valine",         "Val", "V",
                                               "Selenocysteine", "Sec", "U",
                                               "Pyrrolysine",    "Pyl", "O" };

        /// <summary>
        /// List of amino acid ambiguity values with three-letter and one-letter symbols
        /// </summary>
        private static string[] _AmbAminoAcid = { "Asn or Asp",  "Asx", "B",
                                                  "Gln or Glu",  "Glx", "Z",
                                                  "Leu or Ile",  "Xle", "J",
                                                  "Unspecified", "Xaa", "X" };

        /// <summary>
        /// List of nucliotide characters with long text
        /// </summary>
        private static string[] _Nucleotide = { "A", "Adenine",
                                                "C", "Cytosine",
                                                "G", "Guanine",
                                                "T", "Thymine",
                                                "U", "Uracil" };

        /// <summary>
        /// List of nucliotide ambiguity characters with long text
        /// </summary>
        private static string[] _AmbNucleotide = { "W", "Weak (A or T)",
                                                   "S", "Strong (C or G)",
                                                   "M", "Amino (A or C)",
                                                   "K", "Keto (G or T)",
                                                   "R", "Purine (A or G)",
                                                   "Y", "Pyrimidine (C or T)",
                                                   "B", "not A",
                                                   "D", "not C",
                                                   "H", "not G",
                                                   "V", "not T",
                                                   "N", "No idea (not a gap)" };
        #endregion

        #region Public static functions
        /// <summary>
        /// Provide a list of amino acid symbols and long text
        /// </summary>
        /// <param name="symbolLength">Symbol length 1 or 3</param>
        /// <returns>Dictionary with symbols (upper case) and long text</returns>
        public static System.Collections.Generic.Dictionary<string, string> AminoAcids(int symbolLength)
        {
            // Allocate response
            System.Collections.Generic.Dictionary<string, string> result = new Dictionary<string, string>();

            // Evaluate symbol length
            int offset = symbolLength == 1 ? 2 : 1;

            // Copy values
            for (int i = 0; i < _AminoAcid.Length; i += 3)
                result.Add(_AminoAcid[i + offset].ToUpper(), _AminoAcid[i]);

            return result;
        }

        /// <summary>
        /// Provide a list of amino acid ambiguity symbols and long text
        /// </summary>
        /// <param name="symbolLength">Symbol length 1 or 3</param>
        /// <returns>Dictionary with symbols (upper case) and long text</returns>
        public static System.Collections.Generic.Dictionary<string, string> AmbigiousAminoAcids(int symbolLength)
        {
            // Allocate response
            System.Collections.Generic.Dictionary<string, string> result = new Dictionary<string, string>();

            // Evaluate symbol length
            int offset = symbolLength == 1 ? 2 : 1;

            // Copy values
            for (int i = 0; i < _AmbAminoAcid.Length; i += 3)
                result.Add(_AmbAminoAcid[i + offset].ToUpper(), _AmbAminoAcid[i]);

            return result;
        }

        /// <summary>
        /// Provide a list of amino acid and ambiguity symbols mapping table one-letter to three-letter
        /// </summary>
        /// <returns>Dictionary with one-letter symbols (upper case) and three-letter symbols</returns>
        public static System.Collections.Generic.Dictionary<string, string> AminoAcid123()
        {
            // Allocate response
            System.Collections.Generic.Dictionary<string, string> result = new Dictionary<string, string>();

            // Copy values
            for (int i = 0; i < _AminoAcid.Length; i += 3)
                result.Add(_AminoAcid[i + 2].ToUpper(), _AminoAcid[i + 1]);

            for (int i = 0; i < _AmbAminoAcid.Length; i += 3)
                result.Add(_AmbAminoAcid[i + 2].ToUpper(), _AmbAminoAcid[i + 1]);

            return result;
        }

        /// <summary>
        /// Provide a list of amino acid and ambiguity symbols mapping table three-letter to one-letter
        /// </summary>
        /// <returns>Dictionary with one-letter symbols (upper case) and three-letter symbols</returns>
        public static System.Collections.Generic.Dictionary<string, string> AminoAcid321()
        {
            // Allocate response
            System.Collections.Generic.Dictionary<string, string> result = new Dictionary<string, string>();

            // Copy values
            for (int i = 0; i < _AminoAcid.Length; i += 3)
                result.Add(_AminoAcid[i + 1].ToUpper(), _AminoAcid[i + 2]);

            for (int i = 0; i < _AmbAminoAcid.Length; i += 3)
                result.Add(_AmbAminoAcid[i + 1].ToUpper(), _AmbAminoAcid[i + 2]);

            return result;
        }

        /// <summary>
        /// Provide a list of nucleotide symbols and long text
        /// </summary>
        /// <returns>Dictionary with symbols (upper case) and long text</returns>
        public static System.Collections.Generic.Dictionary<string, string> Nucleotides()
        {
            // Allocate response
            System.Collections.Generic.Dictionary<string, string> result = new Dictionary<string, string>();

            // Copy values
            for (int i = 0; i < _Nucleotide.Length; i += 2)
                result.Add(_Nucleotide[i].ToUpper(), _Nucleotide[i + 1]);

            return result;
        }

        /// <summary>
        /// Provide a list of nucleotide ambiguity symbols and long text
        /// </summary>
        /// <returns>Dictionary with symbols (upper case) and long text</returns>
        public static System.Collections.Generic.Dictionary<string, string> AmbigiousNucleotides()
        {
            // Allocate response
            System.Collections.Generic.Dictionary<string, string> result = new Dictionary<string, string>();

            // Copy values
            for (int i = 0; i < _AmbNucleotide.Length; i += 2)
                result.Add(_AmbNucleotide[i].ToUpper(), _AmbNucleotide[i + 1]);

            return result;
        }

        /// <summary>
        /// Provide a list of nucleotide and ambiguity symbols and long text
        /// </summary>
        /// <returns>Dictionary with symbols (upper case) and long text</returns>
        public static System.Collections.Generic.Dictionary<string, string> NucleotideAlphabet()
        {
            // Allocate response
            System.Collections.Generic.Dictionary<string, string> result = new Dictionary<string, string>();

            // Copy values
            for (int i = 0; i < _Nucleotide.Length; i += 2)
                result.Add(_Nucleotide[i].ToUpper(), _Nucleotide[i + 1]);
            for (int i = 0; i < _AmbNucleotide.Length; i += 2)
                result.Add(_AmbNucleotide[i].ToUpper(), _AmbNucleotide[i + 1]);

            return result;
        }

        /// <summary>
        /// Get formatted sequence string
        /// Transforms sequence string to capital letters rsp. 'Abc' form if a legal symbol is detected.
        /// Unrecognized characters stay unchanged.
        /// </summary>
        /// <param name="text">Raw sequence string</param>
        /// <param name="symbolLength">Symbol length</param>
        /// <param name="gapSymbol">Gap symbol</param>
        /// <param name="symbols">Symbol list</param>
        /// <param name="ambSymbols">Ambiguity symbol list</param>
        /// <param name="sb">String builder for builidng the result string</param>
        /// <returns>Formatted sequence string</returns>
        public static string FormattedSequence(string text, int symbolLength, string gapSymbol, Dictionary<string, string> symbols, Dictionary<string, string> ambSymbols, StringBuilder sb)
        {
            if (symbolLength > 0)
            {
                // Clear string builder
                if (sb == null)
                    sb = new StringBuilder();
                else
                    if (sb.Length > 0) sb.Remove(0, sb.Length);

                // Scan source string symbol-wise
                if (gapSymbol == null)
                    gapSymbol = "";
                for (int i = 0; i < text.Length; i += symbolLength)
                {
                    // Extract actual symbol
                    string sym = text.Substring(i, (text.Length - i >= symbolLength ? symbolLength : text.Length - i));
                    sym = sym.Replace('\n', ' ').Replace('\t', ' ').Replace("\r", "");

                    // Symbol is known
                    if (symbols.ContainsKey(sym.ToUpper()) || (ambSymbols.ContainsKey(sym.ToUpper())) || gapSymbol.ToUpper() == sym.ToUpper())
                    {
                        // Format known symbol to Xxx or X
                        sb.Append(text[i].ToString().ToUpper());
                        for (int j = 1; j < symbolLength; j++)
                            if (i + j < text.Length)
                                sb.Append(text[i + j].ToString().ToLower());
                    }
                    else
                        sb.Append(sym); // Don't format unknown symbols
                }
                return sb.ToString();
            }
            // Return unformatted text if no sybols are defined
            return text;
        }

        /// <summary>
        /// Get formatted sequence string
        /// Transforms sequence string to capital letters rsp. 'Abc' form if a legal symbol is detected.
        /// Unrecognized characters stay unchanged.
        /// </summary>
        /// <param name="text">Raw sequence string</param>
        /// <param name="symbolLength">Symbol length</param>
        /// <param name="gapSymbol">Gap symbol</param>
        /// <param name="isNucleotide">'true' if sequence is of type 'Nucleotide'</param>
        /// <param name="enableAmbiguity">'true' if ambiguity symbols are allowed</param>
        /// <returns>Formatted sequence string</returns>
        public static string FormattedSequence(string text, int symbolLength, string gapSymbol, bool isNucleotide, bool enableAmbiguity)
        {
            System.Collections.Generic.Dictionary<string, string> symbols;
            System.Collections.Generic.Dictionary<string, string> ambSymbols;
            if (gapSymbol == null)
                gapSymbol = "";
            if (isNucleotide)
            {
                symbols = MolecularSequence.Nucleotides();
                ambSymbols = enableAmbiguity ? MolecularSequence.AmbigiousNucleotides() : new System.Collections.Generic.Dictionary<string, string>();
            }
            else
            {
                symbols = MolecularSequence.AminoAcids(symbolLength);
                ambSymbols = enableAmbiguity ? MolecularSequence.AmbigiousAminoAcids(symbolLength) : new System.Collections.Generic.Dictionary<string, string>();
            }
            return FormattedSequence(text, symbolLength, gapSymbol, symbols, ambSymbols, null);
        }
        #endregion
    }
}
