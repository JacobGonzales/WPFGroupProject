using GroupProject.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GroupProject.Items
{
    /// <summary>
    /// Interaction logic for wndItems.xaml
    /// </summary>
    public partial class wndItems : Window
    {
        #region Attributes
        /// <summary>
        /// logic class variable
        /// </summary>
        private clsItemsLogic itemsLogic;

        /// <summary>
        /// To keep track if an update succesfully ran.
        /// </summary>
        private bool bWasUpdated;

        /// <summary>
        /// To keep track if entry was deleted.
        /// </summary>
        private bool bWasDeleted;
        #endregion

        #region Functions
        /// <summary>
        /// Constructor for Window Items.
        /// </summary>
        public wndItems()
        {
            try
            {
                InitializeComponent();

                // initialize the clsSearchLogic class
                itemsLogic = new clsItemsLogic(); 

                //Load all the items into data grid
                dgLineItems.ItemsSource = itemsLogic.itemDescListGet;
            }
            catch (Exception ex)
            {
                //This is the top level method so we want to handle the exception
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// When a datagrid selection is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgLineItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Check if an update occured
            if (bWasUpdated)
            {
                bWasUpdated = false;
                return;
            }
            if (bWasDeleted)
            {
                bWasDeleted = false;
                return;
            }

            //Cast dgLineItems to object
            clsItemDescObj itemDesc = (clsItemDescObj)dgLineItems.SelectedItem;

            //Put the itemDesc object into deletion labels
            lblItemCode.Content = itemDesc.sItemCode;
            lblItemDesc.Content = itemDesc.sItemDesc;
            lblCost.Content = itemDesc.sCost;

            //Put the itemDesc object into edit textboxes
            txtEditItemDescription.Text = itemDesc.sItemDesc;
            txtEditCost.Text = itemDesc.sCost;

        }

        /// <summary>
        /// Button to edit selected item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Cast dgLineItems to object
                clsItemDescObj itemDesc = (clsItemDescObj)dgLineItems.SelectedItem;

                //If nothing is selected and delete is clicked.
                if (itemDesc == null)
                {
                    return;
                }

                //Keep track of updated description and cost for messagebox purposes
                string sNewItemDescription = txtEditItemDescription.Text;
                string sNewItemCost = txtEditCost.Text;

                //Pass the itemcode, itemdesc and itemcost for update
                bWasUpdated = itemsLogic.UpdateItemDesc(itemDesc.sItemCode, txtEditItemDescription.Text, txtEditCost.Text);

                //Update data grid information
                dgLineItems.ItemsSource = itemsLogic.itemDescListGet;

                //Display success
                MessageBox.Show("Item Desciprition '" + itemDesc.sItemDesc + "' has been updated to '" + sNewItemDescription + "'\n" + 
                                    "Item Cost '" + itemDesc.sCost + "' has been updated to '" + sNewItemCost + "'"
                                    , "Update Confirmation");

                //Restore labels and edit textbox
                lblItemCode.Content = "N/A";
                lblItemDesc.Content = "N/A";
                lblCost.Content = "N/A";

                //Put the itemDesc object into edit textboxes
                txtEditItemDescription.Text = "";
                txtEditCost.Text = "";
            }
            catch (Exception ex)
            {
                //This is the top level method so we want to handle the exception
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Button to delete selected item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Cast dgLineItems to object
                clsItemDescObj itemDesc = (clsItemDescObj)dgLineItems.SelectedItem;

                //If nothing is selected and delete is clicked.
                if (itemDesc == null)
                {
                    return;
                }

                //Check to make sure the item isn't on a invoice, if true it's on an invoice.
                if (itemsLogic.CheckInvoiceData(itemDesc.sItemCode))
                {
                    //Display message so the user knows what item is in what invoice
                    List<clsLineItemsObj> invoices = itemsLogic.lineItemsListGet;

                    //String for invoices
                    string invoiceString = "";

                    for(int i = 0; i < invoices.Count(); i++)
                    {
                        invoiceString += invoices[i].sInvoiceNum + "\n";
                    }

                    //If it is then don't delete, and give message, return which item the invoice is currently on
                    MessageBox.Show("Cannot delete item. This item is located in invoices:\n" + invoiceString, "Deletion Error");

                    return;
                }

                //Pass itemcode to sql logic for deletion
                bWasDeleted = itemsLogic.DeleteItemDesc(itemDesc.sItemCode, itemDesc.sItemDesc);

                //Update data grid information
                dgLineItems.ItemsSource = itemsLogic.itemDescListGet;

                //Display success
                MessageBox.Show(itemDesc.sItemDesc + " has been deleted.", "Delete Confirmation");

                //Restore labels and edit textbox
                lblItemCode.Content = "N/A";
                lblItemDesc.Content = "N/A";
                lblCost.Content = "N/A";

                //Put the itemDesc object into edit textboxes
                txtEditItemDescription.Text = "";
                txtEditCost.Text = "";

            }
            catch (Exception ex)
            {
                //This is the top level method so we want to handle the exception
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Button to add item to list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Check to make sure there is input in itemcode, description and cost
                if (txtNewItemCode.Text == "")
                {
                    MessageBox.Show("Error: Please add New Item Code.", "Item Code Error");
                    return;
                }
                else if (txtNewItemDescription.Text == "")
                {
                    MessageBox.Show("Error: Please add New Item Description.", "Item Description Error");
                    return;
                }
                else if (txtNewCost.Text == "")
                {
                    MessageBox.Show("Error: Please add New Cost Amount.", "Item Cost Error");
                    return;
                }

                //Check to see if item code is already in list.
                if (itemsLogic.CheckItemData(txtNewItemCode.Text))
                {
                    MessageBox.Show("Error: There can be no duplicate item codes. Please set item code to a unique value.", "Item Code Error");
                    return;
                }

                //Keep track of updated description and cost for messagebox purposes
                string sNewItemCode = txtNewItemCode.Text;
                string sNewItemDescription = txtNewItemDescription.Text;
                string sNewItemCost = txtNewCost.Text;

                //Pass the itemdesc and itemcost for add
                itemsLogic.AddItemDesc(txtNewItemCode.Text, txtNewItemDescription.Text, txtNewCost.Text);

                //Update data grid information
                dgLineItems.ItemsSource = itemsLogic.itemDescListGet;

                //Display success
                MessageBox.Show("Item Code '" + sNewItemCode + "' has been successfully added to the database. \n" + 
                    "Item Description '" + sNewItemDescription + "' has been successfully added to the database. \n" +
                    "Item Cost '" + sNewItemCost + "' has been successfully added to the database.'"
                    , "New Item Added Confirmation");

                //Clear text boxes
                txtNewItemCode.Text = "";
                txtNewItemDescription.Text = "";
                txtNewCost.Text = "";
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
        }
        #endregion
    }
}