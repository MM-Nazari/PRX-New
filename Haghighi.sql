CREATE TABLE [User] (
  [id] integer PRIMARY KEY IDENTITY(1, 1),
  [password] string,
  [phone_number] string,
  [reference_code] integer,
  [type] string
)
GO

CREATE TABLE [Haghighi_User_Profile] (
  [id] integer PRIMARY KEY IDENTITY(1, 1),
  [user_id] int,
  [first_name] string,
  [last_name] string,
  [fathers_name] string,
  [national_number] string UNIQUE,
  [birth_date] date,
  [birth_place] string,
  [birth_certificate_number] string UNIQUE,
  [marital_status] string,
  [gender] string,
  [postal_code] integer,
  [home_phone] string UNIQUE,
  [fax] string UNIQUE,
  [best_time_to_call] datetime,
  [residential_address] string,
  [email] string UNIQUE
)
GO

CREATE TABLE [Haghighi_User_Relationships] (
  [id] integer PRIMARY KEY IDENTITY(1, 1),
  [user_id] integer,
  [full_name] string,
  [relationship_status] string,
  [birth_year] integer,
  [education_level] string,
  [employment_status] string,
  [average_monthly_income] float,
  [average_monthly_expense] float,
  [approximate_assets] float,
  [approximate_liabilities] float
)
GO

CREATE TABLE [Haghighi_User_FinancialProfile] (
  [id] integer PRIMARY KEY IDENTITY(1, 1),
  [user_id] integer UNIQUE,
  [main_continuous_income] float,
  [other_incomes] float,
  [support_from_others] float,
  [continuous_expenses] float,
  [occasional_expenses] float,
  [contribution_to_others] float
)
GO

CREATE TABLE [Haghighi_User_FinancialChanges] (
  [id] integer PRIMARY KEY IDENTITY(1, 1),
  [user_id] integer,
  [description] string
)
GO

CREATE TABLE [Haghighi_User_EducationStatus] (
  [id] integer PRIMARY KEY IDENTITY(1, 1),
  [user_id] integer,
  [last_degree] string,
  [field_of_study] string,
  [graduation_year] integer,
  [issuing_authority] string
)
GO

CREATE TABLE [Haghighi_User_EmploymentHistory] (
  [id] integer PRIMARY KEY IDENTITY(1, 1),
  [user_id] integer,
  [employer_location] string,
  [main_activity] string,
  [position] string,
  [start_date] date,
  [end_date] date,
  [work_address] string UNIQUE,
  [work_phone] string UNIQUE
)
GO

CREATE TABLE [Haghighi_User_FuturePlans] (
  [id] integer PRIMARY KEY IDENTITY(1, 1),
  [user_id] integer,
  [description] text
)
GO

CREATE TABLE [Haghighi_User_InvestmentExperiences] (
  [id] integer PRIMARY KEY IDENTITY(1, 1),
  [user_id] integer,
  [investment_type] string,
  [investment_amount] float,
  [investment_duration_months] float,
  [profit_loss_amount] float,
  [profit_loss_description] string,
  [conversion_reason] string
)
GO

