CREATE DATABASE CityHotel
DROP DATABASE CityHotel
use CityHotel

CREATE TABLE JobRole
(
	JobRoleID tinyint NOT NULL,
	JobDesc varchar(30) NOT NULL,
	HourlyRate money NOT NULL,

	CONSTRAINT pkJobRoleID PRIMARY KEY (JobRoleID)
)


CREATE TABLE Staff
(
	StaffID varchar(7) NOT NULL,
	Title varchar(4) NOT NULL,
	Surname varchar(15) NOT NULL,
	Forename varchar(15) NOT NULL,
	Street varchar(20) NOT NULL,
	Town varchar(15) NOT NULL,
	County varchar(15) NOT NULL,
	PostCode varchar(8) NOT NULL,
	TelNo varchar(11) NOT NULL,
	JobRoleID tinyint NOT NULL,

	CONSTRAINT pkStaffID PRIMARY KEY (StaffID),
	CONSTRAINT fkJobRoleID FOREIGN KEY (JobRoleID) REFERENCES JobRole(JobRoleID),
	CONSTRAINT chkTitleStaff CHECK (Title IN ('Mr','Mrs','Miss','Ms')),
	CONSTRAINT chkPostCodeStaff CHECK (PostCode LIKE '[A-Z][A-Z][0-9][0-9] [0-9][A-Z][A-Z]'),
	CONSTRAINT chkTelNoStaff CHECK (TelNo LIKE REPLICATE('[0-9]', 11))
)

CREATE TABLE customerType
(
	CustTypeID tinyint NOT NULL,
	CustTypeDescription varchar(8) NOT NULL,

	CONSTRAINT pkCustType PRIMARY KEY (CustTypeID),
	CONSTRAINT unqDescription UNIQUE (CustTypeDescription)
)

CREATE TABLE Customer
(
	CustomerNo varchar(7) NOT NULL,
	Title varchar(4) NOT NULL,
	Surname varchar(15) NOT NULL,
	Forename varchar(15) NOT NULL,
	Street varchar(20) NOT NULL,
	Town varchar(15) NOT NULL,
	County varchar(15) NOT NULL,
	PostCode varchar(8) NOT NULL,
	TelNo varchar(11) NOT NULL,
	CustType tinyint NOT NULL,

	CONSTRAINT pkCustomerNo PRIMARY KEY (CustomerNo),
	CONSTRAINT fkCustType FOREIGN KEY (CustType) REFERENCES customerType(CustTypeID),
	CONSTRAINT chkTitle CHECK (Title IN ('Mr','Mrs','Miss','Ms')),
	CONSTRAINT chkPostCode CHECK (PostCode LIKE '[A-Z][A-Z][0-9][0-9] [0-9][A-Z][A-Z]'),
	CONSTRAINT chkTelNo CHECK (TelNo LIKE REPLICATE('[0-9]', 11))
)

CREATE TABLE RoomBooking
(
	RoomBookingID int NOT NULL,
	CustomerNo varchar(7) NOT NULL,
	StartDate date NOT NULL,
	NoDays tinyint NOT NULL,

	CONSTRAINT pkRoomBookingID PRIMARY KEY (RoomBookingID), 
	CONSTRAINT fkCustomerNo FOREIGN KEY (CustomerNo) REFERENCES Customer(CustomerNo),
	--CONSTRAINT chkStartDate CHECK (StartDate >= getDate()),
	CONSTRAINT chkNoDays CHECK (NoDays > 0) 
)

CREATE TABLE ItemType
(
	ItemTypeID tinyint NOT NULL,
	ItemDesc varchar(25) NOT NULL,

	CONSTRAINT pkItemTypeID PRIMARY KEY (ItemTypeID),
	CONSTRAINT unqItemDesc UNIQUE (ItemDesc)
)

CREATE TABLE Menu
(
	ItemID int NOT NULL,
	MenuItemDesc varchar(40) NOT NULL,
	Price money NOT NULL,
	ItemTypeID tinyint NOT NULL,

	CONSTRAINT pkItemID PRIMARY KEY (ItemID),
	CONSTRAINT fkItemTypeID FOREIGN KEY (ItemTypeID) REFERENCES ItemType(ItemTypeID),
	CONSTRAINT unqMenuItemDesc UNIQUE (MenuItemDesc)
)

CREATE TABLE RoomService
(
	RoomBookingID int NOT NULL,
	ItemID int NOT NULL,
	Quantity tinyint NOT NULL,

	CONSTRAINT pkRoomService PRIMARY KEY (RoomBookingID, ItemID),
	CONSTRAINT fkRoomServiceBookingID FOREIGN KEY (RoomBookingID) REFERENCES RoomBooking(RoomBookingID),
	CONSTRAINT fkItemID FOREIGN KEY (ItemID) REFERENCES Menu(ItemID)
)

CREATE TABLE RoomType
(
	RoomTypeID tinyint NOT NULL,
	RoomTypeDesc varchar(6) NOT NULL, -- Single, Double, Triple, Family - Are these correct? If not, increase the size of the varchar. Are there triple rooms? 
	NoBeds tinyint NOT NULL,
	PricePerNight money NOT NULL,

	CONSTRAINT pkRoomTypeID PRIMARY KEY (RoomTypeID),
	CONSTRAINT unqRoomTypeDesc UNIQUE (RoomTypeDesc)
)

CREATE TABLE Room
(
	RoomID varchar(5) NOT NULL,
	RoomTypeID tinyint NOT NULL,
	RoomFloor tinyint NOT NULL,

	CONSTRAINT pkRoomID PRIMARY KEY (RoomID),
	CONSTRAINT fkRoomTypeID FOREIGN KEY (RoomTypeID) REFERENCES RoomType(RoomTypeID)
)

CREATE TABLE RoomBookingDetails
(
	RoomBookingID int NOT NULL,
	RoomID varchar(5) NOT NULL,
	StaffID varchar(7) NOT NULL,
	DateTimeBooked datetime NOT NULL,

	CONSTRAINT pkRoomBookingDetails PRIMARY KEY (RoomBookingID, RoomID),
	CONSTRAINT fkRoomBookingID FOREIGN KEY (RoomBookingID) REFERENCES RoomBooking(RoomBookingID),
	CONSTRAINT fkRoomID FOREIGN KEY (RoomID) REFERENCES Room(RoomID),
	CONSTRAINT fkStaffIDRoom FOREIGN KEY (StaffID) REFERENCES Staff(StaffID),
	--CONSTRAINT chkDateTimeBooked CHECK (DateTimeBooked <= getDate())
)

--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE LoginType
(
	LogTypeID bit NOT NULL,
	LogDesc varchar(20) NOT NULL,

	CONSTRAINT pkLogTypeID PRIMARY KEY (LogTypeID),
	CONSTRAINT unqLogDesc UNIQUE (LogDesc)
)

CREATE TABLE Logins
(
	LoginID int NOT NULL,
	LogUsername varchar(20) NOT NULL,
	LogPassword varchar(20) NOT NULL,
	StaffID varchar(7) NOT NULL,
	LogTypeID bit NOT NULL,

	CONSTRAINT pkLoginID PRIMARY KEY (LoginID),
	CONSTRAINT fkStaffIDLogin FOREIGN KEY (StaffID) REFERENCES Staff(StaffID),
	CONSTRAINT fkLogTypeID FOREIGN KEY (LogTypeID) REFERENCES LoginType(LogTypeID),
	CONSTRAINT unqUsername UNIQUE (LogUsername),
	CONSTRAINT unqPassword UNIQUE (LogPassword)
)

CREATE TABLE BookingRestaurant
(
	BookingID int NOT NULL,
	CustomerNo varchar(7) NOT NULL,
	DateTimeBookedFor datetime NOT NULL,
	NoOfPeople tinyint NOT NULL,

	CONSTRAINT pkBookingID PRIMARY KEY (BookingID),
	CONSTRAINT fkCustomerNoBR FOREIGN KEY (CustomerNo) REFERENCES Customer(CustomerNo),
	--CONSTRAINT chkDateTimeBookedFor CHECK (DateTimeBookedFor >= getDate()) -- Add check to validate time?
)

CREATE TABLE TableDetails
(
	TableID tinyint NOT NULL,
	NoSeats tinyint NOT NULL

	CONSTRAINT pkTableID PRIMARY KEY (TableID)
)

CREATE TABLE BookingMenu
(
	BookingID int NOT NULL,
	ItemID int NOT NULL,
	Quantity tinyint NOT NULL,

	CONSTRAINT pkBookingMenu PRIMARY KEY (BookingID, ItemID),
	CONSTRAINT fkBookingIDBM FOREIGN KEY (BookingID) REFERENCES BookingRestaurant(BookingID),
	CONSTRAINT fkItemIDBM FOREIGN KEY (ItemID) REFERENCES Menu(ItemID)
)

