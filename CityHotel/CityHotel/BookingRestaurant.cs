using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using MyVal;
using CrystalDecisions.CrystalReports;

namespace CityHotel
{
    public partial class BookingRestaurant : Form
    {
        DataSet dsCityHotelDB = new DataSet();

        string connStr, displaySQL, menuSQL, itemTypeFindSQL, custFinfSQL, staffSQL, tableSQL,
               bookingMenuSQL, bookingRestaurantSQL, bookingRestDetailsSQL;

        Button[] btn = new Button[20];

        int maxPoeple = 0, noPeople = 0;

        double menuItemsTotlePrice = 0;

        bool editMode = false;

        SqlDataAdapter daDisplay, daMenu, daItemTypeFind, daCustFind, daStaff,
           daTable, daBookingMenu, daBookingRestaurant, daBookingRestDetails;

        SqlCommandBuilder scbDisplay, scbItemTypeFind, scbStaff,
            scbBookingMenu, scbBookingRestaurant, scbBookingRestDetails;


        SqlCommand cmdCustFind, cmdMenu, cmbTable;

        SqlConnection conn;

        DataRow drBookingMenu, drBookingRestaurant, drBookingRestDetails, drTable, drCustomer, drMenuItem;

        //this boolean will only tell the program to ignroe a message boxes
        //that could appear that may appera at particular times
        bool ignore = false;

        ListViewItem item_remove;

        public BookingRestaurant()
        {
            InitializeComponent();
        }

        private void BookingRestaurant_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < 20; i++)
            {
                //sets all the table buttons functionality
                btn[i] = (Button)restFloor.Controls[19 - i];
                btn[i].Click += new EventHandler(table1_Click);
                btn[i].MouseHover += new EventHandler(table1_MouseHover);
            }

            //connetion str
            connStr = @"Data Source = .; Initial Catalog = CityHotel; Integrated Security = true";
            conn = new SqlConnection(connStr);

            //Queries

            //Essential Queries
            bookingRestaurantSQL = @"select * from BookingRestaurant";
            daBookingRestaurant = new SqlDataAdapter(bookingRestaurantSQL, connStr);
            scbBookingRestaurant = new SqlCommandBuilder(daBookingRestaurant);
            daBookingRestaurant.FillSchema(dsCityHotelDB, SchemaType.Source, "BookingRestaurant");
            daBookingRestaurant.Fill(dsCityHotelDB, "BookingRestaurant");

            //maine display
            displaySQL = @"select br.BookingID, br.CustomerNo, c.Forename, c.Surname, brd.StaffID, s.Forename, s.Surname, brd.DateTimeBooked,
                           br.DateTimeBookedFor, br.NoOfPeople, td.TableID, td.NoSeats , m.ItemID, m.MenuItemDesc, m.Price, bm.Quantity from JobRole as jr
                           join Staff as s on s.JobRoleID = jr.JobRoleID
                           join BookingRestDetails as brd on brd.StaffID = s.StaffID
                           join TableDetails as td on td.TableID = brd.TableID
                           join BookingRestaurant as br on br.BookingID = brd.BookingID
                           join Customer as c on c.CustomerNo = br.CustomerNo
                           join BookingMenu as bm on bm.BookingID = br.BookingID
                           join Menu as m on m.ItemID = bm.ItemID";

            daDisplay = new SqlDataAdapter(displaySQL, connStr);
            scbDisplay = new SqlCommandBuilder(daDisplay);
            daDisplay.FillSchema(dsCityHotelDB, SchemaType.Source, "JobRole");
            daDisplay.Fill(dsCityHotelDB, "JobRole");

            dataGridViewGenDisplay.DataSource = dsCityHotelDB.Tables["JobRole"];
            dataGridViewGenDisplay.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            dataGridViewGenDisplay.ClearSelection();



            bookingMenuSQL = @"select * from BookingMenu";
            daBookingMenu = new SqlDataAdapter(bookingMenuSQL, connStr);
            scbBookingMenu = new SqlCommandBuilder(daBookingMenu);
            daBookingMenu.FillSchema(dsCityHotelDB, SchemaType.Source, "BookingMenu");
            daBookingMenu.Fill(dsCityHotelDB, "BookingMenu");


            bookingRestDetailsSQL = @"select * from BookingRestDetails";
            daBookingRestDetails = new SqlDataAdapter(bookingRestDetailsSQL, connStr);
            scbBookingRestDetails = new SqlCommandBuilder(daBookingRestDetails);
            daBookingRestDetails.FillSchema(dsCityHotelDB, SchemaType.Source, "BookingRestDetails");
            daBookingRestDetails.Fill(dsCityHotelDB, "BookingRestDetails");

            staffSQL = @"select s.StaffID, s.Forename, s.Surname from Staff as s
                         join JobRole as jr on jr.JobRoleID = s.JobRoleID
                         where jr.JobDesc = 'Waiter'";
            daStaff = new SqlDataAdapter(staffSQL, connStr);
            scbStaff = new SqlCommandBuilder(daStaff);
            daStaff.FillSchema(dsCityHotelDB, SchemaType.Source, "Staff");
            daStaff.Fill(dsCityHotelDB, "Staff");

            dataGridViewAvailableStaff.DataSource = dsCityHotelDB.Tables["Staff"];
            dataGridViewAvailableStaff.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            dataGridViewAvailableStaff.ClearSelection();



            itemTypeFindSQL = @"select * from ItemType";
            daItemTypeFind = new SqlDataAdapter(itemTypeFindSQL, connStr);
            scbItemTypeFind = new SqlCommandBuilder(daItemTypeFind);
            daItemTypeFind.FillSchema(dsCityHotelDB, SchemaType.Source, "ItemType");
            daItemTypeFind.Fill(dsCityHotelDB, "ItemType");

            comboBoxItemTypes.DataSource = dsCityHotelDB.Tables["ItemType"];
            comboBoxItemTypes.DisplayMember = "ItemDesc";
            comboBoxItemTypes.ValueMember = "ItemDesc";
            comboBoxItemTypes.SelectedIndex = -1;



            //parameterised queries

            menuSQL = @"select m.ItemID, m.MenuItemDesc, m.Price, it.ItemDesc from Menu as m
                        join ItemType as it on it.ItemTypeID = m.ItemTypeID
                        where it.ItemDesc = @Type";
            cmdMenu = new SqlCommand(menuSQL, conn);
            cmdMenu.Parameters.Add("@Type", SqlDbType.VarChar);
            daMenu = new SqlDataAdapter(cmdMenu);
            daMenu.FillSchema(dsCityHotelDB, SchemaType.Source, "Menu");

            dataGridViewMenu.DataSource = dsCityHotelDB.Tables["Menu"];
            dataGridViewMenu.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            cmdMenu.Parameters["@Type"].Value = comboBoxItemTypes.SelectedValue;


            custFinfSQL = @"select c.CustomerNo, c.Forename + ' ' + c.Surname as name from Customer as c
                            where c.Forename like @Letter";
            cmdCustFind = new SqlCommand(custFinfSQL, conn);
            cmdCustFind.Parameters.Add("@Letter", SqlDbType.VarChar);
            daCustFind = new SqlDataAdapter(cmdCustFind);
            daCustFind.FillSchema(dsCityHotelDB, SchemaType.Source, "Customer");

            comboBoxLetterSearch.SelectedIndex = 0;

