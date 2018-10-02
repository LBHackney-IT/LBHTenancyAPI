CREATE DATABASE StubUH;
GO

USE StubUH;
GO

CREATE TABLE tenagree (tag_ref CHAR(11), prop_ref CHAR(12), cur_bal NUMERIC (9,3), tenure char (3), rent NUMERIC (9,2), service NUMERIC (9,2), other_charge NUMERIC (9,2));
CREATE TABLE araction (tag_ref CHAR(11), action_code CHAR(3), action_type CHAR(3), action_balance NUMERIC(7,2),
                      action_date SMALLDATETIME, action_comment VARCHAR(100), username VARCHAR(40));
CREATE TABLE arag (tag_ref CHAR(11), arag_sid INT, arag_status CHAR(10), arag_startdate SMALLDATETIME,
                  arag_breached BIT, arag_startbal NUMERIC (9,3),
                  arag_clearby SMALLDATETIME);
CREATE TABLE aragdet (arag_sid INT, aragdet_amount NUMERIC(10, 3), aragdet_frequency CHAR(3));
CREATE TABLE contacts (tag_ref CHAR(11), con_name VARCHAR(73), con_address CHAR(200), con_postcode CHAR(10),
                      con_phone1 CHAR(21));
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
	[ethnic_origin] [char](3) NULL,
	[gender] [char](1) NULL,
	[title] [char](10) NULL,
	[initials] [char](3) NULL,
	[forename] [char](24) NULL,
	[surname] [char](20) NULL,
	[age] [numeric](3, 0) NULL,
	[oap] [bit] NOT NULL,
	[relationship] [char](1) NULL,
	[econ_status] [char](1) NULL,
	[responsible] [bit] NOT NULL,
	[wheelch_user] [char](3) NULL,
	[disabled] [char](3) NULL,
	[cl_group_a] [char](3) NULL,
	[cl_group_b] [char](3) NULL,
	[ethnic_colour] [char](3) NULL,
	[at_risk] [bit] NOT NULL,
	[ni_no] [char](12) NULL,
	[full_ed] [bit] NOT NULL,
	[member_sid] [int] NOT NULL,
	[contacts_sid] [int] NULL,
	[tstamp] [timestamp] NULL,
	[comp_avail] [char](200) NULL,
	[comp_display] [char](200) NULL,
	[occupation] [char](3) NULL,
	[asboissued] [bit] NULL,
	[liablemember] [bit] NULL,
	[dob] [dbo].[uhdate] NOT NULL,
	[nationality] [char](3) NULL,
	[ci_surname] [varchar](255) NULL,
	[u_pin_number] [char](20) NULL,
	[ci_title]  AS ([title] collate Latin1_General_CI_AI),
	[ci_forename]  AS ([forename] collate Latin1_General_CI_AI),
	[u_ethnic_other] [char](20) NULL,
	[tenportactcode] [uniqueidentifier] NULL,
	[transgender] [varchar](3) NULL,
	[sex_orient] [varchar](3) NULL,
	[religion_belief] [varchar](3) NULL,
	[marriage_civil] [varchar](3) NULL,
	[first_lang] [varchar](3) NULL,
	[soc_ec_stat] [varchar](3) NULL,
	[soc_class] [varchar](3) NULL,
	[appearance] [varchar](3) NULL,
	[vulnerable] [varchar](3) NULL,
	[hiv_positive] [varchar](3) NULL,
	[crim_rec] [varchar](3) NULL,
	[contact_type] [varchar](3) NULL,
	[corr_type] [varchar](3) NULL,
	[resp_dep] [varchar](3) NULL,
	[pregnant] [bit] NULL,
	[bank_acc_type] [char](3) NOT NULL,
	[homeless] [varchar](3) NULL
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[member] ADD  CONSTRAINT [DF__member__house_re__33F4B129]  DEFAULT (space((1))) FOR [house_ref]
GO

ALTER TABLE [dbo].[member] ADD  CONSTRAINT [DF__member__person_n__34E8D562]  DEFAULT ((0)) FOR [person_no]
GO

ALTER TABLE [dbo].[member] ADD  CONSTRAINT [DF__member__ethnic_o__36D11DD4]  DEFAULT (space((1))) FOR [ethnic_origin]
GO

ALTER TABLE [dbo].[member] ADD  CONSTRAINT [DF__member__gender__37C5420D]  DEFAULT (space((1))) FOR [gender]
GO

ALTER TABLE [dbo].[member] ADD  CONSTRAINT [DF__member__title__38B96646]  DEFAULT (space((1))) FOR [title]
GO

ALTER TABLE [dbo].[member] ADD  CONSTRAINT [DF__member__initials__39AD8A7F]  DEFAULT (space((1))) FOR [initials]
GO

ALTER TABLE [dbo].[member] ADD  CONSTRAINT [DF__member__forename__3AA1AEB8]  DEFAULT (space((1))) FOR [forename]
GO

