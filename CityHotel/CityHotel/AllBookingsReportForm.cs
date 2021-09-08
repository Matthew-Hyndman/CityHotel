using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using MyVal;

namespace CityHotel
{
    public partial class AllBookingsReportForm : Form
    {
        public AllBookingsReportForm()
        {
            InitializeComponent();
        }

        DataSet dsCityHotelDB = new DataSet();
        SqlConnection conn;
        string connStr;

        SqlDataAdapter daBookingRest;

        string bookingRestSQL;

        private void AllBookingsReportForm_Load(object sender, EventArgs e)
        {
            connStr = @"Data Source = .; Initial Catalog = CityHotel; Integrated Security = true";
            conn = new SqlConnection(connStr);

            bookingRestSQL = @"select br.BookingID from BookingRestaurant as br";

            daBookingRest = new SqlDataAdapter(bookingRestSQL, conn);
            daBookingRest.Fill(dsCityHotelDB, "BookingRestaurant");
            comboBoxBooking.DataSource = dsCityHotelDB.Tables["BookingRestaurant"];
            comboBoxBooking.DisplayMember = "BookingID";
            comboBoxBooking.ValueMember = "BookingID";


        }

        private void buttonBooking_Click(object sender, EventArgs e)
        {

            DataTable dtBookings1 = new DataTable();

            string reSQL1 = @"select br.BookingID, (c.Title + ' ' + c.Forename + ' ' + c.Surname) as customerName, td.TableID, td.NoSeats, br.NoOfPeople, br.DateTimeBookedFor from BookingRestaurant as br
                              join Customer as c on c.CustomerNo = br.CustomerNo
                              join BookingRestDetails as brd on brd.BookingID = br.BookingID
                              join TableDetails as td on td.TableID = brd.TableID 
                              join Staff as s on s.StaffID = brd.StaffID
                              where br.bookingID = " + comboBoxBooking.SelectedValue;

            SqlDataAdapter daBkID1 = new SqlDataAdapter(reSQL1, conn);
            daBkID1.Fill(dtBookings1);

            CrystalReportAllBookings cryRep1 = new CrystalReportAllBookings();
            cryRep1.SetDataSource(dtBookings1);

            crystalReportViewerTables.ReportSource = cryRep1;


            DataTable dtBookings2 = new DataTable();

            string reSQL2 = @"select br.BookingID, br.CustomerNo, c.Forename, c.Surname, brd.StaffID, s.Forename, s.Surname, brd.DateTimeBooked,
                            br.DateTimeBookedFor, br.NoOfPeople, m.ItemID, m.MenuItemDesc, m.Price, bm.Quantity from BookingRestaurant as br
                            join Customer as c on c.CustomerNo = br.CustomerNo
                            join BookingRestDetails as brd on brd.BookingID = br.BookingID
                            join BookingMenu as bm on bm.BookingID = br.BookingID
                            join TableDetails as td on td.TableID = brd.TableID 
                            join Staff as s on s.StaffID = brd.StaffID
                            join Menu as m on m.ItemID = bm.ItemID
                            where br.BookingID = " + comboBoxBooking.SelectedValue;

            SqlDataAdapter daBkID2 = new SqlDataAdapter(reSQL2, conn);
            daBkID2.Fill(dtBookings2);

            CrystalReport1 cryRep2 = new CrystalReport1();
            cryRep2.SetDataSource(dtBookings2);

            crystalReportViewerMenuItems.ReportSource = cryRep2;


        }
    }
}
