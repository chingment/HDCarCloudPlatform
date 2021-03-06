USE [UplinkCar]
GO
/****** Object:  Table [dbo].[OrderPayResultNotifyLog]    Script Date: 04/14/2017 17:00:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderPayResultNotifyLog](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SysOrderId] [int] NOT NULL,
	[SysOrderSn] [nvarchar](128) NULL,
	[OrderNo] [nvarchar](128) NULL,
	[MerchantId] [nvarchar](128) NULL,
	[Amount] [nvarchar](128) NULL,
	[TerminalId] [nvarchar](128) NULL,
	[MerchantNo] [nvarchar](128) NULL,
	[BatchNo] [nvarchar](128) NULL,
	[MerchantName] [nvarchar](128) NULL,
	[Issue] [nvarchar](128) NULL,
	[TraceNo] [nvarchar](128) NULL,
	[FailureReason] [nvarchar](1024) NULL,
	[ReferenceNo] [nvarchar](128) NULL,
	[Type] [nvarchar](128) NULL,
	[Result] [nvarchar](128) NULL,
	[CardNo] [nvarchar](128) NULL,
	[Creator] [int] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.OrderPayResultNotifyLog] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Order]    Script Date: 04/14/2017 17:00:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Order](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Sn] [nvarchar](128) NULL,
	[MerchantId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	[ProductType] [int] NOT NULL,
	[ProductName] [nvarchar](128) NULL,
	[Contact] [nvarchar](128) NULL,
	[ContactAddress] [nvarchar](512) NULL,
	[Price] [decimal](18, 2) NOT NULL,
	[Status] [int] NOT NULL,
	[PayTime] [datetime] NULL,
	[CancleTime] [datetime] NULL,
	[CompleteTime] [datetime] NULL,
	[Creator] [int] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[Mender] [int] NULL,
	[LastUpdateTime] [datetime] NULL,
	[ContactPhoneNumber] [nvarchar](128) NULL,
	[SubmitTime] [datetime] NOT NULL,
	[PayWay] [int] NOT NULL,
	[Remarks] [nvarchar](1024) NULL,
	[PId] [int] NULL,
	[FollowStatus] [int] NOT NULL,
	[Discriminator] [nvarchar](max) NULL,
	[ShippingAddress] [nvarchar](512) NULL,
	[MerchantPosMachineId] [int] NULL,
 CONSTRAINT [PK_dbo.Order] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Order] ON
INSERT [dbo].[Order] ([Id], [Sn], [MerchantId], [UserId], [ProductId], [ProductType], [ProductName], [Contact], [ContactAddress], [Price], [Status], [PayTime], [CancleTime], [CompleteTime], [Creator], [CreateTime], [Mender], [LastUpdateTime], [ContactPhoneNumber], [SubmitTime], [PayWay], [Remarks], [PId], [FollowStatus], [Discriminator], [ShippingAddress], [MerchantPosMachineId]) VALUES (1, N'D170414164400000001', 1, 1001, 301, 301, N'POS机押金租金', NULL, NULL, CAST(1600.00 AS Decimal(18, 2)), 3, NULL, NULL, NULL, 0, CAST(0x0000A7550113FC4A AS DateTime), NULL, NULL, NULL, CAST(0x0000A7550113FC4A AS DateTime), 0, NULL, NULL, 0, NULL, NULL, 1)
SET IDENTITY_INSERT [dbo].[Order] OFF
/****** Object:  Table [dbo].[MerchantPosMachine]    Script Date: 04/14/2017 17:00:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MerchantPosMachine](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[MerchantId] [int] NOT NULL,
	[Deposit] [decimal](18, 2) NOT NULL,
	[Rent] [decimal](18, 2) NOT NULL,
	[Status] [int] NOT NULL,
	[PosMachineId] [int] NOT NULL,
	[RentDueDate] [datetime] NULL,
	[BankName] [nvarchar](128) NULL,
	[BankAccountName] [nvarchar](128) NULL,
	[BankAccountNo] [nvarchar](128) NULL,
	[Creator] [int] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[Mender] [int] NULL,
	[LastUpdateTime] [datetime] NULL,
	[BankCardId] [int] NULL,
	[DepositPayTime] [datetime] NULL,
	[ReturnDeposit] [decimal](18, 2) NULL,
	[ReturnTime] [datetime] NULL,
 CONSTRAINT [PK_dbo.MerchantPosMachine] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[MerchantPosMachine] ON
INSERT [dbo].[MerchantPosMachine] ([Id], [UserId], [MerchantId], [Deposit], [Rent], [Status], [PosMachineId], [RentDueDate], [BankName], [BankAccountName], [BankAccountNo], [Creator], [CreateTime], [Mender], [LastUpdateTime], [BankCardId], [DepositPayTime], [ReturnDeposit], [ReturnTime]) VALUES (1, 1001, 1, CAST(1000.00 AS Decimal(18, 2)), CAST(200.00 AS Decimal(18, 2)), 2, 1, NULL, NULL, NULL, NULL, 0, CAST(0x0000A7550113FC4A AS DateTime), NULL, NULL, 1, NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[MerchantPosMachine] OFF
/****** Object:  Table [dbo].[MerchantEstimateCompany]    Script Date: 04/14/2017 17:00:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MerchantEstimateCompany](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MerchantId] [int] NOT NULL,
	[InsuranceCompanyId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.MerchantEstimateCompany] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Merchant]    Script Date: 04/14/2017 17:00:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Merchant](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Creator] [int] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[Mender] [int] NULL,
	[LastUpdateTime] [datetime] NULL,
	[UserId] [int] NOT NULL,
	[YYZZ_RegisterNo] [nvarchar](128) NULL,
	[YYZZ_Type] [nvarchar](128) NULL,
	[YYZZ_Name] [nvarchar](128) NULL,
	[YYZZ_Address] [nvarchar](512) NULL,
	[YYZZ_OperatingPeriodStart] [datetime] NULL,
	[YYZZ_OperatingPeriodEnd] [datetime] NULL,
	[FR_IdCardNo] [nvarchar](128) NULL,
	[FR_Name] [nvarchar](128) NULL,
	[FR_Birthdate] [datetime] NULL,
	[FR_Address] [nvarchar](512) NULL,
	[FR_IssuingAuthority] [nvarchar](128) NULL,
	[FR_ValidPeriodStart] [datetime] NULL,
	[FR_ValidPeriodEnd] [datetime] NULL,
	[ContactName] [nvarchar](128) NULL,
	[ContactPhoneNumber] [nvarchar](128) NULL,
	[ContactAddress] [nvarchar](512) NULL,
	[ClientCode] [nvarchar](128) NULL,
	[Status] [int] NOT NULL,
	[Type] [int] NOT NULL,
	[RepairCapacity] [int] NOT NULL,
	[AreaCode] [nvarchar](128) NULL,
	[Area] [nvarchar](256) NULL,
	[Longitude] [float] NOT NULL,
	[Latitude] [float] NOT NULL,
	[SalesmanId] [int] NULL,
	[FollowUserId] [int] NULL,
 CONSTRAINT [PK_dbo.Merchant] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Merchant] ON
INSERT [dbo].[Merchant] ([Id], [Creator], [CreateTime], [Mender], [LastUpdateTime], [UserId], [YYZZ_RegisterNo], [YYZZ_Type], [YYZZ_Name], [YYZZ_Address], [YYZZ_OperatingPeriodStart], [YYZZ_OperatingPeriodEnd], [FR_IdCardNo], [FR_Name], [FR_Birthdate], [FR_Address], [FR_IssuingAuthority], [FR_ValidPeriodStart], [FR_ValidPeriodEnd], [ContactName], [ContactPhoneNumber], [ContactAddress], [ClientCode], [Status], [Type], [RepairCapacity], [AreaCode], [Area], [Longitude], [Latitude], [SalesmanId], [FollowUserId]) VALUES (1, 0, CAST(0x0000A7550113FC4A AS DateTime), NULL, NULL, 1001, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'852366', 1, 0, 0, NULL, NULL, 0, 0, NULL, NULL)
SET IDENTITY_INSERT [dbo].[Merchant] OFF
/****** Object:  Table [dbo].[InsuranceCompany]    Script Date: 04/14/2017 17:00:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InsuranceCompany](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](128) NULL,
	[ImgUrl] [nvarchar](1024) NULL,
	[Creator] [int] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[Mender] [int] NULL,
	[LastUpdateTime] [datetime] NULL,
	[YBS_MerchantId] [nvarchar](128) NULL,
	[YBS_MerchantName] [nvarchar](128) NULL,
	[YBS_MerchantCode] [nvarchar](128) NULL,
	[YBS_BizCode] [nvarchar](128) NULL,
 CONSTRAINT [PK_dbo.InsuranceCompany] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[InsuranceCompany] ON
INSERT [dbo].[InsuranceCompany] ([Id], [Name], [ImgUrl], [Creator], [CreateTime], [Mender], [LastUpdateTime], [YBS_MerchantId], [YBS_MerchantName], [YBS_MerchantCode], [YBS_BizCode]) VALUES (1, N'平安保险', N'https://file.ins-uplink.cn/upload/CarInsuranceCompany/zgpabx.png', 0, CAST(0x0000A6DA00000000 AS DateTime), NULL, NULL, N'861440363000026', N'三泰保险代理（全线通服务）', N'uplink', N'000003')
INSERT [dbo].[InsuranceCompany] ([Id], [Name], [ImgUrl], [Creator], [CreateTime], [Mender], [LastUpdateTime], [YBS_MerchantId], [YBS_MerchantName], [YBS_MerchantCode], [YBS_BizCode]) VALUES (2, N'太平洋保险', N'https://file.ins-uplink.cn/upload/CarInsuranceCompany/tpybx.png', 0, CAST(0x0000A6DA00000000 AS DateTime), NULL, NULL, N'861440363000026', N'三泰保险代理（全线通服务）', N'uplink', N'000004')
INSERT [dbo].[InsuranceCompany] ([Id], [Name], [ImgUrl], [Creator], [CreateTime], [Mender], [LastUpdateTime], [YBS_MerchantId], [YBS_MerchantName], [YBS_MerchantCode], [YBS_BizCode]) VALUES (3, N'阳光保险', N'https://file.ins-uplink.cn/upload/CarInsuranceCompany/zgrsbx.png', 0, CAST(0x0000A6DA00000000 AS DateTime), NULL, NULL, N'861440363000026', N'三泰保险代理（全线通服务）', N'uplink', N'000005')
INSERT [dbo].[InsuranceCompany] ([Id], [Name], [ImgUrl], [Creator], [CreateTime], [Mender], [LastUpdateTime], [YBS_MerchantId], [YBS_MerchantName], [YBS_MerchantCode], [YBS_BizCode]) VALUES (4, N'亚太保险', N'https://file.ins-uplink.cn/upload/CarInsuranceCompany/ytbx.png', 0, CAST(0x0000A6DA00000000 AS DateTime), NULL, NULL, N'861440363000026', N'三泰保险代理（全线通服务）', N'uplink', N'000006')
INSERT [dbo].[InsuranceCompany] ([Id], [Name], [ImgUrl], [Creator], [CreateTime], [Mender], [LastUpdateTime], [YBS_MerchantId], [YBS_MerchantName], [YBS_MerchantCode], [YBS_BizCode]) VALUES (5, N'人民保险', N'https://file.ins-uplink.cn/upload/CarInsuranceCompany/zgrmbx.png', 0, CAST(0x0000A6DA00000000 AS DateTime), NULL, NULL, N'861440363000026', N'三泰保险代理（全线通服务）', N'uplink', N'000007')
INSERT [dbo].[InsuranceCompany] ([Id], [Name], [ImgUrl], [Creator], [CreateTime], [Mender], [LastUpdateTime], [YBS_MerchantId], [YBS_MerchantName], [YBS_MerchantCode], [YBS_BizCode]) VALUES (6, N'中华保险', N'https://file.ins-uplink.cn/upload/CarInsuranceCompany/zhbx.png', 0, CAST(0x0000A6DA00000000 AS DateTime), NULL, NULL, NULL, NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[InsuranceCompany] OFF
/****** Object:  Table [dbo].[Fund]    Script Date: 04/14/2017 17:00:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Fund](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[MerchantId] [int] NOT NULL,
	[Balance] [decimal](18, 2) NOT NULL,
	[Arrearage] [decimal](18, 2) NOT NULL,
	[Creator] [int] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[Mender] [int] NULL,
	[LastUpdateTime] [datetime] NULL,
 CONSTRAINT [PK_dbo.Fund] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Fund] ON
INSERT [dbo].[Fund] ([Id], [UserId], [MerchantId], [Balance], [Arrearage], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (1, 1, 0, CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, CAST(0x0000A6DA00000000 AS DateTime), NULL, NULL)
INSERT [dbo].[Fund] ([Id], [UserId], [MerchantId], [Balance], [Arrearage], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (2, 2, 0, CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, CAST(0x0000A6DA00000000 AS DateTime), NULL, NULL)
INSERT [dbo].[Fund] ([Id], [UserId], [MerchantId], [Balance], [Arrearage], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (3, 3, 0, CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, CAST(0x0000A6DA00000000 AS DateTime), NULL, NULL)
INSERT [dbo].[Fund] ([Id], [UserId], [MerchantId], [Balance], [Arrearage], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (4, 4, 0, CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, CAST(0x0000A6DA00000000 AS DateTime), NULL, NULL)
INSERT [dbo].[Fund] ([Id], [UserId], [MerchantId], [Balance], [Arrearage], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (5, 1001, 1, CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, CAST(0x0000A7550113FC4A AS DateTime), NULL, NULL)
SET IDENTITY_INSERT [dbo].[Fund] OFF
/****** Object:  Table [dbo].[ExtendedApp]    Script Date: 04/14/2017 17:00:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExtendedApp](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Type] [int] NOT NULL,
	[Name] [nvarchar](128) NULL,
	[LinkUrl] [nvarchar](1024) NULL,
	[ImgUrl] [nvarchar](1024) NULL,
	[AccessCount] [nvarchar](max) NULL,
	[Status] [int] NOT NULL,
	[Description] [nvarchar](1024) NULL,
	[Creator] [int] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[Mender] [int] NULL,
	[LastUpdateTime] [datetime] NULL,
	[IsDisplay] [bit] NULL,
	[AppKey] [nvarchar](128) NULL,
	[AppSecret] [nvarchar](128) NULL,
 CONSTRAINT [PK_dbo.ExtendedApp] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[ExtendedApp] ON
INSERT [dbo].[ExtendedApp] ([Id], [Type], [Name], [LinkUrl], [ImgUrl], [AccessCount], [Status], [Description], [Creator], [CreateTime], [Mender], [LastUpdateTime], [IsDisplay], [AppKey], [AppSecret]) VALUES (1, 1, N'违章查缴', N'http://www.ys16888.com/yscar_wx/pos/redirect/pos_login.do?state=1', N'https://file.ins-uplink.cn/upload/ExtendedApp/CarService/wzcx.png', NULL, 2, NULL, 1, CAST(0x0000A6EC00C1DFB7 AS DateTime), NULL, NULL, 1, N'cgjCarApp', N'971251a0942548adb59c4c88501c7055')
INSERT [dbo].[ExtendedApp] ([Id], [Type], [Name], [LinkUrl], [ImgUrl], [AccessCount], [Status], [Description], [Creator], [CreateTime], [Mender], [LastUpdateTime], [IsDisplay], [AppKey], [AppSecret]) VALUES (2, 1, N'年审代办', N'http://www.ys16888.com/yscar_wx/pos/redirect/pos_login.do?state=2', N'https://file.ins-uplink.cn/upload/ExtendedApp/CarService/nsfu.png', NULL, 2, NULL, 1, CAST(0x0000A6EC00C1F1F9 AS DateTime), NULL, NULL, 1, N'cgjCarApp', N'971251a0942548adb59c4c88501c7055')
INSERT [dbo].[ExtendedApp] ([Id], [Type], [Name], [LinkUrl], [ImgUrl], [AccessCount], [Status], [Description], [Creator], [CreateTime], [Mender], [LastUpdateTime], [IsDisplay], [AppKey], [AppSecret]) VALUES (3, 1, N'其它', N'https://www.ins-uplink.cn/app/ExtendedApp/ComingSoon', N'https://file.ins-uplink.cn/upload/ExtendedApp/CarService/qt1.png', NULL, 2, NULL, 1, CAST(0x0000A6EC00C203B6 AS DateTime), NULL, NULL, 1, N'app1', N'971251a0942548adb59c4c88501c7055')
INSERT [dbo].[ExtendedApp] ([Id], [Type], [Name], [LinkUrl], [ImgUrl], [AccessCount], [Status], [Description], [Creator], [CreateTime], [Mender], [LastUpdateTime], [IsDisplay], [AppKey], [AppSecret]) VALUES (4, 1, N'其它', N'https://www.ins-uplink.cn/app/ExtendedApp/ComingSoon', N'https://file.ins-uplink.cn/upload/ExtendedApp/CarService/qt2.png', NULL, 2, NULL, 1, CAST(0x0000A6EC00C210B3 AS DateTime), NULL, NULL, 1, N'app1', N'971251a0942548adb59c4c88501c7055')
INSERT [dbo].[ExtendedApp] ([Id], [Type], [Name], [LinkUrl], [ImgUrl], [AccessCount], [Status], [Description], [Creator], [CreateTime], [Mender], [LastUpdateTime], [IsDisplay], [AppKey], [AppSecret]) VALUES (5, 2, N'应用1', N'https://www.ins-uplink.cn/app/ExtendedApp/ComingSoon', N'https://file.ins-uplink.cn/upload/ExtendedApp/ThirdPartyApp/app1.png', NULL, 2, NULL, 1, CAST(0x0000A6EC00C478D5 AS DateTime), NULL, NULL, 1, N'app1', N'GPnvagkZ2Rv5cjhpse6451G6EYlE5')
INSERT [dbo].[ExtendedApp] ([Id], [Type], [Name], [LinkUrl], [ImgUrl], [AccessCount], [Status], [Description], [Creator], [CreateTime], [Mender], [LastUpdateTime], [IsDisplay], [AppKey], [AppSecret]) VALUES (6, 2, N'应用2', N'https://www.ins-uplink.cn/app/ExtendedApp/ComingSoon', N'https://file.ins-uplink.cn/upload/ExtendedApp/ThirdPartyApp/app2.png', NULL, 2, NULL, 1, CAST(0x0000A6EC00C48548 AS DateTime), NULL, NULL, 1, N'app1', N'GPnvagkZ2Rv5cjhps33e1G6EYlE5')
INSERT [dbo].[ExtendedApp] ([Id], [Type], [Name], [LinkUrl], [ImgUrl], [AccessCount], [Status], [Description], [Creator], [CreateTime], [Mender], [LastUpdateTime], [IsDisplay], [AppKey], [AppSecret]) VALUES (7, 2, N'应用3', N'https://www.ins-uplink.cn/app/ExtendedApp/ComingSoon', N'https://file.ins-uplink.cn/upload/ExtendedApp/ThirdPartyApp/app3.png', NULL, 2, NULL, 1, CAST(0x0000A6EC00C49057 AS DateTime), NULL, NULL, 1, N'app1', N'GPnvagkZ2Rv5cjhpse541G6EYlE5')
INSERT [dbo].[ExtendedApp] ([Id], [Type], [Name], [LinkUrl], [ImgUrl], [AccessCount], [Status], [Description], [Creator], [CreateTime], [Mender], [LastUpdateTime], [IsDisplay], [AppKey], [AppSecret]) VALUES (8, 2, N'应用4', N'https://www.ins-uplink.cn/app/ExtendedApp/ComingSoon', N'https://file.ins-uplink.cn/upload/ExtendedApp/ThirdPartyApp/app4.png', NULL, 2, NULL, 1, CAST(0x0000A6EC00C49B23 AS DateTime), NULL, NULL, 1, N'app1', N'GPnvagkZ2Rv5cjh33pse1G6EYlE5')
SET IDENTITY_INSERT [dbo].[ExtendedApp] OFF
/****** Object:  Table [dbo].[CarKind]    Script Date: 04/14/2017 17:00:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CarKind](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](128) NULL,
	[AliasName] [nvarchar](128) NULL,
	[CanWaiverDeductible] [bit] NOT NULL,
	[Type] [int] NOT NULL,
	[InputType] [int] NOT NULL,
	[InputUnit] [nvarchar](128) NULL,
	[IsHasDetails] [bit] NOT NULL,
	[Priority] [int] NOT NULL,
	[Creator] [int] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[Mender] [int] NULL,
	[LastUpdateTime] [datetime] NULL,
	[InputValue] [nvarchar](2048) NULL,
 CONSTRAINT [PK_dbo.CarKind] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[CarKind] ([Id], [Name], [AliasName], [CanWaiverDeductible], [Type], [InputType], [InputUnit], [IsHasDetails], [Priority], [Creator], [CreateTime], [Mender], [LastUpdateTime], [InputValue]) VALUES (1, N'交强险', NULL, 0, 1, 1, NULL, 0, 100, 1, CAST(0x0000A6DA00000000 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[CarKind] ([Id], [Name], [AliasName], [CanWaiverDeductible], [Type], [InputType], [InputUnit], [IsHasDetails], [Priority], [Creator], [CreateTime], [Mender], [LastUpdateTime], [InputValue]) VALUES (2, N'车船税（买交强险且未完税者必买）', NULL, 0, 1, 1, NULL, 0, 99, 1, CAST(0x0000A6DA00000000 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[CarKind] ([Id], [Name], [AliasName], [CanWaiverDeductible], [Type], [InputType], [InputUnit], [IsHasDetails], [Priority], [Creator], [CreateTime], [Mender], [LastUpdateTime], [InputValue]) VALUES (3, N'机动车损失保险', NULL, 1, 2, 1, NULL, 0, 98, 1, CAST(0x0000A6DA00000000 AS DateTime), NULL, NULL, N'{"default":"1000"}')
INSERT [dbo].[CarKind] ([Id], [Name], [AliasName], [CanWaiverDeductible], [Type], [InputType], [InputUnit], [IsHasDetails], [Priority], [Creator], [CreateTime], [Mender], [LastUpdateTime], [InputValue]) VALUES (4, N'机动车第三者责任保险', NULL, 1, 2, 3, N'元', 0, 97, 1, CAST(0x0000A6DA00000000 AS DateTime), NULL, NULL, N'{"default":"100w","value":["200w以上","150w","100w","50w","30w","30w","15w","10w","5w"]}')
INSERT [dbo].[CarKind] ([Id], [Name], [AliasName], [CanWaiverDeductible], [Type], [InputType], [InputUnit], [IsHasDetails], [Priority], [Creator], [CreateTime], [Mender], [LastUpdateTime], [InputValue]) VALUES (5, N'机动车车上人员责任保险（司机）', NULL, 1, 2, 3, N'元', 0, 96, 1, CAST(0x0000A6DA00000000 AS DateTime), NULL, NULL, N'{"default":"1w","value":["1w","2w","5w","10w","15w"]}')
INSERT [dbo].[CarKind] ([Id], [Name], [AliasName], [CanWaiverDeductible], [Type], [InputType], [InputUnit], [IsHasDetails], [Priority], [Creator], [CreateTime], [Mender], [LastUpdateTime], [InputValue]) VALUES (6, N'机动车车上人员责任保险（乘客）', NULL, 1, 2, 3, N'元', 0, 95, 1, CAST(0x0000A6DA00000000 AS DateTime), NULL, NULL, N'{"default":"1w","value":["1w","2w","5w","10w","15w"]}')
INSERT [dbo].[CarKind] ([Id], [Name], [AliasName], [CanWaiverDeductible], [Type], [InputType], [InputUnit], [IsHasDetails], [Priority], [Creator], [CreateTime], [Mender], [LastUpdateTime], [InputValue]) VALUES (7, N'机动车全车盗抢保险', NULL, 1, 2, 1, NULL, 0, 94, 1, CAST(0x0000A6DA00000000 AS DateTime), NULL, NULL, N'{"default":"1000"}')
INSERT [dbo].[CarKind] ([Id], [Name], [AliasName], [CanWaiverDeductible], [Type], [InputType], [InputUnit], [IsHasDetails], [Priority], [Creator], [CreateTime], [Mender], [LastUpdateTime], [InputValue]) VALUES (8, N'玻璃单独破碎险', NULL, 0, 3, 3, NULL, 0, 93, 1, CAST(0x0000A6DA00000000 AS DateTime), NULL, NULL, N'{"default":"国产",value:["国产","进口"]}')
INSERT [dbo].[CarKind] ([Id], [Name], [AliasName], [CanWaiverDeductible], [Type], [InputType], [InputUnit], [IsHasDetails], [Priority], [Creator], [CreateTime], [Mender], [LastUpdateTime], [InputValue]) VALUES (9, N'车身划痕损失险', NULL, 1, 3, 2, NULL, 0, 92, 1, CAST(0x0000A6DA00000000 AS DateTime), NULL, NULL, N'{"default":"2000"}')
INSERT [dbo].[CarKind] ([Id], [Name], [AliasName], [CanWaiverDeductible], [Type], [InputType], [InputUnit], [IsHasDetails], [Priority], [Creator], [CreateTime], [Mender], [LastUpdateTime], [InputValue]) VALUES (10, N'机动车损失保险无法找到第三方特约险', NULL, 0, 3, 1, NULL, 0, 91, 1, CAST(0x0000A6DA00000000 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[CarKind] ([Id], [Name], [AliasName], [CanWaiverDeductible], [Type], [InputType], [InputUnit], [IsHasDetails], [Priority], [Creator], [CreateTime], [Mender], [LastUpdateTime], [InputValue]) VALUES (11, N'发动机涉水损失险', NULL, 1, 3, 2, N'元', 0, 90, 1, CAST(0x0000A6DA00000000 AS DateTime), NULL, NULL, N'{"default":"1000"}')
INSERT [dbo].[CarKind] ([Id], [Name], [AliasName], [CanWaiverDeductible], [Type], [InputType], [InputUnit], [IsHasDetails], [Priority], [Creator], [CreateTime], [Mender], [LastUpdateTime], [InputValue]) VALUES (12, N'自燃损失险', NULL, 1, 3, 1, NULL, 0, 89, 1, CAST(0x0000A6DA00000000 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[CarKind] ([Id], [Name], [AliasName], [CanWaiverDeductible], [Type], [InputType], [InputUnit], [IsHasDetails], [Priority], [Creator], [CreateTime], [Mender], [LastUpdateTime], [InputValue]) VALUES (13, N'新增加设备损失险', NULL, 0, 3, 2, N'元', 1, 88, 1, CAST(0x0000A6DA00000000 AS DateTime), NULL, NULL, N'{"default":"1000"}')
INSERT [dbo].[CarKind] ([Id], [Name], [AliasName], [CanWaiverDeductible], [Type], [InputType], [InputUnit], [IsHasDetails], [Priority], [Creator], [CreateTime], [Mender], [LastUpdateTime], [InputValue]) VALUES (14, N'修理期间费用补偿险', NULL, 0, 3, 3, NULL, 0, 87, 1, CAST(0x0000A6DA00000000 AS DateTime), NULL, NULL, N'{"default":"100",value:["100","200","300","400","500"]}')
INSERT [dbo].[CarKind] ([Id], [Name], [AliasName], [CanWaiverDeductible], [Type], [InputType], [InputUnit], [IsHasDetails], [Priority], [Creator], [CreateTime], [Mender], [LastUpdateTime], [InputValue]) VALUES (15, N'车上货物责任险', NULL, 0, 3, 2, N'元', 0, 86, 1, CAST(0x0000A6DA00000000 AS DateTime), NULL, NULL, N'{"default":"1000"}')
INSERT [dbo].[CarKind] ([Id], [Name], [AliasName], [CanWaiverDeductible], [Type], [InputType], [InputUnit], [IsHasDetails], [Priority], [Creator], [CreateTime], [Mender], [LastUpdateTime], [InputValue]) VALUES (16, N'精神损害抚慰金责任险', NULL, 0, 3, 2, N'元', 0, 85, 1, CAST(0x0000A6DA00000000 AS DateTime), NULL, NULL, N'{"default":"1000"}')
INSERT [dbo].[CarKind] ([Id], [Name], [AliasName], [CanWaiverDeductible], [Type], [InputType], [InputUnit], [IsHasDetails], [Priority], [Creator], [CreateTime], [Mender], [LastUpdateTime], [InputValue]) VALUES (17, N'指定修理厂险', NULL, 0, 3, 2, N'元', 0, 84, 1, CAST(0x0000A6DA00000000 AS DateTime), NULL, NULL, N'{"default":"1000"}')
/****** Object:  Table [dbo].[CarInsurePlanKind]    Script Date: 04/14/2017 17:00:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CarInsurePlanKind](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CarInsurePlanId] [int] NOT NULL,
	[CarKindId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.CarInsurePlanKind] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[CarInsurePlanKind] ON
INSERT [dbo].[CarInsurePlanKind] ([Id], [CarInsurePlanId], [CarKindId]) VALUES (1, 1, 1)
INSERT [dbo].[CarInsurePlanKind] ([Id], [CarInsurePlanId], [CarKindId]) VALUES (2, 1, 2)
INSERT [dbo].[CarInsurePlanKind] ([Id], [CarInsurePlanId], [CarKindId]) VALUES (3, 1, 3)
INSERT [dbo].[CarInsurePlanKind] ([Id], [CarInsurePlanId], [CarKindId]) VALUES (4, 1, 4)
INSERT [dbo].[CarInsurePlanKind] ([Id], [CarInsurePlanId], [CarKindId]) VALUES (5, 1, 5)
INSERT [dbo].[CarInsurePlanKind] ([Id], [CarInsurePlanId], [CarKindId]) VALUES (6, 1, 6)
INSERT [dbo].[CarInsurePlanKind] ([Id], [CarInsurePlanId], [CarKindId]) VALUES (7, 1, 7)
INSERT [dbo].[CarInsurePlanKind] ([Id], [CarInsurePlanId], [CarKindId]) VALUES (8, 1, 8)
INSERT [dbo].[CarInsurePlanKind] ([Id], [CarInsurePlanId], [CarKindId]) VALUES (9, 1, 9)
INSERT [dbo].[CarInsurePlanKind] ([Id], [CarInsurePlanId], [CarKindId]) VALUES (10, 2, 1)
INSERT [dbo].[CarInsurePlanKind] ([Id], [CarInsurePlanId], [CarKindId]) VALUES (11, 2, 2)
INSERT [dbo].[CarInsurePlanKind] ([Id], [CarInsurePlanId], [CarKindId]) VALUES (12, 2, 3)
INSERT [dbo].[CarInsurePlanKind] ([Id], [CarInsurePlanId], [CarKindId]) VALUES (13, 2, 4)
INSERT [dbo].[CarInsurePlanKind] ([Id], [CarInsurePlanId], [CarKindId]) VALUES (14, 2, 5)
INSERT [dbo].[CarInsurePlanKind] ([Id], [CarInsurePlanId], [CarKindId]) VALUES (15, 2, 6)
INSERT [dbo].[CarInsurePlanKind] ([Id], [CarInsurePlanId], [CarKindId]) VALUES (16, 2, 7)
INSERT [dbo].[CarInsurePlanKind] ([Id], [CarInsurePlanId], [CarKindId]) VALUES (17, 3, 1)
INSERT [dbo].[CarInsurePlanKind] ([Id], [CarInsurePlanId], [CarKindId]) VALUES (18, 3, 2)
INSERT [dbo].[CarInsurePlanKind] ([Id], [CarInsurePlanId], [CarKindId]) VALUES (19, 3, 4)
INSERT [dbo].[CarInsurePlanKind] ([Id], [CarInsurePlanId], [CarKindId]) VALUES (20, 4, 1)
INSERT [dbo].[CarInsurePlanKind] ([Id], [CarInsurePlanId], [CarKindId]) VALUES (21, 4, 2)
INSERT [dbo].[CarInsurePlanKind] ([Id], [CarInsurePlanId], [CarKindId]) VALUES (22, 5, 1)
INSERT [dbo].[CarInsurePlanKind] ([Id], [CarInsurePlanId], [CarKindId]) VALUES (23, 5, 2)
INSERT [dbo].[CarInsurePlanKind] ([Id], [CarInsurePlanId], [CarKindId]) VALUES (24, 5, 3)
INSERT [dbo].[CarInsurePlanKind] ([Id], [CarInsurePlanId], [CarKindId]) VALUES (25, 5, 4)
INSERT [dbo].[CarInsurePlanKind] ([Id], [CarInsurePlanId], [CarKindId]) VALUES (26, 5, 5)
INSERT [dbo].[CarInsurePlanKind] ([Id], [CarInsurePlanId], [CarKindId]) VALUES (27, 5, 6)
INSERT [dbo].[CarInsurePlanKind] ([Id], [CarInsurePlanId], [CarKindId]) VALUES (28, 5, 7)
INSERT [dbo].[CarInsurePlanKind] ([Id], [CarInsurePlanId], [CarKindId]) VALUES (29, 5, 8)
INSERT [dbo].[CarInsurePlanKind] ([Id], [CarInsurePlanId], [CarKindId]) VALUES (30, 5, 9)
INSERT [dbo].[CarInsurePlanKind] ([Id], [CarInsurePlanId], [CarKindId]) VALUES (31, 5, 10)
INSERT [dbo].[CarInsurePlanKind] ([Id], [CarInsurePlanId], [CarKindId]) VALUES (32, 5, 11)
INSERT [dbo].[CarInsurePlanKind] ([Id], [CarInsurePlanId], [CarKindId]) VALUES (33, 5, 12)
INSERT [dbo].[CarInsurePlanKind] ([Id], [CarInsurePlanId], [CarKindId]) VALUES (34, 5, 13)
INSERT [dbo].[CarInsurePlanKind] ([Id], [CarInsurePlanId], [CarKindId]) VALUES (35, 5, 14)
INSERT [dbo].[CarInsurePlanKind] ([Id], [CarInsurePlanId], [CarKindId]) VALUES (36, 5, 15)
INSERT [dbo].[CarInsurePlanKind] ([Id], [CarInsurePlanId], [CarKindId]) VALUES (37, 5, 16)
INSERT [dbo].[CarInsurePlanKind] ([Id], [CarInsurePlanId], [CarKindId]) VALUES (38, 5, 17)
SET IDENTITY_INSERT [dbo].[CarInsurePlanKind] OFF
/****** Object:  Table [dbo].[CarInsurePlan]    Script Date: 04/14/2017 17:00:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CarInsurePlan](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](128) NULL,
	[ImgUrl] [nvarchar](1024) NULL,
	[IsDelete] [bit] NOT NULL,
	[Priority] [int] NOT NULL,
	[Creator] [int] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[Mender] [int] NULL,
	[LastUpdateTime] [datetime] NULL,
 CONSTRAINT [PK_dbo.CarInsurePlan] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[CarInsurePlan] ON
INSERT [dbo].[CarInsurePlan] ([Id], [Name], [ImgUrl], [IsDelete], [Priority], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (1, N'全险套餐', NULL, 0, 99, 0, CAST(0x0000A6DA00000000 AS DateTime), NULL, NULL)
INSERT [dbo].[CarInsurePlan] ([Id], [Name], [ImgUrl], [IsDelete], [Priority], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (2, N'经济套餐', NULL, 0, 98, 0, CAST(0x0000A6CF00000000 AS DateTime), NULL, NULL)
INSERT [dbo].[CarInsurePlan] ([Id], [Name], [ImgUrl], [IsDelete], [Priority], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (3, N'基础套餐', NULL, 0, 97, 0, CAST(0x0000A6DA00000000 AS DateTime), NULL, NULL)
INSERT [dbo].[CarInsurePlan] ([Id], [Name], [ImgUrl], [IsDelete], [Priority], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (4, N'交强险', NULL, 0, 96, 0, CAST(0x0000A6DA00000000 AS DateTime), NULL, NULL)
INSERT [dbo].[CarInsurePlan] ([Id], [Name], [ImgUrl], [IsDelete], [Priority], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (5, N'自定义', NULL, 0, 95, 0, CAST(0x0000A6DA00000000 AS DateTime), NULL, NULL)
SET IDENTITY_INSERT [dbo].[CarInsurePlan] OFF
/****** Object:  Table [dbo].[CarInsureCommissionRate]    Script Date: 04/14/2017 17:00:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CarInsureCommissionRate](
	[Id] [int] NOT NULL,
	[Type] [int] NOT NULL,
	[ReferenceId] [int] NOT NULL,
	[ReferenceName] [nvarchar](128) NULL,
	[Commercial] [decimal](18, 2) NOT NULL,
	[Compulsory] [decimal](18, 2) NOT NULL,
	[Creator] [int] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[Mender] [int] NULL,
	[LastUpdateTime] [datetime] NULL,
 CONSTRAINT [PK_dbo.CommissionRate] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[CarInsureCommissionRate] ([Id], [Type], [ReferenceId], [ReferenceName], [Commercial], [Compulsory], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (1, 1, 1, N'【全线通】平安保险', CAST(24.00 AS Decimal(18, 2)), CAST(3.00 AS Decimal(18, 2)), 0, CAST(0x0000A6DA00000000 AS DateTime), NULL, NULL)
INSERT [dbo].[CarInsureCommissionRate] ([Id], [Type], [ReferenceId], [ReferenceName], [Commercial], [Compulsory], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (2, 1, 2, N'【全线通】阳光保险', CAST(33.00 AS Decimal(18, 2)), CAST(3.00 AS Decimal(18, 2)), 0, CAST(0x0000A6DA00000000 AS DateTime), NULL, NULL)
INSERT [dbo].[CarInsureCommissionRate] ([Id], [Type], [ReferenceId], [ReferenceName], [Commercial], [Compulsory], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (3, 1, 3, N'【全线通】太平洋保险', CAST(26.00 AS Decimal(18, 2)), CAST(3.00 AS Decimal(18, 2)), 0, CAST(0x0000A6F50183A021 AS DateTime), NULL, NULL)
INSERT [dbo].[CarInsureCommissionRate] ([Id], [Type], [ReferenceId], [ReferenceName], [Commercial], [Compulsory], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (4, 1, 4, N'【全线通】亚太保险', CAST(37.00 AS Decimal(18, 2)), CAST(3.00 AS Decimal(18, 2)), 0, CAST(0x0000A6F501890AD6 AS DateTime), NULL, NULL)
INSERT [dbo].[CarInsureCommissionRate] ([Id], [Type], [ReferenceId], [ReferenceName], [Commercial], [Compulsory], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (5, 1, 5, N'【全线通】人民保险', CAST(28.00 AS Decimal(18, 2)), CAST(3.00 AS Decimal(18, 2)), 0, CAST(0x0000A6F60002C8C0 AS DateTime), NULL, NULL)
INSERT [dbo].[CarInsureCommissionRate] ([Id], [Type], [ReferenceId], [ReferenceName], [Commercial], [Compulsory], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (6, 1, 6, N'【全线通】中华保险', CAST(33.00 AS Decimal(18, 2)), CAST(3.60 AS Decimal(18, 2)), 0, CAST(0x0000A6F900000000 AS DateTime), NULL, NULL)
INSERT [dbo].[CarInsureCommissionRate] ([Id], [Type], [ReferenceId], [ReferenceName], [Commercial], [Compulsory], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (7, 2, 1, N'【易办事】', CAST(1.50 AS Decimal(18, 2)), CAST(0.50 AS Decimal(18, 2)), 0, CAST(0x0000A84700000000 AS DateTime), NULL, NULL)
INSERT [dbo].[CarInsureCommissionRate] ([Id], [Type], [ReferenceId], [ReferenceName], [Commercial], [Compulsory], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (8, 3, 1, N'【商户】平安保险', CAST(21.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, CAST(0x0000A84700000000 AS DateTime), NULL, NULL)
INSERT [dbo].[CarInsureCommissionRate] ([Id], [Type], [ReferenceId], [ReferenceName], [Commercial], [Compulsory], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (9, 3, 2, N'【商户】阳光保险', CAST(30.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, CAST(0x0000A84700000000 AS DateTime), NULL, NULL)
INSERT [dbo].[CarInsureCommissionRate] ([Id], [Type], [ReferenceId], [ReferenceName], [Commercial], [Compulsory], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (10, 3, 3, N'【商户】太平洋保险', CAST(23.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, CAST(0x0000A84700000000 AS DateTime), NULL, NULL)
INSERT [dbo].[CarInsureCommissionRate] ([Id], [Type], [ReferenceId], [ReferenceName], [Commercial], [Compulsory], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (11, 3, 4, N'【商户】亚太保险', CAST(34.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, CAST(0x0000A84700000000 AS DateTime), NULL, NULL)
INSERT [dbo].[CarInsureCommissionRate] ([Id], [Type], [ReferenceId], [ReferenceName], [Commercial], [Compulsory], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (12, 3, 5, N'【商户】人民保险', CAST(25.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, CAST(0x0000A84700000000 AS DateTime), NULL, NULL)
INSERT [dbo].[CarInsureCommissionRate] ([Id], [Type], [ReferenceId], [ReferenceName], [Commercial], [Compulsory], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (13, 3, 6, N'【商户】中华保险', CAST(30.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, CAST(0x0000A84700000000 AS DateTime), NULL, NULL)
/****** Object:  Table [dbo].[CarInsuranceCompany]    Script Date: 04/14/2017 17:00:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CarInsuranceCompany](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[InsuranceCompanyId] [int] NOT NULL,
	[Creator] [int] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[InsuranceCompanyImgUrl] [nvarchar](1024) NULL,
	[Status] [int] NOT NULL,
	[Mender] [int] NULL,
	[LastUpdateTime] [datetime] NULL,
	[InsuranceCompanyName] [nvarchar](128) NULL,
	[Priority] [int] NOT NULL,
 CONSTRAINT [PK_dbo.CarInsuranceCompany] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[CarInsuranceCompany] ON
INSERT [dbo].[CarInsuranceCompany] ([Id], [InsuranceCompanyId], [Creator], [CreateTime], [InsuranceCompanyImgUrl], [Status], [Mender], [LastUpdateTime], [InsuranceCompanyName], [Priority]) VALUES (1, 1, 0, CAST(0x0000A6DA00000000 AS DateTime), N'https://file.ins-uplink.cn/upload/CarInsuranceCompany/zgpabx.png', 2, NULL, NULL, N'平安保险', 10)
INSERT [dbo].[CarInsuranceCompany] ([Id], [InsuranceCompanyId], [Creator], [CreateTime], [InsuranceCompanyImgUrl], [Status], [Mender], [LastUpdateTime], [InsuranceCompanyName], [Priority]) VALUES (2, 2, 0, CAST(0x0000A6DA00000000 AS DateTime), N'https://file.ins-uplink.cn/upload/CarInsuranceCompany/tpybx.png', 2, NULL, NULL, N'太平洋保险', 9)
INSERT [dbo].[CarInsuranceCompany] ([Id], [InsuranceCompanyId], [Creator], [CreateTime], [InsuranceCompanyImgUrl], [Status], [Mender], [LastUpdateTime], [InsuranceCompanyName], [Priority]) VALUES (3, 3, 0, CAST(0x0000A6DA00000000 AS DateTime), N'https://file.ins-uplink.cn/upload/CarInsuranceCompany/ygbx.png', 2, NULL, NULL, N'阳光保险', 8)
INSERT [dbo].[CarInsuranceCompany] ([Id], [InsuranceCompanyId], [Creator], [CreateTime], [InsuranceCompanyImgUrl], [Status], [Mender], [LastUpdateTime], [InsuranceCompanyName], [Priority]) VALUES (4, 4, 0, CAST(0x0000A6DA00000000 AS DateTime), N'https://file.ins-uplink.cn/upload/CarInsuranceCompany/ytbx.png', 2, NULL, NULL, N'亚太保险', 7)
INSERT [dbo].[CarInsuranceCompany] ([Id], [InsuranceCompanyId], [Creator], [CreateTime], [InsuranceCompanyImgUrl], [Status], [Mender], [LastUpdateTime], [InsuranceCompanyName], [Priority]) VALUES (5, 5, 0, CAST(0x0000A6DA00000000 AS DateTime), N'https://file.ins-uplink.cn/upload/CarInsuranceCompany/zgrmbx.png', 2, NULL, NULL, N'人民保险', 6)
SET IDENTITY_INSERT [dbo].[CarInsuranceCompany] OFF
/****** Object:  Table [dbo].[BizProcessesAuditDetails]    Script Date: 04/14/2017 17:00:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BizProcessesAuditDetails](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AuditStep] [int] NOT NULL,
	[BizProcessesAuditId] [int] NOT NULL,
	[Auditor] [int] NULL,
	[AuditComments] [nvarchar](1024) NULL,
	[Description] [nvarchar](1024) NULL,
	[AuditTime] [datetime] NULL,
	[StartTime] [datetime] NOT NULL,
	[EndTime] [datetime] NULL,
	[Creator] [int] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[Mender] [int] NULL,
	[LastUpdateTime] [datetime] NULL,
	[AuditStepEnumName] [nvarchar](128) NULL,
 CONSTRAINT [PK_dbo.BizProcessesAuditDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BizProcessesAudit]    Script Date: 04/14/2017 17:00:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BizProcessesAudit](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AduitType] [int] NOT NULL,
	[AduitReferenceId] [int] NOT NULL,
	[Auditor] [int] NULL,
	[StartTime] [datetime] NOT NULL,
	[EndTime] [datetime] NULL,
	[Status] [int] NOT NULL,
	[Result] [int] NULL,
	[Remark] [nvarchar](1024) NULL,
	[Creator] [int] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[Mender] [int] NULL,
	[LastUpdateTime] [datetime] NULL,
	[Reason] [nvarchar](1024) NULL,
	[JsonData] [nvarchar](max) NULL,
	[AduitTypeEnumName] [nvarchar](128) NULL,
 CONSTRAINT [PK_dbo.BizProcessesAudit] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BankCard]    Script Date: 04/14/2017 17:00:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BankCard](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[MerchantId] [int] NOT NULL,
	[BankId] [int] NOT NULL,
	[BankCode] [nvarchar](128) NULL,
	[BankName] [nvarchar](128) NULL,
	[BankAccountName] [nvarchar](128) NULL,
	[BankAccountNo] [nvarchar](128) NULL,
	[BankAccountPhone] [nvarchar](128) NULL,
	[IsDelete] [bit] NOT NULL,
	[Creator] [int] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[Mender] [int] NULL,
	[LastUpdateTime] [datetime] NULL,
 CONSTRAINT [PK_dbo.BankCard] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[BankCard] ON
INSERT [dbo].[BankCard] ([Id], [UserId], [MerchantId], [BankId], [BankCode], [BankName], [BankAccountName], [BankAccountNo], [BankAccountPhone], [IsDelete], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (1, 1001, 1, 0, NULL, NULL, NULL, NULL, NULL, 0, 0, CAST(0x0000A7550113FC4A AS DateTime), NULL, NULL)
SET IDENTITY_INSERT [dbo].[BankCard] OFF
/****** Object:  Table [dbo].[Bank]    Script Date: 04/14/2017 17:00:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Bank](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](128) NULL,
	[Name] [nvarchar](128) NULL,
	[Creator] [int] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[Mender] [int] NULL,
	[LastUpdateTime] [datetime] NULL,
 CONSTRAINT [PK_dbo.Bank] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[__MigrationHistory]    Script Date: 04/14/2017 17:00:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[__MigrationHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ContextKey] [nvarchar](300) NOT NULL,
	[Model] [varbinary](max) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK_dbo.__MigrationHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC,
	[ContextKey] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[OrderToCarInsureOfferKind]    Script Date: 04/14/2017 17:00:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderToCarInsureOfferKind](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[OrderId] [int] NOT NULL,
	[Creator] [int] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[Mender] [int] NULL,
	[LastUpdateTime] [datetime] NULL,
	[KindId] [int] NOT NULL,
	[KindValue] [nvarchar](128) NULL,
	[KindDetails] [nvarchar](1024) NULL,
	[IsWaiverDeductible] [bit] NOT NULL,
 CONSTRAINT [PK_dbo.OrderToCarInsureOfferKind] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderToCarInsureOfferCompany]    Script Date: 04/14/2017 17:00:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderToCarInsureOfferCompany](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[OrderId] [int] NOT NULL,
	[InsuranceCompanyId] [int] NOT NULL,
	[CommercialPrice] [decimal](18, 2) NULL,
	[TravelTaxPrice] [decimal](18, 2) NULL,
	[CompulsoryPrice] [decimal](18, 2) NULL,
	[Creator] [int] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[Mender] [int] NULL,
	[LastUpdateTime] [datetime] NULL,
	[InsureImgUrl] [nvarchar](1024) NULL,
	[InsureTotalPrice] [decimal](18, 2) NULL,
	[InsuranceOrderId] [nvarchar](128) NULL,
	[InsuranceCompanyName] [nvarchar](128) NULL,
 CONSTRAINT [PK_dbo.OrderToCarInsureOfferCompany] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SysBannerType]    Script Date: 04/14/2017 17:00:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SysBannerType](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](128) NULL,
	[Description] [nvarchar](1024) NULL,
	[Creator] [int] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[Mender] [int] NULL,
	[LastUpdateTime] [datetime] NULL,
 CONSTRAINT [PK_dbo.SysBannerType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[SysBannerType] ([Id], [Name], [Description], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (1, N'首页-头部轮换广告', N'用于在首页头部里展示', 0, CAST(0x0000A6DA00000000 AS DateTime), NULL, NULL)
/****** Object:  Table [dbo].[SysBanner]    Script Date: 04/14/2017 17:00:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SysBanner](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](128) NULL,
	[ImgUrl] [nvarchar](1024) NULL,
	[Priority] [int] NOT NULL,
	[Creator] [int] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[Content] [nvarchar](max) NULL,
	[ReadCount] [int] NOT NULL,
	[Type] [int] NOT NULL,
	[LinkUrl] [nvarchar](1024) NULL,
	[Mender] [int] NULL,
	[LastUpdateTime] [datetime] NULL,
	[Description] [nvarchar](1024) NULL,
	[Source] [nvarchar](512) NULL,
	[Status] [int] NOT NULL,
 CONSTRAINT [PK_dbo.SysBannerImage] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[SysBanner] ON
INSERT [dbo].[SysBanner] ([Id], [Title], [ImgUrl], [Priority], [Creator], [CreateTime], [Content], [ReadCount], [Type], [LinkUrl], [Mender], [LastUpdateTime], [Description], [Source], [Status]) VALUES (1, N'全线通云平台简介', N'https://file.ins-uplink.cn/Upload/Banner/a3ba3032-db8e-462e-9336-cfa9bae5cb6e_O.jpg', 0, 1000, CAST(0x0000A6DA00000000 AS DateTime), N'<p>我们</p>

<p>一个专门为汽车后市场商户提供服务的B2B平台。我们是汽车后市场商户的合作伙伴，只为实现与您共赢。<br />
&nbsp;</p>

<p><strong>我们合作，您将实现？</strong></p>

<p>＊收益增长</p>

<p>我们将提供更多营收资源，让您有更多的新收入</p>

<p>＊利润增长</p>

<p>我们将更好的降低成本，让您有更大的利润空间去赚钱</p>

<p>＊车主更密切</p>

<p>我们有效的结合您原有生意，提供符合车主客户生活周边刚需的产品，提高车主与您的交易频率，从而使您与车主关系更密切。</p>

<p>＊共享成果</p>

<p>您不再是单打独斗，畏惧竞争。我们将使平台内所有商户成为紧密合伙人关系，共同分享平台收益。</p>

<p><strong>我们怎么做？</strong></p>

<p>&nbsp;</p>

<p><strong>1.使您有更多收入来源</strong></p>

<p><strong>在我们平台上，您可以轻松无忧地销售车险，承接理赔，代办交通违章，代办年审，满足车主生活所需。我们将提&nbsp;供全面服务支持您。</strong></p>

<p>&nbsp;</p>

<p><strong>2.让您有更好的利润空间</strong></p>

<p><strong>我们利用平台资源，为您争取更好的产品销售返点，更低的产品进货成本，更方便的售后服务，更有效的平台支持。让您能轻松赚钱，保持更高利润率。</strong></p>

<p>&nbsp;</p>

<p><strong>3.协助您满足车主日常所需</strong></p>

<p><strong>在我们平台，您除了满足车主汽车养护需求之外，还能满足车主生活所需。从电视台栏目直击的原产地生鲜产品，到跨境名优商品等。我们都将为您接入资源。</strong></p>

<p>&nbsp;</p>

<p><strong>4.实现您的伙伴共赢</strong></p>

<p><strong>我们将会联动平台内所有商户伙伴，结成紧密的合伙人关系，全平台的收益均能所有人共享。让您脱离单打独斗的竞争局面，从而实现团队作战的高效成果。</strong></p>

<p>&nbsp;</p>

<p><strong>我们为什么能做到？</strong></p>

<p>&nbsp;</p>

<p><strong>全线通</strong>成立于2001年，近20年的银行、金融、保险、传媒、电商的行业经验，合作伙伴遍及电信、银行、保险、地产、旅游和娱乐消费等各大行业。</p>

<p>2016年，全线通更与深圳银联易办事达成战略合作，借助银联云POS终端，搭建线下车后市场商户网络。并与广东卫视，建设银行，太平洋保险，安心保险，中国平安，中国太平，亚太财险，中国人民保险等实力雄厚的知名公司签订合作协议。</p>

<p>&nbsp;</p>

<p>因为有您的支持，我们才能实现共赢！在这里，我们只为您服务！</p>

<p>&nbsp;</p>

<p>&nbsp;</p>

<ol>
	<li>
	<p>&nbsp;</p>
	</li>
</ol>
', 121, 1, NULL, NULL, NULL, NULL, N'全线通', 2)
SET IDENTITY_INSERT [dbo].[SysBanner] OFF
/****** Object:  Table [dbo].[SysAppKeySecret]    Script Date: 04/14/2017 17:00:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SysAppKeySecret](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Key] [nvarchar](128) NULL,
	[Secret] [nvarchar](128) NULL,
	[Status] [int] NOT NULL,
	[Creator] [int] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[Mender] [int] NULL,
	[LastUpdateTime] [datetime] NULL,
 CONSTRAINT [PK_dbo.SysAppKeySecret] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[SysAppKeySecret] ON
INSERT [dbo].[SysAppKeySecret] ([Id], [Key], [Secret], [Status], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (1, N'test', N'6ZB97cdVz211O08EKZ6yriAYrHXFBowC', 1, 1, CAST(0x0000A6DA00000000 AS DateTime), NULL, NULL)
INSERT [dbo].[SysAppKeySecret] ([Id], [Key], [Secret], [Status], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (2, N'ybs_test', N'6ZB87cdVz222O08EKZ6yri8YrHXFBowA', 1, 1, CAST(0x0000A6DA00000000 AS DateTime), NULL, NULL)
INSERT [dbo].[SysAppKeySecret] ([Id], [Key], [Secret], [Status], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (4, N'uplinkCarApp', N'7460e6512f1940f68c00fe1fdb2b7eb1', 11, 1, CAST(0x0000A6DA00000000 AS DateTime), NULL, NULL)
INSERT [dbo].[SysAppKeySecret] ([Id], [Key], [Secret], [Status], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (5, N'ybsCarApp', N'11ca0a655dce4fd883dc2ca8550cd9f5', 11, 11, CAST(0x0000A6DA00000000 AS DateTime), NULL, NULL)
SET IDENTITY_INSERT [dbo].[SysAppKeySecret] OFF
/****** Object:  Table [dbo].[SalesmanApplyPosRecord]    Script Date: 04/14/2017 17:00:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SalesmanApplyPosRecord](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SalesmanId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[MerchantId] [int] NOT NULL,
	[PosMachineId] [int] NOT NULL,
	[Creator] [int] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[Remarks] [nvarchar](1024) NULL,
 CONSTRAINT [PK_dbo.SalesmanApplyPosRecord] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Product]    Script Date: 04/14/2017 17:00:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Product](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Type] [int] NOT NULL,
	[Name] [nvarchar](128) NULL,
	[Creator] [int] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[Mender] [int] NULL,
	[LastUpdateTime] [datetime] NULL,
	[Price] [decimal](18, 2) NOT NULL,
	[MainImgUrl] [nvarchar](1024) NULL,
	[ElseImgUrls] [nvarchar](2048) NULL,
	[Details] [nvarchar](max) NULL,
	[Status] [int] NOT NULL,
 CONSTRAINT [PK_dbo.Product] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Product] ON
INSERT [dbo].[Product] ([Id], [Type], [Name], [Creator], [CreateTime], [Mender], [LastUpdateTime], [Price], [MainImgUrl], [ElseImgUrls], [Details], [Status]) VALUES (1011, 1011, N'壳牌 (Shell) 蓝喜力半合成机油 Helix HX7 5W-40 SN级 4L', 0, CAST(0x0000A6CF00000000 AS DateTime), NULL, NULL, CAST(232.00 AS Decimal(18, 2)), N'https://file.ins-uplink.cn/Upload/product/e895dd43-45b3-477f-87e9-e8fb00781756_O.jpg', N'["https://file.ins-uplink.cn/Upload/product/4957c461-b856-4042-9d3d-48f5b4e978d0_O.jpg","https://file.ins-uplink.cn/Upload/product/979e5a7e-94e9-407a-98e6-227a44ba8b35_O.jpg","",""]', N'<p><img alt="" src="https://file.ins-uplink.cn/Upload/CkEditorFile/5ffc7261-6a4e-460a-8a6f-906fa24e1099_O.jpg" style="width:100%" /></p>', 0)
INSERT [dbo].[Product] ([Id], [Type], [Name], [Creator], [CreateTime], [Mender], [LastUpdateTime], [Price], [MainImgUrl], [ElseImgUrls], [Details], [Status]) VALUES (2011, 2011, N'车险投保', 0, CAST(0x0000A6DA00000000 AS DateTime), NULL, NULL, CAST(0.00 AS Decimal(18, 2)), NULL, NULL, NULL, 0)
INSERT [dbo].[Product] ([Id], [Type], [Name], [Creator], [CreateTime], [Mender], [LastUpdateTime], [Price], [MainImgUrl], [ElseImgUrls], [Details], [Status]) VALUES (2012, 2012, N'车险续保', 0, CAST(0x0000A6DA00000000 AS DateTime), NULL, NULL, CAST(0.00 AS Decimal(18, 2)), NULL, NULL, NULL, 0)
INSERT [dbo].[Product] ([Id], [Type], [Name], [Creator], [CreateTime], [Mender], [LastUpdateTime], [Price], [MainImgUrl], [ElseImgUrls], [Details], [Status]) VALUES (2013, 2013, N'车险理赔', 0, CAST(0x0000A6DA00000000 AS DateTime), NULL, NULL, CAST(0.00 AS Decimal(18, 2)), NULL, NULL, NULL, 0)
SET IDENTITY_INSERT [dbo].[Product] OFF
/****** Object:  Table [dbo].[PosMachine]    Script Date: 04/14/2017 17:00:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PosMachine](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FuselageNumber] [nvarchar](128) NULL,
	[Creator] [int] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[Mender] [int] NULL,
	[LastUpdateTime] [datetime] NULL,
	[TerminalNumber] [nvarchar](128) NULL,
	[Version] [nvarchar](128) NULL,
	[IsUse] [bit] NOT NULL,
	[DeviceId] [nvarchar](128) NULL,
	[Deposit] [decimal](18, 2) NOT NULL,
	[Rent] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_dbo.PosMachine] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[PosMachine] ON
INSERT [dbo].[PosMachine] ([Id], [FuselageNumber], [Creator], [CreateTime], [Mender], [LastUpdateTime], [TerminalNumber], [Version], [IsUse], [DeviceId], [Deposit], [Rent]) VALUES (1, N'test', 1000, CAST(0x0000A7550113CD54 AS DateTime), 0, CAST(0x0000A7550113FC4A AS DateTime), N'test', N'test', 1, N'869612024498216', CAST(1000.00 AS Decimal(18, 2)), CAST(200.00 AS Decimal(18, 2)))
SET IDENTITY_INSERT [dbo].[PosMachine] OFF
/****** Object:  Table [dbo].[SysRoleMenu]    Script Date: 04/14/2017 17:00:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SysRoleMenu](
	[RoleId] [int] NOT NULL,
	[MenuId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.SysRoleMenu] PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC,
	[MenuId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (1, 1)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (1, 2)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (1, 3)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (1, 4)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (1, 5)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (1, 6)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (1, 7)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (1, 18)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (1, 19)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (1, 20)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (1, 22)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (1, 24)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (1, 27)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (1, 30)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (1, 31)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (1, 35)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (1, 36)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (1, 37)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (1, 38)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (1, 39)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (1, 40)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (1, 41)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (1, 42)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (1, 50)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (1, 53)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (1, 55)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (1, 56)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (1, 60)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (1, 61)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (1, 62)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (1, 63)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (1, 64)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (1, 65)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (1, 66)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (1, 67)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (1, 68)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (1, 69)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (1, 70)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (1, 74)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (1, 75)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (1, 76)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (2, 1)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (2, 20)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (2, 22)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (2, 24)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (2, 25)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (2, 31)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (2, 64)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (2, 65)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (2, 66)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (2, 67)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (2, 68)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (2, 69)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (3, 1)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (3, 18)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (3, 19)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (3, 20)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (3, 22)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (3, 23)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (3, 24)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (3, 31)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (3, 33)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (3, 43)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (3, 45)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (3, 46)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (3, 48)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (3, 50)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (3, 53)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (3, 55)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (3, 56)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (3, 57)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (3, 58)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (3, 60)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (3, 61)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (3, 62)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (3, 63)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (3, 64)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (3, 65)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (3, 66)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (3, 67)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (3, 68)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (3, 69)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (3, 70)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (3, 71)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (3, 72)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (3, 73)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (4, 1)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (4, 18)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (4, 19)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (4, 20)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (4, 22)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (4, 23)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (4, 24)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (4, 27)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (4, 30)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (4, 31)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (4, 43)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (4, 45)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (4, 46)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (4, 48)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (4, 50)
GO
print 'Processed 100 total records'
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (4, 53)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (4, 54)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (4, 55)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (4, 56)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (4, 59)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (4, 60)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (4, 61)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (4, 62)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (4, 63)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (4, 64)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (4, 65)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (4, 66)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (4, 67)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (4, 68)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (4, 69)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (4, 70)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (4, 71)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (4, 72)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (4, 73)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (5, 1)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (5, 20)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (5, 22)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (5, 27)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (5, 28)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (5, 29)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (5, 30)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (5, 60)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (5, 61)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (5, 62)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (5, 63)
INSERT [dbo].[SysRoleMenu] ([RoleId], [MenuId]) VALUES (5, 70)
/****** Object:  Table [dbo].[SysRole]    Script Date: 04/14/2017 17:00:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SysRole](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PId] [int] NOT NULL,
	[Description] [nvarchar](512) NULL,
	[Name] [nvarchar](256) NOT NULL,
	[Creator] [int] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[Mender] [int] NULL,
	[LastUpdateTime] [datetime] NULL,
 CONSTRAINT [PK_dbo.SysRole] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[SysRole] ON
INSERT [dbo].[SysRole] ([Id], [PId], [Description], [Name], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (1, 0, N'www', N'系统管理员', 0, CAST(0x0000000000000000 AS DateTime), 1, CAST(0x0000A6E0017AD447 AS DateTime))
INSERT [dbo].[SysRole] ([Id], [PId], [Description], [Name], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (2, 0, NULL, N'投保报价', 1, CAST(0x0000A6ED015F50F4 AS DateTime), 1000, CAST(0x0000A72C00B03034 AS DateTime))
INSERT [dbo].[SysRole] ([Id], [PId], [Description], [Name], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (3, 0, NULL, N'运营维护（初级）', 1000, CAST(0x0000A72C00B01F3E AS DateTime), 1000, CAST(0x0000A72C00B12148 AS DateTime))
INSERT [dbo].[SysRole] ([Id], [PId], [Description], [Name], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (4, 0, NULL, N'运营维护（高级）', 1000, CAST(0x0000A72C00B039AD AS DateTime), 1000, CAST(0x0000A72C00B130C8 AS DateTime))
INSERT [dbo].[SysRole] ([Id], [PId], [Description], [Name], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (5, 0, NULL, N'财务主管', 1000, CAST(0x0000A72C00B0B913 AS DateTime), NULL, NULL)
SET IDENTITY_INSERT [dbo].[SysRole] OFF
/****** Object:  Table [dbo].[SysProvinceCity]    Script Date: 04/14/2017 17:00:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SysProvinceCity](
	[Id] [nvarchar](128) NOT NULL,
	[PId] [nvarchar](128) NOT NULL,
	[Name] [nvarchar](128) NOT NULL,
	[FullName] [nvarchar](128) NULL,
	[PhoneAreaNo] [nvarchar](128) NULL,
	[Zip] [nvarchar](128) NULL,
	[Creator] [int] NOT NULL,
	[CreateTime] [datetime] NULL,
	[Priority] [int] NOT NULL,
	[IsDelete] [bit] NOT NULL,
 CONSTRAINT [PK_dbo.SysProvinceCity] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SysPermission]    Script Date: 04/14/2017 17:00:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SysPermission](
	[Id] [varchar](128) NOT NULL,
	[Name] [nvarchar](128) NOT NULL,
	[PId] [nvarchar](128) NOT NULL,
	[Description] [nvarchar](512) NULL,
 CONSTRAINT [PK_dbo.SysPermission] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SysPageAccessRecord]    Script Date: 04/14/2017 17:00:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SysPageAccessRecord](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[PageUrl] [varchar](256) NULL,
	[Ip] [nvarchar](128) NULL,
	[AccessTime] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.SysPageAccessRecord] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[SysPageAccessRecord] ON
INSERT [dbo].[SysPageAccessRecord] ([Id], [UserId], [PageUrl], [Ip], [AccessTime]) VALUES (1, 0, N'/', N'112.74.179.185', CAST(0x0000A75501109023 AS DateTime))
INSERT [dbo].[SysPageAccessRecord] ([Id], [UserId], [PageUrl], [Ip], [AccessTime]) VALUES (2, 0, N'/', N'::1', CAST(0x0000A75501109909 AS DateTime))
INSERT [dbo].[SysPageAccessRecord] ([Id], [UserId], [PageUrl], [Ip], [AccessTime]) VALUES (3, 0, N'/', N'112.74.179.185', CAST(0x0000A7550110AB92 AS DateTime))
INSERT [dbo].[SysPageAccessRecord] ([Id], [UserId], [PageUrl], [Ip], [AccessTime]) VALUES (4, 0, N'/', N'113.108.198.138', CAST(0x0000A7550110B41F AS DateTime))
INSERT [dbo].[SysPageAccessRecord] ([Id], [UserId], [PageUrl], [Ip], [AccessTime]) VALUES (5, 0, N'/Common/GetImgVerifyCode', N'113.108.198.138', CAST(0x0000A7550110BE4E AS DateTime))
INSERT [dbo].[SysPageAccessRecord] ([Id], [UserId], [PageUrl], [Ip], [AccessTime]) VALUES (6, 1000, N'/Home/Index', N'113.108.198.138', CAST(0x0000A7550110C912 AS DateTime))
INSERT [dbo].[SysPageAccessRecord] ([Id], [UserId], [PageUrl], [Ip], [AccessTime]) VALUES (7, 1000, N'/biz/report/noactiveaccount', N'113.108.198.138', CAST(0x0000A7550110CB52 AS DateTime))
INSERT [dbo].[SysPageAccessRecord] ([Id], [UserId], [PageUrl], [Ip], [AccessTime]) VALUES (8, 1000, N'/biz/extendedapp/applylist', N'113.108.198.138', CAST(0x0000A7550110CED9 AS DateTime))
INSERT [dbo].[SysPageAccessRecord] ([Id], [UserId], [PageUrl], [Ip], [AccessTime]) VALUES (9, 1000, N'/biz/report/withdraw', N'113.108.198.138', CAST(0x0000A7550110D330 AS DateTime))
INSERT [dbo].[SysPageAccessRecord] ([Id], [UserId], [PageUrl], [Ip], [AccessTime]) VALUES (10, 1000, N'/biz/report/noactiveaccount', N'113.108.198.138', CAST(0x0000A7550110D4F7 AS DateTime))
INSERT [dbo].[SysPageAccessRecord] ([Id], [UserId], [PageUrl], [Ip], [AccessTime]) VALUES (11, 1000, N'/biz/report/withdraw', N'113.108.198.138', CAST(0x0000A7550110D5D5 AS DateTime))
INSERT [dbo].[SysPageAccessRecord] ([Id], [UserId], [PageUrl], [Ip], [AccessTime]) VALUES (12, 1000, N'/biz/report/carinsure', N'113.108.198.138', CAST(0x0000A7550110D664 AS DateTime))
INSERT [dbo].[SysPageAccessRecord] ([Id], [UserId], [PageUrl], [Ip], [AccessTime]) VALUES (13, 1000, N'/biz/order/list', N'113.108.198.138', CAST(0x0000A7550110D7AC AS DateTime))
INSERT [dbo].[SysPageAccessRecord] ([Id], [UserId], [PageUrl], [Ip], [AccessTime]) VALUES (14, 1000, N'/sys/menu/list', N'113.108.198.138', CAST(0x0000A7550110DE84 AS DateTime))
INSERT [dbo].[SysPageAccessRecord] ([Id], [UserId], [PageUrl], [Ip], [AccessTime]) VALUES (15, 1000, N'/sys/Menu/Add', N'113.108.198.138', CAST(0x0000A755011136A6 AS DateTime))
INSERT [dbo].[SysPageAccessRecord] ([Id], [UserId], [PageUrl], [Ip], [AccessTime]) VALUES (16, 1000, N'/sys/Menu/Add', N'113.108.198.138', CAST(0x0000A755011148C4 AS DateTime))
INSERT [dbo].[SysPageAccessRecord] ([Id], [UserId], [PageUrl], [Ip], [AccessTime]) VALUES (17, 1000, N'/sys/Menu/Add', N'113.108.198.138', CAST(0x0000A755011152DA AS DateTime))
INSERT [dbo].[SysPageAccessRecord] ([Id], [UserId], [PageUrl], [Ip], [AccessTime]) VALUES (18, 1000, N'/sys/menu/list', N'113.108.198.138', CAST(0x0000A7550111854C AS DateTime))
INSERT [dbo].[SysPageAccessRecord] ([Id], [UserId], [PageUrl], [Ip], [AccessTime]) VALUES (19, 1000, N'/sys/menu/list', N'113.108.198.138', CAST(0x0000A7550111866D AS DateTime))
INSERT [dbo].[SysPageAccessRecord] ([Id], [UserId], [PageUrl], [Ip], [AccessTime]) VALUES (20, 1000, N'/biz/report/withdraw', N'113.108.198.138', CAST(0x0000A75501118AAF AS DateTime))
INSERT [dbo].[SysPageAccessRecord] ([Id], [UserId], [PageUrl], [Ip], [AccessTime]) VALUES (21, 1000, N'/Home/ChangePassword', N'113.108.198.138', CAST(0x0000A75501118CA3 AS DateTime))
INSERT [dbo].[SysPageAccessRecord] ([Id], [UserId], [PageUrl], [Ip], [AccessTime]) VALUES (22, 1000, N'/Home/ChangePassword', N'113.108.198.138', CAST(0x0000A7550111F931 AS DateTime))
INSERT [dbo].[SysPageAccessRecord] ([Id], [UserId], [PageUrl], [Ip], [AccessTime]) VALUES (23, 0, N'/Home/Login', N'113.108.198.138', CAST(0x0000A75501120B8E AS DateTime))
INSERT [dbo].[SysPageAccessRecord] ([Id], [UserId], [PageUrl], [Ip], [AccessTime]) VALUES (24, 0, N'/Common/GetImgVerifyCode', N'113.108.198.138', CAST(0x0000A75501120BB9 AS DateTime))
INSERT [dbo].[SysPageAccessRecord] ([Id], [UserId], [PageUrl], [Ip], [AccessTime]) VALUES (25, 1000, N'/Home/Index', N'113.108.198.138', CAST(0x0000A75501121761 AS DateTime))
INSERT [dbo].[SysPageAccessRecord] ([Id], [UserId], [PageUrl], [Ip], [AccessTime]) VALUES (26, 1000, N'/biz/carinsureoffer/list', N'113.108.198.138', CAST(0x0000A75501121BC0 AS DateTime))
INSERT [dbo].[SysPageAccessRecord] ([Id], [UserId], [PageUrl], [Ip], [AccessTime]) VALUES (27, 1000, N'/biz/insurancecompany/list', N'113.108.198.138', CAST(0x0000A75501122E35 AS DateTime))
INSERT [dbo].[SysPageAccessRecord] ([Id], [UserId], [PageUrl], [Ip], [AccessTime]) VALUES (28, 1000, N'/sys/role/list', N'113.108.198.138', CAST(0x0000A755011232F9 AS DateTime))
INSERT [dbo].[SysPageAccessRecord] ([Id], [UserId], [PageUrl], [Ip], [AccessTime]) VALUES (29, 1000, N'/sys/role/list', N'113.108.198.138', CAST(0x0000A75501125EF3 AS DateTime))
INSERT [dbo].[SysPageAccessRecord] ([Id], [UserId], [PageUrl], [Ip], [AccessTime]) VALUES (30, 1000, N'/biz/posmachine/list', N'113.108.198.138', CAST(0x0000A7550112624A AS DateTime))
INSERT [dbo].[SysPageAccessRecord] ([Id], [UserId], [PageUrl], [Ip], [AccessTime]) VALUES (31, 1000, N'/biz/PosMachine/Add', N'113.108.198.138', CAST(0x0000A75501126434 AS DateTime))
INSERT [dbo].[SysPageAccessRecord] ([Id], [UserId], [PageUrl], [Ip], [AccessTime]) VALUES (32, 1000, N'/biz/posmachine/list', N'113.108.198.138', CAST(0x0000A7550113D0B8 AS DateTime))
INSERT [dbo].[SysPageAccessRecord] ([Id], [UserId], [PageUrl], [Ip], [AccessTime]) VALUES (33, 1000, N'/biz/posmachine/list', N'113.108.198.138', CAST(0x0000A7550113DDE8 AS DateTime))
INSERT [dbo].[SysPageAccessRecord] ([Id], [UserId], [PageUrl], [Ip], [AccessTime]) VALUES (34, 1000, N'/biz/posmachine/list', N'113.108.198.138', CAST(0x0000A7550113DEFF AS DateTime))
INSERT [dbo].[SysPageAccessRecord] ([Id], [UserId], [PageUrl], [Ip], [AccessTime]) VALUES (35, 1000, N'/biz/posmachine/list', N'113.108.198.138', CAST(0x0000A7550113E5AD AS DateTime))
INSERT [dbo].[SysPageAccessRecord] ([Id], [UserId], [PageUrl], [Ip], [AccessTime]) VALUES (36, 1000, N'/biz/report/noactiveaccount', N'113.108.198.138', CAST(0x0000A7550113E71B AS DateTime))
INSERT [dbo].[SysPageAccessRecord] ([Id], [UserId], [PageUrl], [Ip], [AccessTime]) VALUES (37, 1000, N'/biz/report/noactiveaccount', N'113.108.198.138', CAST(0x0000A75501140061 AS DateTime))
INSERT [dbo].[SysPageAccessRecord] ([Id], [UserId], [PageUrl], [Ip], [AccessTime]) VALUES (38, 1000, N'/', N'113.108.198.138', CAST(0x0000A75501144F73 AS DateTime))
INSERT [dbo].[SysPageAccessRecord] ([Id], [UserId], [PageUrl], [Ip], [AccessTime]) VALUES (39, 1000, N'/Home/Index', N'113.108.198.138', CAST(0x0000A75501144F7A AS DateTime))
INSERT [dbo].[SysPageAccessRecord] ([Id], [UserId], [PageUrl], [Ip], [AccessTime]) VALUES (40, 1000, N'/biz/report/noactiveaccount', N'113.108.198.138', CAST(0x0000A75501171309 AS DateTime))
INSERT [dbo].[SysPageAccessRecord] ([Id], [UserId], [PageUrl], [Ip], [AccessTime]) VALUES (41, 1000, N'/sys/menu/list', N'113.108.198.138', CAST(0x0000A75501171810 AS DateTime))
INSERT [dbo].[SysPageAccessRecord] ([Id], [UserId], [PageUrl], [Ip], [AccessTime]) VALUES (42, 1000, N'/sys/role/list', N'113.108.198.138', CAST(0x0000A75501171C08 AS DateTime))
INSERT [dbo].[SysPageAccessRecord] ([Id], [UserId], [PageUrl], [Ip], [AccessTime]) VALUES (43, 1000, N'/sys/role/list', N'113.108.198.138', CAST(0x0000A75501172770 AS DateTime))
INSERT [dbo].[SysPageAccessRecord] ([Id], [UserId], [PageUrl], [Ip], [AccessTime]) VALUES (44, 1000, N'/biz/report/withdraw', N'113.108.198.138', CAST(0x0000A7550117299E AS DateTime))
INSERT [dbo].[SysPageAccessRecord] ([Id], [UserId], [PageUrl], [Ip], [AccessTime]) VALUES (45, 1000, N'/biz/report/merchant', N'113.108.198.138', CAST(0x0000A75501172AAC AS DateTime))
INSERT [dbo].[SysPageAccessRecord] ([Id], [UserId], [PageUrl], [Ip], [AccessTime]) VALUES (46, 1000, N'/biz/report/carinsure', N'113.108.198.138', CAST(0x0000A75501172B98 AS DateTime))
INSERT [dbo].[SysPageAccessRecord] ([Id], [UserId], [PageUrl], [Ip], [AccessTime]) VALUES (47, 1000, N'/biz/report/depositrent', N'113.108.198.138', CAST(0x0000A75501172C6E AS DateTime))
INSERT [dbo].[SysPageAccessRecord] ([Id], [UserId], [PageUrl], [Ip], [AccessTime]) VALUES (48, 1000, N'/biz/report/merchant', N'113.108.198.138', CAST(0x0000A75501172ECD AS DateTime))
INSERT [dbo].[SysPageAccessRecord] ([Id], [UserId], [PageUrl], [Ip], [AccessTime]) VALUES (49, 1000, N'/biz/report/carinsure', N'113.108.198.138', CAST(0x0000A75501174117 AS DateTime))
INSERT [dbo].[SysPageAccessRecord] ([Id], [UserId], [PageUrl], [Ip], [AccessTime]) VALUES (50, 1000, N'/biz/report/merchant', N'113.108.198.138', CAST(0x0000A755011741E5 AS DateTime))
INSERT [dbo].[SysPageAccessRecord] ([Id], [UserId], [PageUrl], [Ip], [AccessTime]) VALUES (51, 1000, N'/biz/report/carinsure', N'113.108.198.138', CAST(0x0000A755011742D8 AS DateTime))
INSERT [dbo].[SysPageAccessRecord] ([Id], [UserId], [PageUrl], [Ip], [AccessTime]) VALUES (52, 1000, N'/biz/report/depositrent', N'113.108.198.138', CAST(0x0000A75501174389 AS DateTime))
INSERT [dbo].[SysPageAccessRecord] ([Id], [UserId], [PageUrl], [Ip], [AccessTime]) VALUES (53, 1000, N'/biz/report/depositrent', N'113.108.198.138', CAST(0x0000A75501174474 AS DateTime))
INSERT [dbo].[SysPageAccessRecord] ([Id], [UserId], [PageUrl], [Ip], [AccessTime]) VALUES (54, 1000, N'/biz/report/carinsure', N'113.108.198.138', CAST(0x0000A75501174531 AS DateTime))
INSERT [dbo].[SysPageAccessRecord] ([Id], [UserId], [PageUrl], [Ip], [AccessTime]) VALUES (55, 1000, N'/biz/report/withdraw', N'113.108.198.138', CAST(0x0000A755011745A6 AS DateTime))
INSERT [dbo].[SysPageAccessRecord] ([Id], [UserId], [PageUrl], [Ip], [AccessTime]) VALUES (56, 1000, N'/biz/report/merchant', N'113.108.198.138', CAST(0x0000A75501174622 AS DateTime))
INSERT [dbo].[SysPageAccessRecord] ([Id], [UserId], [PageUrl], [Ip], [AccessTime]) VALUES (57, 1000, N'/biz/report/depositrent', N'113.108.198.138', CAST(0x0000A755011753CE AS DateTime))
INSERT [dbo].[SysPageAccessRecord] ([Id], [UserId], [PageUrl], [Ip], [AccessTime]) VALUES (58, 1000, N'/biz/report/rent', N'113.108.198.138', CAST(0x0000A75501175485 AS DateTime))
INSERT [dbo].[SysPageAccessRecord] ([Id], [UserId], [PageUrl], [Ip], [AccessTime]) VALUES (59, 1000, N'/biz/report/rent', N'113.108.198.138', CAST(0x0000A75501175530 AS DateTime))
INSERT [dbo].[SysPageAccessRecord] ([Id], [UserId], [PageUrl], [Ip], [AccessTime]) VALUES (60, 1000, N'/biz/report/salesmanpos', N'113.108.198.138', CAST(0x0000A755011755B5 AS DateTime))
INSERT [dbo].[SysPageAccessRecord] ([Id], [UserId], [PageUrl], [Ip], [AccessTime]) VALUES (61, 1000, N'/biz/order/list', N'113.108.198.138', CAST(0x0000A75501175699 AS DateTime))
INSERT [dbo].[SysPageAccessRecord] ([Id], [UserId], [PageUrl], [Ip], [AccessTime]) VALUES (62, 1000, N'/biz/order/list', N'113.108.198.138', CAST(0x0000A75501175988 AS DateTime))
INSERT [dbo].[SysPageAccessRecord] ([Id], [UserId], [PageUrl], [Ip], [AccessTime]) VALUES (63, 1000, N'/biz/extendedapp/applylist', N'113.108.198.138', CAST(0x0000A75501175C11 AS DateTime))
INSERT [dbo].[SysPageAccessRecord] ([Id], [UserId], [PageUrl], [Ip], [AccessTime]) VALUES (64, 1000, N'/biz/insurancecompany/list', N'113.108.198.138', CAST(0x0000A75501175E13 AS DateTime))
INSERT [dbo].[SysPageAccessRecord] ([Id], [UserId], [PageUrl], [Ip], [AccessTime]) VALUES (65, 1000, N'/biz/carinsurancecompany/list', N'113.108.198.138', CAST(0x0000A755011765A9 AS DateTime))
INSERT [dbo].[SysPageAccessRecord] ([Id], [UserId], [PageUrl], [Ip], [AccessTime]) VALUES (66, 1000, N'/sys/menu/list', N'113.108.198.138', CAST(0x0000A75501176848 AS DateTime))
INSERT [dbo].[SysPageAccessRecord] ([Id], [UserId], [PageUrl], [Ip], [AccessTime]) VALUES (67, 1000, N'/biz/report/carinsure', N'113.108.198.138', CAST(0x0000A755011771E2 AS DateTime))
INSERT [dbo].[SysPageAccessRecord] ([Id], [UserId], [PageUrl], [Ip], [AccessTime]) VALUES (68, 1000, N'/biz/report/depositrent', N'113.108.198.138', CAST(0x0000A7550117748F AS DateTime))
SET IDENTITY_INSERT [dbo].[SysPageAccessRecord] OFF
/****** Object:  Table [dbo].[SysOperateHistory]    Script Date: 04/14/2017 17:00:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SysOperateHistory](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[Content] [nvarchar](512) NULL,
	[CreateTime] [datetime] NOT NULL,
	[Ip] [varchar](128) NULL,
	[ReferenceId] [int] NULL,
	[Type] [int] NOT NULL,
	[Creator] [int] NOT NULL,
 CONSTRAINT [PK_dbo.SysOperateHistory] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[SysOperateHistory] ON
INSERT [dbo].[SysOperateHistory] ([Id], [UserId], [Content], [CreateTime], [Ip], [ReferenceId], [Type], [Creator]) VALUES (1, 1000, N'新建菜单(ID:74)', CAST(0x0000A75501113953 AS DateTime), N'', 74, 1, 1000)
INSERT [dbo].[SysOperateHistory] ([Id], [UserId], [Content], [CreateTime], [Ip], [ReferenceId], [Type], [Creator]) VALUES (2, 1000, N'新建菜单(ID:75)', CAST(0x0000A75501114B47 AS DateTime), N'', 75, 1, 1000)
INSERT [dbo].[SysOperateHistory] ([Id], [UserId], [Content], [CreateTime], [Ip], [ReferenceId], [Type], [Creator]) VALUES (3, 1000, N'新建菜单(ID:76)', CAST(0x0000A755011155C7 AS DateTime), N'', 76, 1, 1000)
INSERT [dbo].[SysOperateHistory] ([Id], [UserId], [Content], [CreateTime], [Ip], [ReferenceId], [Type], [Creator]) VALUES (4, 1000, N'修改菜单(ID:74)', CAST(0x0000A755011163DF AS DateTime), N'', 74, 2, 1000)
INSERT [dbo].[SysOperateHistory] ([Id], [UserId], [Content], [CreateTime], [Ip], [ReferenceId], [Type], [Creator]) VALUES (5, 1000, N'修改菜单(ID:75)', CAST(0x0000A7550111711B AS DateTime), N'', 75, 2, 1000)
INSERT [dbo].[SysOperateHistory] ([Id], [UserId], [Content], [CreateTime], [Ip], [ReferenceId], [Type], [Creator]) VALUES (6, 1000, N'修改菜单(ID:76)', CAST(0x0000A75501117ECC AS DateTime), N'', 76, 2, 1000)
INSERT [dbo].[SysOperateHistory] ([Id], [UserId], [Content], [CreateTime], [Ip], [ReferenceId], [Type], [Creator]) VALUES (7, 1000, N'修改用户admin(ID:1000)密码', CAST(0x0000A755011209B5 AS DateTime), N'', 1000, 3, 1000)
INSERT [dbo].[SysOperateHistory] ([Id], [UserId], [Content], [CreateTime], [Ip], [ReferenceId], [Type], [Creator]) VALUES (8, 1000, N'保存角色(ID:1)菜单', CAST(0x0000A75501125B48 AS DateTime), N'', 1, 2, 1000)
INSERT [dbo].[SysOperateHistory] ([Id], [UserId], [Content], [CreateTime], [Ip], [ReferenceId], [Type], [Creator]) VALUES (9, 1000, N'保存角色(ID:1)菜单', CAST(0x0000A7550117264E AS DateTime), N'', 1, 2, 1000)
SET IDENTITY_INSERT [dbo].[SysOperateHistory] OFF
/****** Object:  Table [dbo].[SysMessage]    Script Date: 04/14/2017 17:00:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SysMessage](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[Content] [nvarchar](512) NULL,
	[CreateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.SysMessage] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SysMenuPermission]    Script Date: 04/14/2017 17:00:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SysMenuPermission](
	[MenuId] [int] NOT NULL,
	[PermissionId] [varchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.SysMenuPermission] PRIMARY KEY CLUSTERED 
(
	[MenuId] ASC,
	[PermissionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
INSERT [dbo].[SysMenuPermission] ([MenuId], [PermissionId]) VALUES (3, N'Sys5000')
INSERT [dbo].[SysMenuPermission] ([MenuId], [PermissionId]) VALUES (4, N'Sys4000')
INSERT [dbo].[SysMenuPermission] ([MenuId], [PermissionId]) VALUES (5, N'Sys2000')
INSERT [dbo].[SysMenuPermission] ([MenuId], [PermissionId]) VALUES (6, N'Sys1000')
INSERT [dbo].[SysMenuPermission] ([MenuId], [PermissionId]) VALUES (11, N'Sys1000')
INSERT [dbo].[SysMenuPermission] ([MenuId], [PermissionId]) VALUES (12, N'Sys1000')
INSERT [dbo].[SysMenuPermission] ([MenuId], [PermissionId]) VALUES (12, N'Sys2000')
INSERT [dbo].[SysMenuPermission] ([MenuId], [PermissionId]) VALUES (12, N'Sys3000')
INSERT [dbo].[SysMenuPermission] ([MenuId], [PermissionId]) VALUES (13, N'Sys1000')
INSERT [dbo].[SysMenuPermission] ([MenuId], [PermissionId]) VALUES (16, N'Sys1000')
INSERT [dbo].[SysMenuPermission] ([MenuId], [PermissionId]) VALUES (16, N'Sys4000')
INSERT [dbo].[SysMenuPermission] ([MenuId], [PermissionId]) VALUES (17, N'Sys1000')
INSERT [dbo].[SysMenuPermission] ([MenuId], [PermissionId]) VALUES (17, N'Sys4000')
INSERT [dbo].[SysMenuPermission] ([MenuId], [PermissionId]) VALUES (19, N'Biz4003')
INSERT [dbo].[SysMenuPermission] ([MenuId], [PermissionId]) VALUES (23, N'Biz5001')
INSERT [dbo].[SysMenuPermission] ([MenuId], [PermissionId]) VALUES (25, N'Biz6001')
INSERT [dbo].[SysMenuPermission] ([MenuId], [PermissionId]) VALUES (28, N'Biz8001')
INSERT [dbo].[SysMenuPermission] ([MenuId], [PermissionId]) VALUES (29, N'Biz8002')
INSERT [dbo].[SysMenuPermission] ([MenuId], [PermissionId]) VALUES (33, N'Biz5002')
INSERT [dbo].[SysMenuPermission] ([MenuId], [PermissionId]) VALUES (36, N'BizA001')
INSERT [dbo].[SysMenuPermission] ([MenuId], [PermissionId]) VALUES (37, N'BizA002')
INSERT [dbo].[SysMenuPermission] ([MenuId], [PermissionId]) VALUES (39, N'Biz9001')
INSERT [dbo].[SysMenuPermission] ([MenuId], [PermissionId]) VALUES (40, N'Biz9002')
INSERT [dbo].[SysMenuPermission] ([MenuId], [PermissionId]) VALUES (41, N'Biz9003')
INSERT [dbo].[SysMenuPermission] ([MenuId], [PermissionId]) VALUES (46, N'BizD001')
INSERT [dbo].[SysMenuPermission] ([MenuId], [PermissionId]) VALUES (48, N'BizC001')
INSERT [dbo].[SysMenuPermission] ([MenuId], [PermissionId]) VALUES (51, N'Biz7001')
INSERT [dbo].[SysMenuPermission] ([MenuId], [PermissionId]) VALUES (52, N'Biz7002')
INSERT [dbo].[SysMenuPermission] ([MenuId], [PermissionId]) VALUES (54, N'Biz5003')
INSERT [dbo].[SysMenuPermission] ([MenuId], [PermissionId]) VALUES (57, N'Biz3001')
INSERT [dbo].[SysMenuPermission] ([MenuId], [PermissionId]) VALUES (58, N'Biz3002')
INSERT [dbo].[SysMenuPermission] ([MenuId], [PermissionId]) VALUES (59, N'Biz3003')
INSERT [dbo].[SysMenuPermission] ([MenuId], [PermissionId]) VALUES (61, N'Biz1001')
INSERT [dbo].[SysMenuPermission] ([MenuId], [PermissionId]) VALUES (62, N'Biz1002')
INSERT [dbo].[SysMenuPermission] ([MenuId], [PermissionId]) VALUES (63, N'Biz1003')
INSERT [dbo].[SysMenuPermission] ([MenuId], [PermissionId]) VALUES (65, N'Biz2001')
INSERT [dbo].[SysMenuPermission] ([MenuId], [PermissionId]) VALUES (66, N'Biz2001')
INSERT [dbo].[SysMenuPermission] ([MenuId], [PermissionId]) VALUES (67, N'Biz2001')
INSERT [dbo].[SysMenuPermission] ([MenuId], [PermissionId]) VALUES (68, N'Biz2001')
INSERT [dbo].[SysMenuPermission] ([MenuId], [PermissionId]) VALUES (69, N'Biz2001')
INSERT [dbo].[SysMenuPermission] ([MenuId], [PermissionId]) VALUES (70, N'Biz1004')
INSERT [dbo].[SysMenuPermission] ([MenuId], [PermissionId]) VALUES (72, N'BizB001')
INSERT [dbo].[SysMenuPermission] ([MenuId], [PermissionId]) VALUES (73, N'BizB002')
/****** Object:  Table [dbo].[SysMenu]    Script Date: 04/14/2017 17:00:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SysMenu](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](128) NOT NULL,
	[PId] [int] NOT NULL,
	[Url] [nvarchar](256) NULL,
	[Description] [nvarchar](512) NULL,
	[Priority] [int] NOT NULL,
	[Creator] [int] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[Mender] [int] NULL,
	[LastUpdateTime] [datetime] NULL,
 CONSTRAINT [PK_dbo.SysMenu] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[SysMenu] ON
INSERT [dbo].[SysMenu] ([Id], [Name], [PId], [Url], [Description], [Priority], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (1, N'后台管理菜单', 0, N'#', N'', 0, 0, CAST(0x0000000000000000 AS DateTime), NULL, NULL)
INSERT [dbo].[SysMenu] ([Id], [Name], [PId], [Url], [Description], [Priority], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (2, N'系统管理', 1, N'#', N'', 44, 0, CAST(0x0000000000000000 AS DateTime), NULL, NULL)
INSERT [dbo].[SysMenu] ([Id], [Name], [PId], [Url], [Description], [Priority], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (3, N'菜单设置', 2, N'/Sys/Menu/List', N'', 6, 0, CAST(0x0000000000000000 AS DateTime), NULL, NULL)
INSERT [dbo].[SysMenu] ([Id], [Name], [PId], [Url], [Description], [Priority], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (4, N'角色设置', 2, N'/Sys/Role/List', N'', 5, 0, CAST(0x0000000000000000 AS DateTime), NULL, NULL)
INSERT [dbo].[SysMenu] ([Id], [Name], [PId], [Url], [Description], [Priority], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (5, N'后台用户', 2, N'/Sys/StaffUser/List', NULL, 4, 0, CAST(0x0000000000000000 AS DateTime), 1, CAST(0x0000A6E10162BAC6 AS DateTime))
INSERT [dbo].[SysMenu] ([Id], [Name], [PId], [Url], [Description], [Priority], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (6, N'所有用户', 2, N'/Sys/User/List', NULL, 4, 0, CAST(0x0000000000000000 AS DateTime), 1000, CAST(0x0000A72C00B586BC AS DateTime))
INSERT [dbo].[SysMenu] ([Id], [Name], [PId], [Url], [Description], [Priority], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (7, N'日志查看', 2, N'/Sys/LogView/List', NULL, 3, 0, CAST(0x0000000000000000 AS DateTime), 1, CAST(0x0000A6E0017CD9AB AS DateTime))
INSERT [dbo].[SysMenu] ([Id], [Name], [PId], [Url], [Description], [Priority], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (18, N'POS机管理', 1, N'#', NULL, 100, 1, CAST(0x0000A6E101698313 AS DateTime), NULL, NULL)
INSERT [dbo].[SysMenu] ([Id], [Name], [PId], [Url], [Description], [Priority], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (19, N'POS机登记信息', 18, N'/Biz/PosMachine/List', NULL, 0, 1, CAST(0x0000A6E101699842 AS DateTime), 1000, CAST(0x0000A72C00B5287E AS DateTime))
INSERT [dbo].[SysMenu] ([Id], [Name], [PId], [Url], [Description], [Priority], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (20, N'商户管理', 1, N'#', NULL, 99, 1, CAST(0x0000A6E1017ADE72 AS DateTime), NULL, NULL)
INSERT [dbo].[SysMenu] ([Id], [Name], [PId], [Url], [Description], [Priority], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (22, N'商户资料查看', 20, N'/Biz/Merchant/List', NULL, 0, 1, CAST(0x0000A6E400DE61DA AS DateTime), 1, CAST(0x0000A6ED013B11CF AS DateTime))
INSERT [dbo].[SysMenu] ([Id], [Name], [PId], [Url], [Description], [Priority], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (23, N'商户资料维护', 20, N'/Biz/Merchant/EditList', NULL, 0, 1, CAST(0x0000A6E400DE6891 AS DateTime), 1000, CAST(0x0000A72C00B52DC9 AS DateTime))
INSERT [dbo].[SysMenu] ([Id], [Name], [PId], [Url], [Description], [Priority], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (24, N'车险投保订单', 1, NULL, NULL, 88, 1, CAST(0x0000A6E400DE7364 AS DateTime), 1, CAST(0x0000A6ED012C6E6E AS DateTime))
INSERT [dbo].[SysMenu] ([Id], [Name], [PId], [Url], [Description], [Priority], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (25, N'投保订单报价', 24, N'/Biz/CarInsureOffer/DealtList', NULL, 0, 1, CAST(0x0000A6E400DE7BB8 AS DateTime), 1000, CAST(0x0000A72C00B54248 AS DateTime))
INSERT [dbo].[SysMenu] ([Id], [Name], [PId], [Url], [Description], [Priority], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (27, N'提现管理', 1, NULL, NULL, 77, 1, CAST(0x0000A6E400DE9631 AS DateTime), NULL, NULL)
INSERT [dbo].[SysMenu] ([Id], [Name], [PId], [Url], [Description], [Priority], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (28, N'提现截单', 27, N'/Biz/Withdraw/CutOff', NULL, 0, 1, CAST(0x0000A6E400DEA046 AS DateTime), 1000, CAST(0x0000A72C00B550A4 AS DateTime))
INSERT [dbo].[SysMenu] ([Id], [Name], [PId], [Url], [Description], [Priority], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (29, N'提现处理', 27, N'/Biz/Withdraw/Dealt', NULL, 0, 1, CAST(0x0000A6E400DEA580 AS DateTime), 1000, CAST(0x0000A72C00B555FF AS DateTime))
INSERT [dbo].[SysMenu] ([Id], [Name], [PId], [Url], [Description], [Priority], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (30, N'提现查询', 27, N'/Biz/Withdraw/List', NULL, 0, 1, CAST(0x0000A6E400DEAAD4 AS DateTime), 1, CAST(0x0000A6EA011F64CF AS DateTime))
INSERT [dbo].[SysMenu] ([Id], [Name], [PId], [Url], [Description], [Priority], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (31, N'投保订单查询', 24, N'/Biz/CarInsureOffer/List', NULL, 0, 1, CAST(0x0000A6E400E0815D AS DateTime), 1, CAST(0x0000A6ED01180CCC AS DateTime))
INSERT [dbo].[SysMenu] ([Id], [Name], [PId], [Url], [Description], [Priority], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (33, N'商户资料初审', 20, N'/Biz/Merchant/PrimaryAuditList', NULL, 0, 1, CAST(0x0000A6E400E0F72A AS DateTime), 1000, CAST(0x0000A72C00B53316 AS DateTime))
INSERT [dbo].[SysMenu] ([Id], [Name], [PId], [Url], [Description], [Priority], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (35, N'基础资料维护', 1, NULL, NULL, 55, 1, CAST(0x0000A6E400E15072 AS DateTime), NULL, NULL)
INSERT [dbo].[SysMenu] ([Id], [Name], [PId], [Url], [Description], [Priority], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (36, N'保险公司设置', 35, N'/Biz/InsuranceCompany/List', NULL, 0, 1, CAST(0x0000A6E400E15D77 AS DateTime), 1000, CAST(0x0000A72C00B5730E AS DateTime))
INSERT [dbo].[SysMenu] ([Id], [Name], [PId], [Url], [Description], [Priority], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (37, N'车险保险公司设置', 35, N'/Biz/CarInsuranceCompany/List', NULL, 0, 1, CAST(0x0000A6E400E165B4 AS DateTime), 1000, CAST(0x0000A72C00B577F2 AS DateTime))
INSERT [dbo].[SysMenu] ([Id], [Name], [PId], [Url], [Description], [Priority], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (38, N'扩展应用管理', 1, NULL, NULL, 66, 1, CAST(0x0000A6E400E1765E AS DateTime), NULL, NULL)
INSERT [dbo].[SysMenu] ([Id], [Name], [PId], [Url], [Description], [Priority], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (39, N'扩展应用申请', 38, N'/Biz/ExtendedApp/ApplyList', NULL, 0, 1, CAST(0x0000A6E400E191AE AS DateTime), 1000, CAST(0x0000A72C00B55F63 AS DateTime))
INSERT [dbo].[SysMenu] ([Id], [Name], [PId], [Url], [Description], [Priority], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (40, N'扩展应用初审', 38, N'/Biz/ExtendedApp/PrimaryAuditList', NULL, 0, 1, CAST(0x0000A6E400E19B7D AS DateTime), 1000, CAST(0x0000A72C00B56649 AS DateTime))
INSERT [dbo].[SysMenu] ([Id], [Name], [PId], [Url], [Description], [Priority], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (41, N'扩展应用复审', 38, N'/Biz/ExtendedApp/SeniorAuditList', NULL, 0, 1, CAST(0x0000A6E400E1A0BC AS DateTime), 1000, CAST(0x0000A72C00B56C7E AS DateTime))
INSERT [dbo].[SysMenu] ([Id], [Name], [PId], [Url], [Description], [Priority], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (42, N'扩展应用查询', 38, N'/Biz/ExtendedApp/List', NULL, 0, 1, CAST(0x0000A6E400E1B3E4 AS DateTime), 1, CAST(0x0000A6E80086F925 AS DateTime))
INSERT [dbo].[SysMenu] ([Id], [Name], [PId], [Url], [Description], [Priority], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (43, N'广告管理', 1, NULL, NULL, 102, 1, CAST(0x0000A6E400E2D986 AS DateTime), NULL, NULL)
INSERT [dbo].[SysMenu] ([Id], [Name], [PId], [Url], [Description], [Priority], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (45, N'产品管理', 1, NULL, NULL, 103, 1, CAST(0x0000A6E400E2ED76 AS DateTime), NULL, NULL)
INSERT [dbo].[SysMenu] ([Id], [Name], [PId], [Url], [Description], [Priority], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (46, N'广告设置', 43, N'/Sys/Banner/TypeList', NULL, 0, 1, CAST(0x0000A6E400E33C08 AS DateTime), 1000, CAST(0x0000A72C00B5144E AS DateTime))
INSERT [dbo].[SysMenu] ([Id], [Name], [PId], [Url], [Description], [Priority], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (48, N'商品设置', 45, N'/Biz/Product/GoodsList', NULL, 0, 1, CAST(0x0000A6E400E367B0 AS DateTime), 1000, CAST(0x0000A72C00B510E4 AS DateTime))
INSERT [dbo].[SysMenu] ([Id], [Name], [PId], [Url], [Description], [Priority], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (50, N'车险理赔订单', 1, NULL, NULL, 87, 1, CAST(0x0000A6ED012C820B AS DateTime), NULL, NULL)
INSERT [dbo].[SysMenu] ([Id], [Name], [PId], [Url], [Description], [Priority], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (51, N'理赔需求核实', 50, N'/Biz/CarClaim/VerifyOrderList', NULL, 0, 1, CAST(0x0000A6ED012CAA9C AS DateTime), 1000, CAST(0x0000A72C00B548CE AS DateTime))
INSERT [dbo].[SysMenu] ([Id], [Name], [PId], [Url], [Description], [Priority], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (52, N'理赔金额核实', 50, N'/Biz/CarClaim/VerifyAmountList', NULL, 0, 1, CAST(0x0000A6ED012CB285 AS DateTime), 1000, CAST(0x0000A72C00B54D2D AS DateTime))
INSERT [dbo].[SysMenu] ([Id], [Name], [PId], [Url], [Description], [Priority], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (53, N'理赔订单查询', 50, N'/Biz/CarClaim/List', NULL, 0, 1, CAST(0x0000A6ED012CBA01 AS DateTime), 1, CAST(0x0000A6ED012CC05A AS DateTime))
INSERT [dbo].[SysMenu] ([Id], [Name], [PId], [Url], [Description], [Priority], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (54, N'商户资料复审', 20, N'/Biz/Merchant/SeniorAuditList', NULL, 0, 1, CAST(0x0000A6ED015EBE1E AS DateTime), 1000, CAST(0x0000A72C00B536F4 AS DateTime))
INSERT [dbo].[SysMenu] ([Id], [Name], [PId], [Url], [Description], [Priority], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (55, N'佣金设置', 1, NULL, NULL, 101, 1000, CAST(0x0000A6EF00D13A4D AS DateTime), NULL, NULL)
INSERT [dbo].[SysMenu] ([Id], [Name], [PId], [Url], [Description], [Priority], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (56, N'佣金比例查询', 55, N'/Biz/CarInsureCommissionRate/List', NULL, 0, 1000, CAST(0x0000A6EF00D1507F AS DateTime), 1000, CAST(0x0000A6F50182EDA8 AS DateTime))
INSERT [dbo].[SysMenu] ([Id], [Name], [PId], [Url], [Description], [Priority], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (57, N'佣金调整申请', 55, N'/Biz/CarInsureCommissionRate/ApplyList', NULL, 0, 1000, CAST(0x0000A6EF00DAA7A9 AS DateTime), 1000, CAST(0x0000A72C00B51BB2 AS DateTime))
INSERT [dbo].[SysMenu] ([Id], [Name], [PId], [Url], [Description], [Priority], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (58, N'佣金调整初审', 55, N'/Biz/CarInsureCommissionRate/PrimaryAuditList', NULL, 0, 1000, CAST(0x0000A6EF00DAB925 AS DateTime), 1000, CAST(0x0000A72C00B51F3E AS DateTime))
INSERT [dbo].[SysMenu] ([Id], [Name], [PId], [Url], [Description], [Priority], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (59, N'佣金调整复审', 55, N'/Biz/CarInsureCommissionRate/SeniorAuditList', NULL, 0, 1000, CAST(0x0000A6EF00DAC628 AS DateTime), 1000, CAST(0x0000A72C00B5231D AS DateTime))
INSERT [dbo].[SysMenu] ([Id], [Name], [PId], [Url], [Description], [Priority], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (60, N'报表管理', 1, NULL, NULL, 105, 1000, CAST(0x0000A6F4017A69B9 AS DateTime), NULL, NULL)
INSERT [dbo].[SysMenu] ([Id], [Name], [PId], [Url], [Description], [Priority], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (61, N'商户账号（未激活)', 60, N'/biz/Report/NoActiveAccount', NULL, 0, 1000, CAST(0x0000A6F4017ACBC3 AS DateTime), 1000, CAST(0x0000A72C00B49F42 AS DateTime))
INSERT [dbo].[SysMenu] ([Id], [Name], [PId], [Url], [Description], [Priority], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (62, N'商户提现报表', 60, N'/BIZ/Report/Withdraw', NULL, 0, 1000, CAST(0x0000A6FF017F1F36 AS DateTime), 1000, CAST(0x0000A72C00B4A475 AS DateTime))
INSERT [dbo].[SysMenu] ([Id], [Name], [PId], [Url], [Description], [Priority], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (63, N'商户信息登记报表', 60, N'/biz/Report/Merchant', NULL, 0, 1000, CAST(0x0000A6FF018ADDFE AS DateTime), 1000, CAST(0x0000A72C00B4A7F6 AS DateTime))
INSERT [dbo].[SysMenu] ([Id], [Name], [PId], [Url], [Description], [Priority], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (64, N'订单查询', 1, NULL, NULL, 104, 1000, CAST(0x0000A7000166DB5B AS DateTime), 1000, CAST(0x0000A72C00B2D243 AS DateTime))
INSERT [dbo].[SysMenu] ([Id], [Name], [PId], [Url], [Description], [Priority], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (65, N'已提交订单', 64, N'/Biz/Order/List?status=1', NULL, 0, 1000, CAST(0x0000A7000166EC0F AS DateTime), 1000, CAST(0x0000A72C00B4B320 AS DateTime))
INSERT [dbo].[SysMenu] ([Id], [Name], [PId], [Url], [Description], [Priority], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (66, N'跟进中订单', 64, N'/Biz/Order/List?status=2', NULL, 0, 1000, CAST(0x0000A7000166F7EA AS DateTime), 1000, CAST(0x0000A72C00B4B6B7 AS DateTime))
INSERT [dbo].[SysMenu] ([Id], [Name], [PId], [Url], [Description], [Priority], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (67, N'待支付订单', 64, N'/Biz/Order/List?status=3', NULL, 0, 1000, CAST(0x0000A7000167049A AS DateTime), 1000, CAST(0x0000A72C00B4BA4B AS DateTime))
INSERT [dbo].[SysMenu] ([Id], [Name], [PId], [Url], [Description], [Priority], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (68, N'已完成订单', 64, N'/Biz/Order/List?status=4', NULL, 0, 1000, CAST(0x0000A70001670E80 AS DateTime), 1000, CAST(0x0000A72C00B4BEFC AS DateTime))
INSERT [dbo].[SysMenu] ([Id], [Name], [PId], [Url], [Description], [Priority], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (69, N'已取消订单', 64, N'/Biz/Order/List?status=5', NULL, 0, 1000, CAST(0x0000A700016718D2 AS DateTime), 1000, CAST(0x0000A72C00B4C22A AS DateTime))
INSERT [dbo].[SysMenu] ([Id], [Name], [PId], [Url], [Description], [Priority], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (70, N'商户车险订单报表', 60, N'/biz/Report/CarInsure', NULL, 0, 1000, CAST(0x0000A70001691023 AS DateTime), 1000, CAST(0x0000A72C00B499F2 AS DateTime))
INSERT [dbo].[SysMenu] ([Id], [Name], [PId], [Url], [Description], [Priority], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (71, N'业务员管理', 1, NULL, NULL, 106, 1000, CAST(0x0000A71D016BA1E7 AS DateTime), NULL, NULL)
INSERT [dbo].[SysMenu] ([Id], [Name], [PId], [Url], [Description], [Priority], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (72, N'业务员设置', 71, N'/Sys/SalesmanUser/List', NULL, 0, 1000, CAST(0x0000A71D016BB35E AS DateTime), 1000, CAST(0x0000A72C00B48BC8 AS DateTime))
INSERT [dbo].[SysMenu] ([Id], [Name], [PId], [Url], [Description], [Priority], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (73, N'业务员申领POS机登记', 71, N'/Biz/ApplyPos/List', NULL, 0, 1000, CAST(0x0000A71D016BC9D5 AS DateTime), 1000, CAST(0x0000A72C00B48F84 AS DateTime))
INSERT [dbo].[SysMenu] ([Id], [Name], [PId], [Url], [Description], [Priority], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (74, N'商户押金租金报表', 60, N'/biz/Report/DepositRent', NULL, 0, 1000, CAST(0x0000A75501113950 AS DateTime), 1000, CAST(0x0000A755011163D3 AS DateTime))
INSERT [dbo].[SysMenu] ([Id], [Name], [PId], [Url], [Description], [Priority], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (75, N'商户租金报表', 60, N'/biz/Report/Rent', NULL, 0, 1000, CAST(0x0000A75501114B46 AS DateTime), 1000, CAST(0x0000A7550111711B AS DateTime))
INSERT [dbo].[SysMenu] ([Id], [Name], [PId], [Url], [Description], [Priority], [Creator], [CreateTime], [Mender], [LastUpdateTime]) VALUES (76, N'业务员对应POS机报表', 60, N'/biz/report/salesmanpos', NULL, 0, 1000, CAST(0x0000A755011155C6 AS DateTime), 1000, CAST(0x0000A75501117ECC AS DateTime))
SET IDENTITY_INSERT [dbo].[SysMenu] OFF
/****** Object:  Table [dbo].[SysUser]    Script Date: 04/14/2017 17:00:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SysUser](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](256) NOT NULL,
	[FullName] [nvarchar](128) NULL,
	[PasswordHash] [nvarchar](68) NOT NULL,
	[SecurityStamp] [nvarchar](36) NOT NULL,
	[PhoneNumber] [nvarchar](20) NULL,
	[HeadImg] [nvarchar](256) NULL,
	[RegisterTime] [datetime] NOT NULL,
	[LastLoginTime] [datetime] NULL,
	[LastLoginIp] [nvarchar](50) NULL,
	[IsDelete] [bit] NOT NULL,
	[Creator] [int] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[Mender] [int] NULL,
	[LastUpdateTime] [datetime] NULL,
	[Email] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEndDateUtc] [datetime] NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
	[IsModifyDefaultPwd] [bit] NOT NULL,
	[Status] [int] NOT NULL,
 CONSTRAINT [PK_dbo.SysUser] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[SysUser] ON
INSERT [dbo].[SysUser] ([Id], [UserName], [FullName], [PasswordHash], [SecurityStamp], [PhoneNumber], [HeadImg], [RegisterTime], [LastLoginTime], [LastLoginIp], [IsDelete], [Creator], [CreateTime], [Mender], [LastUpdateTime], [Email], [EmailConfirmed], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [IsModifyDefaultPwd], [Status]) VALUES (1, N'ins-uplink', N'全线通', N'AD5hJcUUIJ4NxikOI2O1ChwVgoGYwPXDxGHp+nSIX8GHEeQw5h0C9mECSFyXeo/kCw==', N'61c7b4a2-4197-4d32-b9a5-629425fc2000', NULL, NULL, CAST(0x0000A60D00A0CF28 AS DateTime), NULL, NULL, 1, 0, CAST(0x0000A60D00A0CF28 AS DateTime), NULL, NULL, NULL, 0, 0, 0, NULL, 0, 0, 0, 0)
INSERT [dbo].[SysUser] ([Id], [UserName], [FullName], [PasswordHash], [SecurityStamp], [PhoneNumber], [HeadImg], [RegisterTime], [LastLoginTime], [LastLoginIp], [IsDelete], [Creator], [CreateTime], [Mender], [LastUpdateTime], [Email], [EmailConfirmed], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [IsModifyDefaultPwd], [Status]) VALUES (2, N'yibanshi', N'易办事', N'AD5hJcUUIJ4NxikOI2O1ChwVgoGYwPXDxGHp+nSIX8GHEeQw5h0C9mECSFyXeo/kCw==', N'61c7b4a2-4197-4d32-b9a5-629425fc2000', NULL, NULL, CAST(0x0000A6DA00000000 AS DateTime), NULL, NULL, 1, 0, CAST(0x0000A6DA00000000 AS DateTime), NULL, NULL, NULL, 0, 0, 0, NULL, 0, 0, 0, 0)
INSERT [dbo].[SysUser] ([Id], [UserName], [FullName], [PasswordHash], [SecurityStamp], [PhoneNumber], [HeadImg], [RegisterTime], [LastLoginTime], [LastLoginIp], [IsDelete], [Creator], [CreateTime], [Mender], [LastUpdateTime], [Email], [EmailConfirmed], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [IsModifyDefaultPwd], [Status]) VALUES (3, N'withdrawFee', N'提现手续费', N'AD5hJcUUIJ4NxikOI2O1ChwVgoGYwPXDxGHp+nSIX8GHEeQw5h0C9mECSFyXeo/kCw==', N'61c7b4a2-4197-4d32-b9a5-629425fc2000', NULL, NULL, CAST(0x0000A6EE00000000 AS DateTime), NULL, NULL, 1, 0, CAST(0x0000A6EE00000000 AS DateTime), NULL, NULL, NULL, 0, 0, 0, NULL, 0, 0, 0, 0)
INSERT [dbo].[SysUser] ([Id], [UserName], [FullName], [PasswordHash], [SecurityStamp], [PhoneNumber], [HeadImg], [RegisterTime], [LastLoginTime], [LastLoginIp], [IsDelete], [Creator], [CreateTime], [Mender], [LastUpdateTime], [Email], [EmailConfirmed], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [IsModifyDefaultPwd], [Status]) VALUES (4, N'withdrawFundPool', N'提现资金池', N'AD5hJcUUIJ4NxikOI2O1ChwVgoGYwPXDxGHp+nSIX8GHEeQw5h0C9mECSFyXeo/kCw==', N'61c7b4a2-4197-4d32-b9a5-629425fc2000', NULL, NULL, CAST(0x0000A6EE00000000 AS DateTime), NULL, NULL, 1, 0, CAST(0x0000A6EE00000000 AS DateTime), NULL, NULL, NULL, 0, 0, 0, NULL, 0, 0, 0, 0)
INSERT [dbo].[SysUser] ([Id], [UserName], [FullName], [PasswordHash], [SecurityStamp], [PhoneNumber], [HeadImg], [RegisterTime], [LastLoginTime], [LastLoginIp], [IsDelete], [Creator], [CreateTime], [Mender], [LastUpdateTime], [Email], [EmailConfirmed], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [IsModifyDefaultPwd], [Status]) VALUES (1000, N'admin', N'超级管理员', N'APwZ3eNZ+XSa4DDIg5pK/rcX2MqJrT/ZgbKZ+weeiV8EXS0WhhcXTlFuQBQiA2CwlQ==', N'c05c9d59-efdc-431e-866a-aad712546518', NULL, NULL, CAST(0x0000A6DA00000000 AS DateTime), CAST(0x0000A75501121752 AS DateTime), N'113.108.198.138', 0, 0, CAST(0x0000A6DA00000000 AS DateTime), NULL, NULL, NULL, 0, 0, 0, NULL, 0, 0, 0, 0)
INSERT [dbo].[SysUser] ([Id], [UserName], [FullName], [PasswordHash], [SecurityStamp], [PhoneNumber], [HeadImg], [RegisterTime], [LastLoginTime], [LastLoginIp], [IsDelete], [Creator], [CreateTime], [Mender], [LastUpdateTime], [Email], [EmailConfirmed], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [IsModifyDefaultPwd], [Status]) VALUES (1001, N'852366', NULL, N'AMagRQVBYpyNdeXj83LesWy0sA6gu9Sd9taFpLBaHSpX+lVUxeNkQYi/FkYMib6GCg==', N'fc29b603-8691-42f1-a5a3-cd7159b80f7b', NULL, NULL, CAST(0x0000A7550113FC4A AS DateTime), NULL, NULL, 0, 0, CAST(0x0000A7550113FC4A AS DateTime), NULL, NULL, NULL, 0, 0, 0, NULL, 0, 0, 0, 1)
SET IDENTITY_INSERT [dbo].[SysUser] OFF
/****** Object:  Table [dbo].[YBS_ReceiveNotifyLog]    Script Date: 04/14/2017 17:00:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[YBS_ReceiveNotifyLog](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Serialnumber] [nvarchar](128) NULL,
	[Transactioncode] [nvarchar](128) NULL,
	[Datetime] [nvarchar](128) NULL,
	[Merchantcode] [nvarchar](128) NULL,
	[Money] [nvarchar](128) NULL,
	[Paymentnumber] [nvarchar](128) NULL,
	[State] [nvarchar](128) NULL,
	[Channel] [nvarchar](128) NULL,
	[Terminalnumber] [nvarchar](128) NULL,
	[Bankcardnumber] [nvarchar](128) NULL,
	[Creator] [int] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.YBS_ReceiveNotifyLog] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WithdrawCutOffDetails]    Script Date: 04/14/2017 17:00:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WithdrawCutOffDetails](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Creator] [int] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[Mender] [int] NULL,
	[LastUpdateTime] [datetime] NULL,
	[WithdrawStatus] [int] NOT NULL,
	[MerchantId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[WithdrawId] [int] NOT NULL,
	[WithdrawSn] [nvarchar](128) NULL,
	[WithdrawAmount] [decimal](18, 2) NOT NULL,
	[WithdrawAmountByAfterFee] [decimal](18, 2) NOT NULL,
	[WithdrawFee] [decimal](18, 2) NOT NULL,
	[WithdrawFeeRateRule] [nvarchar](128) NULL,
	[WithdrawExpectArriveTime] [datetime] NOT NULL,
	[WithdrawStartTime] [datetime] NOT NULL,
	[WithdrawSettlementStartTime] [datetime] NOT NULL,
	[WithdrawSettlementEndTime] [datetime] NULL,
	[WithdrawFailureReason] [nvarchar](1024) NULL,
	[WithdrawBankCardId] [int] NOT NULL,
	[WithdrawBankName] [nvarchar](128) NULL,
	[WithdrawBankAccountName] [nvarchar](128) NULL,
	[WithdrawBankAccountNo] [nvarchar](128) NULL,
	[WithdrawCutOffId] [int] NOT NULL,
	[WithdrawCutoffTime] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.WithdrawCutOffDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WithdrawCutOff]    Script Date: 04/14/2017 17:00:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[WithdrawCutOff](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Creator] [int] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[Mender] [int] NULL,
	[LastUpdateTime] [datetime] NULL,
	[CutOffTime] [datetime] NOT NULL,
	[BatchNo] [varchar](128) NOT NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[AmountByAfterFee] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_dbo.WithdrawCutOff] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Withdraw]    Script Date: 04/14/2017 17:00:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Withdraw](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Sn] [nvarchar](128) NULL,
	[MerchantId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[FeeRateRule] [nvarchar](128) NULL,
	[ExpectArriveTime] [datetime] NOT NULL,
	[SettlementStartTime] [datetime] NOT NULL,
	[SettlementEndTime] [datetime] NULL,
	[FailureReason] [nvarchar](1024) NULL,
	[Status] [int] NOT NULL,
	[Creator] [int] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[Mender] [int] NULL,
	[LastUpdateTime] [datetime] NULL,
	[BankCardId] [int] NOT NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[AmountByAfterFee] [decimal](18, 2) NOT NULL,
	[Fee] [decimal](18, 2) NOT NULL,
	[WithdrawCutoffTime] [datetime] NULL,
	[WithdrawCutoffId] [int] NULL,
 CONSTRAINT [PK_dbo.Withdraw] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Transactions]    Script Date: 04/14/2017 17:00:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Transactions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Sn] [varchar](128) NULL,
	[Type] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[ChangeAmount] [decimal](18, 2) NOT NULL,
	[Balance] [decimal](18, 2) NOT NULL,
	[Description] [nvarchar](1024) NULL,
	[Creator] [int] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.Transactions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SysVerifyEmail]    Script Date: 04/14/2017 17:00:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SysVerifyEmail](
	[Id] [uniqueidentifier] NOT NULL,
	[Email] [varchar](128) NOT NULL,
	[IsVerify] [bit] NOT NULL,
	[ExpireTime] [datetime] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.SysVerifyEmail] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SysSmsSendHistory]    Script Date: 04/14/2017 17:00:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SysSmsSendHistory](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Token] [nvarchar](128) NULL,
	[ValidCode] [nvarchar](128) NULL,
	[ExpireTime] [datetime] NULL,
	[Creator] [int] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[IsUse] [bit] NOT NULL,
	[Phone] [nvarchar](128) NULL,
	[ApiName] [nvarchar](128) NULL,
	[TemplateParams] [nvarchar](512) NULL,
	[TemplateCode] [nvarchar](128) NULL,
	[FailureReason] [nvarchar](2048) NULL,
	[Result] [int] NOT NULL,
 CONSTRAINT [PK_dbo.SysSmsToken] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SysUserLoginHistory]    Script Date: 04/14/2017 17:00:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SysUserLoginHistory](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[LoginType] [int] NOT NULL,
	[Ip] [varchar](128) NULL,
	[Country] [nvarchar](128) NULL,
	[Province] [nvarchar](128) NULL,
	[City] [nvarchar](128) NULL,
	[LoginTime] [datetime] NOT NULL,
	[Result] [int] NOT NULL,
	[Description] [nvarchar](512) NULL,
 CONSTRAINT [PK_dbo.SysUserLoginHistory] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[SysUserLoginHistory] ON
INSERT [dbo].[SysUserLoginHistory] ([Id], [UserId], [LoginType], [Ip], [Country], [Province], [City], [LoginTime], [Result], [Description]) VALUES (1, 1000, 1, NULL, NULL, NULL, NULL, CAST(0x0000A7550110C6CC AS DateTime), 1, N'登录成功')
INSERT [dbo].[SysUserLoginHistory] ([Id], [UserId], [LoginType], [Ip], [Country], [Province], [City], [LoginTime], [Result], [Description]) VALUES (2, 1000, 1, NULL, NULL, NULL, NULL, CAST(0x0000A75501121752 AS DateTime), 1, N'登录成功')
SET IDENTITY_INSERT [dbo].[SysUserLoginHistory] OFF
/****** Object:  Table [dbo].[SysUserClaim]    Script Date: 04/14/2017 17:00:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SysUserClaim](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.SysUserClaim] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SysSalesmanUser]    Script Date: 04/14/2017 17:00:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SysSalesmanUser](
	[Id] [int] NOT NULL,
	[CtiNo] [nvarchar](50) NULL,
 CONSTRAINT [PK_dbo.SysSalesmanUser] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SysUserRole]    Script Date: 04/14/2017 17:00:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SysUserRole](
	[RoleId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.SysUserRole] PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC,
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[SysUserRole] ([RoleId], [UserId]) VALUES (1, 1000)
/****** Object:  Table [dbo].[SysUserLoginProvider]    Script Date: 04/14/2017 17:00:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SysUserLoginProvider](
	[LoginProvider] [nvarchar](128) NOT NULL,
	[ProviderKey] [nvarchar](128) NOT NULL,
	[UserId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.SysUserLoginProvider] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC,
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SysStaffUser]    Script Date: 04/14/2017 17:00:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SysStaffUser](
	[Id] [int] NOT NULL,
	[CtiNo] [nvarchar](50) NULL,
 CONSTRAINT [PK_dbo.SysStaffUser] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[SysStaffUser] ([Id], [CtiNo]) VALUES (1, NULL)
INSERT [dbo].[SysStaffUser] ([Id], [CtiNo]) VALUES (2, NULL)
INSERT [dbo].[SysStaffUser] ([Id], [CtiNo]) VALUES (3, NULL)
INSERT [dbo].[SysStaffUser] ([Id], [CtiNo]) VALUES (4, NULL)
INSERT [dbo].[SysStaffUser] ([Id], [CtiNo]) VALUES (1000, NULL)
/****** Object:  Table [dbo].[SysClientUser]    Script Date: 04/14/2017 17:00:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SysClientUser](
	[Id] [int] NOT NULL,
	[ClientCode] [nvarchar](128) NULL,
	[MerchantId] [int] NOT NULL,
	[ClientAccountType] [int] NOT NULL,
	[IsTestAccount] [bit] NOT NULL,
 CONSTRAINT [PK_dbo.SysClientUser] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[SysClientUser] ([Id], [ClientCode], [MerchantId], [ClientAccountType], [IsTestAccount]) VALUES (1001, N'852366', 1, 1, 0)
/****** Object:  Table [dbo].[OrderToDepositRent]    Script Date: 04/14/2017 17:00:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderToDepositRent](
	[Id] [int] NOT NULL,
	[Deposit] [decimal](18, 2) NOT NULL,
	[RentTotal] [decimal](18, 2) NOT NULL,
	[RentDueDate] [datetime] NULL,
	[RentMonths] [int] NOT NULL,
	[MonthlyRent] [decimal](18, 2) NOT NULL,
	[RentVersion] [nvarchar](128) NULL,
 CONSTRAINT [PK_dbo.OrderToDepositRent] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[OrderToDepositRent] ([Id], [Deposit], [RentTotal], [RentDueDate], [RentMonths], [MonthlyRent], [RentVersion]) VALUES (1, CAST(1000.00 AS Decimal(18, 2)), CAST(600.00 AS Decimal(18, 2)), NULL, 3, CAST(200.00 AS Decimal(18, 2)), N'2017.02.20')
/****** Object:  Table [dbo].[OrderToCarInsure]    Script Date: 04/14/2017 17:00:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderToCarInsure](
	[Id] [int] NOT NULL,
	[InsuranceCompanyId] [int] NOT NULL,
	[CommercialPrice] [decimal](18, 2) NOT NULL,
	[TravelTaxPrice] [decimal](18, 2) NOT NULL,
	[CompulsoryPrice] [decimal](18, 2) NOT NULL,
	[StartOfferTime] [datetime] NULL,
	[EndOfferTime] [datetime] NULL,
	[CZ_CL_XSZ_ImgUrl] [nvarchar](1024) NULL,
	[CZ_SFZ_ImgUrl] [nvarchar](1024) NULL,
	[CCSJM_WSZM_ImgUrl] [nvarchar](1024) NULL,
	[YCZ_CLDJZ_ImgUrl] [nvarchar](1024) NULL,
	[ZJ1_ImgUrl] [nvarchar](1024) NULL,
	[ZJ2_ImgUrl] [nvarchar](1024) NULL,
	[ZJ3_ImgUrl] [nvarchar](1024) NULL,
	[ZJ4_ImgUrl] [nvarchar](1024) NULL,
	[CarOwnerIdNumber] [nvarchar](128) NULL,
	[ClientRequire] [nvarchar](1024) NULL,
	[InsureImgUrl] [nvarchar](1024) NULL,
	[InsurePlanId] [int] NOT NULL,
	[PeriodStart] [datetime] NULL,
	[PeriodEnd] [datetime] NULL,
	[InsuranceOrderId] [nvarchar](128) NULL,
	[CarOwner] [nvarchar](128) NULL,
	[CarPlateNo] [nvarchar](128) NULL,
	[CarVechicheType] [int] NOT NULL,
	[CarModel] [nvarchar](128) NULL,
	[CarSeat] [int] NOT NULL,
	[CarEngineNo] [nvarchar](128) NULL,
	[CarRegisterDate] [nvarchar](128) NULL,
	[CarUserCharacter] [int] NOT NULL,
	[CarVin] [nvarchar](128) NULL,
	[IsLastYearNewCar] [bit] NOT NULL,
	[IsSameLastYear] [bit] NOT NULL,
	[InsuranceCompanyName] [nvarchar](128) NULL,
	[MerchantCommission] [decimal](18, 2) NULL,
	[CarOwnerAddress] [nvarchar](512) NULL,
	[CarIssueDate] [nvarchar](128) NULL,
	[YiBanShiCommission] [decimal](18, 2) NULL,
	[UplinkCommission] [decimal](18, 2) NULL,
	[CommissionVersion] [nvarchar](128) NULL,
	[TotalCommission] [decimal](18, 2) NULL,
 CONSTRAINT [PK_dbo.OrderToCarInsure] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderToCarClaim]    Script Date: 04/14/2017 17:00:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderToCarClaim](
	[Id] [int] NOT NULL,
	[InsuranceCompanyId] [int] NOT NULL,
	[HandPerson] [nvarchar](128) NULL,
	[HandPersonPhone] [nvarchar](128) NULL,
	[CarOwner] [nvarchar](128) NULL,
	[CarOwnerIdNumber] [nvarchar](128) NULL,
	[CarPlateNo] [nvarchar](128) NULL,
	[AccessoriesPrice] [decimal](18, 2) NOT NULL,
	[WorkingHoursPrice] [decimal](18, 2) NOT NULL,
	[InsuranceCompanyName] [nvarchar](128) NULL,
	[EstimateListImgUrl] [nvarchar](1024) NULL,
	[ClientRequire] [nvarchar](1024) NULL,
	[HandMerchantType] [int] NOT NULL,
	[HandMerchantId] [int] NULL,
	[RepairsType] [int] NOT NULL,
	[EstimatePrice] [decimal](18, 2) NULL,
	[CommissionVersion] [nvarchar](128) NULL,
	[HandOrderId] [int] NULL,
	[UplinkCommission] [decimal](18, 2) NULL,
	[MerchantCommission] [decimal](18, 2) NULL,
	[TotalCommission] [decimal](18, 2) NULL,
 CONSTRAINT [PK_dbo.OrderToCarClaim] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Default [DF__CarInsure__CarKi__66603565]    Script Date: 04/14/2017 17:00:49 ******/
