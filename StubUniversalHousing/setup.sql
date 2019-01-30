CREATE DATABASE StubUH;
GO

USE StubUH;
GO

CREATE TABLE [dbo].[tenagree]
(
  [tag_ref]         char(11)        DEFAULT (space((1)))      NOT NULL,
  [prop_ref]        char(12)        DEFAULT (space((1)))      NULL,
  [house_ref]       char(10)        DEFAULT (space((1)))      NULL,
  [cur_bal]         numeric(9, 2)   DEFAULT ((0))             NULL,
  [tenure]          char(3)         DEFAULT (space((1)))      NULL,
  [rent]            numeric(9, 2)   DEFAULT ((0))             NULL,
  [service]         numeric(9, 2)   DEFAULT ((0))             NULL,
  [other_charge]    numeric(9, 2)   DEFAULT ((0))             NULL,
  [u_saff_rentacc]  char(20)        DEFAULT (space((0)))      NULL, -- Payment Reference
  [cot]             smalldatetime   DEFAULT (NULL)            NULL, -- Creation Date
) ON [PRIMARY]

CREATE TABLE araction
(
  araction_sid INT DEFAULT (0),
  tag_ref CHAR(11),
  action_code CHAR(3),
  action_type CHAR(3),
  action_balance NUMERIC(7,2),
  action_date SMALLDATETIME,
  action_comment VARCHAR(100),
  username VARCHAR(40)
);

CREATE TABLE arag
(
  tag_ref           CHAR(11)          DEFAULT (space(1)),
  arag_sid          INT               DEFAULT (0),
  arag_status       CHAR(10)          DEFAULT (space(1)),
  arag_startdate    SMALLDATETIME     DEFAULT (''),
  arag_breached     BIT               DEFAULT (0),
  arag_startbal     NUMERIC (9,3)     DEFAULT (0),
  arag_clearby      SMALLDATETIME     DEFAULT ('')
);

CREATE TABLE aragdet
(
  arag_sid INT,
  aragdet_amount NUMERIC(10, 3),
  aragdet_frequency CHAR(3)
);

CREATE TABLE contacts
(
  tag_ref CHAR(11)        DEFAULT (space((1))),
  con_name VARCHAR(73),
  con_address CHAR(200)   DEFAULT (space((1))),
  con_postcode CHAR(10)   DEFAULT (space((1))),
  con_phone1 CHAR(21)     DEFAULT (space((1)))
);

CREATE TABLE property
(
  short_address CHAR (200),
  address1 CHAR(255),
  prop_ref CHAR (12),
  post_code CHAR (10)
);

CREATE TABLE rtrans
(
  prop_ref CHAR(12) DEFAULT SPACE(1),
  tag_ref CHAR(11) DEFAULT SPACE(1),
  trans_type CHAR(3) DEFAULT SPACE(1),
  post_date SMALLDATETIME DEFAULT '',
  trans_ref CHAR(12) DEFAULT SPACE(1),
  real_value NUMERIC(9, 2)
);

create table debtype
(
  deb_code          char(3)    default space(1) not null,
  deb_desc          char(20)   default space(1),
  deb_cat           char(1)    default space(1),
  deb_link          char(1)    default space(1),
  deb_review        char(3)    default space(1),
  deb_group         numeric(1) default 0,
  vatable           bit default 0          not null,
  apportion         bit default 1          not null,
  freeprd           bit default 1          not null,
  protected_code    char(3)    default space(1),
  differential_code char(3)    default space(1),
  rs_code           char(3)    default space(1),
  deb_effective     char(1)    default 'C',
  deb_vatrate       char(1)    default space(1),
  u_hbeligable      bit default 0          not null,
  debtype_sid       int        default 0,
  tstamp            timestamp              null,
  comp_avail        char(200)  default '',
  comp_display      char(200)  default '',
  def_days          int        default 0,
  void_charge       bit        default 0,
  core_category     char(3)    default space(1)
);
create table rectype
(
  rec_code     char(3) default space(1) not null,
  rec_desc     char(20)   default space(1),
  rec_group    numeric(1) default 0,
  rec_hb       bit default 0          not null,
  rectype_sid  int        default 0,
  tstamp       timestamp              null,
  comp_avail   char(200)  default '',
  comp_display char(200)  default '',
  rec_dd       bit default 0          not null
)

