CREATE DATABASE StubUH;
GO
USE StubUH;
GO
CREATE TABLE tenagree (tag_ref CHAR(11), cur_bal NUMERIC (9,3));
CREATE TABLE araction (tag_ref CHAR(11), action_code CHAR(3), action_date SMALLDATETIME);
CREATE TABLE arag (tag_ref CHAR(11), arag_status CHAR(10), arag_startdate SMALLDATETIME);
CREATE TABLE contacts (tag_ref CHAR(11), con_name VARCHAR(73), con_address CHAR(200), con_postcode CHAR(10), con_phone1 CHAR(21));
GO
