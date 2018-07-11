CREATE DATABASE StubUH;
GO
USE StubUH;
GO
CREATE TABLE tenagree (tag_ref VARCHAR(11), cur_bal DECIMAL(9,3));
CREATE TABLE araction (tag_ref VARCHAR(11), action_code VARCHAR(3), action_date SMALLDATETIME);
CREATE TABLE arag (tag_ref VARCHAR(11), arag_status VARCHAR(10), arag_startdate SMALLDATETIME);
CREATE TABLE contacts (tag_ref VARCHAR(11), con_name VARCHAR(73), con_address VARCHAR(200), con_postcode VARCHAR(10), con_phone1 VARCHAR(21));
GO
