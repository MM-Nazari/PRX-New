CREATE TABLE [User] (
  [id] integer PRIMARY KEY IDENTITY(1, 1),
  [password] string,
  [phone_number] string,
  [reference_code] integer,
  [type] string
)
GO

CREATE TABLE [Hoghooghi_Users] (
  [id] integer PRIMARY KEY IDENTITY(1, 1),
  [user_id] int,
  [name] string,
  [registration_number] string,
  [registration_date] date,
  [registration_location] string,
  [national_id] string UNIQUE,
  [main_activity_based_on_charter] string,
  [main_activity_based_on_past_three_years_performance] string,
  [postal_code] string UNIQUE,
  [landline_phone] string,
  [fax] string UNIQUE,
  [best_time_to_call] string,
  [address] string,
  [email] string UNIQUE,
  [representative_name] string,
  [representative_national_id] string,
  [representative_mobile_phone] string
)
GO

CREATE TABLE [Hoghooghi_User_Board_of_Directors] (
  [id] int PRIMARY KEY IDENTITY(1, 1),
  [user_id] int,
  [full_name] string,
  [position] string,
  [educational_level] string,
  [field_of_study] string,
  [executive_experience] string,
  [familiarity_with_capital_market] string,
  [personal_investment_experience_in_stock_exchange] string
)
GO

CREATE TABLE [Hoghooghi_User_Investment_Department_Staff] (
  [id] int PRIMARY KEY IDENTITY(1, 1),
  [user_id] int,
  [full_name] string,
  [position] string,
  [educational_level] string,
  [field_of_study] string,
  [executive_experience] string,
  [familiarity_with_capital_market] string,
  [personal_investment_experience_in_stock_exchange] string
)
GO

CREATE TABLE [Hoghooghi_User_Companies_With_Major_Investors] (
  [id] int PRIMARY KEY IDENTITY(1, 1),
  [user_id] int,
  [company_name] string,
  [company_subject] string,
  [percentage_of_total] float
)
GO

CREATE TABLE [Hoghooghi_User_Asset_Income_Status_Two_Years_Ago] (
  [id] int PRIMARY KEY IDENTITY(1, 1),
  [user_id] int,
  [fiscal_year] int,
  [registered_capital] float,
  [approximate_asset_value] float,
  [total_liabilities] float,
  [total_investments] float,
  [operational_income] float,
  [other_income] float,
  [operational_expenses] float,
  [other_expenses] float,
  [operational_profit_or_loss] float,
  [net_profit_or_loss] float,
  [accumulated_profit_or_loss] float
)
GO

CREATE TABLE [Hoghooghi_User_Possible_Changes_In_Income_Expenses] (
  [id] int PRIMARY KEY IDENTITY(1, 1),
  [user_id] int,
  [description] string
)
GO

CREATE TABLE [Hoghooghi_User_FuturePlans] (
  [id] integer PRIMARY KEY IDENTITY(1, 1),
  [user_id] integer,
  [description] string
)
GO

CREATE TABLE [Hoghooghi_User_InvestmentExperiences] (
  [id] integer PRIMARY KEY IDENTITY(1, 1),
  [user_id] integer,
  [investment_type] string,
  [investment_amount] float,
  [investment_duration_months] integer,
  [profit_loss_amount] float,
  [profit_loss_description] string,
  [conversion_reason] string
)
GO

CREATE TABLE [Hoghooghi_User_AssetTypes] (
  [id] integer PRIMARY KEY IDENTITY(1, 1),
  [name] string
)
GO

CREATE TABLE [Haghighi_User_UserAssets] (
  [id] integer PRIMARY KEY IDENTITY(1, 1),
  [user_id] integer,
  [asset_type_id] integer,
  [asset_value] float,
  [asset_percentage] float
)
GO

CREATE TABLE [Hoghooghi_User_Debts] (
  [id] integer PRIMARY KEY IDENTITY(1, 1),
  [user_id] integer,
  [debt_title] string,
  [debt_amount] float,
  [debt_due_date] date,
  [debt_repayment_percentage] float
)
GO

CREATE TABLE [Hoghooghi_User_Withdrawals] (
  [id] integer PRIMARY KEY IDENTITY(1, 1),
  [user_id] integer,
  [withdrawal_amount] float,
  [withdrawal_date] date,
  [withdrawal_reason] string
)
GO

CREATE TABLE [Hoghooghi_User_Deposits] (
  [id] integer PRIMARY KEY IDENTITY(1, 1),
  [user_id] integer,
  [deposit_amount] float,
  [deposit_date] date,
  [deposit_source] string
)
GO

