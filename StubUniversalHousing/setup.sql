CREATE DATABASE StubUH;
GO

USE StubUH;
GO

CREATE TABLE [dbo].[tenagree](
  [tag_ref] [char](11) NOT NULL,
  [prop_ref] [char](12) NULL,
  [house_ref] [char](10) NULL,
  [cur_bal] [numeric](9, 2) NULL,
  [tenure] [char](3) NULL,
  [rent] [numeric](9, 2) NULL,
  [service] [numeric](9, 2) NULL,
  [other_charge] [numeric](9, 2) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__tag_r__2BAB1A99]  DEFAULT (space((1))) FOR [tag_ref]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__prop___2C9F3ED2]  DEFAULT (space((1))) FOR [prop_ref]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__house__2D93630B]  DEFAULT (space((1))) FOR [house_ref]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__tenur__334C3C61]  DEFAULT (space((1))) FOR [tenure]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagree__rent__40A6377F]  DEFAULT ((0)) FOR [rent]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__servi__419A5BB8]  DEFAULT ((0)) FOR [service]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__other__428E7FF1]  DEFAULT ((0)) FOR [other_charge]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__cur_b__4753350E]  DEFAULT ((0)) FOR [cur_bal]
GO

CREATE TABLE araction (tag_ref CHAR(11), action_code CHAR(3), action_type CHAR(3), action_balance NUMERIC(7,2),
                      action_date SMALLDATETIME, action_comment VARCHAR(100), username VARCHAR(40));