CREATE TABLE BookingRestDetails
(
	BookingID int NOT NULL,
	TableID tinyint NOT NULL,
	StaffID varchar(7) NOT NULL,
	DateTimeBooked datetime NOT NULL,

	CONSTRAINT pkBookingRestDetails PRIMARY KEY (BookingID, TableID),
	CONSTRAINT fkTableID FOREIGN KEY (TableID) REFERENCES TableDetails(TableID),
	CONSTRAINT fkBookingID FOREIGN KEY (BookingID) REFERENCES BookingRestaurant(BookingID),
	CONSTRAINT fkStaffIDBRD FOREIGN KEY (StaffID) REFERENCES Staff(StaffID),
	--CONSTRAINT chkDateTimeBookedBRD CHECK (DateTimeBooked <= getDate())
)

--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE EventType
(
	EventTypeID tinyint NOT NULL,
	EventTypeDescription varchar(20) NOT NULL,
	--Age--?

	CONSTRAINT pkEventType PRIMARY KEY (EventTypeID),
	CONSTRAINT unqEventTypeDescription UNIQUE (EventTypeDescription)
)

CREATE TABLE _Event
(
	EventID int NOT NULL,
	EventDesc varchar(30) NOT NULL,
	EventPrice money NOT NULL,
	EventTypeID tinyint NOT NULL,
	EventBookingDateFor datetime NOT NULL, 
	NoTickets tinyint NOT NULL,
	HotelEventPrice money NOT NULL

	CONSTRAINT pkEventID PRIMARY KEY (EventID), 
	CONSTRAINT fkEventTypeID FOREIGN KEY (EventTypeID) REFERENCES EventType(EventTypeID),
	CONSTRAINT unqEventDesc UNIQUE (EventDesc),
	--CONSTRAINT chkEventBookingDateFor CHECK (EventBookingDateFor >= getDate())
)

CREATE TABLE EventBooking
(
	EventBookingID int NOT NULL,
	CustomerNo varchar(7) NOT NULL,

	CONSTRAINT pkEventBookingID PRIMARY KEY (EventBookingID), 
	CONSTRAINT fkCustomerNoEvent FOREIGN KEY (CustomerNo) REFERENCES Customer(CustomerNo),
)


CREATE TABLE EventBookingDets
(
	EventBookingID int NOT NULL,
	EventID int NOT NULL, 
	Quantity tinyint NOT NULL,
	StaffID varchar(7) NOT NULL,
	EventBookingDate datetime NOT NULL,

	CONSTRAINT pkEventBookingDets PRIMARY KEY (EventBookingID, EventID),
	CONSTRAINT fkEventBookingID FOREIGN KEY (EventBookingID) REFERENCES EventBooking(EventBookingID),
	CONSTRAINT fkEventID FOREIGN KEY (EventID) REFERENCES _Event(EventID),
	CONSTRAINT fkStaffIDEvent FOREIGN KEY (StaffID) REFERENCES Staff(StaffID),
	--CONSTRAINT chkEventBookingDate CHECK (EventBookingDate >= getDate())
)

CREATE TABLE ConferenceRoom
(
	ConferenceRoomID varchar(3) NOT NULL,
	ConferenceRoomDesc varchar(30) NOT NULL,
	MaxCapacity tinyint NOT NULL,
	Price money NOT NULL,
	InbuiltScreen bit NOT NULL,
	InbuiltProjector bit NOT NULL,
	InbuiltPA bit NOT NULL,
	Wifi bit NOT NULL,
	AirConditioned bit NOT NULL,
	SuppliesForDelegates bit NOT NULL


	CONSTRAINT pkConferenceRoomID PRIMARY KEY (ConferenceRoomID), 
	CONSTRAINT unqConferenceRoomDesc UNIQUE (ConferenceRoomDesc)
)

CREATE TABLE ConferenceBooking
(
	ConferenceBookingID int NOT NULL,
	CustomerNo varchar(7) NOT NULL,
	ConferenceDateFor datetime NOT NULL,

	CONSTRAINT pkConferenceBookingID PRIMARY KEY (ConferenceBookingID), 
	CONSTRAINT fkCustomerNoConference FOREIGN KEY (CustomerNo) REFERENCES Customer(CustomerNo),
	--CONSTRAINT chkConferenceDateFor CHECK (ConferenceDateFor >= getDate())
)

CREATE TABLE ConferenceBookingDetails
(
	ConferenceBookingID int NOT NULL,
	ConferenceRoomID varchar(3) NOT NULL,
	EventTypeID tinyint NOT NULL,
	ConferenceBookingDate datetime NOT NULL,
	StaffID varchar(7) NOT NULL,

	CONSTRAINT pkConferenceBookingDetails PRIMARY KEY (ConferenceBookingID, ConferenceRoomID, EventTypeID),
	CONSTRAINT fkConferenceBookingID FOREIGN KEY (ConferenceBookingID) REFERENCES ConferenceBooking(ConferenceBookingID),
	CONSTRAINT fkConferenceRoomID FOREIGN KEY (ConferenceRoomID) REFERENCES ConferenceRoom(ConferenceRoomID),
	CONSTRAINT fkEventTypeIDCB FOREIGN KEY (EventTypeID) REFERENCES EventType(EventTypeID),
	CONSTRAINT fkStaffIDConference FOREIGN KEY (StaffID) REFERENCES Staff(StaffID),
	--CONSTRAINT chkConferenceDate CHECK (ConferenceBookingDate <= getDate())
)


--JobDesc varchar(30) NOT NULL,
	--HourlyRate money NOT NULL,
INSERT INTO JobRole (JobRoleID, JobDesc, HourlyRate) VALUES
(0, 'Security', 8.80),
(1, 'Registration', 8.50),
(2, 'Bartender', 8.40),
(3, 'Waiter', 7.80),
(4, 'Room Service', 7.80),
(5, 'Manager', 9.10);

--	StaffID int NOT NULL,
--	Title varchar(4) NOT NULL,
--	Surname varchar(15) NOT NULL,
--	Forename varchar(15) NOT NULL,
--	Street varchar(20) NOT NULL,
--	Town varchar(15) NOT NULL,
--	County varchar(15) NOT NULL,
--	PostCode varchar(8) NOT NULL,
--	TelNo varchar(11) NOT NULL,
--	JobRoleID int NOT NULL,
INSERT INTO Staff (StaffID, Title, Surname, Forename, Street, Town, County, PostCode, TelNo, JobRoleID) VALUES
('STA1000', 'Mr', 'Morrison', 'Brenden', '159 Main Street','Eglinton','Co Derry','BT47 4TB', '02871261548', '5'),   
('STA1001', 'Miss', 'Rankin', 'Kathy', '56 Greenhaven','Drumahoe','Co Derry','BT48 3SY', '02871346562', '5'),  
('STA1002', 'Mr', 'Carlin', 'Patrick', '98 Larch Road','Greysteel','Co Derry','BT47 4TB', '02871249876', '5'), 
('STA1003', 'Mr', 'Cunning', 'Tom', '115 NewLine Road','Newbuildings','Co Derry','BT47 4TB', '02871375921', '0'), 
('STA1004', 'Mr', 'Wilson', 'Robert', '45 Hollyfoot Hill','Ballykelly','Co Derry','BT47 4TB', '07561020748', '0'), 
('STA1005', 'Mr', 'Lynch', 'Conan', '126 Dungiven Road','Derry','Co Derry','BT47 4TB', '02871361782', '0'),
('STA1006', 'Mrs', 'Collins', 'Joanne', '62 Limavady Road','Derry','Co Derry','BT47 4TB', '02871267183', '0'),
('STA1007', 'Mr', 'Gallagher', 'Adam', '76 Church Brae','Derry','Co Derry','BT47 4TB', '02871348527', '0'),
('STA1008', 'Mrs', 'Costello', 'Niamh', '34 Strand Road','Derry','Co Derry','BT47 4TB', '07865255486', '0'), 
('STA1009', 'Mr', 'Doherty', 'Kevin', '624 Glenshane Road','Claudy','Co Derry','BT47 4TB', '07759631458', '1'),
('STA1010', 'Miss', 'Heaney', 'Amanda', '22 Dungiven Road','Limavady','Co Derry','BT47 4TB', '07566584748', '1'), 
('STA1011', 'Mr', 'Evans', 'Rory', '70 Rosewood Avenue','Derry','Co Derry','BT47 4TB', '02871346582', '1'),
('STA1012', 'Mr', 'Coyle', 'Stephen', '32 Culmore Road','Derry','Co Derry','BT47 4TB', '07756914238', '1'), 
('STA1013', 'Mr', 'Murray', 'Mark', '4b Enfield Street','Derry','Co Derry','BT47 4TB', '07925688149', '1'), 
('STA1014', 'Mr', 'Donaghy', 'Martin', '582 Duncrun Road','Limavady','Co Derry','BT47 4TB', '02877741596', '1'), 
('STA1015', 'Miss', 'Jenkins', 'Hannah', '94 Carrickbeg Avenue','Eglinton','Co Derry','BT47 4TB', '02871362894', '2'), 
('STA1016', 'Mrs', 'Matthews', 'Louise', '241 Clagan Road','Claudy','Co Derry','BT47 4TB', '07774698315', '2'), 
('STA1017', 'Miss', 'Cartin', 'Charlene', '200 Rallagh Road','Dungiven','Co Derry','BT47 4TB', '07892579143', '2'), 
('STA1018', 'Mr', 'Holmes', 'Kieran', '34 Spencer Road','Waterside','Co Derry','BT47 4TB', '07985020748', '2'),
('STA1019', 'Mr', 'Morrison', 'John', '16 Main Street','Eglinton','Co Derry','BT47 4TB', '02871261548', '2'),   
('STA1020', 'Miss', 'Rankin', 'Catherine', '22 Greenhaven','Drumahoe','Co Derry','BT48 3SY', '02871346562', '2'),  
('STA1021', 'Mr', 'Carlin', 'Hubert', '42 Larch Road','Greysteel','Co Derry','BT47 4TB', '02871249876', '2'), 