ALTER TABLE [dbo].[member] ADD  CONSTRAINT [DF__member__surname__3B95D2F1]  DEFAULT (space((1))) FOR [surname]
GO

ALTER TABLE [dbo].[member] ADD  CONSTRAINT [DF__member__age__3C89F72A]  DEFAULT ((0)) FOR [age]
GO

ALTER TABLE [dbo].[member] ADD  CONSTRAINT [DF__member__oap__3D7E1B63]  DEFAULT ((0)) FOR [oap]
GO

ALTER TABLE [dbo].[member] ADD  CONSTRAINT [DF__member__relation__3E723F9C]  DEFAULT (space((1))) FOR [relationship]
GO

ALTER TABLE [dbo].[member] ADD  CONSTRAINT [DF__member__econ_sta__3F6663D5]  DEFAULT (space((1))) FOR [econ_status]
GO

ALTER TABLE [dbo].[member] ADD  CONSTRAINT [DF__member__responsi__405A880E]  DEFAULT ((0)) FOR [responsible]
GO

ALTER TABLE [dbo].[member] ADD  CONSTRAINT [DF__member__wheelch___414EAC47]  DEFAULT (space((1))) FOR [wheelch_user]
GO

ALTER TABLE [dbo].[member] ADD  CONSTRAINT [DF__member__disabled__4242D080]  DEFAULT (space((1))) FOR [disabled]
GO

ALTER TABLE [dbo].[member] ADD  CONSTRAINT [DF__member__cl_group__4336F4B9]  DEFAULT (space((1))) FOR [cl_group_a]
GO

ALTER TABLE [dbo].[member] ADD  CONSTRAINT [DF__member__cl_group__442B18F2]  DEFAULT (space((1))) FOR [cl_group_b]
GO

ALTER TABLE [dbo].[member] ADD  CONSTRAINT [DF__member__ethnic_c__451F3D2B]  DEFAULT (space((1))) FOR [ethnic_colour]
GO

ALTER TABLE [dbo].[member] ADD  CONSTRAINT [DF__member__at_risk__46136164]  DEFAULT ((0)) FOR [at_risk]
GO

ALTER TABLE [dbo].[member] ADD  CONSTRAINT [DF__member__ni_no__4707859D]  DEFAULT (space((1))) FOR [ni_no]
GO

ALTER TABLE [dbo].[member] ADD  CONSTRAINT [DF__member__full_ed__47FBA9D6]  DEFAULT ((0)) FOR [full_ed]
GO

ALTER TABLE [dbo].[member] ADD  CONSTRAINT [DF__member__member_s__7E6D9477]  DEFAULT ((0)) FOR [member_sid]
GO

ALTER TABLE [dbo].[member] ADD  CONSTRAINT [DF__member__contacts__03C811C8]  DEFAULT ((0)) FOR [contacts_sid]
GO

ALTER TABLE [dbo].[member] ADD  CONSTRAINT [DF__member__comp_ava__6D705303]  DEFAULT ('') FOR [comp_avail]
GO

ALTER TABLE [dbo].[member] ADD  CONSTRAINT [DF__member__comp_dis__6E64773C]  DEFAULT ('') FOR [comp_display]
GO

ALTER TABLE [dbo].[member] ADD  CONSTRAINT [DF__member__occupati__06910688]  DEFAULT ('') FOR [occupation]
GO

ALTER TABLE [dbo].[member] ADD  CONSTRAINT [DF__member__asboissu__2F931C1B]  DEFAULT ((0)) FOR [asboissued]
GO

ALTER TABLE [dbo].[member] ADD  CONSTRAINT [DF__member__liableme__30874054]  DEFAULT ((0)) FOR [liablemember]
GO

ALTER TABLE [dbo].[member] ADD  CONSTRAINT [DF__member__dob2__15F41100]  DEFAULT ('') FOR [dob]
GO

ALTER TABLE [dbo].[member] ADD  CONSTRAINT [DF__member__national__212697E1]  DEFAULT (space((1))) FOR [nationality]
GO

ALTER TABLE [dbo].[member] ADD  CONSTRAINT [DF_member_u_pin_number]  DEFAULT (space((1))) FOR [u_pin_number]
GO

ALTER TABLE [dbo].[member] ADD  CONSTRAINT [DF_member_u_ethnic_other]  DEFAULT (space((1))) FOR [u_ethnic_other]
GO

ALTER TABLE [dbo].[member] ADD  DEFAULT ('00000000-0000-0000-0000-000000000000') FOR [tenportactcode]
GO

ALTER TABLE [dbo].[member] ADD  DEFAULT ('') FOR [bank_acc_type]
GO

ALTER TABLE [dbo].[member] ADD  DEFAULT (space((0))) FOR [homeless]
GO

GO

