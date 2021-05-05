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
    /// Interaction logic for wndSearch.xaml
    /// </summary>
    public partial class wndSearch : Window
    {
        #region class attributes and variables

        /// <summary>
        /// variable of the selected invoice id
        /// </summary>
        public int selectedInvoiceId;

        /// <summary>
        /// logic class variable
        /// </summary>
        private clsSearchLogic logic;

        #endregion

        #region Logic methods for the ui

        /// <summary>
        /// Constructor of a new instance of the wndSearch object
        /// </summary>
        public wndSearch()
        {
            try
            {
                InitializeComponent();
                selectedInvoiceId = 0; // set selected invoice id to 0
                logic = new clsSearchLogic(); // initialize the clsSearchLogic class
            }
            catch (Exception ex)
            {
                //This is the top level method so we want to handle the exception
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        } // end of constructor

        /// <summary>
        /// Event handler of what happens when the user pressed the select button and will close
        /// out this form and open up the selected invoice to the main window
        /// </summary>
        /// <param name="sender">The select button that got click with the selectBtn name</param>
        /// <param name="e">contains information about the selectBtn click</param>
        private void selectBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // check to see if the user select an invoie
                if (selectDataGrid.SelectedItem != null)
                {
                    // grab the user choice of the invoice and cast to object
                    Database.clsInvoiceObj userSelect = (Database.clsInvoiceObj)selectDataGrid.SelectedItem;

                    // set the window select invoice to the user selection
                    // the main menu can then grab the selectedInvoiceId class variable from this class to use in the main menu and pass around the id to find the matching invoice.
                    selectedInvoiceId = logic.grabInvoiceNum(userSelect.sInvoiceNum);

                    // close the select window
                    this.Hide();
                } else
                {
                    // error message not not selecting and pressing the button
                    searchErrLbl.Content = "Sorry You Have To Chose An Invoice To Select";
                }
            }
            catch (Exception ex)
            {
                //This is the top level method so we want to handle the exception
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        } // end of selectBtn_Click method

        /// <summary>
        /// Event handler of what happens when the user pressed the cancel button that will cancel 
        /// all the actions the user did and return to the initial state.
        /// </summary>
        /// <param name="sender">The cancel button that got click with the cancelBtn name</param>
        /// <param name="e">contains information about the cancelBtn click</param>
        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // reset all values to initial state
                logic.sSelectedInvoiceNum = "";
                logic.sSelectedInvoiceDate = "";
                logic.sSelectedTotal = "";
                selectedInvoiceId = 0; // set selected invoice id to 0

                // close the select window
                this.Hide();
            }
            catch (Exception ex)
            {
                //This is the top level method so we want to handle the exception
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        } // end of cancelBtn_Click method

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

        /// <summary>
        /// Event handler of when the window loads and load up the initial data.
        /// </summary>
        /// <param name="sender">The window object</param>
        /// <param name="e">contains information about the window data</param>
        private void searchWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                // list of invoices
                List<Database.clsInvoiceObj> invoiceList = logic.LoadInvoices();

                // display invoices
                displayInvoices();
                
                // load up all invoice number into the combo box
                invoiceNumComboBx.ItemsSource = logic.grabInvoiceNumList(invoiceList);

                // load up all invoice dates into the combo box
                invoiceDateComboBx.ItemsSource = logic.grabInvoiceDateList(invoiceList);

                // load up all total cost into the combo box
                totalChargeComboBx.ItemsSource = logic.loadOrderedTotal();

            }
            catch (Exception ex)
            {
                //This is the top level method so we want to handle the exception
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        } // end of searchWindow_Loaded method

        /// <summary>
        /// The event handler for when the invoice num combo box is changed in value and what 
        /// to do when the invoice changes or selected.
        /// </summary>
        /// <param name="sender">The combo box with invoiceNumComboBx name</param>
        /// <param name="e">contains information about the invoiceNumComboBx data</param>
        private void invoiceNumComboBx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                // load up all the invoices from logic method to datagrid 
                if (invoiceNumComboBx.SelectedItem == null)
                {
                    // set selected to empty
                    logic.sSelectedInvoiceNum = "";
                } else
                {
                    // set selected to what the user chose
                    logic.sSelectedInvoiceNum = invoiceNumComboBx.SelectedItem.ToString();
                }

                /// display invoices
                displayInvoices();
            }
            catch (Exception ex)
            {
                //This is the top level method so we want to handle the exception
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        } // end of invoiceNumComboBx_SelectionChanged method

        /// <summary>
        /// The event handler for when the invoice date combo box is changed in value and what 
        /// to do when the invoice changes or selected.
        /// </summary>
        /// <param name="sender">The combo box with invoiceDateComboBx name</param>
        /// <param name="e">contains information about the invoiceDateComboBx data</param>
        private void invoiceDateComboBx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                // load up all the invoices from logic method to datagrid 
                if (invoiceDateComboBx.SelectedItem == null)
                {
                    // set selected to empty
                    logic.sSelectedInvoiceDate = "";
                }
                else
                {
                    // set selected to what the user chose
                    logic.sSelectedInvoiceDate = invoiceDateComboBx.SelectedItem.ToString();
                }

                // display invoices
                displayInvoices();
            }
            catch (Exception ex)
            {
                //This is the top level method so we want to handle the exception
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        } // end of totalChargeComboBx_SelectionChanged method

        /// <summary>
        /// The event handler for when the invoice total cost combo box is changed in value and what 
        /// to do when the invoice changes or selected.
        /// </summary>
        /// <param name="sender">The combo box with totalChargeComboBx name</param>
        /// <param name="e">contains information about the totalChargeComboBx data</param>
        private void totalChargeComboBx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                // load up all the invoices from logic method to datagrid 
                if (totalChargeComboBx.SelectedItem == null)
                {
                    // set selected to empty
                    logic.sSelectedTotal = "";
                }
                else
                {
                    // set selected to what the user chose
                    logic.sSelectedTotal = totalChargeComboBx.SelectedItem.ToString();
                }

                // display invoices
                displayInvoices();
            }
            catch (Exception ex)
            {
                //This is the top level method so we want to handle the exception
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        } // end of totalChargeComboBx_SelectionChanged method

        /// <summary>
        /// Event handler for the clear button to clear out the filter and set to initial state.
        /// </summary>
        /// <param name="sender">The button with ClearBtn name</param>
        /// <param name="e">contains information about the Clear button data</param>
        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // display invoices
                displayInvoices();

                // clear invoice num combo box
                invoiceNumComboBx.SelectedItem = null;

                // clear invoice date combo box
                invoiceDateComboBx.SelectedItem = null;

                // clear invoice total combo box
                totalChargeComboBx.SelectedItem = null;
            }
            catch (Exception ex)
            {
                //This is the top level method so we want to handle the exception
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        } // end of ClearBtn_Click method

        /// <summary>
        /// Helper method to display the right data in the datagrid and format the datagrid.
        /// </summary>
        private void displayInvoices()
        {
            try
            {
                // load up all the invoices from logic method to datagrid 
                selectDataGrid.ItemsSource = logic.LoadInvoices(logic.sSelectedInvoiceNum, logic.sSelectedInvoiceDate, logic.sSelectedTotal);

                //change the column header to be more readable
                selectDataGrid.Columns[0].Header = "Invoice #";
                selectDataGrid.Columns[1].Header = "Invoice Date";
                selectDataGrid.Columns[2].Header = "Total Cost";

                // remove extra row
                selectDataGrid.CanUserAddRows = false;
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText("C:\\Error.txt", Environment.NewLine +
                                             "HandleError Exception: " + ex.Message);
            }
        } // end of displayInvoices method

        #endregion

    } // end of class
} // end of namespace
