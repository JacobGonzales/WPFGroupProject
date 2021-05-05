using GroupProject.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GroupProject.Main
{
    /// <summary>
    /// A class that can be used to load the invoice information for the main window
    /// </summary>
    public class clsMainLogic
    {
        /// <summary>
        /// Object used for accessing the invoice database.
        /// </summary>
        clsDataAccess da;

        clsMainSql sqlProvider;

        /// <summary>
        /// Constructor that will initialize the clsDataAccess object.
        /// </summary>
        public clsMainLogic()
        {
            da = new clsDataAccess();
            sqlProvider = new clsMainSql();
        }

        /// <summary>
        /// Gets the invoice based on an invoice ID
        /// </summary>
        /// <param name="invoiceID">Invoice ID to load</param>
        /// <returns>The invoice as a clsInvoiceObj</returns>
        public clsInvoiceObj GetInvoice(string invoiceID)
        {
            try
            {
                int rowsReturned = 0;
                string sql = sqlProvider.GetInvoiceQuery(invoiceID);
                clsInvoiceObj invoice;
                DataSet ds = da.ExecuteSQLStatement(sql, ref rowsReturned);
                if (rowsReturned > 1)
                {
                    throw new Exception("More than one invoice was found for the invoice ID, expected one.");
                }
                else if (rowsReturned == 0)
                {
                    throw new Exception("No invoice was found for the ID, expected one.");
                }
                else
                {
                    invoice = new clsInvoiceObj();
                    invoice.sInvoiceNum = ds.Tables[0].Rows[0]["InvoiceNum"].ToString();
                    invoice.sInvoiceDate = ds.Tables[0].Rows[0]["InvoiceDate"].ToString();
                    invoice.sTotalCost = ds.Tables[0].Rows[0]["TotalCost"].ToString();
                }
                return invoice;
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText("C:\\Error.txt", Environment.NewLine +
                             "HandleError Exception: " + ex.Message);
                return null;
            }
        }


        /// <summary>
        /// Get the line items for an invoice as a list of clsItemDescObj
        /// </summary>
        /// <param name="invoiceID">Invoice ID that the items will be from</param>
        /// <returns>Items in the invoice as a clsItemDescObj list</returns>
        public List<clsItemDescObj> GetInvoiceLineItems(string invoiceID)
        {
            try
            {
                int rowsReturned = 0;
                string sql = sqlProvider.GetInvoiceLineItemsQuery(invoiceID);
                DataSet ds = da.ExecuteSQLStatement(sql, ref rowsReturned);
                List<clsItemDescObj> lineItems = new List<clsItemDescObj> { };
                if (rowsReturned == 0)
                    throw new Exception("No line items were found for the invoice ID, expected at least one");
                int row = 0;
                while (row < rowsReturned)
                {
                    clsItemDescObj lineItem = new clsItemDescObj();
                    lineItem.sItemCode = ds.Tables[0].Rows[row]["ItemCode"].ToString();
                    lineItem.sItemDesc = ds.Tables[0].Rows[row]["ItemDesc"].ToString();
                    lineItem.sCost = ds.Tables[0].Rows[row]["Cost"].ToString();
                    lineItems.Add(lineItem);
                    row++;
                }
                return lineItems;
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText("C:\\Error.txt", Environment.NewLine +
                             "HandleError Exception: " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Get all items from the database.
        /// </summary>
        /// <returns>A list of items from the database as clsItemDescObj</returns>
        public List<clsItemDescObj> GetAllItems()
        {
            try
            {
                int rowsReturned = 0;
                string sql = sqlProvider.GetAllitemsQuery();
                List<clsItemDescObj> items = new List<clsItemDescObj> { };
                DataSet ds = da.ExecuteSQLStatement(sql, ref rowsReturned);
                if (rowsReturned == 0)
                {
                    throw new Exception("No items were returned from the database, expected at least 1");
                }
                int row = 0;
                while (row < rowsReturned)
                {
                    clsItemDescObj currentItem = new clsItemDescObj();
                    currentItem.sItemCode = ds.Tables[0].Rows[row]["ItemCode"].ToString();
                    currentItem.sItemDesc = ds.Tables[0].Rows[row]["ItemDesc"].ToString();
                    currentItem.sCost = ds.Tables[0].Rows[row]["Cost"].ToString();
                    items.Add(currentItem);
                    row++;
                }
                return items;
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText("C:\\Error.txt", Environment.NewLine +
                             "HandleError Exception: " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Calls the database to get the next ID that would be added
        /// </summary>
        /// <returns>The ID to use for the next invoice to be added</returns>
        public string GetMaxInvoiceID()
        {
            try
            {
                string sql = sqlProvider.GetMaxInvoiceIDQuery();
                string returnedValue = da.ExecuteScalarSQL(sql);
                if (string.IsNullOrWhiteSpace(returnedValue))
                {
                    //Since the database is empty, assume this is the first row to be added
                    return "1";
                }
                return (Convert.ToInt32(returnedValue)).ToString();
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText("C:\\Error.txt", Environment.NewLine +
                             "HandleError Exception: " + ex.Message);
                return "";
            }
        }

        /// <summary>
        /// Saves all the line items for an invoice. Expected to have all line items removed previously if invoice exists
        /// </summary>
        /// <param name="invoiceID">Invoice number to be saved for the line items</param>
        /// <param name="lineItems">Items to be saved to the invoice</param>
        /// <returns>Boolean of if all items were saved or not</returns>
        public bool SaveAllLineItems(string invoiceID, List<clsItemDescObj> lineItems)
        {
            try
            {
                int affectedRows = 0;
                for (int i = 1; i < lineItems.Count + 1; i++)
                {
                    string sql = sqlProvider.InsertIndividualLineItemQuery(invoiceID, i, lineItems[i - 1]);
                    affectedRows += da.ExecuteNonQuery(sql);
                }
                //If no rows are affected, something went wrong. Otherwise, it worked
                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText("C:\\Error.txt", Environment.NewLine +
                             "HandleError Exception: " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Insert a new invoice into the database
        /// </summary>
        /// <param name="invoice">Invoice to be saved. Requires a date and total</param>
        /// <returns>Returns if the invoice was saved successfully</returns>
        public bool InsertInvoice(clsInvoiceObj invoice)
        {
            try
            {
                string sql = sqlProvider.InsertInvoiceQuery(invoice);
                int affectedRows = da.ExecuteNonQuery(sql);
                //If no rows are affected, something went wrong. Otherwise, it worked
                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText("C:\\Error.txt", Environment.NewLine +
                             "HandleError Exception: " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Updates an invoice to have the new total cost and invoice date
        /// </summary>
        /// <param name="invoice">Invoice to update with new values set</param>
        /// <returns>Returns if the invoice was saved successfully</returns>
        public bool UpdateInvoice(clsInvoiceObj invoice)
        {
            try
            {
                string sql = sqlProvider.UpdateInvoiceQuery(invoice);
                int affectedRows = da.ExecuteNonQuery(sql);
                //If no rows are affected, something went wrong. Otherwise, it worked
                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText("C:\\Error.txt", Environment.NewLine +
                             "HandleError Exception: " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Deletes all the line items for an invoice
        /// </summary>
        /// <param name="invoice">Invoice that line items should be removed for</param>
        /// <returns>Status for if the invoice lines have been deleted successfully</returns>
        public bool DeleteInvoiceLineItems(clsInvoiceObj invoice)
        {
            try
            {
                string sql = sqlProvider.DeleteAllLineItemsByIDQuery(invoice.sInvoiceNum);
                int affectedRows = da.ExecuteNonQuery(sql);
                //If no rows are affected, something went wrong. Otherwise, it worked
                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText("C:\\Error.txt", Environment.NewLine +
                             "HandleError Exception: " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Deletes an invoice from the database
        /// </summary>
        /// <param name="invoice">Invoice to be deleted</param>
        /// <returns>Status for if the invoice has been deleted successfully</returns>
        public bool DeleteInvoice(clsInvoiceObj invoice)
        {
            try
            {
                string sql = sqlProvider.DeleteInvoiceQuery(invoice);
                int affectedRows = da.ExecuteNonQuery(sql);
                //If no rows are affected, something went wrong. Otherwise, it worked
                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText("C:\\Error.txt", Environment.NewLine +
                             "HandleError Exception: " + ex.Message);
                return false;
            }
        }
    }
}
