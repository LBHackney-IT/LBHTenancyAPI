CREATE DATABASE StubUH;
GO

USE StubUH;
GO

CREATE TABLE tenagree (tag_ref CHAR(11), prop_ref CHAR(12), cur_bal NUMERIC (9,3), tenure char (3), rent NUMERIC (9,2), service NUMERIC (9,2), other_charge NUMERIC (9,2));
CREATE TABLE araction (tag_ref CHAR(11), action_code CHAR(3), action_type CHAR(3), action_balance NUMERIC(7,2),
                      action_date SMALLDATETIME, action_comment VARCHAR(100), username VARCHAR(40));
CREATE TABLE arag (tag_ref CHAR(11), arag_status CHAR(10), arag_startdate SMALLDATETIME,
                  arag_amount NUMERIC (9,3), arag_frequency CHAR(3), arag_breached BIT, arag_startbal NUMERIC (9,3),
                  arag_clearby SMALLDATETIME);
CREATE TABLE contacts (tag_ref CHAR(11), con_name VARCHAR(73), con_address CHAR(200), con_postcode CHAR(10),
                      con_phone1 CHAR(21));
CREATE TABLE property (short_address CHAR (200), address1 CHAR(255), prop_ref CHAR (12), post_code CHAR (10));
CREATE TABLE rtrans (prop_ref CHAR(12) DEFAULT SPACE(1), tag_ref CHAR(11) DEFAULT SPACE(1), trans_type CHAR(3) DEFAULT SPACE(1),
                     post_date SMALLDATETIME DEFAULT '', trans_ref CHAR(12) DEFAULT SPACE(1), real_value NUMERIC(9, 2))
GO