CREATE TABLE [Haghighi_User_AssetTypes] (
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

CREATE TABLE [Haghighi_User_Debts] (
  [id] integer PRIMARY KEY IDENTITY(1, 1),
  [user_id] integer,
  [debt_title] string,
  [debt_amount] float,
  [debt_due_date] date,
  [debt_repayment_percentage] float
)
GO

CREATE TABLE [Haghighi_User_Withdrawals] (
  [id] integer PRIMARY KEY IDENTITY(1, 1),
  [user_id] integer,
  [withdrawal_amount] float,
  [withdrawal_date] date,
  [withdrawal_reason] string
)
GO

CREATE TABLE [Haghighi_User_Deposits] (
  [id] integer PRIMARY KEY IDENTITY(1, 1),
  [user_id] integer,
  [deposit_amount] float,
  [deposit_date] date,
  [deposit_source] string
)
GO

CREATE TABLE [Haghighi_User_Investment] (
  [id] integer PRIMARY KEY IDENTITY(1, 1),
  [user_id] integer,
  [amount] int
)
GO

CREATE TABLE [Haghighi_User_MoreInformation] (
  [id] integer PRIMARY KEY IDENTITY(1, 1),
  [user_id] integer,
  [info] string
)
GO

CREATE TABLE [Haghighi_User_Questions] (
  [id] integer PRIMARY KEY IDENTITY(1, 1),
  [text] string
)
GO

CREATE TABLE [Haghighi_User_Answers] (
  [id] integer PRIMARY KEY IDENTITY(1, 1),
  [user_id] integer,
  [question_id] integer,
  [answer_option_id] integer,
  [answer_text] string
)
GO

CREATE TABLE [Haghighi_User_AnswerOptions] (
  [id] integer PRIMARY KEY IDENTITY(1, 1),
  [question_id] integer,
  [text] string
)
GO

CREATE TABLE [Haghighi_User_TestScores] (
  [id] int PRIMARY KEY IDENTITY(1, 1),
  [user_id] int,
  [score] integer
)
GO

EXEC sp_addextendedproperty
@name = N'Column_Description',
@value = 'Haghighi/Hoghooghi',
@level0type = N'Schema', @level0name = 'dbo',
@level1type = N'Table',  @level1name = 'User',
@level2type = N'Column', @level2name = 'type';
GO

ALTER TABLE [Haghighi_User_Profile] ADD FOREIGN KEY ([user_id]) REFERENCES [User] ([id])
GO

ALTER TABLE [Haghighi_User_Relationships] ADD FOREIGN KEY ([user_id]) REFERENCES [User] ([id])
GO

ALTER TABLE [Haghighi_User_FinancialProfile] ADD FOREIGN KEY ([user_id]) REFERENCES [User] ([id])
GO

ALTER TABLE [Haghighi_User_FinancialChanges] ADD FOREIGN KEY ([user_id]) REFERENCES [User] ([id])
GO

ALTER TABLE [Haghighi_User_EducationStatus] ADD FOREIGN KEY ([user_id]) REFERENCES [User] ([id])
GO

ALTER TABLE [Haghighi_User_EmploymentHistory] ADD FOREIGN KEY ([user_id]) REFERENCES [User] ([id])
GO

ALTER TABLE [Haghighi_User_FuturePlans] ADD FOREIGN KEY ([user_id]) REFERENCES [User] ([id])
GO

ALTER TABLE [Haghighi_User_InvestmentExperiences] ADD FOREIGN KEY ([user_id]) REFERENCES [User] ([id])
GO

ALTER TABLE [Haghighi_User_UserAssets] ADD FOREIGN KEY ([user_id]) REFERENCES [User] ([id])
GO

ALTER TABLE [Haghighi_User_UserAssets] ADD FOREIGN KEY ([asset_type_id]) REFERENCES [Haghighi_User_AssetTypes] ([id])
GO

ALTER TABLE [Haghighi_User_Debts] ADD FOREIGN KEY ([user_id]) REFERENCES [User] ([id])
GO

ALTER TABLE [Haghighi_User_Withdrawals] ADD FOREIGN KEY ([user_id]) REFERENCES [User] ([id])
GO

ALTER TABLE [Haghighi_User_Deposits] ADD FOREIGN KEY ([user_id]) REFERENCES [User] ([id])
GO

ALTER TABLE [Haghighi_User_Investment] ADD FOREIGN KEY ([user_id]) REFERENCES [User] ([id])
GO

ALTER TABLE [Haghighi_User_MoreInformation] ADD FOREIGN KEY ([user_id]) REFERENCES [User] ([id])
GO

ALTER TABLE [Haghighi_User_Answers] ADD FOREIGN KEY ([user_id]) REFERENCES [User] ([id])
GO

ALTER TABLE [Haghighi_User_Answers] ADD FOREIGN KEY ([question_id]) REFERENCES [Haghighi_User_Questions] ([id])
GO

ALTER TABLE [Haghighi_User_Answers] ADD FOREIGN KEY ([answer_option_id]) REFERENCES [Haghighi_User_AnswerOptions] ([id])
GO

ALTER TABLE [Haghighi_User_AnswerOptions] ADD FOREIGN KEY ([question_id]) REFERENCES [Haghighi_User_Questions] ([id])
GO

ALTER TABLE [Haghighi_User_TestScores] ADD FOREIGN KEY ([user_id]) REFERENCES [User] ([id])
GO