('STA1022', 'Mr', 'Wilson', 'Peter', '145 Hollyfoot Hill','Ballykelly','Co Derry','BT47 4TB', '07561020748', '2'), 
('STA1023', 'Mr', 'Lynch', 'Conor', '12 Dungiven Road','Derry','Co Derry','BT47 4TB', '02871361782', '2'),
('STA1024', 'Mrs', 'Collins', 'Anne', '32 Limavady Road','Derry','Co Derry','BT47 4TB', '02871267183', '2'),
('STA1025', 'Mr', 'Gallagher', 'Anthony', '46 Church Brae','Derry','Co Derry','BT47 4TB', '02871348527', '2'),
('STA1026', 'Mrs', 'Costello', 'Naomi', '74 Strand Road','Derry','Co Derry','BT47 4TB', '07865255486', '3'), 
('STA1027', 'Mr', 'Doherty', 'Jack', '523 Glenshane Road','Claudy','Co Derry','BT47 4TB', '07759631458', '3'),
('STA1028', 'Miss', 'Heaney', 'Liz', '43 Dungiven Road','Limavady','Co Derry','BT47 4TB', '07566584748', '3'), 
('STA1029', 'Mr', 'Evans', 'Conal', '20 Rosewood Avenue','Derry','Co Derry','BT47 4TB', '02871346582', '3'),
('STA1030', 'Mr', 'Coyle', 'Sheamous', '12 Culmore Road','Derry','Co Derry','BT47 4TB', '07756914238', '3'), 
('STA1031', 'Mr', 'Murray', 'Bill', '6b Enfield Street','Derry','Co Derry','BT47 4TB', '07925688149', '3'), 
('STA1032', 'Mr', 'Donaghy', 'Stephen', '577 Duncrun Road','Limavady','Co Derry','BT47 4TB', '02877741596', '3'), 
('STA1033', 'Miss', 'Jenkins', 'Sarah', '16 Carrickbeg Avenue','Eglinton','Co Derry','BT47 4TB', '02871362894', '3'), 
('STA1034', 'Mrs', 'Matthews', 'Karen', '1 Clagan Road','Claudy','Co Derry','BT47 4TB', '07774698315', '3'), 
('STA1035', 'Miss', 'Cartin', 'Aine', '10 Rallagh Road','Dungiven','Co Derry','BT47 4TB', '07892579143', '4'), 
('STA1036', 'Mr', 'Holmes', 'Eamon', '14 Spencer Road','Waterside','Co Derry','BT47 4TB', '07985020748', '4'),
('STA1037', 'Mr', 'Weisner', 'Peter', '9 Main Street','Eglinton','Co Derry','BT47 4TB', '02871261548', '4'),   
('STA1038', 'Miss', 'Hyndman', 'Jennifer', '27 Greenhaven','Drumahoe','Co Derry','BT48 3SY', '02871346562', '4'),  
('STA1039', 'Mr', 'Murray', 'Roger', '45 Larch Road','Greysteel','Co Derry','BT47 4TB', '02871249876', '4'), 
('STA1040', 'Mr', 'Doherty', 'John', '5 NewLine Road','Newbuildings','Co Derry','BT47 4TB', '02871375921', '4'), 
('STA1041', 'Mr', 'Campbell', 'Conor', '65 Hollyfoot Hill','Ballykelly','Co Derry','BT47 4TB', '07561020748', '4'), 
('STA1042', 'Mr', 'Hyndman', 'Matthew', '26 Dungiven Road','Derry','Co Derry','BT47 4TB', '02871361782', '4'),
('STA1043', 'Mrs', 'Deeney', 'Teresa', '68 Limavady Road','Derry','Co Derry','BT47 4TB', '02871267183', '4'),
('STA1044', 'Mr', 'Palmer', 'Bradley', '72 Church Brae','Derry','Co Derry','BT47 4TB', '02871348527', '4'),
('STA1045', 'Mrs', 'Jennings', 'Patricia', '2 Strand Road','Derry','Co Derry','BT47 4TB', '07865255486', '4'), 
('STA1046', 'Mr', 'Campbell', 'Wilson', '333 Glenshane Road','Claudy','Co Derry','BT47 4TB', '07759631458', '4'),
('STA1047', 'Mrs', 'Toland', 'Violet', '5 Dungiven Road','Limavady','Co Derry','BT47 4TB', '07566584748', '4'), 
('STA1048', 'Mrs', 'Lyons', 'Jade', '43 Rosewood Avenue','Derry','Co Derry','BT47 4TB', '02871346582', '4'),
('STA1049', 'Mrs', 'Bride', 'Sharon', '55 Culmore Road','Derry','Co Derry','BT47 4TB', '07756914238', '4'), 
('STA1050', 'Mr', 'Cunning', 'Patrick', '15 NewLine Road','Newbuildings','Co Derry','BT47 4TB', '02871375921', '2');

-- CustTypeID bit NOT NULL
-- CustTypeDescription varchar(8) NOT NULL
INSERT INTO customerType (CustTypeID, CustTypeDescription) VALUES 
(1, 'Normal'),   
(2, 'Business') 

--CustomerNo int NOT NULL,
--Title varchar(4) NOT NULL,
--Surname varchar(15) NOT NULL,
--Forename varchar(15) NOT NULL,
--Street varchar(20) NOT NULL,
--Town varchar(15) NOT NULL,
--County varchar(15) NOT NULL,
--PostCode varchar(8) NOT NULL,
--TelNo varchar(11) NOT NULL,
--CustType bit NOT NULL,