CREATE TABLE [Hoghooghi_User_Investment] (
  [id] integer PRIMARY KEY IDENTITY(1, 1),
  [user_id] integer,
  [amount] int
)
GO

CREATE TABLE [Hoghooghi_User_MoreInformation] (
  [id] integer PRIMARY KEY IDENTITY(1, 1),
  [user_id] integer,
  [info] string
)
GO

CREATE TABLE [Hoghooghi_User_Questions] (
  [id] integer PRIMARY KEY IDENTITY(1, 1),
  [text] string
)
GO

CREATE TABLE [Hoghooghi_User_Answers] (
  [id] integer PRIMARY KEY IDENTITY(1, 1),
  [user_id] integer,
  [question_id] integer,
  [answer_option_id] integer,
  [answer_text] string
)
GO

CREATE TABLE [Hoghooghi_User_AnswerOptions] (
  [id] integer PRIMARY KEY IDENTITY(1, 1),
  [question_id] integer,
  [text] string
)
GO

CREATE TABLE [Hoghooghi_User_TestScores] (
  [id] int PRIMARY KEY IDENTITY(1, 1),
  [user_id] int,
  [score] int
)
GO

EXEC sp_addextendedproperty
@name = N'Column_Description',
@value = 'Haghighi/Hoghooghi',
@level0type = N'Schema', @level0name = 'dbo',
@level1type = N'Table',  @level1name = 'User',
@level2type = N'Column', @level2name = 'type';
GO

ALTER TABLE [Hoghooghi_Users] ADD FOREIGN KEY ([user_id]) REFERENCES [User] ([id])
GO

ALTER TABLE [Hoghooghi_User_Board_of_Directors] ADD FOREIGN KEY ([user_id]) REFERENCES [User] ([id])
GO

ALTER TABLE [Hoghooghi_User_Investment_Department_Staff] ADD FOREIGN KEY ([user_id]) REFERENCES [User] ([id])
GO

ALTER TABLE [Hoghooghi_User_Companies_With_Major_Investors] ADD FOREIGN KEY ([user_id]) REFERENCES [User] ([id])
GO

ALTER TABLE [Hoghooghi_User_Asset_Income_Status_Two_Years_Ago] ADD FOREIGN KEY ([user_id]) REFERENCES [User] ([id])
GO

ALTER TABLE [Hoghooghi_User_Possible_Changes_In_Income_Expenses] ADD FOREIGN KEY ([user_id]) REFERENCES [User] ([id])
GO

ALTER TABLE [Hoghooghi_User_FuturePlans] ADD FOREIGN KEY ([user_id]) REFERENCES [User] ([id])
GO

ALTER TABLE [Hoghooghi_User_InvestmentExperiences] ADD FOREIGN KEY ([user_id]) REFERENCES [User] ([id])
GO

ALTER TABLE [Haghighi_User_UserAssets] ADD FOREIGN KEY ([user_id]) REFERENCES [User] ([id])
GO

ALTER TABLE [Haghighi_User_UserAssets] ADD FOREIGN KEY ([asset_type_id]) REFERENCES [Hoghooghi_User_AssetTypes] ([id])
GO

ALTER TABLE [Hoghooghi_User_Debts] ADD FOREIGN KEY ([user_id]) REFERENCES [User] ([id])
GO

ALTER TABLE [Hoghooghi_User_Withdrawals] ADD FOREIGN KEY ([user_id]) REFERENCES [User] ([id])
GO

ALTER TABLE [Hoghooghi_User_Deposits] ADD FOREIGN KEY ([user_id]) REFERENCES [User] ([id])
GO

ALTER TABLE [Hoghooghi_User_Investment] ADD FOREIGN KEY ([user_id]) REFERENCES [User] ([id])
GO

ALTER TABLE [Hoghooghi_User_MoreInformation] ADD FOREIGN KEY ([user_id]) REFERENCES [User] ([id])
GO

ALTER TABLE [Hoghooghi_User_Answers] ADD FOREIGN KEY ([user_id]) REFERENCES [User] ([id])
GO

ALTER TABLE [Hoghooghi_User_Answers] ADD FOREIGN KEY ([question_id]) REFERENCES [Hoghooghi_User_Questions] ([id])
GO

ALTER TABLE [Hoghooghi_User_Answers] ADD FOREIGN KEY ([answer_option_id]) REFERENCES [Hoghooghi_User_AnswerOptions] ([id])
GO

ALTER TABLE [Hoghooghi_User_AnswerOptions] ADD FOREIGN KEY ([question_id]) REFERENCES [Hoghooghi_User_Questions] ([id])
GO

ALTER TABLE [Hoghooghi_User_TestScores] ADD FOREIGN KEY ([user_id]) REFERENCES [User] ([id])
GO
