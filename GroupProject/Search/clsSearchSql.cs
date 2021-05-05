using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Group project to handle invoices and items.
/// Search feature to show all the invoices or you can filter the results by the values
/// and let the user to select the invoice.
/// 
/// Author: Henry Doan with use of the Toolbox in Visual Studios IDE in the designer
///         Shawn Cowder with HandleError method.
/// </summary>
namespace GroupProject.Search
{
    /// <summary>
    /// Class that houses all of the sql query needed for the search feature of the project
    /// </summary>
    public class clsSearchSql
    {
        #region Logic methods for the sql statements

        /// <summary>
        /// A method that returns a sql query to grab all details of all
        /// the invoices
        /// </summary>
        /// <returns>a sql query to grab the invoices and details</returns>
        public string SelectInvoicesData()
        {
            try
            {
                string sSql = "SELECT * FROM Invoices";
                return sSql;
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText("C:\\Error.txt", Environment.NewLine +
                                             "HandleError Exception: " + ex.Message);
                // return empty string because something is wrong
                return "";
            }
        } // end of SelectInvoicesData method

        /// <summary>
        /// A method that returns a sql query to grab all details of the invoices
        /// that match with the given invoice id.
        /// </summary>
        /// <param name="sInvoiceId">id of a invoice passed in</param>
        /// <returns>a sql query to grab the invoices with the id</returns>
        public string SelectInvoiceByIdData(string sInvoiceId)
        {
            try
            {
                string sSql = "SELECT * FROM Invoices " +
                "WHERE InvoiceNum = " + sInvoiceId;
                return sSql;
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText("C:\\Error.txt", Environment.NewLine +
                                             "HandleError Exception: " + ex.Message);
                // return empty string because something is wrong
                return "";
            }
        } // end of SelectInvoiceByIdData method

        /// <summary>
        /// A method that returns a sql query to grab all details of the invoices
        /// that match with the given invoice date.
        /// </summary>
        /// <param name="sInvoiceId">date of a invoice passed in</param>
        /// <returns>a sql query to grab the invoices with the date</returns>
        public string SelectInvoiceByDateData(string sInvoiceDate)
        {
            try
            {
                string sSql = "SELECT * FROM Invoices " +
                "WHERE InvoiceDate = " + sInvoiceDate;
                return sSql;
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText("C:\\Error.txt", Environment.NewLine +
                                             "HandleError Exception: " + ex.Message);
                // return empty string because something is wrong
                return "";
            }
        } // end of SelectInvoiceByDateData method

        /// <summary>
        /// A method that returns a sql query to grab all details of the invoices
        /// that match with the given total cost.
        /// </summary>
        /// <param name="sInvoiceCost">total cost of a invoice passed in</param>
        /// <returns>a sql query to grab the invoices with the total cost</returns>
        public string SelectInvoiceByTotalData(string sInvoiceCost)
        {
            try
            {
                string sSql = "SELECT * FROM Invoices " +
                "WHERE TotalCost = " + sInvoiceCost;
                return sSql;
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText("C:\\Error.txt", Environment.NewLine +
                                             "HandleError Exception: " + ex.Message);
                // return empty string because something is wrong
                return "";
            }
        } // end of SelectInvoiceByTotalData method

        /// <summary>
        /// A method that returns a sql query to grab all details of the invoices
        /// ordered by total cost ascending and distinct values.
        /// </summary>
        /// <returns>a sql query to grab the invoices ordered by total cost ascending and distinct value</returns>
        public string SelectInvoiceByTotalOrderedData()
        {
            try
            {
                string sSql = "SELECT DISTINCT * FROM Invoices " +
                "ORDER BY TotalCost ASC" ;
                return sSql;
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText("C:\\Error.txt", Environment.NewLine +
                                             "HandleError Exception: " + ex.Message);
                // return empty string because something is wrong
                return "";
            }
        } // end of SelectInvoiceByTotalOrderedData method


        /// <summary>
        /// A method that builds a dynamic sql query with the given params.
        /// </summary>
        /// <param name="sInvoiceNum">Default to a empty string or the passed in invoice number</param>
        /// <param name="sDate">Default to a empty string or the passed in invoice date</param>
        /// <param name="sCost">Default to a empty string or the passed in invoice total cost</param>
        /// <returns>a sql query to grab the invoices with the given params</returns>
        public string SelectInvoiceByParamsData(string sInvoiceNum = "", string sDate = "", string sCost = "")
        {
            try
            {
                // initially return all invoices
                string sSql = "SELECT * FROM Invoices WHERE 1=1 ";

                // if there is a given invoice num
                if (sInvoiceNum != "")
                {
                    // concatenate the searched invoice number
                    sSql += "AND InvoiceNum=" + sInvoiceNum;
                }

                // if there is a given invoice date
                if (sDate != "")
                {
                    // concatenate the searched invoice date
                    sSql += "AND InvoiceDate=#" + sDate + "#";
                }

                // if there is a given invoice total cost
                if (sCost != "")
                {
                    // concatenate the searched invoice total cost
                    sSql += "AND TotalCost="+ sCost;
                }

                return sSql;
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText("C:\\Error.txt", Environment.NewLine +
                                             "HandleError Exception: " + ex.Message);
                // return empty string because something is wrong
                return "";
            }
        } // end of SelectInvoiceByParamsData method

        /// <summary>
        /// A method that returns a sql query to grab all unique invoice nums of the invoices
        /// </summary>
        /// <returns>a sql query to grab all the invoice's invoice num</returns>
        public string SelectInvoiceIdsData()
        {
            try
            {
                string sSql = "SELECT DISTINCT InvoiceNum FROM Invoices";
                return sSql;
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText("C:\\Error.txt", Environment.NewLine +
                                             "HandleError Exception: " + ex.Message);
                // return empty string because something is wrong
                return "";
            }
        } // end of SelectInvoiceIdsData method

        /// <summary>
        /// A method that returns a sql query to grab all unique invoice dates of the invoices
        /// </summary>
        /// <returns>a sql query to grab all the invoice's invoice dates</returns>
        public string SelectInvoiceDatessData()
        {
            try
            {
                string sSql = "SELECT DISTINCT InvoiceDate FROM Invoices";
                return sSql;
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText("C:\\Error.txt", Environment.NewLine +
                                             "HandleError Exception: " + ex.Message);
                // return empty string because something is wrong
                return "";
            }
        } // end of SelectInvoiceDatessData method

        /// <summary>
        /// A method that returns a sql query to grab all unique invoice total costs of the invoices
        /// </summary>
        /// <returns>a sql query to grab all the invoice's invoice total costs</returns>
        public string SelectInvoiceTotalsData()
        {
            try
            {
                string sSql = "SELECT DISTINCT TotalCost FROM Invoices";
                return sSql;
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText("C:\\Error.txt", Environment.NewLine +
                                             "HandleError Exception: " + ex.Message);
                // return empty string because something is wrong
                return "";
            }
        } // end of SelectInvoiceIdsData method

        #endregion
    } // end of class
} // end of namespace