            listBoxCustomerSearch.DataSource = dsCityHotelDB.Tables["Customer"];
            listBoxCustomerSearch.DisplayMember = "name";
            listBoxCustomerSearch.ValueMember = "CustomerNo";

            tableSQL = @"select * from TableDetails
                         where TableID like @No";
            cmbTable = new SqlCommand(tableSQL, conn);
            cmbTable.Parameters.Add("@No", SqlDbType.TinyInt);
            daTable = new SqlDataAdapter(cmbTable);
            daTable.FillSchema(dsCityHotelDB, SchemaType.Source, "TableDetails");

            ignore = true;
            dateTimePickerFor.MinDate = DateTime.Now;
            ignore = false;

            listBoxCustomerSearch.ClearSelected();
            timerDate.Start();

        }

        
        private void listViewSelectedItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            buttonRemoveItem.Enabled = true;
            try
            {
                item_remove = listViewSelectedItems.SelectedItems[0];
            }
            catch (ArgumentOutOfRangeException aoore)
            {
                if (Convert.ToInt32(item_remove.SubItems[3].Text) > 1)
                    MessageBox.Show("Please select a row below to remove an item");
            }
        }

        private void buttonRemoveItem_Click(object sender, EventArgs e)
        {
            try
            {
                ListViewItem findItemF = listViewMenuItemsF.FindItemWithText(listViewSelectedItems.SelectedItems[0].SubItems[1].Text);

                int selectedIndex = Convert.ToInt32(listViewSelectedItems.SelectedItems[0].Index);

                if (listViewSelectedItems.SelectedItems[0].SubItems[3].Text == "1")
                {
                    listViewSelectedItems.Items.Remove(item_remove);
                    listViewMenuItemsF.Items.Remove(findItemF);

                }
                else
                {
                    listViewSelectedItems.SelectedItems[0].SubItems[3].Text =
                        (Convert.ToInt32(listViewSelectedItems.SelectedItems[0].SubItems[3].Text) - 1).ToString();

                    listViewSelectedItems.Items[selectedIndex].Selected = true;


                    listViewMenuItemsF.Items[selectedIndex].SubItems[1].Text =
                        (Convert.ToInt32(listViewMenuItemsF.Items[selectedIndex].SubItems[1].Text) - 1).ToString();

                }

                menuItemsTotlePrice -= Convert.ToDouble(item_remove.SubItems[2].Text);

                if (menuItemsTotlePrice < 0)
                    menuItemsTotlePrice = 0;

                labelTotelPrice.Text = menuItemsTotlePrice.ToString();


            }
            catch (ArgumentOutOfRangeException aoore)
            {
                //if (Convert.ToInt32(item_remove.SubItems[3].Text) > 1)
                MessageBox.Show("Please select a row below to remove an item");
            }

        }

        private void buttonAddItem_Click(object sender, EventArgs e)
        {
            int nrs = dataGridViewMenu.SelectedRows[0].Index;
            try
            {
                drMenuItem = dsCityHotelDB.Tables["Menu"].Rows[nrs];
                int findIndex = Convert.ToInt32(drMenuItem["ItemID"].ToString()) - 1;


                ListViewItem surchItems = listViewSelectedItems.FindItemWithText(drMenuItem["ItemID"].ToString());
                ListViewItem surchItemsF = listViewMenuItemsF.FindItemWithText(drMenuItem["MenuItemDesc"].ToString());


                try
                {
                    if (Convert.ToInt32(surchItems.SubItems[3].Text) >= 1)
                    {
                        surchItems.SubItems[3].Text = (Convert.ToInt32(listViewSelectedItems.Items[surchItems.Index].SubItems[3].Text) + 1).ToString();
                        surchItemsF.SubItems[1].Text = (Convert.ToInt32(listViewSelectedItems.Items[surchItemsF.Index].SubItems[3].Text)).ToString();
                    }
                }
                catch (NullReferenceException)
                {
                    ListViewItem item = new ListViewItem(drMenuItem["ItemID"].ToString());
                    item.SubItems.Add(drMenuItem["MenuItemDesc"].ToString());
                    item.SubItems.Add(drMenuItem["Price"].ToString());
                    item.SubItems.Add("1");

                    listViewSelectedItems.Items.Add(item);

                    ListViewItem itemF = new ListViewItem(drMenuItem["MenuItemDesc"].ToString());
                    itemF.SubItems.Add("1");
                    listViewMenuItemsF.Items.Add(itemF);

                }


                menuItemsTotlePrice += Convert.ToDouble(drMenuItem["Price"].ToString());
                labelTotelPrice.Text = menuItemsTotlePrice.ToString();
            }
            catch (IndexOutOfRangeException)
            {
                MessageBox.Show("Please Select a Menu Item by selecting a item type in the combobox");
            }

        }

        private void dataGridViewMenu_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            buttonAddItem.Enabled = true;
        }

        private void tabBookingDetails_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!editMode)
            {
                labelBookingNo.Text = set_add_Booking_label();
                buttonAdd.Enabled = true;
            }
            else
            {
                buttonAdd.Enabled = false;
            }
            labelMaxSeatsF.Text = labelMaxPeople.Text;

            if (labelTimeFor.Text != "- - -")
            {
                labelDayTimeChosenF.Text = dateTimePickerFor.Text + " " + labelTimeFor.Text + ":00";
            }
            else
                labelDayTimeChosenF.Text = "- - -";

            if (menuItemsTotlePrice == 0)
            {
                labelTotelPrice.Text = "- - -";
            }
            else
                labelTotelPrice.Text = menuItemsTotlePrice.ToString();


            if (numericUpDownNoPeople.Value == 0)
                labelNoOfPeopleF.Text = numericUpDownNoPeople.Value.ToString();
            else
                labelNoOfPeopleF.Text = "- - -";


            if (numericUpDownNoPeople.Value <= 0)
            {
                labelNoOfPeopleF.Text = "- - -";
            }
            else
            {
                labelNoOfPeopleF.Text = numericUpDownNoPeople.Value.ToString();
            }


            try
            {
                labelStaffMemberF.Text = dataGridViewAvailableStaff.SelectedRows[0].Cells[0].Value + " " +
                                         dataGridViewAvailableStaff.SelectedRows[0].Cells[1].Value + " " +
                                         dataGridViewAvailableStaff.SelectedRows[0].Cells[2].Value;
            }
            catch (ArgumentOutOfRangeException aoore)
            {
                labelStaffMemberF.Text = "- - -";
            }

        }

        private void listViewTimeFor_Click(object sender, EventArgs e)
        {


            labelTimeFor.Text = listViewTimeFor.SelectedItems[0].Text;

        }

        private void tabNav1_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (!editMode)
            {
                if (!ignore)
                {
                    try
                    {
                        if (listViewTimeFor.SelectedItems[0].Text == "") { }
                    }
                    catch (ArgumentOutOfRangeException aoore)
                    {
                        MessageBox.Show("Please select a time for the booking!");
                        ignore = true;
                        tabNav.SelectedIndex = 0;
                        ignore = false;
                    }
                }
            }

            if (editMode && !ignore)
            {
                if (MessageBox.Show("you are currently editing a recored\n" +
                    "would you like to cancel edit?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
                    == System.Windows.Forms.DialogResult.Yes)
                {
                    ignore = true;
                    clearAll();
                    tabNav.SelectedIndex = 0;
                    ignore = false;

                    editMode = false;
                    editModeStat.Text = "Disabled";
                    editModeStat.ForeColor = Color.Red;

                    maxPoeple = 0;
                    noPeople = 0;

                    try
                    {
                        for (int i = 0; i < dataGridViewAvailableStaff.ColumnCount; i++)
                            dataGridViewAvailableStaff.SelectedRows[0].Cells[i].Selected = false;
                    }
                    catch (ArgumentOutOfRangeException aoore) { }


                    try
                    {
                        for (int i = 0; i < dataGridViewGenDisplay.ColumnCount; i++)
                            dataGridViewGenDisplay.SelectedRows[0].Cells[i].Selected = false;
                    }
                    catch (ArgumentOutOfRangeException aoore) { }


                    try
                    {
                        for (int i = 0; i < dataGridViewMenu.ColumnCount; i++)
                            dataGridViewMenu.SelectedRows[0].Cells[i].Selected = false;
                    }
                    catch (ArgumentOutOfRangeException aoore) { }

                }
                else
                {
                    editMode = false;
                    ignore = false;
                    tabNav.SelectedIndex = 1;
                    ignore = true;
                    editMode = true;
                }

                try
                {
                    if (Convert.ToInt32(dataGridViewGenDisplay.SelectedRows[0].Cells[0].Value) > 0)
                        setTablesFormat();
                }
                catch (ArgumentOutOfRangeException) { }

            }
            

        }


        private string set_add_Booking_label()
        {
            string str = "";

            bool notFound = true;
            if (dsCityHotelDB.Tables["BookingRestaurant"].Rows.Count == 0)
            {
                str = "10001";
            }
            else
            {
                int change = 10001;
                for (int i = 0; i < dsCityHotelDB.Tables["Jobrole"].Rows.Count; i++)
                {
                    drBookingRestaurant = dsCityHotelDB.Tables["JobRole"].Rows[i];

                    if (dataGridViewGenDisplay.Rows[i].Cells[0].Value.ToString() != change.ToString())
                    {
                        change++;
                    }

                    if (Convert.ToInt32(drBookingRestaurant["BookingID"].ToString()) != change)
                    {
                        str = change.ToString();
                        notFound = false;
                        break;
                    }

                }

                if (notFound)
                {
                    str = (dsCityHotelDB.Tables["BookingRestaurant"].Rows.Count + 10001).ToString();
                }
            }
            return str;
        }

        private void listBoxCustomerSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                drCustomer = dsCityHotelDB.Tables["Customer"].Rows[listBoxCustomerSearch.SelectedIndex];
                labelCustomerNoF.Text = drCustomer["CustomerNo"].ToString()
                                + " " + drCustomer["name"].ToString();
            }
            catch (IndexOutOfRangeException ioore)
            {
                labelCustomerNoF.Text = "- - -";
            }
        }

        private void comboBoxLetterSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                dsCityHotelDB.Tables["Customer"].Clear();
                cmdCustFind.Parameters["@Letter"].Value = comboBoxLetterSearch.SelectedItem.ToString() + "%";
                daCustFind.Fill(dsCityHotelDB, "Customer");
                listBoxCustomerSearch.Update();
                listBoxCustomerSearch.ClearSelected();
            }
            catch (NullReferenceException nre)
            {
                //MessageBox.Show(nre.ToString());
            }

        }

        private bool datecomp(DateTime dateBooked, DateTime dateValue, int noSeats)
        {
            bool ok = true;
            int maxTimeHr = 0;
            int maxTimeMin = 0;

            if (noSeats > 0 && noSeats <= 2)
                maxTimeHr = 1;

            else if (noSeats > 2 && noSeats <= 4)
            {
                maxTimeHr = 1;
                maxTimeMin = 30;
            }
            else if (noSeats > 4 && noSeats <= 6)
                maxTimeHr = 2;

            else if (noSeats > 6 && noSeats <= 8)
            {
                maxTimeHr = 2;
                maxTimeMin = 30;
            }
            else
                maxTimeHr = 3;

            TimeSpan timeS = new TimeSpan(0, maxTimeHr, maxTimeMin, 0);

            int result = DateTime.Compare(dateBooked, dateValue);
            int resultPlus = DateTime.Compare(dateBooked.Add(timeS), dateValue);

            if (result == 0)
                ok = false;

            else if (result < 0)
                ok = false;

            else if (resultPlus < 0)
                ok = false;

            if (result > 0)
                ok = true;

            return ok;
        }

        private void listViewTables_Click(object sender, EventArgs e)
        {
            if (listViewTables.SelectedItems.Count != 0)
            {

                ListViewItem item = listViewTables.SelectedItems[0];

                for (int i = 0; i < 20; i++)
                {

                    if (listViewTables.SelectedItems[0].Text == btn[i].Text)
                    {
                        btn[i].Enabled = true;
                        btn[i].BackColor = Color.DarkGray;

                        dsCityHotelDB.Tables["TableDetails"].Clear();
                        cmbTable.Parameters["@No"].Value = Convert.ToInt32(btn[i].Text);
                        daTable.Fill(dsCityHotelDB, "TableDetails");

                        drTable = dsCityHotelDB.Tables["TableDetails"].Rows[0];

                        labelMaxPeople.Text = (int.Parse(labelMaxPeople.Text) - int.Parse(drTable["NoSeats"].ToString())).ToString();
                        maxPoeple -= int.Parse(drTable["NoSeats"].ToString());

                        break;
                    }
                }

                if (labelMaxPeople.Text == "0")
                {
                    labelMaxPeople.Text = "- - -";
                    labelMaxSeatsF.Text = "- - - ";
                }

                ListViewItem itemF = listViewTableF.FindItemWithText(listViewTables.SelectedItems[0].Text);

                listViewTables.Items.Remove(item);
                listViewTableF.Items.Remove(itemF);
            }

        }


        private void comboBoxItemTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            buttonAddItem.Enabled = false;
            try
            {
                dsCityHotelDB.Tables["Menu"].Clear();
                cmdMenu.Parameters["@Type"].Value = comboBoxItemTypes.SelectedValue.ToString();
                daMenu.Fill(dsCityHotelDB, "Menu");
                dataGridViewMenu.Update();
                dataGridViewMenu.ClearSelection();
            }
            catch (NullReferenceException nre)
            {
                //MessageBox.Show(nre.ToString());
            }
        }

        private void table1_MouseHover(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            ToolTip tt = new ToolTip();

            dsCityHotelDB.Tables["TableDetails"].Clear();
            cmbTable.Parameters["@No"].Value = Convert.ToInt32(b.Text);
            daTable.Fill(dsCityHotelDB, "TableDetails");

            drTable = dsCityHotelDB.Tables["TableDetails"].Rows[0];

            tt.SetToolTip(b, "No:" + b.Text + " and " + drTable["NoSeats"].ToString() + " Seats");
        }

        private void table1_Click(object sender, EventArgs e)
        {

            Button b = (Button)sender;
            String str = b.Text;

            filldisplayListBoxTables(str);


            b.Enabled = false;
            b.BackColor = Color.Red;

        }

        private void filldisplayListBoxTables(String str)
        {
            dsCityHotelDB.Tables["TableDetails"].Clear();
            cmbTable.Parameters["@No"].Value = Convert.ToInt32(str);
            daTable.Fill(dsCityHotelDB, "TableDetails");

            drTable = dsCityHotelDB.Tables["TableDetails"].Rows[0];
            ListViewItem item = new ListViewItem(drTable["TableID"].ToString());
            item.SubItems.Add(drTable["NoSeats"].ToString());

            listViewTables.Items.Add(item);

            listViewTableF.Items.Add((ListViewItem)item.Clone());


            maxPoeple += Convert.ToInt32(drTable["NoSeats"].ToString());

            labelMaxPeople.Text = maxPoeple.ToString();

        }

        private void timerDate_Tick(object sender, EventArgs e)
        {
            labelToDayF.Text = getDateTime();
        }

        private void clearAll()
        {
            listBoxCustomerSearch.ClearSelected();
            comboBoxLetterSearch.SelectedIndex = 0;
            comboBoxLetterSearch.Text = "";

            ignore = true;
            listViewTables.Items.Clear();
            dateTimePickerFor.MinDate = DateTime.Now;
            numericUpDownNoPeople.Value = 0;
            dateTimePickerFor.Text = "";

            ignore = false;

            labelMaxPeople.Text = "- - -";
            dataGridViewAvailableStaff.ClearSelection();

            try
            {
                listViewTimeFor.SelectedItems[0].Selected = false;
            }
            catch (ArgumentOutOfRangeException aoore) { }

            labelTimeFor.Text = "- - -";
            dataGridViewMenu.ClearSelection();
            comboBoxItemTypes.SelectedIndex = 0;
            listViewSelectedItems.Items.Clear();
            labelBookingNo.Text = "- - -";
            labelCustomerNoF.Text = "- - -";
            labelDayTimeChosenF.Text = "- - -";
            labelMaxSeatsF.Text = "- - -";
            labelNoOfPeopleF.Text = "- - -";
            listViewTableF.Items.Clear();
            listViewMenuItemsF.Items.Clear();
            labelTotelPrice.Text = "- - -";
            tabBookingDetails.SelectedIndex = 0;
            ignore = true;
            tabNav.SelectedIndex = 0;
            ignore = false;
            menuItemsTotlePrice = 0;

        }

        private string getDateTime()
        {
            return DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            bool ok = true;

            if (numericUpDownNoPeople.Value == 0)
            {
                errP.SetError(numericUpDownNoPeople, "Number of people can only be more than 0");
                tabBookingDetails.SelectedIndex = 0;
                ok = false;
            }

            if (labelStaffMemberF.Text == "- - -")
            {
                errP.SetError(labelAvalibleStaff, "Select a staff member");
                tabBookingDetails.SelectedIndex = 0;
                ok = false;
            }

            if (labelMaxSeatsF.Text == "- - -")
            {
                errP.SetError(listViewTables, "Please select one tabel or more");
                tabBookingDetails.SelectedIndex = 0;
                ok = false;
            }

            if (labelCustomerNoF.Text == "- - -")
            {
                errP.SetError(listBoxCustomerSearch, "Please select a customer name");
                tabBookingDetails.SelectedIndex = 0;
                ok = false;
            }

            if (labelDayTimeChosenF.Text == "- - -")
            {
                errP.SetError(labelDateTime, "Please selecta a date and time for the booking");
                tabBookingDetails.SelectedIndex = 0;
                ok = false;
            }

            if (listViewSelectedItems.Items.Count == 0)
            {
                MessageBox.Show("Please select an item from the menu");
                tabBookingDetails.SelectedIndex = 1;
                ok = false;
            }

            if (ok)
            {
                errP.Clear();
                drBookingRestaurant = dsCityHotelDB.Tables["BookingRestaurant"].NewRow();

                drBookingRestaurant["BookingID"] = Convert.ToInt32(labelBookingNo.Text);
                drBookingRestaurant["CustomerNo"] = drCustomer["CustomerNo"].ToString();
                drBookingRestaurant["DateTimeBookedFor"] = labelDayTimeChosenF.Text;
                drBookingRestaurant["NoOfPeople"] = labelNoOfPeopleF.Text;
                dsCityHotelDB.Tables["BookingRestaurant"].Rows.Add(drBookingRestaurant);



                for (int i = 0; i < listViewTables.Items.Count; i++)
                {
                    drBookingRestDetails = dsCityHotelDB.Tables["BookingRestDetails"].NewRow();

                    drBookingRestDetails["BookingID"] = Convert.ToInt32(labelBookingNo.Text);
                    drBookingRestDetails["TableID"] = Convert.ToInt32(listViewTables.Items[i].SubItems[0].Text);
                    drBookingRestDetails["StaffID"] = dataGridViewAvailableStaff.SelectedRows[0].Cells[0].Value;
                    drBookingRestDetails["DateTimeBooked"] = labelToDayF.Text;
                    dsCityHotelDB.Tables["BookingRestDetails"].Rows.Add(drBookingRestDetails);

                }



                for (int i = 0; i < listViewSelectedItems.Items.Count; i++)
                {
                    drBookingMenu = dsCityHotelDB.Tables["BookingMenu"].NewRow();

                    drBookingMenu["BookingID"] = Convert.ToInt32(labelBookingNo.Text);
                    drBookingMenu["ItemID"] = Convert.ToInt32(listViewSelectedItems.Items[i].SubItems[0].Text);
                    drBookingMenu["Quantity"] = Convert.ToInt32(listViewSelectedItems.Items[i].SubItems[3].Text);
                    dsCityHotelDB.Tables["BookingMenu"].Rows.Add(drBookingMenu);
                }


                daBookingRestaurant.Update(dsCityHotelDB, "BookingRestaurant");
                daBookingRestaurant.Update(dsCityHotelDB.Tables["BookingRestaurant"]);

                daBookingRestDetails.Update(dsCityHotelDB, "BookingRestDetails");
                daBookingRestDetails.Update(dsCityHotelDB.Tables["BookingRestDetails"]);

                daBookingMenu.Update(dsCityHotelDB, "BookingMenu");
                daBookingMenu.Update(dsCityHotelDB.Tables["BookingMenu"]);

                dsCityHotelDB.Tables["JobRole"].Clear();
                daDisplay.Fill(dsCityHotelDB, "JobRole");

                menuItemsTotlePrice = 0;
                maxPoeple = 0;
                noPeople = 0;

                ignore = true;
                clearAll();
                ignore = false;
            }
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            bool ok = true;

            int nrs = Convert.ToInt32(dataGridViewGenDisplay.SelectedRows[0].Cells[0].Value);
            drBookingRestaurant = dsCityHotelDB.Tables["BookingRestaurant"].Rows[nrs - 10001];


            if (numericUpDownNoPeople.Value == 0)
            {
                errP.SetError(numericUpDownNoPeople, "Number of people can only be more than 0");
                tabBookingDetails.SelectedIndex = 0;
                ok = false;
            }

            if (labelStaffMemberF.Text == "- - -")
            {
                errP.SetError(labelAvalibleStaff, "Select a staff member");
                tabBookingDetails.SelectedIndex = 0;
                ok = false;
            }

            if (labelMaxSeatsF.Text == "- - -")
            {
                errP.SetError(listViewTables, "Please select one tabel or more");
                tabBookingDetails.SelectedIndex = 0;
                ok = false;
            }

            if (labelCustomerNoF.Text == "- - -")
            {
                errP.SetError(listBoxCustomerSearch, "Please select a customer name");
                tabBookingDetails.SelectedIndex = 0;
                ok = false;
            }

            if (labelDayTimeChosenF.Text == "- - -")
            {
                errP.SetError(labelDateTime, "Please selecta a date and time for the booking");
                tabBookingDetails.SelectedIndex = 0;
                ok = false;
            }

            if (listViewSelectedItems.Items.Count == 0)
            {
                MessageBox.Show("Please select an item from the menu");
                tabBookingDetails.SelectedIndex = 1;
                ok = false;
            }
            

            if (ok)
            {
                errP.Clear();
                drBookingRestaurant.BeginEdit();

                drBookingRestaurant["BookingID"] = labelBookingNo.Text;
                drBookingRestaurant["CustomerNo"] = listBoxCustomerSearch.SelectedValue;
                drBookingRestaurant["DateTimeBookedFor"] = labelDayTimeChosenF.Text;
                drBookingRestaurant["NoOfPeople"] = labelNoOfPeopleF.Text;

                drBookingRestaurant.EndEdit();

                string str;

                int bookingMenuMax = dsCityHotelDB.Tables["BookingMenu"].Rows.Count - 1;
                int bookingDetailMax = dsCityHotelDB.Tables["BookingRestDetails"].Rows.Count - 1;

                for (int i = bookingMenuMax; i > 0; i--)
                {
                    if (dataGridViewGenDisplay.SelectedRows[0].Cells[0].Value.ToString() ==
                        dsCityHotelDB.Tables["BookingMenu"].Rows[i].ItemArray[0].ToString())
                    {
                        drBookingMenu = dsCityHotelDB.Tables["BookingMenu"].Rows[i];
                        drBookingMenu.Delete();
                        daBookingMenu.Update(dsCityHotelDB, "BookingMenu");
                    }
                }

                for (int i = bookingDetailMax; i > 0; i--)
                {
                    if (dataGridViewGenDisplay.SelectedRows[0].Cells[0].Value.ToString() ==
                         dsCityHotelDB.Tables["BookingRestDetails"].Rows[i].ItemArray[0].ToString())
                    {
                        drBookingRestDetails = dsCityHotelDB.Tables["BookingRestDetails"].Rows[i];
                        drBookingRestDetails.Delete();
                        daBookingRestDetails.Update(dsCityHotelDB, "BookingRestDetails");
                    }
                }

                daBookingRestaurant.Update(dsCityHotelDB.Tables["BookingRestaurant"]);
                daBookingRestDetails.Update(dsCityHotelDB.Tables["BookingRestDetails"]);
                daBookingMenu.Update(dsCityHotelDB.Tables["BookingMenu"]);


                for (int i = 0; i < listViewTables.Items.Count; i++)
                {
                    drBookingRestDetails = dsCityHotelDB.Tables["BookingRestDetails"].NewRow();

                    drBookingRestDetails["BookingID"] = Convert.ToInt32(labelBookingNo.Text);
                    drBookingRestDetails["TableID"] = Convert.ToInt32(listViewTables.Items[i].SubItems[0].Text);
                    drBookingRestDetails["StaffID"] = dataGridViewAvailableStaff.SelectedRows[0].Cells[0].Value;
                    drBookingRestDetails["DateTimeBooked"] = labelToDayF.Text;
                    dsCityHotelDB.Tables["BookingRestDetails"].Rows.Add(drBookingRestDetails);

                }

                for (int i = 0; i < listViewSelectedItems.Items.Count; i++)
                {
                    drBookingMenu = dsCityHotelDB.Tables["BookingMenu"].NewRow();

                    drBookingMenu["BookingID"] = Convert.ToInt32(labelBookingNo.Text);
                    drBookingMenu["ItemID"] = Convert.ToInt32(listViewSelectedItems.Items[i].SubItems[0].Text);
                    drBookingMenu["Quantity"] = Convert.ToInt32(listViewSelectedItems.Items[i].SubItems[3].Text);
                    dsCityHotelDB.Tables["BookingMenu"].Rows.Add(drBookingMenu);
                }


                daBookingRestaurant.Update(dsCityHotelDB, "BookingRestaurant");
                daBookingRestaurant.Update(dsCityHotelDB.Tables["BookingRestaurant"]);

                daBookingRestDetails.Update(dsCityHotelDB, "BookingRestDetails");
                daBookingRestDetails.Update(dsCityHotelDB.Tables["BookingRestDetails"]);

                daBookingMenu.Update(dsCityHotelDB, "BookingMenu");
                daBookingMenu.Update(dsCityHotelDB.Tables["BookingMenu"]);

                dsCityHotelDB.Tables["JobRole"].Clear();
                daDisplay.Fill(dsCityHotelDB, "JobRole");

                menuItemsTotlePrice = 0;
                maxPoeple = 0;
                noPeople = 0;

                editModeStat.Text = "Disabled";
                editModeStat.ForeColor = Color.Red;

                editMode = false;
                ignore = true;
                clearAll();
                ignore = false;
            }
        }

        private void dataGridViewGenDisplay_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

            clearAll();
            buttonEdit.Enabled = true;
            buttonDelete.Enabled = true;


            labelBookingNo.Text = dataGridViewGenDisplay.SelectedRows[0].Cells[0].Value.ToString();

            labelStaffMemberF.Text = dataGridViewGenDisplay.SelectedRows[0].Cells[4].Value.ToString() + " " +
                                     dataGridViewGenDisplay.SelectedRows[0].Cells[5].Value.ToString() + " " +
                                     dataGridViewGenDisplay.SelectedRows[0].Cells[6].Value.ToString();

            labelCustomerNoF.Text = dataGridViewGenDisplay.SelectedRows[0].Cells[1].Value + " " +
                                    dataGridViewGenDisplay.SelectedRows[0].Cells[2].Value + " " +
                                    dataGridViewGenDisplay.SelectedRows[0].Cells[3].Value + " ";

            char[] findLetter = dataGridViewGenDisplay.SelectedRows[0].Cells[2].Value.ToString().ToCharArray();
            comboBoxLetterSearch.SelectedItem = findLetter[0].ToString();

            string letterSearch = dataGridViewGenDisplay.SelectedRows[0].Cells[2].Value + " " +
                                    dataGridViewGenDisplay.SelectedRows[0].Cells[3].Value;

            for (int i = 0; i < listBoxCustomerSearch.Items.Count; i++)
            {

                listBoxCustomerSearch.SelectedIndex = i;

                if (listBoxCustomerSearch.SelectedItem.ToString() == letterSearch)
                {
                    break;
                }
            }

            for (int i = 0; i < dataGridViewAvailableStaff.Rows.Count; i++)
            {
                if (dataGridViewAvailableStaff.Rows[i].Cells[0].Value.ToString() == dataGridViewGenDisplay.SelectedRows[0].Cells[4].Value.ToString())
                {
                    dataGridViewAvailableStaff.Rows[i].Selected = true;
                    break;
                }
            }

            ignore = true;

            labelTimeFor.Text = Convert.ToDateTime(dataGridViewGenDisplay.SelectedRows[0].Cells[8].Value.ToString()).ToShortTimeString();
            try
            {
                dateTimePickerFor.Text = Convert.ToDateTime(dataGridViewGenDisplay.SelectedRows[0].Cells[8].Value.ToString()).ToShortDateString();
            }
            catch (ArgumentOutOfRangeException aoore)
            {
                TimeSpan ts = new TimeSpan(1, 0, 0, 0);
                dateTimePickerFor.Text = DateTime.Now.Add(ts).ToShortDateString();
            }

            numericUpDownNoPeople.Value = Convert.ToInt32(dataGridViewGenDisplay.SelectedRows[0].Cells[9].Value.ToString());

            labelMaxPeople.Text = dataGridViewGenDisplay.SelectedRows[0].Cells[11].Value.ToString();


            labelNoOfPeopleF.Text = numericUpDownNoPeople.ToString();
            labelMaxSeatsF.Text = dataGridViewGenDisplay.SelectedRows[0].Cells[11].Value.ToString();


            for (int i = 0; i < dataGridViewGenDisplay.Rows.Count-1; i++)
            {
                int bookingNo = Convert.ToInt32(dataGridViewGenDisplay.SelectedRows[0].Cells[0].Value);
                int bookingNoComp = Convert.ToInt32(dataGridViewGenDisplay.Rows[i].Cells[0].Value);
                int tableNo = Convert.ToInt32(dataGridViewGenDisplay.Rows[i].Cells[10].Value);
                int noSeats = Convert.ToInt32(dataGridViewGenDisplay.Rows[i].Cells[11].Value);

                //if same booking is fround
                if (bookingNo == bookingNoComp)
                {
                    bool newTable = false;

                    for (int j = 0; j < 20; j++)
                    {
                        bool exists = false;

                        //looks for exiting items in the number of bookings
                        for (int k = 0; k <= listViewTables.Items.Count-1; k++)
                        {
                            exists = false;
                            try
                            {
                                if (listViewTables.Items[k].SubItems[0].Text == tableNo.ToString())
                                {
                                    exists = true;
                                    break;
                                }
                            }
                            catch (NullReferenceException nre) { }
                        }

                        //if item dose not exitst in  list and mactch the bookingNo
                        if (!exists)
                        {
                            try
                            {
                                newTable = false;
                                if (listViewTables.FindItemWithText(TableNo.ToString()) != listViewTables.Items[j])
                                {
                                    newTable = true;
                                    break;
                                }
                            }
                            catch (ArgumentOutOfRangeException aoore)
                            {
                                if (j == 0)
                                    newTable = true;
                                break;
                            }
                        }
                    }

                    if (newTable)
                    {                
                        ListViewItem item = new ListViewItem(tableNo.ToString());
                        item.SubItems.Add(noSeats.ToString());

                        listViewTables.Items.Add(item);

                        listViewTableF.Items.Add((ListViewItem)item.Clone());

                        maxPoeple += Convert.ToInt32(noSeats.ToString());

                        labelMaxPeople.Text = maxPoeple.ToString();
                    }
                }
            }

            for (int i = 0; i < dataGridViewGenDisplay.Rows.Count - 1; i++)
            {
                int bookingNo = Convert.ToInt32(dataGridViewGenDisplay.SelectedRows[0].Cells[0].Value);
                int bookingNoComp = Convert.ToInt32(dataGridViewGenDisplay.Rows[i].Cells[0].Value);

                int tableNo = Convert.ToInt32(dataGridViewGenDisplay.SelectedRows[0].Cells[10].Value);
                int tableNoComp = Convert.ToInt32(dataGridViewGenDisplay.Rows[i].Cells[10].Value);

                if (bookingNo == bookingNoComp && tableNo == tableNoComp)
                {
                    int itemID = Convert.ToInt32(dataGridViewGenDisplay.Rows[i].Cells[12].Value);
                    string itemDesc = dataGridViewGenDisplay.Rows[i].Cells[13].Value.ToString();
                    double price = Convert.ToDouble(dataGridViewGenDisplay.Rows[i].Cells[14].Value);
                    int qty = Convert.ToInt32(dataGridViewGenDisplay.Rows[i].Cells[15].Value);


                    int findIndex = itemID - 1;

                    ListViewItem surchItems = listViewSelectedItems.FindItemWithText(itemID.ToString());
                    ListViewItem surchItemsF = listViewMenuItemsF.FindItemWithText(itemDesc.ToString());


                    ListViewItem item = new ListViewItem(itemID.ToString());
                    item.SubItems.Add(itemDesc);
                    item.SubItems.Add(price.ToString());
                    item.SubItems.Add(qty.ToString());
                    listViewSelectedItems.Items.Add(item);

                    ListViewItem itemF = new ListViewItem(itemDesc);
                    itemF.SubItems.Add(qty.ToString());
                    listViewMenuItemsF.Items.Add(itemF);

                    menuItemsTotlePrice += price * qty;
                    labelTotelPrice.Text = menuItemsTotlePrice.ToString();
                }

            }

            editModeStat.Text = "Enabled";
            editModeStat.ForeColor = Color.Green;


            editMode = true;
            tabNav.SelectedIndex = 1;

            ignore = false;
            setTablesFormat();
        }


        private void buttonDelete_Click(object sender, EventArgs e)
        {
            errP.Clear();

            int bookingMenuMax = dsCityHotelDB.Tables["BookingMenu"].Rows.Count - 1;
            int bookingDetailMax = dsCityHotelDB.Tables["BookingRestDetails"].Rows.Count - 1;

            for (int i = bookingMenuMax; i > 0; i--)
            {
                if (dataGridViewGenDisplay.SelectedRows[0].Cells[0].Value.ToString() ==
                    dsCityHotelDB.Tables["BookingMenu"].Rows[i].ItemArray[0].ToString())
                {
                    drBookingMenu = dsCityHotelDB.Tables["BookingMenu"].Rows[i];
                    drBookingMenu.Delete();
                    daBookingMenu.Update(dsCityHotelDB, "BookingMenu");
                }
            }


            for (int i = bookingDetailMax; i > 0; i--)
            {
                if (dataGridViewGenDisplay.SelectedRows[0].Cells[0].Value.ToString() ==
                     dsCityHotelDB.Tables["BookingRestDetails"].Rows[i].ItemArray[0].ToString())
                {
                    drBookingRestDetails = dsCityHotelDB.Tables["BookingRestDetails"].Rows[i];
                    drBookingRestDetails.Delete();
                    daBookingRestDetails.Update(dsCityHotelDB, "BookingRestDetails");
                }
            }

            drBookingRestaurant = dsCityHotelDB.Tables["BookingRestaurant"].Rows.Find(dataGridViewGenDisplay.SelectedRows[0].Cells[0].Value);

            drBookingRestaurant.Delete();


            daBookingRestaurant.Update(dsCityHotelDB, "BookingRestaurant");
            daBookingRestaurant.Update(dsCityHotelDB.Tables["BookingRestaurant"]);

            daBookingRestDetails.Update(dsCityHotelDB, "BookingRestDetails");
            daBookingRestDetails.Update(dsCityHotelDB.Tables["BookingRestDetails"]);

            daBookingMenu.Update(dsCityHotelDB, "BookingMenu");
            daBookingMenu.Update(dsCityHotelDB.Tables["BookingMenu"]);

            dsCityHotelDB.Tables["JobRole"].Clear();
            daDisplay.Fill(dsCityHotelDB, "JobRole");

            menuItemsTotlePrice = 0;
            maxPoeple = 0;
            noPeople = 0;

            editModeStat.Text = "Disabled";
            editModeStat.ForeColor = Color.Red;

            editMode = false;

            ignore = true;
            clearAll();
            ignore = false;
        }


        //when no of people, time and date changes set and adapt the booked tables
        private void numericUpDownNoPeople_ValueChanged(object sender, EventArgs e)
        {
            if (!editMode && listViewTables.Items.Count > 0 && !ignore && tabNav.SelectedIndex == 1)
            {
                if (MessageBox.Show("you currently have selected some tables\n" +
                "changeing this field will complectly clear all\n" +
                "current selected tables, are you sure you want to\n" +
                "execute this action?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
                == System.Windows.Forms.DialogResult.Yes)
                {
                    setTablesFormat();

                    int max = listViewTables.Items.Count;
                    for (int i = 0; i < max; i++)
                    {
                        listViewTables.Items[0].Selected = true;
                        setTables();
                    }

                    reSetTables();

                    maxPoeple = 0;
                    labelMaxPeople.Text = "- - -";
                    labelMaxSeatsF.Text = "- - -";


                    noPeople = 0;
                    labelNoOfPeople.Text = "- - -";
                    labelNoOfPeopleF.Text = "- - -";

                }

            }
            else if (!editMode && listViewTables.Items.Count == 0 && !ignore && tabNav.SelectedIndex == 1)
            {
                reSetTables();
            }


            if (editMode && listViewTables.Items.Count > 0 && !ignore && tabNav.SelectedIndex == 1)
            {
                if (MessageBox.Show("you are currently editing a recored\n" +
                    "changeing this field will complectly clear all\n" +
                    "current selected tables, are you sure you want to\n" +
                    "execute this action?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
                    == System.Windows.Forms.DialogResult.Yes)
                {
                    setTablesFormat();

                    int max = listViewTables.Items.Count;
                    for (int i = 0; i < max; i++)
                    {
                        listViewTables.Items[0].Selected = true;
                        setTables();
                    }

                    reSetTables();

                    maxPoeple = 0;
                    labelMaxPeople.Text = "- - -";
                    labelMaxSeatsF.Text = "- - -";


                    noPeople = 0;
                    labelNoOfPeople.Text = "- - -";
                    labelNoOfPeopleF.Text = "- - -";

                }
            }
            else if (editMode && listViewTables.Items.Count == 0 && !ignore && tabNav.SelectedIndex == 1)
            {
                reSetTables();
            }


        }



        private void listViewTimeFor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!editMode && listViewTables.Items.Count > 0 && !ignore && tabNav.SelectedIndex == 1)
            {
                if (MessageBox.Show("you currently have selected some tables\n" +
                "changeing this field will complectly clear all\n" +
                "current selected tables, are you sure you want to\n" +
                "execute this action?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
                == System.Windows.Forms.DialogResult.Yes)
                {
                    setTablesFormat();

                    int max = listViewTables.Items.Count;
                    for (int i = 0; i < max; i++)
                    {
                        listViewTables.Items[0].Selected = true;
                        setTables();
                    }

                    reSetTables();

                    maxPoeple = 0;
                    labelMaxPeople.Text = "- - -";
                    labelMaxSeatsF.Text = "- - -";


                    noPeople = 0;
                    labelNoOfPeople.Text = "- - -";
                    labelNoOfPeopleF.Text = "- - -";

                }

            }
            else if (!editMode && listViewTables.Items.Count == 0 && !ignore && tabNav.SelectedIndex == 1)
            {
                reSetTables();
            }


            if (editMode && listViewTables.Items.Count > 0 && !ignore && tabNav.SelectedIndex == 1)
            {
                if (MessageBox.Show("you are currently editing a recored\n" +
                    "changeing this field will complectly clear all\n" +
                    "current selected tables, are you sure you want to\n" +
                    "execute this action?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
                    == System.Windows.Forms.DialogResult.Yes)
                {
                    setTablesFormat();

                    int max = listViewTables.Items.Count;
                    for (int i = 0; i < max; i++)
                    {
                        listViewTables.Items[0].Selected = true;
                        setTables();
                    }

                    reSetTables();

                    maxPoeple = 0;
                    labelMaxPeople.Text = "- - -";
                    labelMaxSeatsF.Text = "- - -";


                    noPeople = 0;
                    labelNoOfPeople.Text = "- - -";
                    labelNoOfPeopleF.Text = "- - -";

                }
            }
            else if (editMode && listViewTables.Items.Count == 0 && !ignore && tabNav.SelectedIndex == 1)
            {
                reSetTables();
            }




        }


        private void dateTimePickerFor_ValueChanged(object sender, EventArgs e)
        {
            if (!editMode && listViewTables.Items.Count > 0 && !ignore && tabNav.SelectedIndex == 1)
            {
                if (MessageBox.Show("you currently have selected some tables\n" +
                "changeing this field will complectly clear all\n" +
                "current selected tables, are you sure you want to\n" +
                "execute this action?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
                == System.Windows.Forms.DialogResult.Yes)
                {
                    setTablesFormat();

                    int max = listViewTables.Items.Count;
                    for (int i = 0; i < max; i++)
                    {
                        listViewTables.Items[0].Selected = true;
                        setTables();
                    }

                    reSetTables();

                    maxPoeple = 0;
                    labelMaxPeople.Text = "- - -";
                    labelMaxSeatsF.Text = "- - -";


                    noPeople = 0;
                    labelNoOfPeople.Text = "- - -";
                    labelNoOfPeopleF.Text = "- - -";

                }

            }
            else if (!editMode && listViewTables.Items.Count == 0 && !ignore && tabNav.SelectedIndex == 1)
            {
                reSetTables();
            }


            if (editMode && listViewTables.Items.Count > 0 && !ignore && tabNav.SelectedIndex == 1)
            {
                if (MessageBox.Show("you are currently editing a recored\n" +
                    "changeing this field will complectly clear all\n" +
                    "current selected tables, are you sure you want to\n" +
                    "execute this action?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
                    == System.Windows.Forms.DialogResult.Yes)
                {
                    setTablesFormat();

                    int max = listViewTables.Items.Count;
                    for (int i = 0; i < max; i++)
                    {
                        listViewTables.Items[0].Selected = true;
                        setTables();
                    }

                    reSetTables();

                    maxPoeple = 0;
                    labelMaxPeople.Text = "- - -";
                    labelMaxSeatsF.Text = "- - -";


                    noPeople = 0;
                    labelNoOfPeople.Text = "- - -";
                    labelNoOfPeopleF.Text = "- - -";

                }
            }
            else if (editMode && listViewTables.Items.Count == 0 && !ignore && tabNav.SelectedIndex == 1)
            {
                reSetTables();
            }

        }


        private void setTables()
        {
            if (listViewTables.SelectedItems.Count != 0)
            {

                ListViewItem item = listViewTables.SelectedItems[0];

                for (int i = 0; i < 20; i++)
                {

                    if (listViewTables.SelectedItems[0].Text == btn[i].Text)
                    {
                        btn[i].Enabled = true;
                        btn[i].BackColor = Color.DarkGray;

                        dsCityHotelDB.Tables["TableDetails"].Clear();
                        cmbTable.Parameters["@No"].Value = Convert.ToInt32(btn[i].Text);
                        daTable.Fill(dsCityHotelDB, "TableDetails");

                        drTable = dsCityHotelDB.Tables["TableDetails"].Rows[0];

                        labelMaxPeople.Text = (int.Parse(labelMaxPeople.Text) - int.Parse(drTable["NoSeats"].ToString())).ToString();
                        maxPoeple -= int.Parse(drTable["NoSeats"].ToString());

                        break;
                    }
                }

                if (labelMaxPeople.Text == "0")
                {
                    labelMaxPeople.Text = "- - -";
                    labelMaxSeatsF.Text = "- - - ";
                }

                ListViewItem itemF = listViewTableF.FindItemWithText(listViewTables.SelectedItems[0].Text);

                listViewTables.Items.Remove(item);
                listViewTableF.Items.Remove(itemF);
            }

        }

        //sets the booked tables
        private void setTablesFormat()
        {
            bool fail = false;

            try
            {

                //if all the no of people and DateTime is set


                try
                {
                    DataRow dr;

                    bool[] booked = new bool[20];

                    for (int i = 0; i < booked.Length; i++)
                    {
                        booked[i] = false;
                    }

                    noPeople = Convert.ToInt32(numericUpDownNoPeople.Value.ToString());

                    for (int i = 0; i < dsCityHotelDB.Tables["JobRole"].Rows.Count; i++)
                    {
                        dr = dsCityHotelDB.Tables["JobRole"].Rows[i];
                        if (!datecomp(Convert.ToDateTime(dr["DateTimeBookedFor"].ToString()), Convert.ToDateTime(dateTimePickerFor.Text +
                            " " + labelTimeFor.Text + ":00"), noPeople))
                        {
                            fail = true;
                            booked[Convert.ToInt32(dr["TableID"].ToString()) - 1] = true;
                        }
                    }

                    for (int i = 0; i < 20; i++)
                    {
                        if (!booked[i])
                        {
                            btn[i].Enabled = true;
                            btn[i].BackColor = Color.DarkGray;
                        }
                        else
                        {
                            btn[i].Enabled = false;
                            btn[i].BackColor = Color.Purple;
                        }

                        try
                        {
                            if (btn[i].Text == listViewTables.Items[listViewTables.Items.IndexOfKey(btn[i].Text)].ToString())
                            {
                                btn[i].Enabled = false;
                                btn[i].BackColor = Color.Red;
                            }

                        }
                        catch (NullReferenceException nre) { }

                        catch (ArgumentOutOfRangeException aoore) { }
                    }
                }
                catch (ArgumentOutOfRangeException aoore) { }

                catch (NullReferenceException nre) { }
            }
            catch (FormatException fe)
            {
                if (!ignore)
                {
                    if (tabNav.SelectedIndex == 0 && !fail)
                    {
                        MessageBox.Show("Plese enter a date and time!");
                        ignore = true;
                        tabNav.SelectedIndex = 0;
                        ignore = false;
                        fail = false;
                    }
                }
            }

        }

        private void reSetTables()
        {

            bool fail = false;

            try
            {

                //if all the no of people and DateTime is set


                try
                {
                    DataRow dr;

                    bool[] booked = new bool[20];

                    for (int i = 0; i < booked.Length; i++)
                    {
                        booked[i] = false;
                    }

                    noPeople = Convert.ToInt32(numericUpDownNoPeople.Value.ToString());

                    for (int i = 0; i < dsCityHotelDB.Tables["JobRole"].Rows.Count; i++)
                    {
                        dr = dsCityHotelDB.Tables["JobRole"].Rows[i];
                        if (!datecomp(Convert.ToDateTime(dr["DateTimeBookedFor"].ToString()), Convert.ToDateTime(dateTimePickerFor.Text +
                            " " + labelTimeFor.Text + ":00"), noPeople))
                        {
                            fail = true;
                            booked[Convert.ToInt32(dr["TableID"].ToString()) - 1] = true;
                        }
                    }

                    for (int i = 0; i < 20; i++)
                    {

                        if (!booked[i])
                        {
                            btn[i].Enabled = true;
                            btn[i].BackColor = Color.DarkGray;
                        }
                        else
                        {
                            btn[i].Enabled = false;
                            btn[i].BackColor = Color.Purple;

                        }

                        try
                        {

                            if (btn[i].Text == listViewTables.Items[listViewTables.Items.IndexOfKey(btn[i].Text)].ToString())
                            {
                                btn[i].Enabled = false;
                                btn[i].BackColor = Color.Red;
                            }

                        }
                        catch (NullReferenceException nre) { }

                        catch (ArgumentOutOfRangeException aoore) { }


                    }
                }
                catch (ArgumentOutOfRangeException aoore) { }

                catch (NullReferenceException nre) { }


                for (int i = 0; i < dataGridViewGenDisplay.Rows.Count - 1; i++)
                {
                    int bookingNo = Convert.ToInt32(dataGridViewGenDisplay.SelectedRows[0].Cells[0].Value);
                    int bookingNoComp = Convert.ToInt32(dataGridViewGenDisplay.Rows[i].Cells[0].Value);
                    int tableNo = Convert.ToInt32(dataGridViewGenDisplay.Rows[i].Cells[10].Value);
                    int noSeats = Convert.ToInt32(dataGridViewGenDisplay.Rows[i].Cells[11].Value);

                    //if same booking is fround
                    if (bookingNo == bookingNoComp)
                    {
                        bool newTable = false;

                        for (int j = 0; j < 20; j++)
                        {
                            bool exists = false;

                            //looks for exiting items in the number of bookings
                            for (int k = 0; k <= listViewTables.Items.Count - 1; k++)
                            {
                                exists = false;
                                try
                                {
                                    if (listViewTables.Items[k].SubItems[0].Text == tableNo.ToString())
                                    {
                                        exists = true;
                                        break;
                                    }
                                }
                                catch (NullReferenceException nre) { }
                            }

                            //if item dose not exitst in  list and mactch the bookingNo
                            if (!exists)
                            {
                                try
                                {
                                    newTable = false;
                                    if (listViewTables.FindItemWithText(TableNo.ToString()) != listViewTables.Items[j])
                                    {
                                        newTable = true;
                                        break;
                                    }
                                }
                                catch (ArgumentOutOfRangeException aoore)
                                {
                                    if (j == 0)
                                        newTable = true;
                                    break;
                                }
                            }
                        }

                        if (newTable)
                        {
                            ListViewItem item = new ListViewItem(tableNo.ToString());
                            item.SubItems.Add(noSeats.ToString());

                            listViewTables.Items.Add(item);

                            listViewTableF.Items.Add((ListViewItem)item.Clone());

                            maxPoeple += Convert.ToInt32(noSeats.ToString());

                            labelMaxPeople.Text = maxPoeple.ToString();
                        }
                    }
                }


                int max = listViewTables.Items.Count;
                for (int i = 0; i < max; i++)
                {
                    listViewTables.Items[0].Selected = true;
                    setTables();
                }

                for (int k = 0; k < dataGridViewGenDisplay.Rows.Count - 1; k++)
                {
                    int bookingID = Convert.ToInt32(dataGridViewGenDisplay.Rows[k].Cells[0].Value.ToString());
                    if (dataGridViewGenDisplay.Rows[k].Cells[0].Value.ToString() ==
                        dataGridViewGenDisplay.SelectedRows[0].Cells[0].Value.ToString())
                    {
                        for (int i = 0; i < dataGridViewGenDisplay.Rows.Count - 1; i++)
                        {
                            for (int j = 0; j < 20; j++)
                            {
                                if (btn[j].Text == dataGridViewGenDisplay.Rows[i].Cells[10].Value.ToString() &&
                                    bookingID.ToString() == dataGridViewGenDisplay.Rows[i].Cells[0].Value.ToString())
                                {
                                    btn[j].Enabled = true;
                                    btn[j].BackColor = Color.DarkGray;
                                }
                            }
                        }
                    }
                }

            }
            catch (FormatException fe)
            {
                if (tabNav.SelectedIndex == 0 && !fail)
                {
                    MessageBox.Show("Plese enter a date and time!");
                    ignore = true;
                    tabNav.SelectedIndex = 0;
                    ignore = false;
                    fail = false;
                }
            }


        }

        private void buttonReport_Click(object sender, EventArgs e)
        {
            AllBookingsReportForm frm = new AllBookingsReportForm();            
            frm.Show();
        }
    }
}