ALTER TABLE [dbo].[CarInsurePlanKind] ADD  DEFAULT ((0)) FOR [CarKindId]
GO
/****** Object:  Default [DF__ExtendedA__Statu__5BE2A6F2]    Script Date: 04/14/2017 17:00:49 ******/
ALTER TABLE [dbo].[ExtendedApp] ADD  CONSTRAINT [DF__ExtendedA__Statu__5BE2A6F2]  DEFAULT ((0)) FOR [Status]
GO
/****** Object:  Default [DF__ExtendedA__Creat__5CD6CB2B]    Script Date: 04/14/2017 17:00:49 ******/
ALTER TABLE [dbo].[ExtendedApp] ADD  CONSTRAINT [DF__ExtendedA__Creat__5CD6CB2B]  DEFAULT ((0)) FOR [Creator]
GO
/****** Object:  Default [DF__ExtendedA__Creat__5DCAEF64]    Script Date: 04/14/2017 17:00:49 ******/
ALTER TABLE [dbo].[ExtendedApp] ADD  CONSTRAINT [DF__ExtendedA__Creat__5DCAEF64]  DEFAULT ('1900-01-01T00:00:00.000') FOR [CreateTime]
GO
/****** Object:  Default [DF__ExtendedA__IsDis__5EBF139D]    Script Date: 04/14/2017 17:00:49 ******/
ALTER TABLE [dbo].[ExtendedApp] ADD  CONSTRAINT [DF__ExtendedA__IsDis__5EBF139D]  DEFAULT ((0)) FOR [IsDisplay]
GO
/****** Object:  Default [DF__Merchant__Creato__5FB337D6]    Script Date: 04/14/2017 17:00:49 ******/
ALTER TABLE [dbo].[Merchant] ADD  CONSTRAINT [DF__Merchant__Creato__5FB337D6]  DEFAULT ((0)) FOR [Creator]
GO
/****** Object:  Default [DF__Merchant__Create__60A75C0F]    Script Date: 04/14/2017 17:00:49 ******/
ALTER TABLE [dbo].[Merchant] ADD  CONSTRAINT [DF__Merchant__Create__60A75C0F]  DEFAULT ('1900-01-01T00:00:00.000') FOR [CreateTime]
GO
/****** Object:  Default [DF__Merchant__UserId__619B8048]    Script Date: 04/14/2017 17:00:49 ******/
ALTER TABLE [dbo].[Merchant] ADD  CONSTRAINT [DF__Merchant__UserId__619B8048]  DEFAULT ((0)) FOR [UserId]
GO
/****** Object:  Default [DF__Merchant__Status__628FA481]    Script Date: 04/14/2017 17:00:49 ******/
ALTER TABLE [dbo].[Merchant] ADD  CONSTRAINT [DF__Merchant__Status__628FA481]  DEFAULT ((0)) FOR [Status]
GO
/****** Object:  Default [DF__Merchant__Type__6383C8BA]    Script Date: 04/14/2017 17:00:49 ******/
ALTER TABLE [dbo].[Merchant] ADD  CONSTRAINT [DF__Merchant__Type__6383C8BA]  DEFAULT ((0)) FOR [Type]
GO
/****** Object:  Default [DF__Merchant__Repair__6477ECF3]    Script Date: 04/14/2017 17:00:49 ******/
ALTER TABLE [dbo].[Merchant] ADD  CONSTRAINT [DF__Merchant__Repair__6477ECF3]  DEFAULT ((0)) FOR [RepairCapacity]
GO
/****** Object:  Default [DF__MerchantP__PosMa__656C112C]    Script Date: 04/14/2017 17:00:49 ******/
ALTER TABLE [dbo].[MerchantPosMachine] ADD  CONSTRAINT [DF__MerchantP__PosMa__656C112C]  DEFAULT ((0)) FOR [PosMachineId]
GO
/****** Object:  Default [DF__MerchantP__RentD__66603565]    Script Date: 04/14/2017 17:00:49 ******/
ALTER TABLE [dbo].[MerchantPosMachine] ADD  CONSTRAINT [DF__MerchantP__RentD__66603565]  DEFAULT ('1900-01-01T00:00:00.000') FOR [RentDueDate]
GO
/****** Object:  Default [DF__MerchantP__Creat__6754599E]    Script Date: 04/14/2017 17:00:49 ******/
ALTER TABLE [dbo].[MerchantPosMachine] ADD  CONSTRAINT [DF__MerchantP__Creat__6754599E]  DEFAULT ((0)) FOR [Creator]
GO
/****** Object:  Default [DF__MerchantP__Creat__68487DD7]    Script Date: 04/14/2017 17:00:49 ******/
ALTER TABLE [dbo].[MerchantPosMachine] ADD  CONSTRAINT [DF__MerchantP__Creat__68487DD7]  DEFAULT ('1900-01-01T00:00:00.000') FOR [CreateTime]
GO
/****** Object:  Default [DF__Order__SubmitTim__693CA210]    Script Date: 04/14/2017 17:00:49 ******/
ALTER TABLE [dbo].[Order] ADD  CONSTRAINT [DF__Order__SubmitTim__693CA210]  DEFAULT ('1900-01-01T00:00:00.000') FOR [SubmitTime]
GO
/****** Object:  Default [DF__Order__PayWay__6A30C649]    Script Date: 04/14/2017 17:00:49 ******/
ALTER TABLE [dbo].[Order] ADD  CONSTRAINT [DF__Order__PayWay__6A30C649]  DEFAULT ((0)) FOR [PayWay]
GO
/****** Object:  Default [DF__Order__Following__6B24EA82]    Script Date: 04/14/2017 17:00:49 ******/
ALTER TABLE [dbo].[Order] ADD  CONSTRAINT [DF__Order__Following__6B24EA82]  DEFAULT ((0)) FOR [FollowStatus]
GO
/****** Object:  Default [DF__OrderToCa__Insur__6C190EBB]    Script Date: 04/14/2017 17:00:49 ******/
ALTER TABLE [dbo].[OrderToCarInsure] ADD  CONSTRAINT [DF__OrderToCa__Insur__6C190EBB]  DEFAULT ((0)) FOR [InsurePlanId]
GO
/****** Object:  Default [DF__OrderToCa__Insur__6D0D32F4]    Script Date: 04/14/2017 17:00:49 ******/
ALTER TABLE [dbo].[OrderToCarInsure] ADD  CONSTRAINT [DF__OrderToCa__Insur__6D0D32F4]  DEFAULT ((0)) FOR [InsuranceOrderId]
GO
/****** Object:  Default [DF__OrderToCa__CarSe__6E01572D]    Script Date: 04/14/2017 17:00:49 ******/
ALTER TABLE [dbo].[OrderToCarInsure] ADD  CONSTRAINT [DF__OrderToCa__CarSe__6E01572D]  DEFAULT ((0)) FOR [CarSeat]
GO
/****** Object:  Default [DF__OrderToCa__CarUs__6EF57B66]    Script Date: 04/14/2017 17:00:49 ******/
ALTER TABLE [dbo].[OrderToCarInsure] ADD  CONSTRAINT [DF__OrderToCa__CarUs__6EF57B66]  DEFAULT ((0)) FOR [CarUserCharacter]
GO
/****** Object:  Default [DF__OrderToCa__KindI__6FE99F9F]    Script Date: 04/14/2017 17:00:49 ******/
ALTER TABLE [dbo].[OrderToCarInsureOfferKind] ADD  CONSTRAINT [DF__OrderToCa__KindI__6FE99F9F]  DEFAULT ((0)) FOR [KindId]
GO
/****** Object:  Default [DF__OrderToCa__IsWai__70DDC3D8]    Script Date: 04/14/2017 17:00:49 ******/
ALTER TABLE [dbo].[OrderToCarInsureOfferKind] ADD  CONSTRAINT [DF__OrderToCa__IsWai__70DDC3D8]  DEFAULT ((0)) FOR [IsWaiverDeductible]
GO
/****** Object:  Default [DF__PosMachin__Creat__71D1E811]    Script Date: 04/14/2017 17:00:49 ******/
ALTER TABLE [dbo].[PosMachine] ADD  CONSTRAINT [DF__PosMachin__Creat__71D1E811]  DEFAULT ((0)) FOR [Creator]
GO
/****** Object:  Default [DF__PosMachin__Creat__72C60C4A]    Script Date: 04/14/2017 17:00:49 ******/
ALTER TABLE [dbo].[PosMachine] ADD  CONSTRAINT [DF__PosMachin__Creat__72C60C4A]  DEFAULT ('1900-01-01T00:00:00.000') FOR [CreateTime]
GO
/****** Object:  Default [DF__PosMachin__IsUse__73BA3083]    Script Date: 04/14/2017 17:00:49 ******/
ALTER TABLE [dbo].[PosMachine] ADD  CONSTRAINT [DF__PosMachin__IsUse__73BA3083]  DEFAULT ((0)) FOR [IsUse]
GO
/****** Object:  Default [DF__PosMachin__Depos__74AE54BC]    Script Date: 04/14/2017 17:00:49 ******/
ALTER TABLE [dbo].[PosMachine] ADD  CONSTRAINT [DF__PosMachin__Depos__74AE54BC]  DEFAULT ((0)) FOR [Deposit]
GO
/****** Object:  Default [DF__PosMachine__Rent__75A278F5]    Script Date: 04/14/2017 17:00:49 ******/
ALTER TABLE [dbo].[PosMachine] ADD  CONSTRAINT [DF__PosMachine__Rent__75A278F5]  DEFAULT ((0)) FOR [Rent]
GO
/****** Object:  Default [DF__Product__Price__76969D2E]    Script Date: 04/14/2017 17:00:49 ******/
ALTER TABLE [dbo].[Product] ADD  CONSTRAINT [DF__Product__Price__76969D2E]  DEFAULT ((0)) FOR [Price]
GO
/****** Object:  Default [DF__SysBanner__ReadC__787EE5A0]    Script Date: 04/14/2017 17:00:49 ******/
ALTER TABLE [dbo].[SysBanner] ADD  CONSTRAINT [DF__SysBanner__ReadC__787EE5A0]  DEFAULT ((0)) FOR [ReadCount]
GO
/****** Object:  Default [DF__SysBannerI__Type__797309D9]    Script Date: 04/14/2017 17:00:49 ******/
ALTER TABLE [dbo].[SysBanner] ADD  CONSTRAINT [DF__SysBannerI__Type__797309D9]  DEFAULT ((0)) FOR [Type]
GO
/****** Object:  Default [DF__SysBanner__IsLin__7A672E12]    Script Date: 04/14/2017 17:00:49 ******/
ALTER TABLE [dbo].[SysBanner] ADD  CONSTRAINT [DF__SysBanner__IsLin__7A672E12]  DEFAULT ((0)) FOR [LinkUrl]
GO
/****** Object:  Default [DF__SysClient__Merch__7C4F7684]    Script Date: 04/14/2017 17:00:49 ******/
ALTER TABLE [dbo].[SysClientUser] ADD  CONSTRAINT [DF__SysClient__Merch__7C4F7684]  DEFAULT ((0)) FOR [MerchantId]
GO
/****** Object:  Default [DF__SysClient__Clien__7D439ABD]    Script Date: 04/14/2017 17:00:49 ******/
ALTER TABLE [dbo].[SysClientUser] ADD  CONSTRAINT [DF__SysClient__Clien__7D439ABD]  DEFAULT ((0)) FOR [ClientAccountType]
GO
/****** Object:  Default [DF__SysClient__IsTes__7E37BEF6]    Script Date: 04/14/2017 17:00:49 ******/
ALTER TABLE [dbo].[SysClientUser] ADD  CONSTRAINT [DF__SysClient__IsTes__7E37BEF6]  DEFAULT ((0)) FOR [IsTestAccount]
GO
/****** Object:  Default [DF__SysMenu__Creator__17F790F9]    Script Date: 04/14/2017 17:00:49 ******/
ALTER TABLE [dbo].[SysMenu] ADD  CONSTRAINT [DF__SysMenu__Creator__17F790F9]  DEFAULT ((0)) FOR [Creator]
GO
/****** Object:  Default [DF__SysMenu__CreateT__18EBB532]    Script Date: 04/14/2017 17:00:49 ******/
ALTER TABLE [dbo].[SysMenu] ADD  CONSTRAINT [DF__SysMenu__CreateT__18EBB532]  DEFAULT ('1900-01-01T00:00:00.000') FOR [CreateTime]
GO
/****** Object:  Default [DF__SysOperate__Type__0A9D95DB]    Script Date: 04/14/2017 17:00:49 ******/
ALTER TABLE [dbo].[SysOperateHistory] ADD  DEFAULT ((0)) FOR [Type]
GO
/****** Object:  Default [DF__SysOperat__Creat__0B91BA14]    Script Date: 04/14/2017 17:00:49 ******/
ALTER TABLE [dbo].[SysOperateHistory] ADD  DEFAULT ((0)) FOR [Creator]
GO
/****** Object:  Default [DF__SysRole__Creator__0C85DE4D]    Script Date: 04/14/2017 17:00:49 ******/
ALTER TABLE [dbo].[SysRole] ADD  DEFAULT ((0)) FOR [Creator]
GO
/****** Object:  Default [DF__SysRole__CreateT__0D7A0286]    Script Date: 04/14/2017 17:00:49 ******/
ALTER TABLE [dbo].[SysRole] ADD  DEFAULT ('1900-01-01T00:00:00.000') FOR [CreateTime]
GO
/****** Object:  Default [DF__SysUser__IsModif__08B54D69]    Script Date: 04/14/2017 17:00:49 ******/
ALTER TABLE [dbo].[SysUser] ADD  CONSTRAINT [DF__SysUser__IsModif__08B54D69]  DEFAULT ((0)) FOR [IsModifyDefaultPwd]
GO
/****** Object:  Default [DF__SysUser__Status__09A971A2]    Script Date: 04/14/2017 17:00:49 ******/
ALTER TABLE [dbo].[SysUser] ADD  CONSTRAINT [DF__SysUser__Status__09A971A2]  DEFAULT ((0)) FOR [Status]
GO
/****** Object:  Default [DF__Withdraw__BankCa__06CD04F7]    Script Date: 04/14/2017 17:00:49 ******/
ALTER TABLE [dbo].[Withdraw] ADD  CONSTRAINT [DF__Withdraw__BankCa__06CD04F7]  DEFAULT ((0)) FOR [BankCardId]
GO
/****** Object:  Default [DF__Withdraw__Amount__07C12930]    Script Date: 04/14/2017 17:00:49 ******/
ALTER TABLE [dbo].[Withdraw] ADD  CONSTRAINT [DF__Withdraw__Amount__07C12930]  DEFAULT ((0)) FOR [Amount]
GO
/****** Object:  Default [DF__Withdraw__Amount__08B54D69]    Script Date: 04/14/2017 17:00:49 ******/
ALTER TABLE [dbo].[Withdraw] ADD  CONSTRAINT [DF__Withdraw__Amount__08B54D69]  DEFAULT ((0)) FOR [AmountByAfterFee]
GO
/****** Object:  Default [DF__Withdraw__Fee__09A971A2]    Script Date: 04/14/2017 17:00:49 ******/
ALTER TABLE [dbo].[Withdraw] ADD  CONSTRAINT [DF__Withdraw__Fee__09A971A2]  DEFAULT ((0)) FOR [Fee]
GO
/****** Object:  Default [DF__WithdrawC__CutOf__14270015]    Script Date: 04/14/2017 17:00:49 ******/
ALTER TABLE [dbo].[WithdrawCutOff] ADD  DEFAULT ('1900-01-01T00:00:00.000') FOR [CutOffTime]
GO
/****** Object:  Default [DF__WithdrawC__Batch__151B244E]    Script Date: 04/14/2017 17:00:49 ******/
ALTER TABLE [dbo].[WithdrawCutOff] ADD  DEFAULT ('') FOR [BatchNo]
GO
/****** Object:  Default [DF__WithdrawC__Amoun__160F4887]    Script Date: 04/14/2017 17:00:49 ******/
ALTER TABLE [dbo].[WithdrawCutOff] ADD  DEFAULT ((0)) FOR [Amount]
GO
/****** Object:  Default [DF__WithdrawC__Amoun__17036CC0]    Script Date: 04/14/2017 17:00:49 ******/
ALTER TABLE [dbo].[WithdrawCutOff] ADD  DEFAULT ((0)) FOR [AmountByAfterFee]
GO
/****** Object:  Default [DF__WithdrawC__Withd__0E6E26BF]    Script Date: 04/14/2017 17:00:49 ******/
ALTER TABLE [dbo].[WithdrawCutOffDetails] ADD  CONSTRAINT [DF__WithdrawC__Withd__0E6E26BF]  DEFAULT ((0)) FOR [WithdrawStatus]
GO
/****** Object:  Default [DF__WithdrawC__Merch__0F624AF8]    Script Date: 04/14/2017 17:00:49 ******/
ALTER TABLE [dbo].[WithdrawCutOffDetails] ADD  CONSTRAINT [DF__WithdrawC__Merch__0F624AF8]  DEFAULT ((0)) FOR [MerchantId]
GO
/****** Object:  Default [DF__WithdrawC__UserI__10566F31]    Script Date: 04/14/2017 17:00:49 ******/
ALTER TABLE [dbo].[WithdrawCutOffDetails] ADD  CONSTRAINT [DF__WithdrawC__UserI__10566F31]  DEFAULT ((0)) FOR [UserId]
GO
/****** Object:  Default [DF__WithdrawC__Withd__114A936A]    Script Date: 04/14/2017 17:00:49 ******/
ALTER TABLE [dbo].[WithdrawCutOffDetails] ADD  CONSTRAINT [DF__WithdrawC__Withd__114A936A]  DEFAULT ((0)) FOR [WithdrawId]
GO
/****** Object:  Default [DF__WithdrawC__Withd__123EB7A3]    Script Date: 04/14/2017 17:00:49 ******/
ALTER TABLE [dbo].[WithdrawCutOffDetails] ADD  CONSTRAINT [DF__WithdrawC__Withd__123EB7A3]  DEFAULT ((0)) FOR [WithdrawAmount]
GO
/****** Object:  Default [DF__WithdrawC__Withd__1332DBDC]    Script Date: 04/14/2017 17:00:49 ******/
ALTER TABLE [dbo].[WithdrawCutOffDetails] ADD  CONSTRAINT [DF__WithdrawC__Withd__1332DBDC]  DEFAULT ((0)) FOR [WithdrawAmountByAfterFee]
GO
/****** Object:  Default [DF__WithdrawC__Withd__14270015]    Script Date: 04/14/2017 17:00:49 ******/
ALTER TABLE [dbo].[WithdrawCutOffDetails] ADD  CONSTRAINT [DF__WithdrawC__Withd__14270015]  DEFAULT ((0)) FOR [WithdrawFee]
GO
/****** Object:  Default [DF__WithdrawC__Withd__151B244E]    Script Date: 04/14/2017 17:00:49 ******/
ALTER TABLE [dbo].[WithdrawCutOffDetails] ADD  CONSTRAINT [DF__WithdrawC__Withd__151B244E]  DEFAULT ('1900-01-01T00:00:00.000') FOR [WithdrawExpectArriveTime]
GO
/****** Object:  Default [DF__WithdrawC__Withd__160F4887]    Script Date: 04/14/2017 17:00:49 ******/
ALTER TABLE [dbo].[WithdrawCutOffDetails] ADD  CONSTRAINT [DF__WithdrawC__Withd__160F4887]  DEFAULT ('1900-01-01T00:00:00.000') FOR [WithdrawStartTime]
GO
/****** Object:  Default [DF__WithdrawC__Withd__17036CC0]    Script Date: 04/14/2017 17:00:49 ******/
ALTER TABLE [dbo].[WithdrawCutOffDetails] ADD  CONSTRAINT [DF__WithdrawC__Withd__17036CC0]  DEFAULT ('1900-01-01T00:00:00.000') FOR [WithdrawSettlementStartTime]
GO
/****** Object:  Default [DF__WithdrawC__Withd__17F790F9]    Script Date: 04/14/2017 17:00:49 ******/
ALTER TABLE [dbo].[WithdrawCutOffDetails] ADD  CONSTRAINT [DF__WithdrawC__Withd__17F790F9]  DEFAULT ((0)) FOR [WithdrawBankCardId]
GO
/****** Object:  Default [DF__WithdrawC__Withd__18EBB532]    Script Date: 04/14/2017 17:00:49 ******/
ALTER TABLE [dbo].[WithdrawCutOffDetails] ADD  CONSTRAINT [DF__WithdrawC__Withd__18EBB532]  DEFAULT ((0)) FOR [WithdrawCutOffId]
GO
/****** Object:  Default [DF__WithdrawC__Withd__19DFD96B]    Script Date: 04/14/2017 17:00:49 ******/
ALTER TABLE [dbo].[WithdrawCutOffDetails] ADD  CONSTRAINT [DF__WithdrawC__Withd__19DFD96B]  DEFAULT ('1900-01-01T00:00:00.000') FOR [WithdrawCutoffTime]
GO
/****** Object:  ForeignKey [FK_dbo.OrderToCarClaim_dbo.Order_Id]    Script Date: 04/14/2017 17:00:49 ******/
ALTER TABLE [dbo].[OrderToCarClaim]  WITH CHECK ADD  CONSTRAINT [FK_dbo.OrderToCarClaim_dbo.Order_Id] FOREIGN KEY([Id])
REFERENCES [dbo].[Order] ([Id])
GO
ALTER TABLE [dbo].[OrderToCarClaim] CHECK CONSTRAINT [FK_dbo.OrderToCarClaim_dbo.Order_Id]
GO
/****** Object:  ForeignKey [FK_dbo.OrderToCarInsure_dbo.Order_Id]    Script Date: 04/14/2017 17:00:49 ******/
ALTER TABLE [dbo].[OrderToCarInsure]  WITH CHECK ADD  CONSTRAINT [FK_dbo.OrderToCarInsure_dbo.Order_Id] FOREIGN KEY([Id])
REFERENCES [dbo].[Order] ([Id])
GO
ALTER TABLE [dbo].[OrderToCarInsure] CHECK CONSTRAINT [FK_dbo.OrderToCarInsure_dbo.Order_Id]
GO
/****** Object:  ForeignKey [FK_dbo.OrderToDepositRent_dbo.Order_Id]    Script Date: 04/14/2017 17:00:49 ******/
ALTER TABLE [dbo].[OrderToDepositRent]  WITH CHECK ADD  CONSTRAINT [FK_dbo.OrderToDepositRent_dbo.Order_Id] FOREIGN KEY([Id])
REFERENCES [dbo].[Order] ([Id])
GO
ALTER TABLE [dbo].[OrderToDepositRent] CHECK CONSTRAINT [FK_dbo.OrderToDepositRent_dbo.Order_Id]
GO
/****** Object:  ForeignKey [FK_dbo.SysClientUser_dbo.SysUser_Id]    Script Date: 04/14/2017 17:00:49 ******/
ALTER TABLE [dbo].[SysClientUser]  WITH CHECK ADD  CONSTRAINT [FK_dbo.SysClientUser_dbo.SysUser_Id] FOREIGN KEY([Id])
REFERENCES [dbo].[SysUser] ([Id])
GO
ALTER TABLE [dbo].[SysClientUser] CHECK CONSTRAINT [FK_dbo.SysClientUser_dbo.SysUser_Id]
GO
/****** Object:  ForeignKey [FK_dbo.SysSalesmanUser_dbo.SysUser_Id]    Script Date: 04/14/2017 17:00:49 ******/
ALTER TABLE [dbo].[SysSalesmanUser]  WITH CHECK ADD  CONSTRAINT [FK_dbo.SysSalesmanUser_dbo.SysUser_Id] FOREIGN KEY([Id])
REFERENCES [dbo].[SysUser] ([Id])
GO
ALTER TABLE [dbo].[SysSalesmanUser] CHECK CONSTRAINT [FK_dbo.SysSalesmanUser_dbo.SysUser_Id]
GO
/****** Object:  ForeignKey [FK_dbo.SysStaffUser_dbo.SysUser_Id]    Script Date: 04/14/2017 17:00:49 ******/
ALTER TABLE [dbo].[SysStaffUser]  WITH CHECK ADD  CONSTRAINT [FK_dbo.SysStaffUser_dbo.SysUser_Id] FOREIGN KEY([Id])
REFERENCES [dbo].[SysUser] ([Id])
GO
ALTER TABLE [dbo].[SysStaffUser] CHECK CONSTRAINT [FK_dbo.SysStaffUser_dbo.SysUser_Id]
GO
/****** Object:  ForeignKey [FK_dbo.SysUserClaim_dbo.SysUser_UserId]    Script Date: 04/14/2017 17:00:49 ******/
ALTER TABLE [dbo].[SysUserClaim]  WITH CHECK ADD  CONSTRAINT [FK_dbo.SysUserClaim_dbo.SysUser_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[SysUser] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SysUserClaim] CHECK CONSTRAINT [FK_dbo.SysUserClaim_dbo.SysUser_UserId]
GO
/****** Object:  ForeignKey [FK_dbo.SysUserLoginProvider_dbo.SysUser_UserId]    Script Date: 04/14/2017 17:00:49 ******/
ALTER TABLE [dbo].[SysUserLoginProvider]  WITH CHECK ADD  CONSTRAINT [FK_dbo.SysUserLoginProvider_dbo.SysUser_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[SysUser] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SysUserLoginProvider] CHECK CONSTRAINT [FK_dbo.SysUserLoginProvider_dbo.SysUser_UserId]
GO
/****** Object:  ForeignKey [FK_dbo.SysUserRole_dbo.SysRole_RoleId]    Script Date: 04/14/2017 17:00:49 ******/
ALTER TABLE [dbo].[SysUserRole]  WITH CHECK ADD  CONSTRAINT [FK_dbo.SysUserRole_dbo.SysRole_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[SysRole] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SysUserRole] CHECK CONSTRAINT [FK_dbo.SysUserRole_dbo.SysRole_RoleId]
GO
/****** Object:  ForeignKey [FK_dbo.SysUserRole_dbo.SysUser_UserId]    Script Date: 04/14/2017 17:00:49 ******/
ALTER TABLE [dbo].[SysUserRole]  WITH CHECK ADD  CONSTRAINT [FK_dbo.SysUserRole_dbo.SysUser_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[SysUser] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SysUserRole] CHECK CONSTRAINT [FK_dbo.SysUserRole_dbo.SysUser_UserId]
GO
