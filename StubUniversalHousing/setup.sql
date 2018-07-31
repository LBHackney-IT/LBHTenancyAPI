CREATE DATABASE StubUH;
GO

USE StubUH;
GO

CREATE TABLE tenagree (tag_ref CHAR(11), cur_bal NUMERIC (9,3));
CREATE TABLE araction (tag_ref CHAR(11), action_code CHAR(3),action_code_name CHAR(50),action_balance DECIMAL,
                      action_date SMALLDATETIME,action_comment char(100),uh_username CHAR(50));
CREATE TABLE arag (tag_ref CHAR(11), arag_status CHAR(10), arag_startdate SMALLDATETIME,
                  arag_amount NUMERIC (9,3), arag_frequency CHAR(3), arag_breached BIT, arag_startbal NUMERIC (9,3),
                  arag_clearby SMALLDATETIME);
CREATE TABLE contacts (tag_ref CHAR(11), con_name VARCHAR(73), con_address CHAR(200), con_postcode CHAR(10),
                      con_phone1 CHAR(21));

CREATE TABLE rtrans
(
  prop_ref        char(12)      default space(1),
  tag_ref         char(11)      default space(1),
  trans_type      char(3)       default space(1),
  post_date       smalldatetime default '',
  trans_ref       char(12)      default space(1),
  real_value       numeric(9, 2)
)
GO