INSERT INTO customer (CustomerNo, Title, Surname, Forename, Street, Town, County, PostCode, TelNo, CustType) VALUES 
('CUS1000', 'Mr', 'GUEST', 'GUEST', 'N/A','N/A','N/A','NA00 0NA', '00000000000', '1'), 
('CUS1001', 'Mr', 'Morrison', 'Mark', '19 Main Street','Eglinton','Co Derry','BT47 4TB', '02871261548', '1'),   
('CUS1002', 'Mr', 'Young', 'Joe', '52 Greenhaven','Drumahoe','Co Derry','BT48 3SY', '02871346562', '1'),  
('CUS1003', 'Miss', 'Tia', 'Caitlen', '9 Larch Road','Greysteel','Co Derry','BT47 4TB', '02871249876', '1'), 
('CUS1004', 'Mr', 'Carling', 'Matt', '105 NewLine Road','Newbuildings','Co Derry','BT47 4TB', '02871375921', '1'), 
('CUS1005', 'Mrs', 'Miller', 'Lilly', '25 Hollyfoot Hill','Ballykelly','Co Derry','BT47 4TB', '07561020748', '1'), 
('CUS1006', 'Mr', 'Brown', 'Dan', '75 Dungiven Road','Derry','Co Derry','BT47 4TB', '02871361782', '1'),
('CUS1007', 'Mr', 'Coxon', 'Joe', '6 Limavady Road','Derry','Co Derry','BT47 4TB', '02871267183', '2'),
('CUS1008', 'Mr', 'Calhoun', 'Armin', '36 Church Brae','Derry','Co Derry','BT47 4TB', '02871348527', '1'),
('CUS1009', 'Mrs', 'Burris', 'Niamh', '84 Strand Road','Derry','Co Derry','BT47 4TB', '07865255486', '2'), 
('CUS1010', 'Mr', 'Downs', 'Dave', '64 Glenshane Road','Claudy','Co Derry','BT47 4TB', '07759631458', '1'),
('CUS1011', 'Miss', 'Heaney', 'Edith', '12 Dungiven Road','Limavady','Co Derry','BT47 4TB', '07566584748', '1'), 
('CUS1012', 'Mr', 'Lee', 'Rory', '72 Rosewood Avenue','Derry','Co Derry','BT47 4TB', '02871346582', '1'),
('CUS1013', 'Mr', 'McDonald', 'Steven', '30 Culmore Road','Derry','Co Derry','BT47 4TB', '07756914238', '2'), 
('CUS1014', 'Mr', 'Murray', 'Jeff', '3b Enfield Street','Derry','Co Derry','BT47 4TB', '07925688149', '1'), 
('CUS1015', 'Mr', 'Morrison', 'May', '52 Duncrun Road','Limavady','Co Derry','BT47 4TB', '02877741596', '1'), 
('CUS1016', 'Miss', 'Brown', 'Hannah', '81 Carrickbeg Avenue','Eglinton','Co Derry','BT47 4TB', '02871362894', '2'), 
('CUS1017', 'Mrs', 'Goldsmith', 'Elisa', '141 Clagan Road','Claudy','Co Derry','BT47 4TB', '07774698315', '1'), 
('CUS1018', 'Miss', 'Hunter', 'Delia', '211 Rallagh Road','Dungiven','Co Derry','BT47 4TB', '07892579143', '1'), 
('CUS1019', 'Mr', 'Maguire', 'Cormac', '24 Spencer Road','Waterside','Co Derry','BT47 4TB', '07985020748', '1'),
('CUS1020', 'Mr', 'Xander', 'Johnny', '17 Main Street','Eglinton','Co Derry','BT47 4TB', '02871261548', '1'),   
('CUS1021', 'Miss', 'Rankin', 'Xander', '20 Greenhaven','Drumahoe','Co Derry','BT48 3SY', '02871346562', '1'),  
('CUS1022', 'Mr', 'Muri', 'Jill', '32 Larch Road','Greysteel','Co Derry','BT47 4TB', '02871249876', '1'), 
('CUS1023', 'Mr', 'Howard', 'Niall', '10 NewLine Road','Newbuildings','Co Derry','BT47 4TB', '02871375921', '1'), 
('CUS1024', 'Mr', 'Kinghts', 'Reuben', '11 Hollyfoot Hill','Ballykelly','Co Derry','BT47 4TB', '07561020748', '1'), 
('CUS1025', 'Mr', 'Solomon', 'Connor', '16 Dungiven Road','Derry','Co Derry','BT47 4TB', '02871361782', '1'),
('CUS1026', 'Mrs', 'Hunter', 'Danni', '21 Limavady Road','Derry','Co Derry','BT47 4TB', '02871267183', '2'),
('CUS1027', 'Mr', 'Xander', 'Zack', '27 Church Brae','Derry','Co Derry','BT47 4TB', '02871348527', '1'),
('CUS1028', 'Mrs', 'Ross', 'Eva', '62 Strand Road','Derry','Co Derry','BT47 4TB', '07865255486', '2'), 
('CUS1029', 'Mr', 'McDonald', 'Jack', '52 Glenshane Road','Claudy','Co Derry','BT47 4TB', '07759631458', '1'),
('CUS1030', 'Miss', 'Hunter', 'April', '22 Dungiven Road','Limavady','Co Derry','BT47 4TB', '07566584748', '1'), 
('CUS1031', 'Mr', 'Evans', 'Lori', '14 Rosewood Avenue','Derry','Co Derry','BT47 4TB', '02871346582', '1'),
('CUS1032', 'Mr', 'Barr', 'Matthew', '18 Culmore Road','Derry','Co Derry','BT47 4TB', '07756914238', '2'), 
('CUS1033', 'Mr', 'Murphy', 'Billy', '3b Enfield Street','Derry','Co Derry','BT47 4TB', '07925688149', '1'), 
('CUS1034', 'Mr', 'King', 'Stephen', '57 Duncrun Road','Limavady','Co Derry','BT47 4TB', '02877741596', '1'), 
('CUS1035', 'Miss', 'Moyer', 'Una', '9 Carrickbeg Avenue','Eglinton','Co Derry','BT47 4TB', '02871362894', '2'), 
('CUS1036', 'Mrs', 'Watt', 'Milly', '15 Clagan Road','Claudy','Co Derry','BT47 4TB', '07774698315', '1'), 
('CUS1037', 'Miss', 'Kendall', 'Gemma', '12 Rallagh Road','Dungiven','Co Derry','BT47 4TB', '07892579143', '1'), 
('CUS1038', 'Mr', 'Holder', 'Richard', '17 Spencer Road','Waterside','Co Derry','BT47 4TB', '07985020748', '1'),
('CUS1039', 'Mr', 'Carr', 'Patrick', '9 Main Street','Eglinton','Co Derry','BT47 4TB', '02871261548', '1'),   
('CUS1040', 'Miss', 'Watt', 'Natasha', '16 Greenhaven','Drumahoe','Co Derry','BT48 3SY', '02871346562', '1'),  
('CUS1041', 'Mr', 'Willson', 'Wade', '35 Larch Road','Greysteel','Co Derry','BT47 4TB', '02871249876', '1'), 
('CUS1042', 'Mr', 'Wise', 'Alen', '19 NewLine Road','Newbuildings','Co Derry','BT47 4TB', '02871375921', '1'), 
('CUS1043', 'Mr', 'York', 'Evan', '6 Hollyfoot Hill','Ballykelly','Co Derry','BT47 4TB', '07561020748', '1'), 
('CUS1044', 'Mr', 'Noel', 'Mark', '21 Dungiven Road','Derry','Co Derry','BT47 4TB', '02871361782', '1'),
('CUS1045', 'Mrs', 'Harrell', 'Roxy', '47 Limavady Road','Derry','Co Derry','BT47 4TB', '02871267183', '2'),
('CUS1046', 'Mr', 'North', 'Franklin', '25 Church Brae','Derry','Co Derry','BT47 4TB', '02871348527', '1'),
('CUS1047', 'Mrs', 'Hamilton', 'Bethaney', '6 Strand Road','Derry','Co Derry','BT47 4TB', '07865255486', '2'), 
('CUS1048', 'Mr', 'Gale', 'Tyler', '33 Glenshane Road','Claudy','Co Derry','BT47 4TB', '07759631458', '1'),
('CUS1049', 'Mrs', 'Moore', 'Zoey', '9 Dungiven Road','Limavady','Co Derry','BT47 4TB', '07566584748', '1'), 
('CUS1050', 'Mrs', 'Lyons', 'Zahrah', '31 Rosewood Avenue','Derry','Co Derry','BT47 4TB', '02871346582', '1');

--RoomBookingID int NOT NULL,
--CustomerNo int NOT NULL,
--StartDate date NOT NULL,
--NoDays tinyint NOT NULL,
INSERT INTO RoomBooking (RoomBookingID, CustomerNo, StartDate, NoDays) VALUES

(10008, 'CUS1007', '2018-12-18', 2),
(10009, 'CUS1008', '2018-01-19', 1),
(10010, 'CUS1009', '2018-02-20', 3),
(10011, 'CUS1010', '2018-03-21', 4),
(10012, 'CUS1011', '2018-04-22', 5),
(10013, 'CUS1012', '2018-01-23', 6),
(10014, 'CUS1013', '2018-01-24', 7),
(10015, 'CUS1014', '2018-01-25', 9),
(10016, 'CUS1015', '2019-02-26', 10),
(10017, 'CUS1016', '2019-02-27', 11),
(10018, 'CUS1017', '2019-03-28', 14),
(10019, 'CUS1018', '2019-04-29', 7),
(10020, 'CUS1019', '2019-05-30', 2),
(10021, 'CUS1020', '2019-01-31', 2),
(10022, 'CUS1021', '2019-02-01', 3),
(10023, 'CUS1022', '2019-03-02', 3),
(10024, 'CUS1023', '2019-04-03', 1),
(10025, 'CUS1024', '2018-05-04', 2),
(10026, 'CUS1025', '2018-06-05', 4),
(10027, 'CUS1026', '2018-02-06', 5),
(10028, 'CUS1027', '2018-04-07', 3),
(10029, 'CUS1028', '2018-05-08', 4),
(10030, 'CUS1029', '2018-03-09', 1),
(10001, 'CUS1040', '2018-05-11', 2),
(10002, 'CUS1001', '2018-06-12', 4),
(10003, 'CUS1002', '2018-07-13', 6),
(10004, 'CUS1003', '2018-08-14', 7),
(10005, 'CUS1004', '2018-09-15', 12),
(10006, 'CUS1005', '2018-10-16', 14),
(10007, 'CUS1006', '2018-11-17', 7);
--ItemTypeID int NOT NULL,
--ItemDesc varchar(25) NOT NULL,
INSERT INTO ItemType (ItemTypeID, ItemDesc) VALUES
(1, 'Starter'),
(2,' Main'),
(3, 'Deserts'),
(4, 'Sides'),
(5, 'Drinks'),
(6, 'Breakfast');


