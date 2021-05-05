using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

/// <summary>
/// Group project to handle invoices and items.
/// Search feature to show all the invoices or you can filter the results by the values
/// and let the user to select the invoice.
/// 
/// Author: Henry Doan with use of the Toolbox in Visual Studios IDE in the designer
///         Shawn Cowder with HandleError method.
/// </summary>
namespace GroupProject.Database
{
    /// <summary>
    /// Blueprint of what the invoice object is.
    /// </summary>
    public class clsInvoiceObj
    {
        #region Invoice attributes 
        public string sInvoiceNum { get; set; } // primary key
        public string sInvoiceDate { get; set; }
        public string sTotalCost { get; set; }
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
    } // end of class
} // end of namespace
