
create FUNCTION [dbo].[fgetEmailSendINV_Error_ByPlant] 
(
	@plant varchar(10)
)
RETURNS varchar(500)
AS
BEGIN
	DECLARE @Result varchar(500)
	select top 1 @Result=isnull(email_INV_Error,'') from [TBL_CST_INVOICE_CONFIG] b where b.plant=@plant
  	RETURN @Result
END
go

  create proc SPP_CUST_INVOICE_DUPLICATED_ERUN
  @red_invoice varchar(10),
  @serial_no varchar(10),
  @plant varchar(10)
  as
  select a.invoice_no as Billing,a.cust_code,a.plant,a.invoice_date as BillingDate,a.BU
  ,dbo.fgetEmailSendINV_Error_ByPlant(@plant) as emailerror
   from [TBL_CST_INVOICE] a
   where a.red_invoice=@red_invoice and a.serial_no=@serial_no

go
--declare @date datetime
--set @date=getdate()
--exec SPP_CST_INVOICE_INSERT_TEST '1211131','22703232323','Sales','BC17E','9999999','VNS3','\BC17E_0000001_2270086193_121096952_VNS3.pdf',@date
alter PROCEDURE [dbo].[SPP_CST_INVOICE_INSERT]
	@cust_code nvarchar(50),
	@invoice_no nvarchar(50),
	@invoice_type nvarchar(50),
	@red_invoice nvarchar(50),
	@serial_no nvarchar(50),
	@plant nvarchar(50),
	@atmid nvarchar(max),
	@created_date datetime
AS
BEGIN
	DECLARE @invoice_id decimal(18,0), @ems_id int, @bu nvarchar(10)

	SELECT TOP 1 @bu=bu FROM TBL_CST_INVOICE_CONFIG WHERE plant=@Plant

	--Khuyen them de check trung so erun 22Apr2019
	declare @billno varchar(20)
	declare @cust_code_exits varchar(20)
	declare @BillingDate datetime
	declare @plante varchar(10)
	declare @bue varchar(10)
	select top 1 @billno=a.invoice_no,@cust_code_exits=a.cust_code,@plante=a.plant,@BillingDate=a.invoice_date,@bue=a.BU from [TBL_CST_INVOICE] a with (NOLOCK)  where a.red_invoice=@red_invoice and a.serial_no=@serial_no and invoice_type=@invoice_type and a.invoice_no<>@invoice_no
	if (@billno is not null)
   begin
	 declare @note nvarchar(500)
	 set @note='Duplicated ERUN with: Bill-'+@billno+' Date-'+isnull(convert(varchar(10),@BillingDate,103),'')+' Cust-'+@cust_code_exits+' Plant-'+@plante
	 	
		
		INSERT INTO [TBL_CST_INVOICE_DUPLICATE]
			   ([cust_code]
			   ,[invoice_no]
			   ,[invoice_type]
			   ,[red_invoice]
			   ,[serial_no]
			   ,[plant]
			   ,[atmid]
			   ,[created_date]
			   ,[last_updated],Note)
		 VALUES
			   (@cust_code
			   ,@invoice_no
			   ,@invoice_type
			   ,@red_invoice
			   ,@serial_no
			   ,@plant
			   ,@atmid
			   ,@created_date
			   ,GETDATE(),@note)
		--sent email
		declare @body nvarchar(500)

		set @note='Bill-'+@billno+' Date-'+isnull(convert(varchar(10),@BillingDate,103),'')+' Cust-'+@cust_code_exits+' Plant-'+@plante+': old existing having eRunning Number: '+@serial_no;

		set @body='Dear all,<br/><br/>There is a duplication eRunning number: '+@serial_no+'<br/><br/>SAP billing number:<br/><br/>     '+@invoice_no+': New billing<br/><br/>     '+@note
			INSERT INTO VN_WINJOB.[dbo].[TBL_EMS_MAILLIST]([mail_from],[subject],[body],[mail_cc],[mail_to],[source],[source_id])
		values('einvoice@dksh.com',N'E-invoice duplicated',@body,'', dbo.fgetEmailSendINV_Error_ByPlant(@plant),'Einvoice', NULL)
	 

   end
   else
   begin--Ket thuc Khuyen Them

		IF EXISTS(SELECT top 1 [cust_code] FROM [TBL_CST_INVOICE]
				   WHERE [cust_code]=@cust_code AND [invoice_no]=@invoice_no AND [plant]=@plant
					 AND [red_invoice]=@red_invoice AND [serial_no]=@serial_no AND [invoice_type]=@invoice_type)
		BEGIN
			SELECT @invoice_id=ID FROM [TBL_CST_INVOICE]
				   WHERE [cust_code]=@cust_code AND [invoice_no]=@invoice_no AND [plant]=@plant
					 AND [red_invoice]=@red_invoice AND [serial_no]=@serial_no AND [invoice_type]=@invoice_type

			INSERT INTO [TBL_CST_INVOICE_DUPLICATE]
				   ([cust_code]
				   ,[invoice_no]
				   ,[invoice_type]
				   ,[red_invoice]
				   ,[serial_no]
				   ,[plant]
				   ,[atmid]
				   ,[created_date]
				   ,[last_updated])
			 VALUES
				   (@cust_code
				   ,@invoice_no
				   ,@invoice_type
				   ,@red_invoice
				   ,@serial_no
				   ,@plant
				   ,@atmid
				   ,@created_date
				   ,GETDATE())
		END
		ELSE
		begin
			INSERT INTO [TBL_CST_INVOICE]
				   ([cust_code]
				   ,[invoice_no]
				   ,[invoice_type]
				   ,[red_invoice]
				   ,[serial_no]
				   ,[plant]
				   ,[atmid]
				   ,[bu]
				   ,[created_date]
				   ,[last_updated])
			 VALUES
				   (@cust_code
				   ,@invoice_no
				   ,@invoice_type
				   ,@red_invoice
				   ,@serial_no
				   ,@plant
				   ,@atmid
				   ,@bu
				   ,@created_date
				   ,GETDATE())
		end
		--SET @invoice_id=@@IDENTITY --Khuyen chinh lai thanh scope_identity 20Mar2018
		SELECT @invoice_id=SCOPE_IDENTITY()

		--If is new customer, insert TBL_CST_INVOICE_CUSTOMER
		IF NOT EXISTS(SELECT cust_code FROM TBL_CST_INVOICE_CUSTOMER WITH (NOLOCK)
					   WHERE bu=@bu AND cust_code=@cust_code)
			INSERT INTO [TBL_CST_INVOICE_CUSTOMER]
				   ([bu],[cust_code],[status],[last_updated])
			VALUES(@bu,@cust_code,0,getdate())
	end
END