--LogTypeID bit NOT NULL,
--LogDesc varchar(20) NOT NULL,
INSERT INTO LoginType (LogTypeID, LogDesc) VALUES 
(0, 'Admin'),
(1, 'Regular Employee');

--LoginID int NOT NULL,

--ItemID int NOT NULL,
--MenuItemDesc varchar(40) NOT NULL,
--Price money NOT NULL,
--ItemTypeID int NOT NULL,
INSERT INTO LOGINS (LoginID, LogUsername, LogPassword, StaffID, LogTypeID)
values
(100000, 'BMor10000', 'bN3w,.Day0', 'STA1000', 0),
(100001, 'KRan10001', 'kN3w,.Day0', 'STA1001', 0),
(100002, 'PCar10002', 'pNew,.Day0', 'STA1002', 0),
(100003, 'TCun10003', 't5Up3rV150r0', 'STA1003', 0),
(100004, 'RWil10004', 'r5Up3rV150r0', 'STA1004', 0),
(100005, 'CLyn10005', 'c5Up3rV150r0', 'STA1005', 0),
(100006, 'JCol10006', 'j5Up3rV150r0', 'STA1006', 0),
(100007, 'AGal10007', 'a5Up3rV150r0', 'STA1007', 0),
(100008, 'NCos10008', 'n5Up3rV150r0','STA1008', 0),
(100009, 'KDoh10009', 'kR3c3Pt10n0', 'STA1009', 0),
(100010, 'AHea10010', 'aR3c3Pt10n0', 'STA1010', 0),
(100011, 'REva10011', 'rR3c3Pt10n0', 'STA1011', 0),
(100012, 'SCoy10012', 'sR3c3Pt10n0', 'STA1012', 0),
(100013, 'MMur10013', 'mR3c3Pt10n0', 'STA1013', 0),
(100014, 'MDon10014', 'mR3c3Pt10n1', 'STA1014', 0),
(100015 , 'HJen10015', 'hpArT/t1M30', 'STA1015', 1),
(100016 , 'LMat10016', 'lpArT/t1M30', 'STA1016', 1),
(100017 , 'CCar10017', 'cpArT/t1M30', 'STA1017', 1),
(100018 , 'KHol10018', 'kpArT/t1M30', 'STA1018', 1),
(100019 , 'JMor10019', 'jpArT/t1M30', 'STA1019', 1),
(100020 , 'CRan10020', 'cpArT/t1M31', 'STA1020', 1),
(100021 , 'HCar10021', 'hpArT/t1M31', 'STA1021', 1),
(100022 , 'PCun10022', 'ppArT/t1M30', 'STA1022', 1),
(100023 , 'PWil10023', 'ppArT/t1M31', 'STA1023', 1),
(100024 , 'CLyn10024', 'cpArT/t1M32', 'STA1024', 1),
(100025 , 'ACol10025', 'apArT/t1M30', 'STA1025', 1),
(100026 , 'AGal10026', 'apArT/t1M31', 'STA1026', 1),
(100027 , 'NCos10027', 'nFu11/t1m30', 'STA1027', 1),
(100028 , 'JDoh10028', 'jFu11/t1m30', 'STA1028', 1),
(100029 , 'LHea10029', 'lFu11/t1m30', 'STA1029', 1),
(100030 , 'CEva10030', 'cFu11/t1m30', 'STA1030', 1),
(100031 , 'SCoy10031', 'sFu11/t1m30', 'STA1031', 1),
(100032 , 'BMur10032', 'bFu11/t1m30', 'STA1032', 1),
(100033 , 'SDon10033', 'sFu11/t1m31', 'STA1033', 1),
(100034 , 'SJen10034', 'sFu11/t1m32', 'STA1034', 1),
(100035 , 'KMat10035', 'kFu11/t1m30', 'STA1035', 1),
(100036 , 'ACat10036', 'aFu11/t1m30', 'STA1036', 1),
(100037 , 'EHol10037', 'eFu11/t1m30', 'STA1037', 1),
(100038 , 'PWei10038', 'pFu11/t1m30', 'STA1038', 1),
(100039 , 'JHyn10039', 'jFu11/t1m31', 'STA1039', 1),
(100040 , 'RMur10040', 'rFu11/t1m30', 'STA1040', 1),
(100041 , 'JDoh10041', 'jFu11/t1m32', 'STA1041', 1),
(100042 , 'CCam10042', 'cFu11/t1m31', 'STA1042', 0),
(100043 , 'MHyn10043', 'mFu11/t1m30', 'STA1043', 0),
(100044 , 'TDee10044', 'tFu11/t1m30', 'STA1044', 1),
(100045 , 'BPam10045', 'bFu11/t1m31', 'STA1045', 0),
(100046 , 'PJen10046', 'pFu11/t1m31', 'STA1046', 1),
(100047 , 'WCam10047', 'wFu11/t1m30', 'STA1047', 1),
(100048 , 'VTol10048', 'vFu11/t1m30', 'STA1048', 1),
(100049 , 'JLyo10049', 'jFu11/t1m33', 'STA1049', 1),
(100050 , 'SBri10050', 'sFu11/t1m33', 'STA1050', 1);

INSERT INTO MENU (ItemID, MenuItemDesc, Price, ItemTypeID) VALUES
( 1,'Crisp Buttermilk Chicken Wings',	 5.50, 1),
(2, 'Soup of the Day',			 4.50, 1),
(3, 'Pressed Ham Hock',			 5.50, 1),
(4, 'Deep fried Irish Brie',		 4.95, 1),
(5, 'Classic Caesar Salad',		 5.50, 1),
(6, 'Cherry Valley Duck Brest',		 17.50, 2),
(7, 'Corn-fed Chicken Supreme',		 14.95, 2),
(8, 'Slaney Valley Rump of Lamb',	 17.95, 2),
(9, 'Pan-Fried Salmon',			 14.50, 2),
(10, 'Grilled Seabass',			 15.50, 2),
(11, 'Cheese Cake of the Day',		 4.95, 3),
(12, 'Dark Chocolate Mousse',		 4.95, 3),
(13, 'Passionfruit Panna Cotta',	 4.95, 3),
(14, 'Pear and Almond Tart',		 4.95, 3),
(15, 'Sticky Toffee Pudding',		 4.95, 3),
(16, 'Chips',				 2.50, 4),
(17, 'Tobaco Onions',			 2.50, 4),
(18, 'Frinch Fries',			 2.50, 4),
(19, 'Champ',				 2.50, 4),
(20, 'Chilli Chips',			 2.50, 4),
(21, 'Tea',				 2.15, 5),
(22, 'Coffee',				 2.15, 5),
(23, 'Mojito',				 6.50, 5),
(24, 'Trulli Pinot Grigio, Italy',	 17.95, 5),
(25, 'Fortant Merlot Rosé, France',	 19.95, 5),
(26, 'Breakfast Fry',			 7.50, 6),
(27, 'Irish Brunch',			 8.50, 6),
(28, 'Breakfast of the Day',		 5.00, 6),
(29, 'Grand Fry',			 10.00, 6),
(30, 'Small Fry',			 5.00, 6)


--RoomServiceID int NOT NULL,
--ItemID int NOT NULL,
--Quantity tinyint NOT NULL
INSERT INTO RoomService (RoomBookingID, ItemID, Quantity) VALUES
(10001, 25, 2),--DRINKS
(10002, 26, 2),--Breakfast
(10003, 23, 2),--DRINKS
(10004, 27, 2),--Breakfast
(10005, 24, 2),--DRINKS
(10006, 28, 2),--Breakfast
(10007, 22, 2),--DRINKS
(10008, 29, 2),--Breakfast
(10009, 21, 2),--DRINKS
(10010, 30, 4),--Breakfast
(10011, 30, 6),--Breakfast
(10012, 30, 2),--Breakfast
(10013, 30, 4),--Breakfast
(10014, 25, 2),--DRINKS
(10015, 26, 1),--Breakfast
(10016, 27, 1),--Breakfast
(10017, 22, 2),--DRINKS
(10018, 23, 1),--DRINKS
(10019, 24,1),--DRINKS
(10020, 21, 2),--DRINKS
(10021, 24, 2),--DRINKS
(10022, 30, 1);--Breakfast

--RoomTypeID tinyint NOT NULL,
--RoomTypeDesc varchar(6) NOT NULL, -- Single, Double, Triple, Family - Are these correct? If not, increase the size of the varchar. Are there triple rooms? 
--NoBeds tinyint NOT NULL,
--PricePerNight money NOT NULL,
INSERT INTO RoomType (RoomTypeID, RoomTypeDesc, Nobeds, PricePerNight) VALUES 
(1, 'Single', 1, 75.00),
(2, 'Double', 1, 95.00),
(3, 'Triple', 3, 115.00),
(4, 'Family', 4, 140.00)


