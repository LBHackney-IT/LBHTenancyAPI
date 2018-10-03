CREATE DATABASE StubUH;
GO

USE StubUH;
GO

CREATE TABLE [dbo].[tenagree](
	[tag_ref] [char](11) NOT NULL,
	[prop_ref] [char](12) NULL,
	[house_ref] [char](10) NULL,
	[tag_desc] [char](200) NULL,
	[prd_sno] [int] NULL,
	[cot] [smalldatetime] NULL,
	[eot] [smalldatetime] NULL,
	[tenure] [char](3) NULL,
	[prd_code] [char](2) NULL,
	[spec_terms] [bit] NOT NULL,
	[other_accounts] [bit] NOT NULL,
	[active] [bit] NOT NULL,
	[present] [bit] NOT NULL,
	[terminated] [bit] NOT NULL,
	[free_active] [bit] NOT NULL,
	[nop] [bit] NOT NULL,
	[ra_date] [smalldatetime] NULL,
	[rentgrp_ref] [char](3) NULL,
	[succession_date] [smalldatetime] NULL,
	[ori_rent] [numeric](9, 2) NULL,
	[ori_service] [numeric](9, 2) NULL,
	[rent] [numeric](9, 2) NULL,
	[service] [numeric](9, 2) NULL,
	[other_charge] [numeric](9, 2) NULL,
	[differential] [numeric](9, 2) NULL,
	[tenancy_rent] [numeric](9, 2) NULL,
	[tenancy_service] [numeric](9, 2) NULL,
	[tenancy_other] [numeric](9, 2) NULL,
	[cur_bal] [numeric](9, 2) NULL,
	[cur_nr_bal] [numeric](9, 2) NULL,
	[additional_debit] [bit] NOT NULL,
	[occ_status] [char](3) NULL,
	[master_tag] [char](11) NULL,
	[prdno_on_vac] [int] NULL,
	[year_on_vac] [int] NULL,
	[occ_phase] [int] NULL,
	[hb_expire] [smalldatetime] NULL,
	[ass_date] [smalldatetime] NULL,
	[fd_charge] [bit] NOT NULL,
	[hb_freq] [char](3) NULL,
	[reason_term] [char](3) NULL,
	[receiptcard] [bit] NOT NULL,
	[recgrossorder] [text] NULL,
	[lastgrosscol] [numeric](1, 0) NULL,
	[lastreccol] [numeric](1, 0) NULL,
	[lastrecline] [numeric](2, 0) NULL,
	[cardbal] [numeric](9, 2) NULL,
	[recstatus] [numeric](1, 0) NULL,
	[curcardno] [numeric](3, 0) NULL,
	[recgrosdate] [smalldatetime] NULL,
	[cur_action_set] [int] NULL,
	[cur_action_no] [int] NULL,
	[tag_action] [char](3) NULL,
	[agr_type] [char](1) NULL,
	[rech_tag_ref] [char](11) NULL,
	[master_tag_ref] [char](11) NULL,
	[sup_ref] [char](12) NULL,
	[nosp] [bit] NOT NULL,
	[ntq] [bit] NOT NULL,
	[eviction] [bit] NOT NULL,
	[committee] [bit] NOT NULL,
	[suppossorder] [bit] NOT NULL,
	[possorder] [bit] NOT NULL,
	[courtapp] [bit] NOT NULL,
	[nospexpire] [smalldatetime] NULL,
	[courtdate] [smalldatetime] NULL,
	[ntqexpire] [smalldatetime] NULL,
	[visitdate] [smalldatetime] NULL,
	[tenure_ori] [char](3) NULL,
	[occ_phase_ori] [int] NULL,
	[open_item] [bit] NOT NULL,
	[allocation_method] [char](3) NULL,
	[man_scheme] [char](11) NULL,
	[anal_method] [char](1) NULL,
	[inv_type] [char](1) NULL,
	[con_key] [int] NULL,
	[major_phase] [int] NULL,
	[forwardaddress] [text] NULL,
	[acc_type] [char](1) NULL,
	[tenagree_sid] [int] NULL,
	[noticegiven] [bit] NULL,
	[potentialenddate] [smalldatetime] NOT NULL,
	[rtb_date] [smalldatetime] NULL,
	[rtb_issued_by] [char](30) NULL,
	[rtb_year] [int] NULL,
	[rtb_work] [char](40) NULL,
	[rtb_amount] [int] NULL,
	[rtb_project] [char](20) NULL,
	[rtb_recharge] [numeric](11, 2) NULL,
	[rtb_budget] [numeric](11, 2) NULL,
	[last_action_date] [smalldatetime] NULL,
	[last_action] [char](3) NULL,
	[high_action_date] [smalldatetime] NULL,
	[high_action] [char](3) NULL,
	[last_balance] [numeric](10, 2) NULL,
	[tag_action_date] [smalldatetime] NULL,
	[ent_act_status] [char](3) NULL,
	[monitoring] [char](1) NULL,
	[monit_date] [smalldatetime] NULL,
	[monit_prd_type] [char](1) NULL,
	[next_monit_date] [smalldatetime] NULL,
	[process_group_id] [int] NULL,
	[arrears_case] [bit] NULL,
	[cur_araction_sid] [int] NULL,
	[pmandata] [text] NULL,
	[cur_action_subno] [int] NULL,
	[collect_cash] [bit] NULL,
	[tstamp] [timestamp] NULL,
	[evictdate] [smalldatetime] NULL,
	[lettertext] [text] NULL,
	[core_ver] [char](10) NULL,
	[w2propactiondate] [smalldatetime] NULL,
	[rtb_effective] [smalldatetime] NULL,
	[rtb_term] [smalldatetime] NULL,
	[s125_issued] [bit] NULL,
	[comp_avail] [char](200) NULL,
	[comp_display] [char](200) NULL,
	[revdatann] [char](3) NULL,
	[phased] [bit] NULL,
	[ten_b_forward] [numeric](10, 2) NULL,
	[vm_propref] [char](16) NULL,
	[noticegiven_dt] [smalldatetime] NULL,
	[keysrecd_dt] [smalldatetime] NULL,
	[u_rent_round] [char](3) NULL,
	[u_rent_patch] [char](3) NULL,
	[u_acc_closed_by] [char](3) NULL,
	[u_rent_card_printed] [datetime] NULL,
	[u_financial_group] [char](3) NULL,
	[u_inhibit_statements] [bit] NULL,
	[u_inhibit_writeoffs] [bit] NULL,
	[u_oracle_hb_int] [bit] NULL,
	[u_message] [text] NULL,
	[u_nok_relationship] [char](3) NULL,
	[u_account_type] [char](3) NULL,
	[u_rent_zone] [char](3) NULL,
	[u_rent_subzone] [char](3) NULL,
	[u_letting_date] [datetime] NULL,
	[u_comments] [text] NULL,
	[u_part_period_option] [bit] NULL,
	[u_intro_start_date] [datetime] NULL,
	[u_intro_end_date] [datetime] NULL,
	[u_sublet] [bit] NULL,
	[u_saff_rentacc] [char](20) NULL,
	[u_saff_tenancy] [char](12) NULL,
	[u_inform_hb] [bit] NULL,
	[u_sublet_end] [smalldatetime] NULL,
	[u_notice_served] [smalldatetime] NULL,
	[u_notice_effective] [smalldatetime] NULL,
	[u_bal_dispute] [numeric](10, 2) NULL,
	[u_money_judgement] [numeric](10, 2) NULL,
	[u_referred_legal] [numeric](10, 2) NULL,
	[u_mw_payment_due] [smalldatetime] NULL,
	[u_periods_in_arrears] [numeric](4, 0) NULL,
	[u_complaint_active] [bit] NULL,
	[u_pay_by_book] [bit] NULL,
	[u_new_book] [bit] NULL,
	[u_charging_order] [numeric](10, 2) NULL,
	[u_notice_type] [char](3) NULL,
	[u_court_outcome] [char](3) NULL,
	[u_notice_expiry] [smalldatetime] NULL,
	[u_saff_auto] [bit] NULL,
	[u_nom2] [char](10) NULL,
	[u_payment_expected] [char](3) NOT NULL,
	[u_ignore_arr_policy] [bit] NULL,
	[dtstamp] [smalldatetime] NOT NULL,
	[intro_date] [smalldatetime] NOT NULL,
	[intro_ext_date] [smalldatetime] NOT NULL,
	[tenpay_freq] [char](3) NULL,
	[tenpay_start] [smalldatetime] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__tag_r__2BAB1A99]  DEFAULT (space((1))) FOR [tag_ref]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__prop___2C9F3ED2]  DEFAULT (space((1))) FOR [prop_ref]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__house__2D93630B]  DEFAULT (space((1))) FOR [house_ref]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__tag_d__2E878744]  DEFAULT (space((1))) FOR [tag_desc]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__prd_s__306FCFB6]  DEFAULT ((0)) FOR [prd_sno]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagree__cot__3163F3EF]  DEFAULT ('') FOR [cot]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagree__eot__32581828]  DEFAULT ('') FOR [eot]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__tenur__334C3C61]  DEFAULT (space((1))) FOR [tenure]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__prd_c__3440609A]  DEFAULT (space((1))) FOR [prd_code]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__spec___353484D3]  DEFAULT ((0)) FOR [spec_terms]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__other__3628A90C]  DEFAULT ((0)) FOR [other_accounts]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__activ__371CCD45]  DEFAULT ((0)) FOR [active]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__prese__3810F17E]  DEFAULT ((0)) FOR [present]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__termi__390515B7]  DEFAULT ((0)) FOR [terminated]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__free___39F939F0]  DEFAULT ((1)) FOR [free_active]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagree__nop__3AED5E29]  DEFAULT ((0)) FOR [nop]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__ra_da__3BE18262]  DEFAULT ('') FOR [ra_date]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__rentg__3CD5A69B]  DEFAULT (space((1))) FOR [rentgrp_ref]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__succe__3DC9CAD4]  DEFAULT ('') FOR [succession_date]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__ori_r__3EBDEF0D]  DEFAULT ((0)) FOR [ori_rent]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__ori_s__3FB21346]  DEFAULT ((0)) FOR [ori_service]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagree__rent__40A6377F]  DEFAULT ((0)) FOR [rent]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__servi__419A5BB8]  DEFAULT ((0)) FOR [service]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__other__428E7FF1]  DEFAULT ((0)) FOR [other_charge]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__diffe__4382A42A]  DEFAULT ((0)) FOR [differential]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__tenan__4476C863]  DEFAULT ((0)) FOR [tenancy_rent]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__tenan__456AEC9C]  DEFAULT ((0)) FOR [tenancy_service]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__tenan__465F10D5]  DEFAULT ((0)) FOR [tenancy_other]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__cur_b__4753350E]  DEFAULT ((0)) FOR [cur_bal]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__cur_n__48475947]  DEFAULT ((0)) FOR [cur_nr_bal]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__addit__493B7D80]  DEFAULT ((0)) FOR [additional_debit]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__occ_s__4A2FA1B9]  DEFAULT (space((1))) FOR [occ_status]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__maste__4B23C5F2]  DEFAULT (space((1))) FOR [master_tag]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__prdno__4C17EA2B]  DEFAULT ((0)) FOR [prdno_on_vac]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__year___4D0C0E64]  DEFAULT ((0)) FOR [year_on_vac]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__occ_p__4E00329D]  DEFAULT ((0)) FOR [occ_phase]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__hb_ex__4EF456D6]  DEFAULT ('') FOR [hb_expire]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__ass_d__4FE87B0F]  DEFAULT ('') FOR [ass_date]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__fd_ch__50DC9F48]  DEFAULT ((1)) FOR [fd_charge]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__hb_fr__51D0C381]  DEFAULT (space((1))) FOR [hb_freq]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__reaso__52C4E7BA]  DEFAULT (space((1))) FOR [reason_term]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__recei__53B90BF3]  DEFAULT ((0)) FOR [receiptcard]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__recgr__54AD302C]  DEFAULT (space((1))) FOR [recgrossorder]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__lastg__55A15465]  DEFAULT ((0)) FOR [lastgrosscol]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__lastr__5695789E]  DEFAULT ((0)) FOR [lastreccol]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__lastr__57899CD7]  DEFAULT ((0)) FOR [lastrecline]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__cardb__587DC110]  DEFAULT ((0)) FOR [cardbal]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__recst__5971E549]  DEFAULT ((0)) FOR [recstatus]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__curca__5A660982]  DEFAULT ((0)) FOR [curcardno]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__recgr__5B5A2DBB]  DEFAULT ('') FOR [recgrosdate]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__cur_a__5C4E51F4]  DEFAULT ((0)) FOR [cur_action_set]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__cur_a__5D42762D]  DEFAULT ((0)) FOR [cur_action_no]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__tag_a__5E369A66]  DEFAULT (space((1))) FOR [tag_action]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__agr_t__5F2ABE9F]  DEFAULT (space((1))) FOR [agr_type]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__rech___601EE2D8]  DEFAULT (space((1))) FOR [rech_tag_ref]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__maste__61130711]  DEFAULT (space((1))) FOR [master_tag_ref]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__sup_r__62072B4A]  DEFAULT (space((1))) FOR [sup_ref]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagree__nosp__62FB4F83]  DEFAULT ((0)) FOR [nosp]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagree__ntq__63EF73BC]  DEFAULT ((0)) FOR [ntq]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__evict__64E397F5]  DEFAULT ((0)) FOR [eviction]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__commi__65D7BC2E]  DEFAULT ((0)) FOR [committee]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__suppo__66CBE067]  DEFAULT ((0)) FOR [suppossorder]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__posso__67C004A0]  DEFAULT ((0)) FOR [possorder]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__court__68B428D9]  DEFAULT ((0)) FOR [courtapp]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__nospe__69A84D12]  DEFAULT ('') FOR [nospexpire]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__court__6A9C714B]  DEFAULT ('') FOR [courtdate]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__ntqex__6B909584]  DEFAULT ('') FOR [ntqexpire]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__visit__6C84B9BD]  DEFAULT ('') FOR [visitdate]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__tenur__6D78DDF6]  DEFAULT (space((1))) FOR [tenure_ori]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__occ_p__6E6D022F]  DEFAULT ((0)) FOR [occ_phase_ori]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__open___6F612668]  DEFAULT ((0)) FOR [open_item]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__alloc__70554AA1]  DEFAULT (space((1))) FOR [allocation_method]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__man_s__71496EDA]  DEFAULT (space((1))) FOR [man_scheme]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__anal___723D9313]  DEFAULT (space((1))) FOR [anal_method]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__inv_t__7331B74C]  DEFAULT (' ') FOR [inv_type]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__con_k__7425DB85]  DEFAULT ((0)) FOR [con_key]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__major__7519FFBE]  DEFAULT ((0)) FOR [major_phase]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__forwa__760E23F7]  DEFAULT (space((1))) FOR [forwardaddress]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__acc_t__77024830]  DEFAULT (space((1))) FOR [acc_type]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__tenag__77F66C69]  DEFAULT ((0)) FOR [tenagree_sid]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__notic__78EA90A2]  DEFAULT ((0)) FOR [noticegiven]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__poten__79DEB4DB]  DEFAULT ('') FOR [potentialenddate]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__rtb_d__7AD2D914]  DEFAULT ('') FOR [rtb_date]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__rtb_i__7BC6FD4D]  DEFAULT (space((1))) FOR [rtb_issued_by]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__rtb_y__7CBB2186]  DEFAULT ((0)) FOR [rtb_year]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__rtb_w__7DAF45BF]  DEFAULT (space((1))) FOR [rtb_work]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__rtb_a__7EA369F8]  DEFAULT ((0)) FOR [rtb_amount]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__rtb_p__7F978E31]  DEFAULT (space((1))) FOR [rtb_project]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__rtb_r__008BB26A]  DEFAULT ((0)) FOR [rtb_recharge]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__rtb_b__017FD6A3]  DEFAULT ((0)) FOR [rtb_budget]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__last___0273FADC]  DEFAULT ('') FOR [last_action_date]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__last___03681F15]  DEFAULT (space((1))) FOR [last_action]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__high___045C434E]  DEFAULT ('') FOR [high_action_date]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__high___05506787]  DEFAULT (space((1))) FOR [high_action]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__last___06448BC0]  DEFAULT ((0)) FOR [last_balance]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF____tenagre__tag_a__0738AFF9]  DEFAULT ('') FOR [tag_action_date]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF__tenagree__ent_ac__04BC3601]  DEFAULT ('') FOR [ent_act_status]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF__tenagree__monito__05B05A3A]  DEFAULT ('') FOR [monitoring]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF__tenagree__monit___06A47E73]  DEFAULT ('') FOR [monit_date]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF__tenagree__monit___0798A2AC]  DEFAULT ('') FOR [monit_prd_type]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF__tenagree__next_m__088CC6E5]  DEFAULT ('') FOR [next_monit_date]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF__tenagree__proces__0980EB1E]  DEFAULT ((0)) FOR [process_group_id]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF__tenagree__arrear__0A750F57]  DEFAULT ((0)) FOR [arrears_case]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF__tenagree__cur_ar__0B693390]  DEFAULT ((0)) FOR [cur_araction_sid]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF__tenagree__pmanda__0C5D57C9]  DEFAULT ('') FOR [pmandata]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF__tenagree__cur_ac__0D517C02]  DEFAULT ((0)) FOR [cur_action_subno]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF__tenagree__collec__4828290D]  DEFAULT ((1)) FOR [collect_cash]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF__tenagree__evictd__3026824A]  DEFAULT ('') FOR [evictdate]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF__tenagree__letter__311AA683]  DEFAULT ('') FOR [lettertext]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF__tenagree__core_v__20DA1D19]  DEFAULT ('') FOR [core_ver]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF__tenagree__w2prop__1162CF5F]  DEFAULT ('') FOR [w2propactiondate]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF__tenagree__rtb_ef__2460953A]  DEFAULT ('') FOR [rtb_effective]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF__tenagree__rtb_te__2554B973]  DEFAULT ('') FOR [rtb_term]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF__tenagree__s125_i__2648DDAC]  DEFAULT ((0)) FOR [s125_issued]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF__tenagree__comp_a__2FC82645]  DEFAULT ('') FOR [comp_avail]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF__tenagree__comp_d__30BC4A7E]  DEFAULT ('') FOR [comp_display]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF__tenagree__revdat__66CE426C]  DEFAULT ('') FOR [revdatann]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF__tenagree__phased__67C266A5]  DEFAULT ((0)) FOR [phased]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF__tenagree__ten_b___0F1C2AE8]  DEFAULT ((0)) FOR [ten_b_forward]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF__tenagree__vm_pro__20719F73]  DEFAULT ('') FOR [vm_propref]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF__tenagree__notice__2165C3AC]  DEFAULT ('') FOR [noticegiven_dt]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF__tenagree__keysre__2259E7E5]  DEFAULT ('') FOR [keysrecd_dt]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF_tenagree_u_rent_round]  DEFAULT (space((1))) FOR [u_rent_round]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF_tenagree_u_rent_patch]  DEFAULT (space((1))) FOR [u_rent_patch]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF_tenagree_u_acc_closed_by]  DEFAULT (space((1))) FOR [u_acc_closed_by]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF_tenagree_u_rent_card_printed]  DEFAULT ('') FOR [u_rent_card_printed]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF_tenagree_u_financial_group]  DEFAULT (space((1))) FOR [u_financial_group]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF_tenagree_u_inhibit_statements]  DEFAULT ((0)) FOR [u_inhibit_statements]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF_tenagree_u_inihibit_writeoffs]  DEFAULT ((0)) FOR [u_inhibit_writeoffs]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF_tenagree_u_oracle_hb_interface]  DEFAULT ((0)) FOR [u_oracle_hb_int]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF_tenagree_u_message]  DEFAULT (space((1))) FOR [u_message]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF_tenagree_u_nok_relationship]  DEFAULT (space((1))) FOR [u_nok_relationship]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF_tenagree_u_account_type]  DEFAULT (space((1))) FOR [u_account_type]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF_tenagree_u_rent_zone]  DEFAULT (space((1))) FOR [u_rent_zone]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF_tenagree_u_rent_subzone]  DEFAULT (space((1))) FOR [u_rent_subzone]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF_tenagree_u_letting_date]  DEFAULT ('') FOR [u_letting_date]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF_tenagree_u_comments]  DEFAULT (space((1))) FOR [u_comments]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF_tenagree_u_part_period_option]  DEFAULT ((0)) FOR [u_part_period_option]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF_tenagree_u_intro_start_date]  DEFAULT ('') FOR [u_intro_start_date]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF_tenagree_u_intro_end_date]  DEFAULT ('') FOR [u_intro_end_date]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF_tenagree_u_sublet]  DEFAULT ((0)) FOR [u_sublet]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF_tenagree_u_saff_rentacc]  DEFAULT (space((1))) FOR [u_saff_rentacc]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF_tenagree_u_saff_tenancy]  DEFAULT (space((1))) FOR [u_saff_tenancy]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF_tenagree_u_inform_hb]  DEFAULT ((0)) FOR [u_inform_hb]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF_tenagree_u_sublet_end]  DEFAULT ('') FOR [u_sublet_end]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF_tenagree_u_notice_served]  DEFAULT ('') FOR [u_notice_served]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF_tenagree_u_notice_effective]  DEFAULT ('') FOR [u_notice_effective]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF_tenagree_u_bal_dispute]  DEFAULT ((0)) FOR [u_bal_dispute]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF_tenagree_u_money_judgement]  DEFAULT ((0)) FOR [u_money_judgement]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF_tenagree_u_referred_legal]  DEFAULT ((0)) FOR [u_referred_legal]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF_tenagree_u_mw_payment_due]  DEFAULT ('') FOR [u_mw_payment_due]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF_tenagree_u_periods_in_arrears]  DEFAULT ((0)) FOR [u_periods_in_arrears]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF_tenagree_u_complaint_active]  DEFAULT ((0)) FOR [u_complaint_active]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF_tenagree_u_pay_by_book]  DEFAULT ((0)) FOR [u_pay_by_book]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF_tenagree_u_new_book]  DEFAULT ((0)) FOR [u_new_book]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF_tenagree_u_charging_order]  DEFAULT ((0)) FOR [u_charging_order]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF_tenagree_u_notice_type]  DEFAULT (space((1))) FOR [u_notice_type]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF_tenagree_u_court_outcome]  DEFAULT (space((1))) FOR [u_court_outcome]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF_tenagree_u_notice_expiry]  DEFAULT ((0)) FOR [u_notice_expiry]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF__tenagree__u_saff__4BA70F2D]  DEFAULT ((0)) FOR [u_saff_auto]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF_tenagree_u_nom2]  DEFAULT (space((1))) FOR [u_nom2]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [u_payment_expected]  DEFAULT ('000') FOR [u_payment_expected]
GO

ALTER TABLE [dbo].[tenagree] ADD  CONSTRAINT [DF_tenagree_u_ignore_arr_policy]  DEFAULT ((0)) FOR [u_ignore_arr_policy]
GO

ALTER TABLE [dbo].[tenagree] ADD  DEFAULT (getdate()) FOR [dtstamp]
GO

ALTER TABLE [dbo].[tenagree] ADD  DEFAULT ('') FOR [intro_date]
GO

ALTER TABLE [dbo].[tenagree] ADD  DEFAULT ('') FOR [intro_ext_date]
GO

ALTER TABLE [dbo].[tenagree] ADD  DEFAULT ('') FOR [tenpay_freq]
GO

ALTER TABLE [dbo].[tenagree] ADD  DEFAULT ('') FOR [tenpay_start]
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
	[dob] [datetime] NOT NULL,
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

