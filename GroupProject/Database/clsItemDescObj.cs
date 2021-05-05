using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GroupProject.Database
{
    public class clsItemDescObj
    {
        #region Item Description attributes 
        public string sItemCode { get; set; } 
        public string sItemDesc { get; set; }
        public string sCost { get; set; }
        #endregion

        #region Helper methods

        /// <summary>
        /// Handle the error.
        /// Made by Shawn Cowder
        /// </summary>
        /// <param name="sClass">The class in which the error occurred in.</param>
        /// <param name="sMethod">The method in which the error occurred in.</param>
        /// <param name="sMessage">Message of the error.</param>
        private void HandleError(string sClass, string sMethod, string sMessage)
        {
            try
            {
                //Would write to a file or database here.
                MessageBox.Show(sClass + "." + sMethod + " -> " + sMessage);
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText("C:\\Error.txt", Environment.NewLine +
                                             "HandleError Exception: " + ex.Message);
            }
        } // end of HandleError method

        #endregion
    }
}
