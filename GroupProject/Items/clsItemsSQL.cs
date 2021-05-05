using GroupProject.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GroupProject.Items
{
    public class clsItemsSQL
    {
        #region Attributes
        /// <summary>
        /// Object used for accessing the invoice database.
        /// </summary>
        clsDataAccess da;
        #endregion


        #region Sql statements
        /// <summary>
        /// Constructor that will initialize the clsDataAccess object.
        /// </summary>
        public clsItemsSQL()
        {
            da = new clsDataAccess();
        }
        /// <summary>
        /// Grab all rows from items desc database table.
        /// </summary>
        /// <returns></returns>
        public List<clsItemDescObj> SelectItemDescData()
        {
            try
            {
                int rowsReturned = 0;
                string sSql = "SELECT * FROM ItemDesc ORDER BY ItemCode";

                DataSet ds = da.ExecuteSQLStatement(sSql, ref rowsReturned);
                List<clsItemDescObj> itemList = new List<clsItemDescObj> { };

                if (rowsReturned == 0)
                {
                    throw new Exception("No itmes were found.");
                }
                int row = 0;
                while (row < rowsReturned)
                {
                    clsItemDescObj item = new clsItemDescObj();
                    item.sItemCode = ds.Tables[0].Rows[row]["ItemCode"].ToString();
                    item.sItemDesc = ds.Tables[0].Rows[row]["ItemDesc"].ToString();
                    item.sCost = ds.Tables[0].Rows[row]["Cost"].ToString();
                    itemList.Add(item);
                    row++;
                }
                return itemList;
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText("C:\\Error.txt", Environment.NewLine +
                                             "HandleError Exception: " + ex.Message);
                // return empty string because something is wrong
                return null;
            }
        }

        /// <summary>
        /// Grab all rows from Line Item Database Table.
        /// </summary>
        /// <returns></returns>
        public List<clsLineItemsObj> SelectLineItemData()
        {
            try
            {
                int rowsReturned = 0;
                string sSql = "SELECT * FROM LineItems";

                DataSet ds = da.ExecuteSQLStatement(sSql, ref rowsReturned);
                List<clsLineItemsObj> itemList = new List<clsLineItemsObj> { };

                if (rowsReturned == 0)
                {
                    throw new Exception("No items were found.");
                }
                int row = 0;
                while (row < rowsReturned)
                {
                    clsLineItemsObj item = new clsLineItemsObj();
                    item.sInvoiceNum = ds.Tables[0].Rows[row]["InvoiceNum"].ToString();
                    item.sLineItemNum = ds.Tables[0].Rows[row]["LineItemNum"].ToString();
                    item.sItemCode = ds.Tables[0].Rows[row]["ItemCode"].ToString();
                    itemList.Add(item);
                    row++;
                }
                return itemList;
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText("C:\\Error.txt", Environment.NewLine +
                                             "HandleError Exception: " + ex.Message);
                // return empty string because something is wrong
                return null;
            }
        }


        /// <summary>
        /// Adding a new item
        /// </summary>
        /// <param name="sItemDesc"></param>
        /// <param name="dCost"></param>
        /// <returns></returns>
        public bool AddSelectedItem(string sItemCode, string sItemDesc, string sCost)
        {
            try
            {
                string sSql = "INSERT INTO ItemDesc(ItemCode, ItemDesc, Cost) VALUES ('" + sItemCode + "', '" + sItemDesc + "', '" + sCost + "')";
                int affectedRows = da.ExecuteNonQuery(sSql);
                //If no rows are affected, something went wrong. Otherwise, it worked
                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText("C:\\Error.txt", Environment.NewLine +
                                             "HandleError Exception: " + ex.Message);
                // return empty string because something is wrong
                return false;
            }
        }

        /// <summary>
        /// Updating the selected item
        /// </summary>
        /// <param name="sItemCode"></param>
        /// <param name="sItemDesc"></param>
        /// <param name="dCost"></param>
        /// <returns></returns>
        public bool UpdateSelectedItem(string sItemCode, string sItemDesc, string sCost)
        {
            try
            {
                string sSql = "UPDATE ItemDesc SET ItemDesc = '" + sItemDesc + "', Cost = '" + sCost + "' WHERE ItemCode='" + sItemCode + "'";
                int affectedRows = da.ExecuteNonQuery(sSql);
                //If no rows are affected, something went wrong. Otherwise, it worked
                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText("C:\\Error.txt", Environment.NewLine +
                                             "HandleError Exception: " + ex.Message);
                // return empty string because something is wrong
                return false;
            }
        }

        /// <summary>
        /// Delete the selected item
        /// </summary>
        /// <param name="iItemCode"></param>
        /// <param name="dItemDescription"></param>
        /// <returns></returns>
        public bool DeleteSelectedItem(string sItemCode, string sItemDesc)
        {
            try
            {
                string sSql = "Delete FROM ItemDesc WHERE ItemCode='" + sItemCode + "' AND ItemDesc='" + sItemDesc + "'";
                int affectedRows = da.ExecuteNonQuery(sSql);

                //If no rows are affected, something went wrong. Otherwise, it worked
                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText("C:\\Error.txt", Environment.NewLine +
                                             "HandleError Exception: " + ex.Message);
                // return empty string because something is wrong
                return false;
            }
        }

        /// <summary>
        /// Handle the error.
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
        }
        #endregion
    }
}
