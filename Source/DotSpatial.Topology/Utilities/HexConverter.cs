// ********************************************************************************************************
// Product Name: DotSpatial.Topology.dll
// Description:  The basic topology module for the new dotSpatial libraries
// ********************************************************************************************************
// The contents of this file are subject to the Lesser GNU Public License (LGPL)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license  Alternately, you can access an earlier version of this content from
// the Net Topology Suite, which is also protected by the GNU Lesser Public License and the sourcecode
// for the Net Topology Suite can be obtained here: http://sourceforge.net/projects/nts.
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from the Net Topology Suite, which is a C# port of the Java Topology Suite.
//
// The Initial Developer to integrate this code into MapWindow 6.0 is Ted Dunsford.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// |         Name         |    Date    |                              Comment
// |----------------------|------------|------------------------------------------------------------
// |                      |            |
// ********************************************************************************************************

using System;

namespace DotSpatial.Topology.Utilities
{
    /// <summary>
    ///
    /// </summary>
    public class HexConverter
    {
        /// <summary>
        /// Only static methods!
        /// </summary>
        private HexConverter() { }

        /// <summary>
        /// Convert the given numeric value (passed as string) of the base specified by <c>baseIn</c>
        /// to the value specified by <c>baseOut</c>.
        /// </summary>
        /// <param name="valueIn">Numeric value to be converted, as string.</param>
        /// <param name="baseIn">Base of input value.</param>
        /// <param name="baseOut">Base to use for conversion.</param>
        /// <returns>Converted value, as string.</returns>
        public static string ConvertAny2Any(string valueIn, int baseIn, int baseOut)
        {
            string result = "Error";

            valueIn = valueIn.ToUpper();
            const string codice = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            // test per limite errato sulle basi in input e/o in output
            if ((baseIn < 2) || (baseIn > 36) ||
                 (baseOut < 2) || (baseOut > 36))
                return result;

            if (valueIn.Trim().Length == 0)
                return result;

            // se baseIn e baseOut sono uguali la conversione è già fatta!
            if (baseIn == baseOut)
                return valueIn;

            // determinazione del valore totale
            double valore = 0;
            try
            {
                // se il campo è in base 10 non c'è bisogno di calcolare il valore
                if (baseIn == 10)
                    valore = double.Parse(valueIn);
                else
                {
                    char[] c = valueIn.ToCharArray();

                    // mi serve per l'elevazione a potenza e la trasformazione
                    // in valore base 10 della cifra
                    int posizione = c.Length;

                    // ciclo sui caratteri di valueIn
                    // calcolo del valore decimale

                    for (int k = 0; k < c.Length; k++)
                    {
                        // valore posizionale del carattere
                        int valPos = codice.IndexOf(c[k]);

                        // verifica per caratteri errati
                        if ((valPos < 0) || (valPos > baseIn - 1))
                            return result;

                        posizione--;
                        valore += valPos * Math.Pow(baseIn, posizione);
                    }
                }

                // generazione del risultato final
                // se il risultato da generare è in base 10 non c'è
                // bisogno di calcoli
                if (baseOut == 10)
                {
                    result = valore.ToString();
                }
                else
                {
                    result = String.Empty;
                    while (valore > 0)
                    {
                        int resto = (int)(valore % baseOut);
                        valore = (valore - resto) / baseOut;
                        result = codice.Substring(resto, 1) + result;
                    }
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }
    }
}