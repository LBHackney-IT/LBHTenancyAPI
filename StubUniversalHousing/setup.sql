CREATE DATABASE StubUH;
GO
USE StubUH;
GO
CREATE TABLE tenagree (tag_ref NVARCHAR(MAX), cur_bal FLOAT);
CREATE TABLE araction (tag_ref NVARCHAR(MAX), action_code NVARCHAR(MAX), action_date SMALLDATETIME);
CREATE TABLE arag (tag_ref NVARCHAR(MAX), arag_status NVARCHAR(MAX), start_date SMALLDATETIME);
CREATE TABLE contacts (tag_ref NVARCHAR(MAX), con_name NVARCHAR(MAX), con_address NVARCHAR(MAX), con_postcode NVARCHAR(MAX), con_phone1 NVARCHAR(MAX));

