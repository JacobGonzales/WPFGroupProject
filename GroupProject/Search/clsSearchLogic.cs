using GroupProject.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
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
namespace GroupProject.Search
{
    /// <summary>
    /// Has all the logic to make the ui logic work and is the middle class to the database.
    /// </summary>
    public class clsSearchLogic
    {
        #region class attributes and variables
        /// <summary>
        /// Data access class.
        /// </summary>
        clsDataAccess db;

        /// <summary>
        /// sql query access class
        /// </summary>
        private clsSearchSql searchSql;

        /// <summary>
        /// Number of invoices
        /// </summary>
        private int iINum;

        /// <summary>
        /// Data set that holds the return values
        /// </summary>
        private DataSet ds;

        /// <summary>
        /// list of the invoices
        /// </summary>
        private List<clsInvoiceObj> invoiceList;

        /// <summary>
        /// String of the selected Invoice number
        /// </summary>
        public string sSelectedInvoiceNum;

        /// <summary>
        /// String of the selected Invoice date
        /// </summary>
        public string sSelectedInvoiceDate;

        /// <summary>
        /// String of the selected Invoice total cost
        /// </summary>
        public string sSelectedTotal;

        #endregion

        #region Logic methods for the ui

        /// <summary>
        /// Constructor of a new instance of the clsSearchLogic object
        /// </summary>
        public clsSearchLogic()
        {
            try
            {
                // initialize data access class
                db = new clsDataAccess();
                searchSql = new clsSearchSql(); // initialize a new search sql class
                iINum = 0; // no invoices yet
                ds = new DataSet(); // new dataset
                invoiceList = new List<clsInvoiceObj>(); // new list of invoices objects
                sSelectedInvoiceNum = ""; // not selected
                sSelectedInvoiceDate = "";  // not selected
                sSelectedTotal = "";  // not selected
            }
            catch (Exception ex)
            {
                //This is the top level method so we want to handle the exception
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        } // end of constructor

        public List<clsInvoiceObj> LoadInvoices(string sInvoiceNumInput = "", string sInvoiceDateInput = "", string sTotalInput = "")
        {
            try
            {
                // initial invoice object
                clsInvoiceObj invoice;

                // pass in the feilds needed to get the results correct
                ds = db.ExecuteSQLStatement(searchSql.SelectInvoiceByParamsData(sInvoiceNumInput, sInvoiceDateInput, sTotalInput), ref iINum);

                //list of invoice objects
                invoiceList = new List<clsInvoiceObj>();

                // loop through the data and create invoice classes
                for (int i = 0; i < iINum; i++)
                {
                    // new invoice object
                    invoice = new clsInvoiceObj();

                    // fill invoice attributes
                    invoice.sInvoiceNum = ds.Tables[0].Rows[i][0].ToString();
                    invoice.sInvoiceDate = ds.Tables[0].Rows[i]["InvoiceDate"].ToString();
                    invoice.sTotalCost = ds.Tables[0].Rows[i]["TotalCost"].ToString();

                    // add the flight to the list
                    invoiceList.Add(invoice);
                }
                // return the list of invoices
                return invoiceList;
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText("C:\\Error.txt", Environment.NewLine +
                                             "HandleError Exception: " + ex.Message);
                // return list of invoices that is empty
                return invoiceList;
            }
        } // end of LoadInvoices method

        /// <summary>
        /// Helper method to return a list of all the total cost
        /// </summary>
        /// <returns>A string list of the total cost</returns>
        public List<string> loadOrderedTotal()
        {
            try
            {
                // initial total object
                string total;

                int iTotalNum = 0;
                // pass in the feilds needed to get the results correct
                ds = db.ExecuteSQLStatement(searchSql.SelectInvoiceByTotalOrderedData(), ref iTotalNum);

                //list of invoice objects
                List<string> invoiceTotalList = new List<string>();

                // loop through the data and create invoice classes
                for (int i = 0; i < iTotalNum; i++)
                {
                    // new invoice object
                    total = "";

                    // fill invoice attributes
                    total = ds.Tables[0].Rows[i]["TotalCost"].ToString();

                    // add the flight to the list
                    invoiceTotalList.Add(total);
                }
                // return the list of invoices
                return invoiceTotalList;
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText("C:\\Error.txt", Environment.NewLine +
                                             "HandleError Exception: " + ex.Message);
                List<string> invoiceTotalList = new List<string>();
                // return list of total that is empty
                return invoiceTotalList;
            }
        } // end of loadOrderedTotal method

        /// <summary>
        /// Helper method to convert the invoice number to a integer
        /// </summary>
        /// <param name="sInvoiceNum">Invoice number as a string</param>
        /// <returns>Invoice number as a integer or 0 if there are errors</returns>
        public int grabInvoiceNum(string sInvoiceNum)
        {
            try
            {
                int iInvoiceNum; // invoice number as integer

                // parse the invoice number of the user selection to a integer
                bool isNum = Int32.TryParse(sInvoiceNum, out iInvoiceNum);

                // able to parse
                if (isNum)
                {
                    // return invoice num as integer
                    return iInvoiceNum;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText("C:\\Error.txt", Environment.NewLine +
                                             "HandleError Exception: " + ex.Message);
                // return list of total that is empty
                return 0;
            }
        } // end of grabInvoiceNum method

        /// <summary>
        /// Helper method of grabing invoice numbers using LINQ to grab values to load up the combo boxes
        /// </summary>
        /// <param name="invoiceList">List of invoices</param>
        /// <returns>list of the invoice numbers</returns>
        public List<string> grabInvoiceNumList(List<clsInvoiceObj> invoiceList)
        {
            try
            {
                return invoiceList.Select(i => i.sInvoiceNum).ToList();
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText("C:\\Error.txt", Environment.NewLine +
                                             "HandleError Exception: " + ex.Message);
                // return list of invoice nums
                return invoiceList.Select(i => i.sInvoiceNum).ToList();
            }
        } // end of grabInvoiceNumList method

        /// <summary>
        /// Helper method of grabing invoice dates using LINQ to grab values to load up the combo boxes
        /// </summary>
        /// <param name="invoiceList">List of invoices</param>
        /// <returns>list of the invoice dates</returns>
        public List<string> grabInvoiceDateList(List<clsInvoiceObj> invoiceList)
        {
            try
            {
                return invoiceList.Select(i => i.sInvoiceDate).Distinct().ToList();
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText("C:\\Error.txt", Environment.NewLine +
                                             "HandleError Exception: " + ex.Message);
                // return list of invoice nums
                return invoiceList.Select(i => i.sInvoiceDate).ToList();
            }
        } // end of grabInvoiceDateList method

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
                // Would write to a file or database here.
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
} // end of name space