--RoomID varchar(5) NOT NULL,
--RoomTypeID tinyint NOT NULL,
--RoomFloor tinyint NOT NULL,
INSERT INTO Room (RoomID, RoomTypeID, RoomFloor) VALUES
('F1R01', 1, 1),
('F1R02', 1, 1),
('F1R03', 1, 1),
('F1R04', 2, 2),
('F1R05', 2, 2),
('F1R06', 2, 2),
('F1R07', 3, 3),
('F1R08', 3, 3),
('F1R09', 3, 3),
('F1R10', 4, 4),
('F2R01', 1, 2),
('F2R02', 1, 2),
('F2R03', 1, 2),
('F2R04', 2, 2),
('F2R05', 2, 2),
('F2R06', 2, 2),
('F2R07', 3, 3),
('F2R08', 3, 3),
('F2R09', 3, 3),
('F2R10', 4, 4),
('F3R01', 1, 3),
('F3R02', 1, 3),
('F3R03', 1, 3),
('F3R04', 2, 2),
('F3R05', 2, 2),
('F3R06', 2, 2),
('F3R07', 2, 2),
('F3R08', 3, 3),
('F3R09', 3, 3),
('F3R10', 3, 3),
('F4R01', 3, 3),
('F4R02', 4, 4),
('F4R03', 4, 4),
('F4R04', 4, 4);

--RoomBookingID int NOT NULL,
--RoomID varchar(5) NOT NULL,
--StaffID varchar(7) NOT NULL,
--DateTimeBooked datetime NOT NULL,
INSERT INTO RoomBookingDetails (RoomBookingID, RoomID, StaffID, DateTimeBooked ) VALUES
(10001, 'F1R01',  'STA1000', '2018-12-12T13:00:00' ), 
(10002, 'F1R04',  'STA1001', '2018-01-23T14:00:00') ,
(10003, 'F1R07',  'STA1002', '2018-02-03T15:00:00'), 
(10004, 'F1R10',  'STA1003', '2018-11-10T16:00:00'), 
(10005, 'F2R02', 'STA1004', '2018-10-09T17:00:00'), 
(10006, 'F2R05',  'STA1005', '2018-12-01T18:00:00'),
(10007, 'F2R08',  'STA1006', '2018-11-27T19:00:00'), 
(10008, 'F2R10',  'STA1007', '2018-09-19T20:00:00'), 
(10009, 'F3R03',  'STA1008', '2018-01-17T13:00:00'), 
(10010, 'F3R06',  'STA1009', '2018-02-16T12:00:00'), 
(10011, 'F3R09',  'STA1010', '2018-01-15T11:00:00'), 
(10012, 'F3R10',  'STA1011', '2018-12-26T10:00:00'), 
(10013, 'F4R01', 'STA1012', '2018-11-20T14:00:00'), 
(10014, 'F4R04', 'STA1014', '2018-11-21T17:00:00'), 
(10015, 'F4R02', 'STA1029', '2018-12-08T16:00:00'); 


--BookingID int NOT NULL,
--CustomerNo varchar(7) NOT NULL,
--DateTimeBookedFor datetime NOT NULL,
--NoOfPeople tinyint NOT NULL,
INSERT INTO BookingRestaurant (BookingID, CustomerNo, DateTimeBookedFor, NoOfPeople ) VALUES
(10011, 'CUS1040', '2019-02-26T22:00:00', 4),
(10001, 'CUS1030', '2019-01-13T13:00:00', 2), 
(10002, 'CUS1031', '2019-01-15T14:00:00', 4),
(10003, 'CUS1032', '2019-01-16T15:00:00', 6),
(10004, 'CUS1033', '2019-01-10T16:00:00', 1),
(10005, 'CUS1034', '2019-01-10T17:00:00', 3),
(10006, 'CUS1035', '2019-01-22T18:00:00', 5),
(10007, 'CUS1036', '2019-02-11T19:00:00', 3),
(10008, 'CUS1037', '2019-02-01T20:00:00', 2),
(10009, 'CUS1038', '2019-03-17T21:00:00', 1),
(10010, 'CUS1039', '2019-02-26T22:00:00', 4);


--TableID tinyint NOT NULL,
--NoSeats tinyint NOT NULL
INSERT INTO TableDetails (TableID, NoSeats) VALUES 
(1, 2),
(2, 4),
(3, 6),
(4, 8),
(5, 2),
(6, 4),
(7, 6),
(8, 8),
(9, 2),
(10, 2),
(11, 2),
(12, 2),
(13, 2),
(14, 4),
(15, 4),
(16, 4),
(17, 2),
(18, 6),
(19, 6),
(20, 8);

--BookingID int NOT NULL,
--ItemID int NOT NULL,
--Quantity tinyint NOT NULL,
INSERT INTO BookingMenu (BookingID, ItemID, Quantity) VALUES

(10011, 15, 2),
(10011, 16, 2),
(10011, 17, 2),
(10011, 5, 2),
(10011, 10, 2),
(10011, 11, 2),

(10001, 1, 2),
(10001, 6, 2),
(10001, 11, 2),

(10002, 2, 4),
(10002, 7, 4),
(10002, 12, 2),
(10002, 13, 2),

(10003, 1, 2),
(10003, 6, 2),
(10003, 11, 2),
(10003, 3, 2),
(10003, 8, 2),
(10003, 13, 2),
(10003, 2, 2),
(10003, 7, 2),
(10003, 12, 2),

(10004, 4, 1),
(10004, 8, 1),
(10004, 14, 1),

(10005, 10, 2),
(10005, 11, 2),
(10005, 12, 2),
(10005, 4, 1),
(10005, 8, 1),
(10005, 14, 1),

(10006, 2, 2),
(10006, 8, 2),
(10006, 15, 2),
(10006, 1, 2),
(10006, 6, 2),
(10006, 11, 2),
(10006, 9, 2),
(10006, 19, 2),

(10007, 3, 2),
(10007, 9, 2),
(10007, 14, 2),
(10007, 1, 1),
(10007, 6, 1),
(10007, 15, 1),

(10008, 5, 2),
(10008, 10, 2),
(10008, 11, 2),

(10009, 3, 1),
(10009, 7, 1),
(10009, 12, 1),

(10010, 15, 2),
(10010, 16, 2),
(10010, 17, 2),
(10010, 5, 2),
(10010, 10, 2),
(10010, 11, 2);

--BookingID int NOT NULL,
--TableID tinyint NOT NULL,
--StaffID varchar(7) NOT NULL,
--DateTimeBooked datetime NOT NULL,
INSERT INTO BookingRestDetails (BookingID, TableID, StaffID, DateTimeBooked) VALUES
(10001, 1, 'STA1026', '2019-01-14T13:00:00' ), 
(10002, 2, 'STA1027', '2019-01-16T13:00:00') , 
(10003, 3,'STA1028','2019-01-17T13:00:00'), 
(10004, 9,'STA1029', '2019-01-11T13:00:00') , 
(10005, 6,'STA1030','2019-01-19T13:00:00'), 
(10006, 7,'STA1031','2019-01-23T13:00:00'), 
(10007, 10, 'STA1032','2019-02-13T13:00:00'),
(10007, 11, 'STA1033','2019-02-13T13:00:00'),
(10008, 9,'STA1034', '2019-02-02T13:00:00'), 
(10009, 1,'STA1031', '2019-03-18T13:00:00'), 
(10010, 6, 'STA1026', '2019-02-27T13:00:00'),
(10011, 6, 'STA1026', '2019-04-27T13:00:00');


--EventTypeID bit NOT NULL,
--EventTypeDescription varchar(8) NOT NULL,
INSERT INTO EventType (EventTypeID, EventTypeDescription) VALUES
(1, 'Concert'),
(2, 'Magic Show'),
(3, 'Comedian'),
(4, 'Press Conference'),
(5, 'Weddings'),
(6, 'Recruitment');

--EventID int NOT NULL,
--EventDesc varchar(15) NOT NULL,
--EventPrice money NOT NULL,
--EventTypeID bit NOT NULL,
--EventBookingDateFor datetime NOT NULL, 
--NoTickets tinyint NOT NULL,
INSERT INTO _Event (EventID, EventDesc,EventPrice, EventTypeID, EventBookingDateFor, NoTickets, HotelEventPrice) VALUES
(10001, 'The Whistling Donkeys', 15.00, 1, '2019-03-02T20:30:00', 200, 700.00),
(10002, 'Paul Nardini', 10.00, 2, '2019-03-09T21:00:00', 200, 90.00),
(10003, 'Colin Geddis', 7.00, 3, '2019-03-16T20:30:00', 100, 100.00),
(10004, 'Jimeoin', 15.00, 3, '2019-04-02T20:30:00', 150, 300.00),
(10005, 'Jika Jika', 10.00, 1, '2019-04-09T21:00:00', 200, 100.00),
(10006, 'Ross McRae', 7.00, 2, '2019-04-16T20:30:00', 100, 250.00),
(10007, 'Mr. Majestyck', 15.00, 2, '2019-05-02T20:30:00', 150, 50.00),
(10008, 'Ed Bryne', 10.00, 3, '2019-05-09T21:00:00', 200, 160.00),
(10009, 'Conor McGinty', 7.00, 1, '2019-05-16T20:30:00', 100, 70.00);