CREATE TABLE [dbo].[member]
(
	[house_ref]   char(10)          DEFAULT (space((1)))    NOT NULL,
	[person_no]   numeric(2, 0)     DEFAULT ((0))           NOT NULL,
	[title]       char(10)          DEFAULT (space((1)))    NULL,
	[forename]    char(24)          DEFAULT (space((1)))    NULL,
	[surname]     char(20)          DEFAULT (space((1)))    NULL,
	[age]         numeric(3, 0)     DEFAULT ((0))           NULL,
	[responsible] bit               DEFAULT ((0))           NOT NULL,
) ON [PRIMARY]
GO

INSERT INTO debtype (deb_code, deb_desc)
VALUES

 ('D20','Section 20 Rebate')
,('D25','Section 125 Rebate')
,('DAT','Assignment SC Trans')
,('DBR','Basic Rent (No VAT)')
,('DCB','Cleaning (Block)')
,('DCC','Court Costs')
,('DCE','Cleaning (Estate)')
,('DCI','Contents Insurance')
,('DCO','Concierge')
,('DCP','Car Port')
,('DCT','Communal Digital TV')
,('DGA','Garage (Attached)')
,('DGM','Grounds Maintenance')
,('DGR','Ground Rent')
,('DHA','Host Amenity')
,('DHE','Heating')
,('DHM','Heating Maintenance')
,('DHW','Hot Water')
,('DIN','Interest')
,('DKF','Lost Key Fobs')
,('DLD','\Legacy Debit')
,('DLK','Lost Key Charge')
,('DLL','Landlord Lighting')
,('DMC','Major Works Capital')
,('DMF','TA Management Fee')
,('DML','Major Works Loan')
,('DMR','Major Works Revenue')
,('DPP','Parking Permits')
,('DRP','Rchrg. Repairs Debit')
,('DRR','Rechargeable Repairs')
,('DSA','SC Adjustment')
,('DSB','SC Balancing Charge')
,('DSC','Service Charges')
,('DST','Storage')
,('DTA','Basic Rent Temp Acc')
,('DTC','Travellers Charge')
,('DTL','Tenants Levy')
,('DTV','Television License')
,('DVA','VAT Charge')
,('DWR','Water Rates')
,('DWS','Water Standing Chrg.')
,('DWW','Watersure Reduction')

INSERT INTO rectype (rec_code, rec_desc)
VALUES
('','')
,('RBA','Bailiff Payment')
,('RBP','Bank Payment')
,('RBR','Post Office Payment')
,('RCI','Rep. Cash Incentive')
,('RCO','Cash Office Payments')
,('RCP','Debit / Credit Card')
,('RDD','Direct Debit')
,('RDN','Direct Debit Unpaid')
,('RDP','Deduction (Work & P)')
,('RDR','BACS Refund')
,('RDS','Deduction (Salary)')
,('RDT','DSS Transfer')
,('REF','Tenant Refund')
,('RHA','HB Adjustment')
,('RHB','Housing Benefit')
,('RIT','Internal Transfer')
,('RML','MW Loan Payment')
,('ROB','\Opening Balance')
,('RPD','Prompt Pay. Discount')
,('RPO','Postal Order')
,('RPY','PayPoint')
,('RQP','Cheque Payments')
,('RRC','Returned Cheque')
,('RRP','Recharge Rep. Credit')
,('RSO','Standing Order')
,('RTM','TMO Reversal')
,('RWA','Rent waiver')
,('WOF','Write Off ')
,('WON','Write On')


