using GroupProject.Database;
using GroupProject.Items;
using GroupProject.Main;
using GroupProject.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;


namespace GroupProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        clsMainLogic sqlProvider;
        clsInvoiceObj invoice = null;
        bool isNewInvoice = false;
        List<clsItemDescObj> lineItems;
        /// <summary>
        /// Constructor for the main window
        /// </summary>
        public MainWindow()
        {
            sqlProvider = new clsMainLogic();
            Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
            InitializeComponent();
            LoadItems();
        }

        /// <summary>
        /// Load the search window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchHeader_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var search = new wndSearch();
                search.ShowDialog();
                //if the invoice value is not 0, that means the search window has an invoice selected
                if (search.selectedInvoiceId != 0)
                {
                    //items may have changed, so refresh the combo box
                    LoadItems();
                    LoadInvoice(search.selectedInvoiceId);
                }
            }
            catch (Exception ex)
            {
                //This is the top level method so we want to handle the exception
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Load the items window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ItemsHeader_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Open items window
                var items = new wndItems();
                items.ShowDialog();
                //only reload if there is an invoice loaded and that invoice has a good value
                if (invoice != null && !string.IsNullOrWhiteSpace(invoice.sInvoiceNum))
                {
                    LoadInvoice(Convert.ToInt32(invoice.sInvoiceNum)); //if items were alraedy displayed for an invoice, reload so it's accurate
                }
                LoadItems();
            }
            catch (Exception ex)
            {
                //This is the top level method so we want to handle the exception
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Handle the add invoice button being clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddInvoice_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UpdateIsEnabledField(true);
                ItemsHeader.IsEnabled = false;
                isNewInvoice = true;
                lblInvoiceNum.Content = "Invoice #TBD";
                lineItems = new List<clsItemDescObj> { };
                dgInvoiceItems.ItemsSource = lineItems;
                invoice = new clsInvoiceObj();
                btnEditInvoice.IsEnabled = false;
                btnDeleteInvoice.IsEnabled = false;
                //reset in case an invoice was already being editted
                dpInvoiceDate.SelectedDate = null;
                lblTotalCost.Content = "";
            }
            catch (Exception ex)
            {
                //This is the top level method so we want to handle the exception
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Handle the edit invoice button being clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEditInvoice_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UpdateIsEnabledField(true);
                isNewInvoice = false;
                ItemsHeader.IsEnabled = false;
            }
            catch (Exception ex)
            {
                //This is the top level method so we want to handle the exception
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Handle the delete invoice button being clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteInvoice_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!sqlProvider.DeleteInvoiceLineItems(invoice))
                {
                    MessageBox.Show("Failed to remove line items from the database.");
                    return;
                }
                if (!sqlProvider.DeleteInvoice(invoice))
                {
                    MessageBox.Show("Failed to remove the invoice from the database. Line items removed successfully");
                    return;
                }
                isNewInvoice = false;
                lblInvoiceNum.Content = "Invoice";
                dgInvoiceItems.ItemsSource = null;
                dgInvoiceItems.Items.Refresh();
                invoice = null;
                btnEditInvoice.IsEnabled = false;
                btnDeleteInvoice.IsEnabled = false;
                dpInvoiceDate.SelectedDate = null;
                lblTotalCost.Content = "";
                btnEditInvoice.IsEnabled = false;
                btnDeleteInvoice.IsEnabled = false;
                UpdateIsEnabledField(false);
                MessageBox.Show("Invoice deleted successfully.");
            }
            catch (Exception ex)
            {
                //This is the top level method so we want to handle the exception
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Loads all items in the items combo box
        /// </summary>
        private void LoadItems()
        {
            try
            {
                var items = sqlProvider.GetAllItems();
                if (items != null)
                {
                    cbAddItem.ItemsSource = sqlProvider.GetAllItems();
                    cbAddItem.DisplayMemberPath = "sItemDesc";
                    cbAddItem.SelectedValuePath = "sItemCode";
                }
                else
                {
                    throw new Exception("Failed to retrieve items from the database");
                }
            }
            catch (Exception ex)
            {
                //This is the top level method so we want to handle the exception
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Update the invoice fields to be enabled or disabled
        /// </summary>
        /// <param name="updateTo">What to update the IsEnabled to. True is enabled, false is disabled</param>
        private void UpdateIsEnabledField(bool updateTo)
        {
            try
            {
                lblDatePlaceHolder.IsEnabled = updateTo;
                dpInvoiceDate.IsEnabled = updateTo;
                lblTotalPlaceHolder.IsEnabled = updateTo;
                lblTotalCost.IsEnabled = updateTo;
                lblAddItemPlaceHolder.IsEnabled = updateTo;
                cbAddItem.IsEnabled = updateTo;
                lblItemCostPlaceHolder.IsEnabled = updateTo;
                lblItemCost.IsEnabled = updateTo;
                btnSaveInvoice.IsEnabled = updateTo;
                btnCancel.IsEnabled = updateTo;
                btnAddItem.IsEnabled = updateTo;
                btnRemoveItem.IsEnabled = updateTo;
                dgInvoiceItems.IsEnabled = updateTo;
            }
            catch (Exception ex)
            {
                //This is the top level method so we want to handle the exception
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Handle when the save button is clicked. Saves the invoice if valid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveInvoice_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(invoice.sInvoiceDate))
                {
                    MessageBox.Show("An invoice must have a date before it can be saved.");
                    return;
                }
                else if (string.IsNullOrWhiteSpace(invoice.sTotalCost) || Convert.ToInt32(invoice.sTotalCost) == 0)
                {
                    MessageBox.Show("An invoice must have items before it can be saved.");
                    return;
                }
                bool invoiceSaved;
                if (isNewInvoice)
                {
                    invoiceSaved = sqlProvider.InsertInvoice(invoice);
                    //get the max invoice that was recently saved
                    invoice.sInvoiceNum = sqlProvider.GetMaxInvoiceID();
                    if (string.IsNullOrWhiteSpace(invoice.sInvoiceNum))
                    {
                        throw new Exception("Failed to retrieve the invoice number after saving");
                    }
                    lblInvoiceNum.Content = $"Invoice #{invoice.sInvoiceNum}";
                }
                else
                    invoiceSaved = sqlProvider.UpdateInvoice(invoice);
                if (!invoiceSaved)
                {
                    MessageBox.Show("An error occurred while saving the invoice, changes have not been saved.");
                    return;
                }

                //assume this is the case for instances like new invoices
                bool isOkToSaveItems = true;
                if (!isNewInvoice)
                    isOkToSaveItems = sqlProvider.DeleteInvoiceLineItems(invoice);
                if (!isOkToSaveItems)
                {
                    MessageBox.Show("An error occurred while attempting to reset the line items in the database. Invoice saved, items not.");
                    return;
                }
                bool itemsSaved = sqlProvider.SaveAllLineItems(invoice.sInvoiceNum, lineItems);
                if (itemsSaved)
                {
                    MessageBox.Show("Invoice saved successfully!");
                    isNewInvoice = false;
                    btnEditInvoice.IsEnabled = true;
                    btnDeleteInvoice.IsEnabled = true;
                    LockInvoice();
                }
                else
                {
                    MessageBox.Show("An error occurred while attempting to save the line items in the database. Invoice saved, items not. Previous items still saved.");
                }
            }
            catch (Exception ex)
            {
                //This is the top level method so we want to handle the exception
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Handle when the selected item is changed. Displays the current item's cost
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbAddItem_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                clsItemDescObj selectedItem = (clsItemDescObj)cbAddItem.SelectedItem;
                //account for selecting the empty value
                if (selectedItem != null)
                {
                    lblItemCost.Content = $"{Convert.ToInt32(selectedItem.sCost):C}";
                }
            }
            catch (Exception ex)
            {
                //This is the top level method so we want to handle the exception
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Handles when the cancel button is clicked. Clears out the form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LockInvoice();
            }
            catch (Exception ex)
            {
                //This is the top level method so we want to handle the exception
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void LockInvoice()
        {
            try
            {
                //if this is a new invoice, clear all the fields. Otherwise, just lock the data
                if (isNewInvoice)
                {
                    lineItems.Clear();
                    dgInvoiceItems.Items.Refresh();
                    dgInvoiceItems.SelectedIndex = -1;
                    lblTotalCost.Content = "";
                    lblInvoiceNum.Content = "Invoice";
                    dpInvoiceDate.SelectedDate = null;
                    invoice = null;
                }
                //set the add item combobox to display nothing
                cbAddItem.SelectedIndex = -1;
                //always remove the item cost since no item should be selected
                lblItemCost.Content = "";
                //make it so nothing can be editted
                UpdateIsEnabledField(false);
                //allow editing items again
                ItemsHeader.IsEnabled = true;
            }
            catch (Exception ex)
            {
                //This is the top level method so we want to handle the exception
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Handles when the add item button is clicked, add an item to the invoice
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                clsItemDescObj selectedItem = (clsItemDescObj)cbAddItem.SelectedItem;
                //account for selecting the empty value
                if (selectedItem != null)
                {
                    lineItems.Add(selectedItem);
                    //need to manually force a refresh
                    dgInvoiceItems.Items.Refresh();
                    CalculateInvoiceTotal();
                }
                else
                {
                    MessageBox.Show("No item was selected so no item was added.");
                }
            }
            catch (Exception ex)
            {
                //This is the top level method so we want to handle the exception
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }


        /// <summary>
        /// Handle when the remove button is clicked, removes the selected item from the invoice
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemoveItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                clsItemDescObj selectedItem = (clsItemDescObj)dgInvoiceItems.SelectedItem;
                //account for selecting the empty value
                if (selectedItem != null)
                {
                    lineItems.Remove(selectedItem);
                    //need to manually force a refresh
                    dgInvoiceItems.Items.Refresh();
                    //if this isn't set, the next selection is multiple rows.
                    dgInvoiceItems.SelectedIndex = -1;
                    CalculateInvoiceTotal();
                }
                else
                {
                    MessageBox.Show("No item was selected so an item was not removed.");
                }
            }
            catch (Exception ex)
            {
                //This is the top level method so we want to handle the exception
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Handles calculating the invoice total based on the items added to the invoice
        /// </summary>
        private void CalculateInvoiceTotal()
        {
            try
            {
                int total = lineItems.Sum(i => Convert.ToInt32(i.sCost));
                lblTotalCost.Content = $"{total:C}";
                //maintain keeping track of the total cost for the invoice
                invoice.sTotalCost = total.ToString();
            }
            catch (Exception ex)
            {
                //This is the top level method so we want to handle the exception
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Handles when the date picker value is changed and makes it the value is always tracked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dpInvoiceDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (dpInvoiceDate.SelectedDate.HasValue)
                    invoice.sInvoiceDate = dpInvoiceDate.SelectedDate.Value.ToString("M/dd/yyyy");
                else if (invoice != null)
                    invoice.sInvoiceDate = "";
            }
            catch (Exception ex)
            {
                //This is the top level method so we want to handle the exception
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void LoadInvoice(int invoiceID)
        {
            try
            {
                invoice = sqlProvider.GetInvoice(invoiceID.ToString());
                if (invoice == null)
                    throw new Exception("Failed to retrieve the invoice from the database");
                lineItems = sqlProvider.GetInvoiceLineItems(invoiceID.ToString());
                if (lineItems == null)
                    throw new Exception("Failed to retrieve line items from the database");
                DateTime invoiceDate;
                if (DateTime.TryParse(invoice.sInvoiceDate, out invoiceDate))
                    dpInvoiceDate.SelectedDate = invoiceDate;
                else
                    MessageBox.Show("Failed to load invoice date.");
                int totalCost;
                if (int.TryParse(invoice.sTotalCost, out totalCost))
                    lblTotalCost.Content = $"{totalCost:C}";
                else
                    MessageBox.Show("Failed to load the invoice total cost");
                dgInvoiceItems.ItemsSource = lineItems;
                btnEditInvoice.IsEnabled = true;
                btnDeleteInvoice.IsEnabled = true;
                lblInvoiceNum.Content = $"Invoice #{invoiceID}";
            }
            catch (Exception ex)
            {
                //This is the top level method so we want to handle the exception
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Handle the error at the top level. Log and display message
        /// </summary>
        /// <param name="sClass">Class error occurred in</param>
        /// <param name="sMethod">Method error occurred in</param>
        /// <param name="sMessage">Message from exception</param>
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
    }
}