--EventBookingID int NOT NULL,
--CustomerNo varchar(7) NOT NULL,
INSERT INTO EventBooking (EventBookingID, CustomerNo) VALUES
(10034,'CUS1000'),
(10111,'CUS1000'),
(10035,'CUS1000'),
(10036, 'CUS1000'),
(10037,'CUS1000'),
(10038, 'CUS1000'),
(10039, 'CUS1000'),
(10040, 'CUS1000'),
(10041, 'CUS1000'),
(10042, 'CUS1000'),
(10043, 'CUS1000'),
(10044, 'CUS1000'),
(10045, 'CUS1000'),
(10046, 'CUS1000'),
(10047,'CUS1000'),
(10048, 'CUS1000'),
(10049, 'CUS1000'),
(10050, 'CUS1000'),
(10051,'CUS1000'),
(10052, 'CUS1000'),
(10053,'CUS1000'),
(10054, 'CUS1000'),
(10055,'CUS1000'),
(10056, 'CUS1000'),
(10057, 'CUS1000'),
(10058,'CUS1000'),
(10059,'CUS1000'),
(10060,'CUS1000'),
(10061,'CUS1000'),
(10062, 'CUS1000'),
(10063,'CUS1000'),
(10064, 'CUS1000'),
(10065, 'CUS1000'),
(10066, 'CUS1000'),
(10067, 'CUS1000'),
(10068,'CUS1000'),
(10069, 'CUS1000'),
(10070, 'CUS1000'),
(10071, 'CUS1000'),
(10072,'CUS1000'),
(10073, 'CUS1000'),
(10074, 'CUS1000'),
(10075,'CUS1000'),
(10076, 'CUS1000'),
(10077, 'CUS1000'),
(10078, 'CUS1000'),
(10079,'CUS1000'),
(10080, 'CUS1000'),
(10081,'CUS1000'),
(10082, 'CUS1000'),
(10083,'CUS1000'),
(10084,'CUS1000'),
(10085, 'CUS1000'),
(10086, 'CUS1000'),
(10087, 'CUS1000'),
(10088, 'CUS1000'),
(10089, 'CUS1000'),
(10090, 'CUS1000'),
(10091,'CUS1000'),
(10092,'CUS1000'),
(10093, 'CUS1000'),
(10094, 'CUS1000'),
(10095, 'CUS1000'),
(10096, 'CUS1000'),
(10097,'CUS1000'),
(10098,'CUS1000'),
(10099, 'CUS1000'),
(10100, 'CUS1000'),
(10101, 'CUS1000'),
(10102,'CUS1000'),
(10103, 'CUS1000'),
(10104,'CUS1000'),
(10105, 'CUS1000'),
(10106, 'CUS1000'),
(10107, 'CUS1000'),
(10108, 'CUS1000'),
(10001, 'CUS1041'),
(10002, 'CUS1042'),
(10003, 'CUS1043'),
(10004, 'CUS1050'),
(10005, 'CUS1044'),
(10006, 'CUS1045'),
(10007, 'CUS1046'),
(10008, 'CUS1047'),
(10009, 'CUS1048'),
(10010, 'CUS1049'),
(10031, 'CUS1000'),
(10011, 'CUS1050'),
(10012, 'CUS1041'),
(10013, 'CUS1042'),
(10014, 'CUS1043'),
(10015, 'CUS1044'),
(10016, 'CUS1045'),
(10017, 'CUS1046'),
(10018, 'CUS1047'),
(10019, 'CUS1048'),
(10020, 'CUS1049'),
(10032, 'CUS1000'),
(10021, 'CUS1041'),
(10022, 'CUS1042'),
(10023, 'CUS1043'),
(10024, 'CUS1044'),
(10025, 'CUS1045'),
(10026, 'CUS1046'),
(10027, 'CUS1047'),
(10028, 'CUS1048'),
(10029, 'CUS1049'),
(10030, 'CUS1050'),
(10033, 'CUS1000');
--EventBookingID int NOT NULL,
--EventID int NOT NULL, 
--Quantity tinyint NOT NULL,
--StaffID varchar(7) NOT NULL,
--EventBookingDate datetime NOT NULL,
INSERT INTO EventBookingDets(EventBookingID, EventID, Quantity, StaffID, EventBookingDate) VALUES
(10001, 10001, 1, 'STA1009', '2019-02-01T13:30:00'),--200
(10002, 10001, 2, 'STA1009', '2019-02-01T13:30:22'),
(10003, 10001, 3, 'STA1009', '2019-02-01T13:30:35'),
(10004, 10001, 4, 'STA1009', '2019-02-01T13:30:25'),
(10031, 10001, 5, 'STA1010', '2019-02-01T13:31:00'),--190
(10034, 10001, 5, 'STA1010', '2019-02-01T13:31:20'),
(10111, 10001, 4, 'STA1010', '2019-02-01T13:31:40'),--180
(10035, 10001, 4, 'STA1010', '2019-02-01T13:32:00'),
(10036, 10001, 4, 'STA1010', '2019-02-01T13:32:20'),
(10037, 10001, 4, 'STA1010', '2019-02-01T13:32:40'),
(10038, 10001, 4, 'STA1010', '2019-02-01T13:33:00'),
(10039, 10001, 4, 'STA1010', '2019-02-01T13:33:30'),
(10040, 10001, 4, 'STA1010', '2019-02-01T13:34:00'),
(10041, 10001, 4, 'STA1010', '2019-02-01T13:34:30'),
(10042, 10001, 4, 'STA1010', '2019-02-01T13:35:00'),
(10043, 10001, 4, 'STA1010', '2019-02-01T13:35:20'),
(10044, 10001, 4, 'STA1010', '2019-02-01T13:36:00'),
(10045, 10001, 3, 'STA1010', '2019-02-01T13:36:30'),--136
(10046, 10001, 3, 'STA1010', '2019-02-01T13:37:00'),
(10047, 10001, 3, 'STA1010', '2019-02-01T13:37:20'),
(10048, 10001, 3, 'STA1010', '2019-02-01T13:37:40'),
(10049, 10001, 3, 'STA1010', '2019-02-01T13:38:00'),
(10050, 10001, 3, 'STA1010', '2019-02-01T13:38:20'),
(10051, 10001, 3, 'STA1010', '2019-02-01T13:38:40'),
(10052, 10001, 3, 'STA1010', '2019-02-01T13:39:00'),
(10053, 10001, 3, 'STA1010', '2019-02-01T13:39:20'),
(10054, 10001, 3, 'STA1010', '2019-02-01T13:39:40'),
(10055, 10001, 2, 'STA1010', '2019-02-01T13:40:00'),--106
(10056, 10001, 2, 'STA1010', '2019-02-01T13:40:20'),
(10057, 10001, 2, 'STA1010', '2019-02-01T13:40:40'),
(10058, 10001, 2, 'STA1010', '2019-02-01T13:41:25'),
(10059, 10001, 2, 'STA1010', '2019-02-01T13:42:00'),
(10060, 10001, 2, 'STA1010', '2019-02-01T13:43:00'),
(10061, 10001, 2, 'STA1010', '2019-02-01T13:44:00'),
(10062, 10001, 2, 'STA1010', '2019-02-01T13:44:20'),
(10063, 10001, 2, 'STA1010', '2019-02-01T13:44:40'),
(10064, 10001, 2, 'STA1010', '2019-02-01T13:45:00'),
(10065, 10001, 2, 'STA1010', '2019-02-01T13:45:30'),
(10066, 10001, 2, 'STA1010', '2019-02-01T13:46:00'),
(10067, 10001, 2, 'STA1010', '2019-02-01T13:47:20'),
(10068, 10001, 2, 'STA1010', '2019-02-01T13:48:40'),--80
(10069, 10001, 2, 'STA1010', '2019-02-01T13:49:00'),
(10070, 10001, 2, 'STA1010', '2019-02-01T13:49:30'),
(10071, 10001, 2, 'STA1010', '2019-02-01T13:50:00'),
(10072, 10001, 2, 'STA1010', '2019-02-01T13:50:30'),
(10073, 10001, 2, 'STA1010', '2019-02-01T13:51:00'),
(10074, 10001, 2, 'STA1010', '2019-02-01T13:52:00'),
(10075, 10001, 2, 'STA1010', '2019-02-01T13:53:00'),
(10076, 10001, 2, 'STA1010', '2019-02-01T13:53:20'),
(10077, 10001, 2, 'STA1010', '2019-02-01T13:53:40'),
(10078, 10001, 2, 'STA1010', '2019-02-01T13:54:00'),--60
(10079, 10001, 2, 'STA1010', '2019-02-01T13:54:20'),
(10080, 10001, 2, 'STA1010', '2019-02-01T13:54:40'),
(10081, 10001, 2, 'STA1010', '2019-02-01T13:55:00'),
(10082, 10001, 2, 'STA1010', '2019-02-01T13:56:00'),
(10083, 10001, 2, 'STA1010', '2019-02-01T13:56:30'),
(10084, 10001, 2, 'STA1010', '2019-02-01T13:57:00'),
(10085, 10001, 2, 'STA1010', '2019-02-01T13:57:20'),
(10086, 10001, 2, 'STA1010', '2019-02-01T13:57:40'),
(10087, 10001, 2, 'STA1010', '2019-02-01T13:58:00'),
(10088, 10001, 2, 'STA1010', '2019-02-01T13:58:20'),--40
(10089, 10001, 2, 'STA1010', '2019-02-01T13:58:40'),
(10090, 10001, 2, 'STA1010', '2019-02-01T13:59:00'),
(10091, 10001, 2, 'STA1010', '2019-02-01T13:59:30'),
(10092, 10001, 2, 'STA1010', '2019-02-01T14:00:00'),
(10093, 10001, 2, 'STA1010', '2019-02-01T14:00:30'),
(10094, 10001, 2, 'STA1010', '2019-02-01T14:01:00'),
(10095, 10001, 2, 'STA1010', '2019-02-01T14:01:30'),
(10096, 10001, 2, 'STA1010', '2019-02-01T14:02:00'),
(10097, 10001, 2, 'STA1010', '2019-02-01T14:02:30'),
(10098, 10001, 2, 'STA1010', '2019-02-01T14:03:00'),--20
(10099, 10001, 2, 'STA1010', '2019-02-01T14:04:00'),
(10100, 10001, 2, 'STA1010', '2019-02-01T14:04:30'),
(10101, 10001, 2, 'STA1010', '2019-02-01T14:05:00'),
(10102, 10001, 2, 'STA1010', '2019-02-01T14:05:30'),
(10103, 10001, 2, 'STA1010', '2019-02-01T14:06:00'),
(10104, 10001, 2, 'STA1010', '2019-02-01T14:06:30'),
(10105, 10001, 2, 'STA1010', '2019-02-01T14:07:00'),
(10106, 10001, 2, 'STA1010', '2019-02-01T14:07:30'),
(10107, 10001, 2, 'STA1010', '2019-02-01T14:08:00'),
(10108, 10001, 2, 'STA1010', '2019-02-01T14:08:20'),

