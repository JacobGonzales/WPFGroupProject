using GroupProject.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GroupProject.Items
{
    public class clsItemsLogic
    {
        #region Class Attributes
        /// <summary>
        /// Define items sql object.
        /// </summary>
        private clsItemsSQL itemsSQL;

        /// <summary>
        /// List of line item objects.
        /// </summary>
        private List<clsLineItemsObj> lineItemsList;

        /// <summary>
        /// List of item description.
        /// </summary>
        private List<clsItemDescObj> itemDescList;

        /// <summary>
        /// List specifically for keeping track of items in invoices.
        /// </summary>
        private List<clsLineItemsObj> invoicesConnectedToItem;
        #endregion

        #region Get Set Private Variables
        /// <summary>
        /// Get line items list
        /// </summary>
        public List<clsLineItemsObj> lineItemsListGet
        {

            get
            {
                try
                {
                    invoicesConnectedToItem = itemsSQL.SelectLineItemData();
                    return invoicesConnectedToItem;
                }
                catch (Exception ex)
                {
                    //This is the top level method so we want to handle the exception
                    HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                                MethodInfo.GetCurrentMethod().Name, ex.Message);
                    return null;
                }
            }
        }

        /// <summary>
        /// Get item desc list
        /// </summary>
        public List<clsItemDescObj> itemDescListGet
        {
            get
            {
                try
                {
                    itemDescList = itemsSQL.SelectItemDescData();
                    return itemDescList;
                }
                catch (Exception ex)
                {
                    //This is the top level method so we want to handle the exception
                    HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                                MethodInfo.GetCurrentMethod().Name, ex.Message);
                    return null;
                }
            }
        }
        #endregion

        #region Functions
        /// <summary>
        /// Contructor to initialize variables
        /// </summary>
        public clsItemsLogic()
        {
            try
            {
                //SQL logic for implementation.
                itemsSQL = new clsItemsSQL();   //Object to access sql statements

                //Lists need in implementation.
                lineItemsList = new List<clsLineItemsObj>();            //New list of line items objects.
                itemDescList = new List<clsItemDescObj>();              //New list of item description objects.
                invoicesConnectedToItem = new List<clsLineItemsObj>();  //Keep track of item connected to invoice

                //Populate Line items list with database rows.
                lineItemsList = itemsSQL.SelectLineItemData();

                //Populate item Desc list with database rows.
                itemDescList = itemsSQL.SelectItemDescData();
            }
            catch (Exception ex)
            {
                //This is the top level method so we want to handle the exception
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Check if item is in a invoice.
        /// </summary>
        /// <param name="sItemCode"></param>
        /// <returns>false if not found</returns>
        public bool CheckInvoiceData(string sItemCode)
        {
            try
            {
                //Populate Line items list with database rows.
                lineItemsList = itemsSQL.SelectLineItemData();

                //Clear list of connected items to invoices
                invoicesConnectedToItem.Clear();

                //Boolean to keep track if invoice is in invoice
                bool bIsItInInvoice = false;

                //Check list of Line items
                for (int i = 0; i < lineItemsList.Count; i++)
                {
                    if (lineItemsList[i].sItemCode == sItemCode)
                    {
                        invoicesConnectedToItem.Add(lineItemsList[i]);
                        bIsItInInvoice = true;
                    }
                }

                return bIsItInInvoice;
            }
            catch (Exception ex)
            {
                //This is the top level method so we want to handle the exception
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Check to see if new item is duplicate item code.
        /// </summary>
        /// <param name="sItemCode"></param>
        /// <returns></returns>
        public bool CheckItemData(string sItemCode)
        {
            try
            {
                //Populate Line items list with database rows.
                itemDescList = itemsSQL.SelectItemDescData();

                //Check list of Line items
                for (int i = 0; i < itemDescList.Count; i++)
                {
                    if (itemDescList[i].sItemCode == sItemCode)
                    {
                        //Item code matches a item in list
                        return true;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                //This is the top level method so we want to handle the exception
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Delete item desc in database.
        /// </summary>
        /// <param name="sItemCode"></param>
        /// <param name="sItemDesc"></param>
        public bool DeleteItemDesc(string sItemCode, string sItemDesc)
        {
            try
            {
                //Pass item code into sql
                itemsSQL.DeleteSelectedItem(sItemCode, sItemDesc);

                //Update itemDescList with updated list.
                itemDescList = itemsSQL.SelectItemDescData();

                //Return true if successfully ran.
                return true;
            }
            catch (Exception ex)
            {
                //This is the top level method so we want to handle the exception
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Update item in database.
        /// </summary>
        /// <param name="sItemCode"></param>
        /// <param name="sItemDesc"></param>
        /// <param name="sCost"></param>
        /// <returns>true for success</returns>
        public bool UpdateItemDesc(string sItemCode, string sItemDesc, string sCost)
        {
            try
            {
                //Pass item code into sql
                itemsSQL.UpdateSelectedItem(sItemCode, sItemDesc, sCost);

                //Update itemDescList with updated list.
                itemDescList = itemsSQL.SelectItemDescData();

                return true;
            }
            catch (Exception ex)
            {
                //This is the top level method so we want to handle the exception
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Add item to database, ItemDesc table 
        /// </summary>
        /// <param name="sItemDesc"></param>
        /// <param name="sCost"></param>
        public void AddItemDesc(string sItemCode, string sItemDesc, string sCost)
        {
            try
            {
                //Pass item code into sql
                itemsSQL.AddSelectedItem(sItemCode, sItemDesc, sCost);

                //Update itemDescList with updated list.
                itemDescList = itemsSQL.SelectItemDescData();
            }
            catch (Exception ex)
            {
                //This is the top level method so we want to handle the exception
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

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

    }
}
