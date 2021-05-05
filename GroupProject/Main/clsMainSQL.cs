using GroupProject.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupProject.Main
{
    public class clsMainSql
    {
        /// <summary>
        /// Returns the sql for getting a single invoice by ID
        /// </summary>
        /// <param name="invoiceID">ID for invoice to be grabbed</param>
        /// <returns>Sql for the query that can be ran</returns>
        public string GetInvoiceQuery(string invoiceID)
        {
            try
            {
                return $"SELECT InvoiceNum, InvoiceDate, TotalCost FROM Invoices WHERE InvoiceNum = {invoiceID}";
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText("C:\\Error.txt", Environment.NewLine +
                                             "HandleError Exception: " + ex.Message);
                // return empty string because something is wrong
                return "";
            }
        }

        /// <summary>
        /// Returns the sql for grabbing all the line items for an invoice
        /// </summary>
        /// <param name="invoiceID">ID of the invoice line items should be attached to</param>
        /// <returns>Sql for the query that can be ran</returns>
        public string GetInvoiceLineItemsQuery(string invoiceID)
        {
            try
            {
                return $"SELECT LineItems.ItemCode, ItemDesc.ItemDesc, ItemDesc.Cost FROM LineItems, ItemDesc Where LineItems.ItemCode = ItemDesc.ItemCode And LineItems.InvoiceNum = {invoiceID}";
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText("C:\\Error.txt", Environment.NewLine +
                                             "HandleError Exception: " + ex.Message);
                // return empty string because something is wrong
                return "";
            }
        }

        /// <summary>
        /// Get the SQL for all items to be returned
        /// </summary>
        /// <returns>Sql for the query that can be ran</returns>
        public string GetAllitemsQuery()
        {
            try
            {
                return $"SELECT ItemCode, ItemDesc, Cost from ItemDesc ORDER BY ItemDesc";
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText("C:\\Error.txt", Environment.NewLine +
                                             "HandleError Exception: " + ex.Message);
                // return empty string because something is wrong
                return "";
            }
        }

        /// <summary>
        /// Returns the query that can be used for getting the next max ID
        /// </summary>
        /// <returns>Sql for the query that can be ran</returns>
        public string GetMaxInvoiceIDQuery()
        {
            try
            {
                return $"SELECT TOP 1 InvoiceNum from Invoices ORDER BY InvoiceNum DESC";
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText("C:\\Error.txt", Environment.NewLine +
                                             "HandleError Exception: " + ex.Message);
                // return empty string because something is wrong
                return "";
            }
        }

        /// <summary>
        /// Gets the query that can be used to remove all ine items from an invoice being deleted
        /// </summary>
        /// <param name="invoiceID">ID of invoice being deleted</param>
        /// <returns>Sql for the query that can be ran</returns>
        public string DeleteAllLineItemsByIDQuery(string invoiceID)
        {
            try
            {
                return $"DELETE FROM LineItems WHERE InvoiceNum = {invoiceID}";
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText("C:\\Error.txt", Environment.NewLine +
                                             "HandleError Exception: " + ex.Message);
                // return empty string because something is wrong
                return "";
            }
        }

        /// <summary>
        /// Gets the Sql for inserting a single line item
        /// </summary>
        /// <returns>Sql for the query that can be ran</returns>
        public string InsertIndividualLineItemQuery(string invoiceID, int iteration, clsItemDescObj lineItem)
        {
            try
            {
                return $"INSERT INTO LineItems (InvoiceNum, LineItemNum, ItemCode) VALUES ({invoiceID}, {iteration}, '{lineItem.sItemCode}')";
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText("C:\\Error.txt", Environment.NewLine +
                                             "HandleError Exception: " + ex.Message);
                // return empty string because something is wrong
                return "";
            }
        }

        /// <summary>
        /// Gets the sql for inserting a single invoice row
        /// </summary>
        /// <param name="invoice">Represents the invoice to be inserted</param>
        /// <returns>Sql for the query that can be ran</returns>
        public string InsertInvoiceQuery(clsInvoiceObj invoice)
        {
            try
            {
                return $"Insert INTO Invoices(InvoiceDate, TotalCost) VALUES(#{invoice.sInvoiceDate}#, {invoice.sTotalCost})";
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText("C:\\Error.txt", Environment.NewLine +
                                             "HandleError Exception: " + ex.Message);
                // return empty string because something is wrong
                return "";
            }
        }

        /// <summary>
        /// Gets the sql for updating an invoice
        /// </summary>
        /// <param name="invoice">Represents the invoice to be updated</param>
        /// <returns>Sql for the query that can be ran</returns>
        public string UpdateInvoiceQuery(clsInvoiceObj invoice)
        {
            try
            {
                return $"UPDATE Invoices SET InvoiceDate = #{invoice.sInvoiceDate}#, TotalCost = {invoice.sTotalCost} WHERE InvoiceNum = {invoice.sInvoiceNum}";
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText("C:\\Error.txt", Environment.NewLine +
                                             "HandleError Exception: " + ex.Message);
                // return empty string because something is wrong
                return "";
            }
        }

        /// <summary>
        /// Gets the sql for deleting an invoice
        /// </summary>
        /// <param name="invoice">Invoice to be deleted</param>
        /// <returns>Sql for the query that can be ran</returns>
        public string DeleteInvoiceQuery(clsInvoiceObj invoice)
        {
            try
            {
                return $"DELETE FROM Invoices WHERE InvoiceNum = {invoice.sInvoiceNum}";
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText("C:\\Error.txt", Environment.NewLine +
                                             "HandleError Exception: " + ex.Message);
                // return empty string because something is wrong
                return "";
            }
        }
    }
}