(10005, 10002, 1, 'STA1011', '2019-02-02T13:30:00'),
(10006, 10002, 2, 'STA1011', '2019-02-02T13:31:00'),
(10007, 10002, 3, 'STA1011', '2019-02-02T13:32:00'),
(10032, 10002, 5, 'STA1012', '2019-02-02T13:30:00'),

(10008, 10003, 4, 'STA1013', '2019-02-03T13:31:00'),
(10009, 10003, 1, 'STA1013', '2019-02-03T13:32:00'),
(10010, 10003, 2, 'STA1013', '2019-02-03T13:33:00'),
(10033, 10003, 5, 'STA1014', '2019-02-03T13:30:00'),

(10011, 10004, 1, 'STA1009', '2019-02-04T13:31:00'),
(10012, 10004, 2, 'STA1009', '2019-02-04T13:32:00'),
(10013, 10004, 3, 'STA1009', '2019-02-04T13:33:00'),
(10014, 10005, 4, 'STA1010', '2019-02-05T13:31:00'),
(10015, 10005, 1, 'STA1010', '2019-02-05T13:32:00'),
(10016, 10005, 2, 'STA1010', '2019-02-05T13:33:00'),
(10017, 10006, 3, 'STA1011', '2019-02-06T13:30:00'),
(10018, 10006, 4, 'STA1011', '2019-02-06T13:30:00'),
(10019, 10006, 2, 'STA1011', '2019-02-06T13:30:00'),
(10020, 10006, 4, 'STA1011', '2019-02-06T13:30:00'),

(10021, 10007, 1, 'STA1012', '2019-02-07T13:30:00'),
(10022, 10007, 2, 'STA1012', '2019-02-07T13:30:00'),
(10023, 10007, 3, 'STA1012', '2019-02-07T13:30:00'),
(10024, 10007, 4, 'STA1013', '2019-02-07T13:30:00'),
(10025, 10008, 1, 'STA1013', '2019-02-08T13:30:00'),
(10026, 10008, 2, 'STA1013', '2019-02-08T13:30:00'),
(10027, 10008, 3, 'STA1013', '2019-02-08T13:30:00'),
(10028, 10009, 4, 'STA1014', '2019-02-09T13:30:00'),
(10029, 10009, 3, 'STA1014', '2019-02-09T13:30:00'),
(10030, 10009, 4, 'STA1014', '2019-02-09T13:30:00');

--ConferenceRoomID varchar(3) NOT NULL,
--ConferenceRoomDesc varchar(15) NOT NULL,
--MaxCapacity tinyint NOT NULL,
--Price money NOT NULL,
--ITEquipped bit NOT NULL,
INSERT INTO ConferenceRoom (ConferenceRoomID, ConferenceRoomDesc, MaxCapacity, Price, InbuiltScreen, InbuiltProjector, InbuiltPA, Wifi, AirConditioned, SuppliesForDelegates) VALUES
('CR1', 'Writers Room', 30, 100.00, 0, 0, 0, 1, 1, 1),
('CR2', 'Berkeley Room', 40, 130.00, 0, 0, 0, 1, 1, 1),
('CR3', 'Amelia Room', 50, 300.00, 1, 0, 0, 1, 1, 1),
('CR4', 'Hervey Room', 60, 330.00, 1, 0, 0, 1, 1, 1),
('CR5', 'McCorkell Room', 70, 400.00, 1, 0, 0, 1, 1, 1),
('CR6', 'Alexander Room', 100, 500.00, 1, 0, 0, 1, 1, 1),
('CR7', 'Corinthian Ballroom', 150, 600.00, 1, 1, 1, 1, 1, 0);


--ConferenceBookingID int NOT NULL,
--CustomerNo varchar(7) NOT NULL,
--ConferenceDateFor datetime NOT NULL,

--UNABLE TO DO ONE CONFERENCEBOOKINGID FOR ONE CUSTOMERNO MAKING MULTIPLE BOOKINGS
INSERT INTO ConferenceBooking (ConferenceBookingID, CustomerNo, ConferenceDateFor) VALUES
(50001, 'CUS1007', '2019-03-20T12:00:00'),
(50002, 'CUS1007', '2019-03-27T12:00:00'),
(50003, 'CUS1007', '2019-04-05T12:00:00'),
(50004, 'CUS1007', '2019-04-12T12:00:00'),

(50005, 'CUS1009', '2019-05-20T12:00:00'),
(50006, 'CUS1013', '2019-06-27T12:00:00'),
(50007, 'CUS1016', '2019-07-05T12:00:00'),
(50008, 'CUS1028', '2019-08-12T12:00:00'),
(50009, 'CUS1035', '2019-09-20T12:00:00'),
(50010, 'CUS1045', '2019-03-12T12:00:00'),
(50011, 'CUS1047', '2019-04-25T12:00:00');

--ConferenceBookingID int NOT NULL,
--ConferenceRoomID varchar(3) NOT NULL,
--EventTypeID tinyint NOT NULL,
--ConferenceBookingDate datetime NOT NULL,
--StaffID varchar(7) NOT NULL,
INSERT INTO ConferenceBookingDetails(ConferenceBookingID, ConferenceRoomID, EventTypeID, ConferenceBookingDate, StaffID) VALUES
(50001, 'CR6', 6, '2019-03-13T12:00:00', 'STA1009' ),
(50005, 'CR1', 4, '2019-05-13T12:00:00', 'STA1010'),
(50006, 'CR2', 4, '2019-06-14T12:00:00', 'STA1011'),
(50007, 'CR3', 4, '2019-06-28T12:00:00', 'STA1012'),
(50008, 'CR4', 4, '2019-08-05T12:00:00', 'STA1013'),
(50009, 'CR5', 4, '2019-09-13T12:00:00', 'STA1014'),
(50010, 'CR7', 5, '2018-05-28T12:00:00', 'STA1011');