CREATE TABLE [dbo].[arag](
	[arag_ref] [char](15) NULL,
	[tag_ref] [char](11) NULL,
	[arag_startbal] [numeric](10, 2) NULL,
	[arag_whichbal] [char](3) NULL,
	[arag_startdate] [smalldatetime] NULL,
	[arag_firstno] [int] NULL,
	[arag_firstunit] [char](3) NULL,
	[arag_subno] [int] NULL,
	[arag_subunit] [char](10) NULL,
	[arag_lastcheckbal] [numeric](10, 2) NULL,
	[arag_lastcheckdate] [smalldatetime] NULL,
	[arag_lastexpectedbal] [numeric](10, 2) NULL,
	[arag_breached] [bit] NOT NULL,
	[arag_status] [char](10) NULL,
	[arag_cancelbal] [numeric](10, 2) NULL,
	[arag_statusdate] [smalldatetime] NULL,
	[arag_statususer] [char](3) NULL,
	[arag_amount] [numeric](10, 2) NULL,
	[arag_clearby] [smalldatetime] NULL,
	[arag_frequency] [char](3) NULL,
	[arag_comment] [text] NULL,
	[arag_nextcheck] [smalldatetime] NULL,
	[arag_tolerance] [int] NULL,
	[arag_sid] [int] NULL,
	[arag_pmandata] [text] NULL,
	[tstamp] [timestamp] NULL,
	[comp_avail] [char](200) NULL,
	[comp_display] [char](200) NULL,
	[u_saffron_id] [char](8) NULL,
	[u_saff_rentacc] [char](12) NULL,
	[u_new_book] [bit] NULL,
	[u_pay_start] [smalldatetime] NULL,
	[u_no_payments] [int] NULL,
	[arag_fcadate] [smalldatetime] NULL,
	[arag_noprepay] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[arag] ADD  DEFAULT (space(1)) FOR [arag_ref]
GO

ALTER TABLE [dbo].[arag] ADD  DEFAULT (space(1)) FOR [tag_ref]
GO

ALTER TABLE [dbo].[arag] ADD  DEFAULT (0) FOR [arag_startbal]
GO

ALTER TABLE [dbo].[arag] ADD  DEFAULT (space(1)) FOR [arag_whichbal]
GO

ALTER TABLE [dbo].[arag] ADD  DEFAULT ('') FOR [arag_startdate]
GO

ALTER TABLE [dbo].[arag] ADD  DEFAULT (0) FOR [arag_firstno]
GO

ALTER TABLE [dbo].[arag] ADD  DEFAULT (space(1)) FOR [arag_firstunit]
GO

ALTER TABLE [dbo].[arag] ADD  DEFAULT (0) FOR [arag_subno]
GO

ALTER TABLE [dbo].[arag] ADD  DEFAULT (space(1)) FOR [arag_subunit]
GO

ALTER TABLE [dbo].[arag] ADD  DEFAULT (0) FOR [arag_lastcheckbal]
GO

ALTER TABLE [dbo].[arag] ADD  DEFAULT ('') FOR [arag_lastcheckdate]
GO

ALTER TABLE [dbo].[arag] ADD  DEFAULT (0) FOR [arag_lastexpectedbal]
GO

ALTER TABLE [dbo].[arag] ADD  DEFAULT (0) FOR [arag_breached]
GO

ALTER TABLE [dbo].[arag] ADD  DEFAULT (space(1)) FOR [arag_status]
GO

ALTER TABLE [dbo].[arag] ADD  DEFAULT (0) FOR [arag_cancelbal]
GO

ALTER TABLE [dbo].[arag] ADD  DEFAULT ('') FOR [arag_statusdate]
GO

ALTER TABLE [dbo].[arag] ADD  DEFAULT (space(1)) FOR [arag_statususer]
GO

ALTER TABLE [dbo].[arag] ADD  DEFAULT (0) FOR [arag_amount]
GO

ALTER TABLE [dbo].[arag] ADD  DEFAULT ('') FOR [arag_clearby]
GO

ALTER TABLE [dbo].[arag] ADD  DEFAULT (space(1)) FOR [arag_frequency]
GO

ALTER TABLE [dbo].[arag] ADD  DEFAULT (space(1)) FOR [arag_comment]
GO

ALTER TABLE [dbo].[arag] ADD  DEFAULT ('') FOR [arag_nextcheck]
GO

ALTER TABLE [dbo].[arag] ADD  DEFAULT (0) FOR [arag_tolerance]
GO

ALTER TABLE [dbo].[arag] ADD  DEFAULT (0) FOR [arag_sid]
GO

ALTER TABLE [dbo].[arag] ADD  DEFAULT ('') FOR [arag_pmandata]
GO

ALTER TABLE [dbo].[arag] ADD  DEFAULT ('') FOR [comp_avail]
GO

ALTER TABLE [dbo].[arag] ADD  DEFAULT ('') FOR [comp_display]
GO

ALTER TABLE [dbo].[arag] ADD  CONSTRAINT [DF_arag_u_saffron_id]  DEFAULT (space((1))) FOR [u_saffron_id]
GO

ALTER TABLE [dbo].[arag] ADD  CONSTRAINT [DF_arag_u_saff_rentacc]  DEFAULT (space((1))) FOR [u_saff_rentacc]
GO

ALTER TABLE [dbo].[arag] ADD  CONSTRAINT [DF_arag_u_new_book]  DEFAULT ((0)) FOR [u_new_book]
GO

ALTER TABLE [dbo].[arag] ADD  CONSTRAINT [DF_arag_u_pay_start]  DEFAULT ('') FOR [u_pay_start]
GO

ALTER TABLE [dbo].[arag] ADD  CONSTRAINT [DF_arag_u_no_payments]  DEFAULT ((0)) FOR [u_no_payments]
GO

ALTER TABLE [dbo].[arag] ADD  DEFAULT ('') FOR [arag_fcadate]
GO

ALTER TABLE [dbo].[arag] ADD  DEFAULT ((0)) FOR [arag_noprepay]
GO



CREATE TABLE aragdet (arag_sid INT, aragdet_amount NUMERIC(10, 3), aragdet_frequency CHAR(3));
CREATE TABLE [dbo].[contacts](
	[con_key] [int] NULL,
	[con_ref] [char](12) NULL,
	[con_name] [varchar](73) NULL,
	[con_address] [char](200) NULL,
	[con_phone1] [char](21) NULL,
	[con_phone2] [char](21) NULL,
	[con_phone3] [char](21) NULL,
	[con_postcode] [char](10) NULL,
	[con_type] [char](1) NULL,
	[tag_ref] [char](11) NULL,
	[prop_ref] [char](12) NULL,
	[email_address] [char](50) NULL,
	[app_ref] [char](10) NULL,
	[contacts_sid] [int] NULL,
	[intreason] [char](3) NULL,
	[vunreason] [char](3) NULL,
	[locreason] [char](3) NULL,
	[intcomment] [text] NULL,
	[vuncomment] [text] NULL,
	[loccomment] [text] NULL,
	[tstamp] [timestamp] NULL,
	[comp_avail] [char](200) NULL,
	[comp_display] [char](200) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[contacts] ADD  CONSTRAINT [DF__contacts__con_ke__0F2D40CE]  DEFAULT ((0)) FOR [con_key]
GO

ALTER TABLE [dbo].[contacts] ADD  CONSTRAINT [DF__contacts__con_re__10216507]  DEFAULT (space((1))) FOR [con_ref]
GO

ALTER TABLE [dbo].[contacts] ADD  CONSTRAINT [DF__contacts__con_ad__1209AD79]  DEFAULT (space((1))) FOR [con_address]
GO

ALTER TABLE [dbo].[contacts] ADD  CONSTRAINT [DF__contacts__con_ph__12FDD1B2]  DEFAULT (space((1))) FOR [con_phone1]
GO

ALTER TABLE [dbo].[contacts] ADD  CONSTRAINT [DF__contacts__con_ph__13F1F5EB]  DEFAULT (space((1))) FOR [con_phone2]
GO

ALTER TABLE [dbo].[contacts] ADD  CONSTRAINT [DF__contacts__con_ph__14E61A24]  DEFAULT (space((1))) FOR [con_phone3]
GO

ALTER TABLE [dbo].[contacts] ADD  CONSTRAINT [DF__contacts__con_po__15DA3E5D]  DEFAULT (space((1))) FOR [con_postcode]
GO

ALTER TABLE [dbo].[contacts] ADD  CONSTRAINT [DF__contacts__con_ty__16CE6296]  DEFAULT (space((1))) FOR [con_type]
GO

ALTER TABLE [dbo].[contacts] ADD  CONSTRAINT [DF__contacts__tag_re__17C286CF]  DEFAULT (space((1))) FOR [tag_ref]
GO

ALTER TABLE [dbo].[contacts] ADD  CONSTRAINT [DF__contacts__prop_r__18B6AB08]  DEFAULT (space((1))) FOR [prop_ref]
GO

ALTER TABLE [dbo].[contacts] ADD  CONSTRAINT [DF__contacts__email___19AACF41]  DEFAULT (space((1))) FOR [email_address]
GO

ALTER TABLE [dbo].[contacts] ADD  CONSTRAINT [DF__contacts__app_re__1A9EF37A]  DEFAULT (space((1))) FOR [app_ref]
GO

ALTER TABLE [dbo].[contacts] ADD  CONSTRAINT [DF__contacts__contac__41648637]  DEFAULT ((0)) FOR [contacts_sid]
GO

ALTER TABLE [dbo].[contacts] ADD  CONSTRAINT [DF__contacts__intrea__17442F39]  DEFAULT (space((1))) FOR [intreason]
GO

ALTER TABLE [dbo].[contacts] ADD  CONSTRAINT [DF__contacts__vunrea__18385372]  DEFAULT (space((1))) FOR [vunreason]
GO

ALTER TABLE [dbo].[contacts] ADD  CONSTRAINT [DF__contacts__locrea__192C77AB]  DEFAULT (space((1))) FOR [locreason]
GO

ALTER TABLE [dbo].[contacts] ADD  CONSTRAINT [DF__contacts__intcom__1A209BE4]  DEFAULT (space((1))) FOR [intcomment]
GO

ALTER TABLE [dbo].[contacts] ADD  CONSTRAINT [DF__contacts__vuncom__1B14C01D]  DEFAULT (space((1))) FOR [vuncomment]
GO

ALTER TABLE [dbo].[contacts] ADD  CONSTRAINT [DF__contacts__loccom__1C08E456]  DEFAULT (space((1))) FOR [loccomment]
GO

ALTER TABLE [dbo].[contacts] ADD  CONSTRAINT [DF__contacts__comp_a__7175EE11]  DEFAULT ('') FOR [comp_avail]
GO

ALTER TABLE [dbo].[contacts] ADD  CONSTRAINT [DF__contacts__comp_d__726A124A]  DEFAULT ('') FOR [comp_display]
GO
CREATE TABLE property (short_address CHAR (200), address1 CHAR(255), prop_ref CHAR (12), post_code CHAR (10));
CREATE TABLE rtrans (prop_ref CHAR(12) DEFAULT SPACE(1), tag_ref CHAR(11) DEFAULT SPACE(1), trans_type CHAR(3) DEFAULT SPACE(1),
                     post_date SMALLDATETIME DEFAULT '', trans_ref CHAR(12) DEFAULT SPACE(1), real_value NUMERIC(9, 2))
create table debtype
(
  deb_code          char(3) default space(1) not null,
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

CREATE TABLE [dbo].[member](
	[house_ref] [char](10) NOT NULL,
	[person_no] [numeric](2, 0) NOT NULL,
	[title] [char](10) NULL,
	[forename] [char](24) NULL,
	[surname] [char](20) NULL,
	[age] [numeric](3, 0) NULL,
	[responsible] [bit] NOT NULL,
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[member] ADD  CONSTRAINT [DF__member__house_re__33F4B129]  DEFAULT (space((1))) FOR [house_ref]
GO

ALTER TABLE [dbo].[member] ADD  CONSTRAINT [DF__member__person_n__34E8D562]  DEFAULT ((0)) FOR [person_no]
GO


ALTER TABLE [dbo].[member] ADD  CONSTRAINT [DF__member__title__38B96646]  DEFAULT (space((1))) FOR [title]
GO


ALTER TABLE [dbo].[member] ADD  CONSTRAINT [DF__member__forename__3AA1AEB8]  DEFAULT (space((1))) FOR [forename]
GO

ALTER TABLE [dbo].[member] ADD  CONSTRAINT [DF__member__surname__3B95D2F1]  DEFAULT (space((1))) FOR [surname]
GO


ALTER TABLE [dbo].[member] ADD  CONSTRAINT [DF__member__responsi__405A880E]  DEFAULT ((0)) FOR [responsible]
GO

