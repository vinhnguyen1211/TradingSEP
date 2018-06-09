USE [master]
GO
/****** Object:  Database [vlutrading3545]    Script Date: 5/24/2018 7:57:59 PM ******/
CREATE DATABASE [vlutrading3545]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'vlutrading3545', FILENAME = N'c:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\vlutrading3545.mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'vlutrading3545_log', FILENAME = N'c:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\vlutrading3545_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [vlutrading3545] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [vlutrading3545].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [vlutrading3545] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [vlutrading3545] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [vlutrading3545] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [vlutrading3545] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [vlutrading3545] SET ARITHABORT OFF 
GO
ALTER DATABASE [vlutrading3545] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [vlutrading3545] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [vlutrading3545] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [vlutrading3545] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [vlutrading3545] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [vlutrading3545] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [vlutrading3545] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [vlutrading3545] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [vlutrading3545] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [vlutrading3545] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [vlutrading3545] SET  DISABLE_BROKER 
GO
ALTER DATABASE [vlutrading3545] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [vlutrading3545] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [vlutrading3545] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [vlutrading3545] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [vlutrading3545] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [vlutrading3545] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [vlutrading3545] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [vlutrading3545] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [vlutrading3545] SET  MULTI_USER 
GO
ALTER DATABASE [vlutrading3545] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [vlutrading3545] SET DB_CHAINING OFF 
GO
ALTER DATABASE [vlutrading3545] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [vlutrading3545] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
USE [vlutrading3545]
GO
/****** Object:  StoredProcedure [dbo].[updateLastLoginIpAddress]    Script Date: 5/24/2018 7:57:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[updateLastLoginIpAddress]
    @userId int,
    @ip_address varchar(20)
AS
BEGIN
     UPDATE [user] 
		 SET [ip_last_login]=@ip_address, [last_login_date] = getdate() WHERE id = @userId
END


GO
/****** Object:  StoredProcedure [dbo].[updateLastLogoutIpAddress]    Script Date: 5/24/2018 7:57:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[updateLastLogoutIpAddress]
    @userId int,
    @ip_address varchar(20)
AS
BEGIN
     UPDATE [user] 
		 SET ip_last_logout=@ip_address, last_logout_date = getdate() WHERE id = @userId
END

GO
/****** Object:  Table [dbo].[item]    Script Date: 5/24/2018 7:57:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[item](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[item_name] [nvarchar](100) NULL,
	[description] [nvarchar](max) NULL,
	[quantity] [int] NULL,
	[status] [int] NULL,
	[seller_id] [int] NOT NULL,
	[buyer_id] [int] NULL,
	[create_by] [varchar](20) NULL,
	[create_date] [datetime] NULL,
	[update_by] [varchar](20) NULL,
	[update_date] [datetime] NULL,
	[index_image] [varchar](max) NULL,
	[detail_image1] [varchar](max) NULL,
	[detail_image2] [varchar](max) NULL,
	[detail_image3] [varchar](max) NULL,
	[detail_image4] [varchar](max) NULL,
	[detail_image5] [varchar](max) NULL,
	[price] [decimal](18, 3) NULL,
 CONSTRAINT [PK_item] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[item_status]    Script Date: 5/24/2018 7:57:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[item_status](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[status] [nvarchar](255) NOT NULL,
	[description] [nvarchar](500) NULL,
	[create_by] [varchar](20) NULL,
	[create_date] [datetime] NULL,
	[update_by] [varchar](20) NULL,
	[update_date] [datetime] NULL,
 CONSTRAINT [PK_item_status] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Order]    Script Date: 5/24/2018 7:57:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Order](
	[item_id] [int] NOT NULL,
	[user_id] [int] NOT NULL,
	[status] [bit] NOT NULL,
	[username] [varchar](20) NULL,
	[item_name] [nvarchar](100) NULL,
 CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED 
(
	[item_id] ASC,
	[user_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[role]    Script Date: 5/24/2018 7:57:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[role](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[role_name] [nvarchar](50) NULL,
	[description] [nvarchar](255) NULL,
	[create_by] [varchar](20) NULL,
	[create_date] [datetime] NULL,
	[update_by] [varchar](20) NULL,
	[update_date] [datetime] NULL,
 CONSTRAINT [PK_role] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[security_question]    Script Date: 5/24/2018 7:57:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[security_question](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[question] [nvarchar](255) NOT NULL,
	[is_active] [int] NULL,
 CONSTRAINT [PK_security_question] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[user]    Script Date: 5/24/2018 7:57:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[user](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[username] [varchar](20) NOT NULL,
	[password] [varchar](50) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
	[role] [int] NULL,
	[email] [varchar](50) NOT NULL,
	[id_security_question] [int] NULL,
	[answer_security_question] [varchar](50) NULL,
	[is_active] [int] NULL,
	[last_login_date] [datetime] NULL,
	[last_logout_date] [datetime] NULL,
	[ip_last_login] [varchar](20) NULL,
	[ip_last_logout] [varchar](20) NULL,
	[create_by] [varchar](20) NULL,
	[create_date] [datetime] NULL,
	[update_by] [varchar](20) NULL,
	[update_date] [datetime] NULL,
 CONSTRAINT [PK_user] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[user_personal_information]    Script Date: 5/24/2018 7:57:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[user_personal_information](
	[id] [int] NOT NULL,
	[id_user] [int] NOT NULL,
	[phone] [varchar](15) NULL,
	[address] [nvarchar](100) NULL,
	[update_by] [varchar](20) NULL,
	[update_date] [datetime] NULL,
 CONSTRAINT [PK_user_personal_information] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[item] ON 

INSERT [dbo].[item] ([id], [item_name], [description], [quantity], [status], [seller_id], [buyer_id], [create_by], [create_date], [update_by], [update_date], [index_image], [detail_image1], [detail_image2], [detail_image3], [detail_image4], [detail_image5], [price]) VALUES (11, N'Bag', N'<p style="margin-bottom: 15px; padding: 0px; text-align: justify; color: rgb(0, 0, 0); font-family: &quot;Open Sans&quot;, Arial, sans-serif;">malesuada urna, in blandit enim. Etiam finibus pulvinar ligula, et molestie magna volutpat sed. Quisque eu erat ac magna faucibus lacinia in quis risus. Etiam lorem sem, fringilla sollicitudin ullamcorper sit amet, vestibulum tincidunt risus. Etiam commodo tellus vitae lacus dictum commodo. Suspendisse dapibus laoreet commodo. Sed ac purus ut ipsum commodo tincidunt at quis tortor. Nulla ut nulla nisi. Mauris commodo congue mauris, ut vulputate risus sagittis ac. Morbi ultrices mattis lobortis. Cras eget leo vulputate, maximus tortor sed, blandit felis. Maecenas semper turpis sed justo hendrerit bibendum. In hac habitasse platea dictumst.</p><p style="margin-bottom: 15px; padding: 0px; text-align: justify; color: rgb(0, 0, 0); font-family: &quot;Open Sans&quot;, Arial, sans-serif;"><strong>Sed dolor felis, consequat id venenatis sed, suscipit ac nisi. Sed malesuada ante a orci facilisis mattis. Fusce commodo metus id turpis ornare pellentesque. Etiam iaculis placerat mattis. Curabitur dui ante, ultrices et lectus aliquet, maximus tristique velit. Aliquam sed pulvinar augue, nec fringilla felis. Fusce risus arcu, pulvinar faucibus consequat quis, scelerisque non massa. Fusce et congue nisi. In blandit in quam in fringilla. Maecenas ornare posuere sem, et mollis lectus. Donec vitae tortor at massa fringilla ultrices eget iaculis justo. Nam sed lobortis quam.</strong></p><p style="margin-bottom: 15px; padding: 0px; text-align: justify; color: rgb(0, 0, 0); font-family: &quot;Open Sans&quot;, Arial, sans-serif;">Vivamus purus ipsum, euismod sagittis elit in, porttitor ultrices metus. Curabitur venenatis pretium leo, id bibendum risus convallis id. Aenean gravida orci nisl, a pretium ligula interdum ut. Proin sodales, justo ut dictum ullamcorper, quam ex imperdiet velit, sit amet consectetur arcu turpis sed sem. Integer in metus in nunc feugiat auctor. Nunc eu molestie risus. Cras dapibus dapibus quam ut convallis. Fusce dictum felis tellus, at luctus ipsum mattis quis. Integer blandit augue non sapien congue, tempor euismod nulla pellentesque. Morbi at lacus mauris. Mauris posuere bibendum rutrum. Praesent pretium vehicula tellus, sed accumsan lorem ullamcorper eget. Nam auctor feugiat venenatis. Fusce luctus erat sit amet posuere sollicitudin. Sed dolor metus, tristique vitae placerat in, placerat et justo.</p><p style="text-align: right; margin-bottom: 15px; padding: 0px; color: rgb(0, 0, 0); font-family: &quot;Open Sans&quot;, Arial, sans-serif;">Phasellus at sem ut enim condimentum laoreet. Vestibulum eu sagittis erat, at pellentesque nunc. Maecenas sapien erat, molestie quis diam eget, imperdiet pretium massa. Nam porta justo at justo ornare, lacinia fringilla erat porttitor. Etiam quis malesuada ex, ut faucibus neque. Morbi gravida nulla id nisl ullamcorper mollis. Aliquam nec ullamcorper elit. Proin nisi mi, ornare et blandit vel, sagittis non dolor. Phasellus id tortor vel dolor porttitor convallis finibus eu odio. Aenean pellentesque dictum quam, sed faucibus lacus vulputate id. Ut sit amet urna ut diam pulvinar aliquet at commodo velit. Quisque eget risus dapibus, placerat felis eget, tincidunt quam. Pellentesque non lobortis erat, non auctor massa. Aliquam massa risus, dapibus sed quam eu, tristique tristique massa. Phasellus ut cursus odio.</p><p style="margin-bottom: 15px; padding: 0px; text-align: justify; color: rgb(0, 0, 0); font-family: &quot;Open Sans&quot;, Arial, sans-serif;"><em>Curabitur interdum gravida neque, elementum interdum enim cursus suscipit. Integer a augue eu orci mattis ultricies eget eget elit. Quisque non lacus sed diam lobortis suscipit. Morbi euismod quam sit amet lorem aliquam, eget congue turpis mattis. Nam id libero sed est convallis consequat id quis dolor. Sed varius laoreet mauris, a mollis risus ultricies quis. Cras consectetur vestibulum nulla, id ultricies lectus porttitor ac. Etiam ultricies scelerisque est, ut scelerisque ex laoreet a. Sed lacinia quis dui lobortis pharetra. Quisque bibendum nibh vehicula arcu sodales consequat. Donec fringilla orci sed feugiat laoreet. Duis at erat orci. Donec et urna ac tellus fermentum dictum. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nunc commodo auctor lectus. Nulla facilisi.</em></p><br><p style="margin-bottom: 15px; padding: 0px; text-align: justify; color: rgb(0, 0, 0); font-family: &quot;Open Sans&quot;, Arial, sans-serif;"><img src="http://wulala.com.my/image/cache/data/LV/neverfull119/unnamed%20(7)-780x975.jpg" alt=""><br></p>', 4, 2, 1, NULL, N'vinh', CAST(0x0000A8D70174179D AS DateTime), N'vinh', CAST(0x0000A8D70174179D AS DateTime), N'69810ba5-340f-4b25-a66c-53335dcc1581.jpg', NULL, NULL, NULL, NULL, NULL, CAST(10.000 AS Decimal(18, 3)))
INSERT [dbo].[item] ([id], [item_name], [description], [quantity], [status], [seller_id], [buyer_id], [create_by], [create_date], [update_by], [update_date], [index_image], [detail_image1], [detail_image2], [detail_image3], [detail_image4], [detail_image5], [price]) VALUES (12, N'Watch', N'<p>Girl Watch</p><p>Boy Watch</p><p>Watch for working or hanging out</p>', 10, 2, 1, NULL, N'vinh', CAST(0x0000A8D701748D0E AS DateTime), N'vinh', CAST(0x0000A8D701748D0E AS DateTime), N'main-product01.jpg', NULL, NULL, NULL, NULL, NULL, CAST(15.000 AS Decimal(18, 3)))
INSERT [dbo].[item] ([id], [item_name], [description], [quantity], [status], [seller_id], [buyer_id], [create_by], [create_date], [update_by], [update_date], [index_image], [detail_image1], [detail_image2], [detail_image3], [detail_image4], [detail_image5], [price]) VALUES (13, N'Shoe/Sneaker', N'<p>Sport Shoe</p><p>Comfortable</p><p>Blue Shoe</p>', 1, 2, 1, NULL, N'vinh', CAST(0x0000A8D701752F34 AS DateTime), N'vinh', CAST(0x0000A8D701752F34 AS DateTime), N'main-product01.jpg', NULL, NULL, NULL, NULL, NULL, CAST(20.000 AS Decimal(18, 3)))
INSERT [dbo].[item] ([id], [item_name], [description], [quantity], [status], [seller_id], [buyer_id], [create_by], [create_date], [update_by], [update_date], [index_image], [detail_image1], [detail_image2], [detail_image3], [detail_image4], [detail_image5], [price]) VALUES (14, N'Boots', N'<p>Boots for Girl</p><p>Black and Brown colour</p><p>High Boots</p><p>Just 1 pair</p>', 1, 1, 1, NULL, N'vinh', CAST(0x0000A8D701757121 AS DateTime), N'vinh', CAST(0x0000A8D701757121 AS DateTime), N'main-product01.jpg', NULL, NULL, NULL, NULL, NULL, CAST(25.000 AS Decimal(18, 3)))
INSERT [dbo].[item] ([id], [item_name], [description], [quantity], [status], [seller_id], [buyer_id], [create_by], [create_date], [update_by], [update_date], [index_image], [detail_image1], [detail_image2], [detail_image3], [detail_image4], [detail_image5], [price]) VALUES (15, N'new item', N'Enter text here...', 1, 1, 1, NULL, N'vinh', CAST(0x0000A8D800A82B48 AS DateTime), N'vinh', CAST(0x0000A8D800A82B48 AS DateTime), N'main-product01.jpg', NULL, NULL, NULL, NULL, NULL, CAST(30.000 AS Decimal(18, 3)))
INSERT [dbo].[item] ([id], [item_name], [description], [quantity], [status], [seller_id], [buyer_id], [create_by], [create_date], [update_by], [update_date], [index_image], [detail_image1], [detail_image2], [detail_image3], [detail_image4], [detail_image5], [price]) VALUES (16, N'Item ', N'Enter text here...', 2, 1, 1, NULL, N'vinh', CAST(0x0000A8D900004696 AS DateTime), N'vinh', CAST(0x0000A8D900004696 AS DateTime), N'main-product01.jpg', N'', N'', N'', N'', N'', CAST(35.000 AS Decimal(18, 3)))
INSERT [dbo].[item] ([id], [item_name], [description], [quantity], [status], [seller_id], [buyer_id], [create_by], [create_date], [update_by], [update_date], [index_image], [detail_image1], [detail_image2], [detail_image3], [detail_image4], [detail_image5], [price]) VALUES (17, N'dsadsadas', N'Enter text here...', 1, 1, 1, NULL, N'vinh', CAST(0x0000A8D90001351F AS DateTime), N'vinh', CAST(0x0000A8D90001351F AS DateTime), N'main-product01.jpg', N'5a64afe8-3149-463e-9a68-48f1f3ba1991.jpg', N'42e4e69d-451d-4392-86c9-8efeda511719.jpg', N'c1059921-4fe8-409c-a249-768b5069f830.jpg', N'ad681ccb-5b56-4bf6-b635-15096c025e04.jpg', N'9dc5528e-279e-4c02-a813-a685a903886b.jpg', CAST(40.000 AS Decimal(18, 3)))
INSERT [dbo].[item] ([id], [item_name], [description], [quantity], [status], [seller_id], [buyer_id], [create_by], [create_date], [update_by], [update_date], [index_image], [detail_image1], [detail_image2], [detail_image3], [detail_image4], [detail_image5], [price]) VALUES (18, N'9g45AM ', N'<p style="margin-bottom: 15px; padding: 0px; text-align: justify; color: rgb(0, 0, 0); font-family: &quot;Open Sans&quot;, Arial, sans-serif;">malesuada urna, in blandit enim. Etiam finibus pulvinar ligula, et molestie magna volutpat sed. Quisque eu erat ac magna faucibus lacinia in quis risus. Etiam lorem sem, fringilla sollicitudin ullamcorper sit amet, vestibulum tincidunt risus. Etiam commodo tellus vitae lacus dictum commodo. Suspendisse dapibus laoreet commodo. Sed ac purus ut ipsum commodo tincidunt at quis tortor. Nulla ut nulla nisi. Mauris commodo congue mauris, ut vulputate risus sagittis ac. Morbi ultrices mattis lobortis. Cras eget leo vulputate, maximus tortor sed, blandit felis. Maecenas semper turpis sed justo hendrerit bibendum. In hac habitasse platea dictumst.</p><p style="margin-bottom: 15px; padding: 0px; text-align: justify; color: rgb(0, 0, 0); font-family: &quot;Open Sans&quot;, Arial, sans-serif;"><strong>Sed dolor felis, consequat id venenatis sed, suscipit ac nisi. Sed malesuada ante a orci facilisis mattis. Fusce commodo metus id turpis ornare pellentesque. Etiam iaculis placerat mattis. Curabitur dui ante, ultrices et lectus aliquet, maximus tristique velit. Aliquam sed pulvinar augue, nec fringilla felis. Fusce risus arcu, pulvinar faucibus consequat quis, scelerisque non massa. Fusce et congue nisi. In blandit in quam in fringilla. Maecenas ornare posuere sem, et mollis lectus. Donec vitae tortor at massa fringilla ultrices eget iaculis justo. Nam sed lobortis quam.</strong></p><p style="margin-bottom: 15px; padding: 0px; text-align: justify; color: rgb(0, 0, 0); font-family: &quot;Open Sans&quot;, Arial, sans-serif;">Vivamus purus ipsum, euismod sagittis elit in, porttitor ultrices metus. Curabitur venenatis pretium leo, id bibendum risus convallis id. Aenean gravida orci nisl, a pretium ligula interdum ut. Proin sodales, justo ut dictum ullamcorper, quam ex imperdiet velit, sit amet consectetur arcu turpis sed sem. Integer in metus in nunc feugiat auctor. Nunc eu molestie risus. Cras dapibus dapibus quam ut convallis. Fusce dictum felis tellus, at luctus ipsum mattis quis. Integer blandit augue non sapien congue, tempor euismod nulla pellentesque. Morbi at lacus mauris. Mauris posuere bibendum rutrum. Praesent pretium vehicula tellus, sed accumsan lorem ullamcorper eget. Nam auctor feugiat venenatis. Fusce luctus erat sit amet posuere sollicitudin. Sed dolor metus, tristique vitae placerat in, placerat et justo.</p><p style="text-align: right; margin-bottom: 15px; padding: 0px; color: rgb(0, 0, 0); font-family: &quot;Open Sans&quot;, Arial, sans-serif;">Phasellus at sem ut enim condimentum laoreet. Vestibulum eu sagittis erat, at pellentesque nunc. Maecenas sapien erat, molestie quis diam eget, imperdiet pretium massa. Nam porta justo at justo ornare, lacinia fringilla erat porttitor. Etiam quis malesuada ex, ut faucibus neque. Morbi gravida nulla id nisl ullamcorper mollis. Aliquam nec ullamcorper elit. Proin nisi mi, ornare et blandit vel, sagittis non dolor. Phasellus id tortor vel dolor porttitor convallis finibus eu odio. Aenean pellentesque dictum quam, sed faucibus lacus vulputate id. Ut sit amet urna ut diam pulvinar aliquet at commodo velit. Quisque eget risus dapibus, placerat felis eget, tincidunt quam. Pellentesque non lobortis erat, non auctor massa. Aliquam massa risus, dapibus sed quam eu, tristique tristique massa. Phasellus ut cursus odio.</p><p style="margin-bottom: 15px; padding: 0px; text-align: justify; color: rgb(0, 0, 0); font-family: &quot;Open Sans&quot;, Arial, sans-serif;"><em>Curabitur interdum gravida neque, elementum interdum enim cursus suscipit. Integer a augue eu orci mattis ultricies eget eget elit. Quisque non lacus sed diam lobortis suscipit. Morbi euismod quam sit amet lorem aliquam, eget congue turpis mattis. Nam id libero sed est convallis consequat id quis dolor. Sed varius laoreet mauris, a mollis risus ultricies quis. Cras consectetur vestibulum nulla, id ultricies lectus porttitor ac. Etiam ultricies scelerisque est, ut scelerisque ex laoreet a. Sed lacinia quis dui lobortis pharetra. Quisque bibendum nibh vehicula arcu sodales consequat. Donec fringilla orci sed feugiat laoreet. Duis at erat orci. Donec et urna ac tellus fermentum dictum. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nunc commodo auctor lectus. Nulla facilisi.</em></p><br><p style="margin-bottom: 15px; padding: 0px; text-align: justify; color: rgb(0, 0, 0); font-family: &quot;Open Sans&quot;, Arial, sans-serif;"><img src="http://wulala.com.my/image/cache/data/LV/neverfull119/unnamed%20(7)-780x975.jpg" alt=""><br></p>', 3, 1, 1, NULL, N'vinh', CAST(0x0000A8D900A1BA48 AS DateTime), N'vinh', CAST(0x0000A8D900A1BA48 AS DateTime), N'main-product01.jpg', N'48ec0c18-69ad-4acf-a1c3-f0451f3e3508.jpg', N'7690529b-8f94-4eb3-8765-3d7515291c9b.jpg', N'e00d6e05-cf29-4511-9656-bfba77aec41b.jpg', N'c1e79332-b79a-4105-91a8-a4e30aa0ba96.jpg', N'2d07753f-6c78-48d1-b25c-5a81b551048a.jpg', CAST(100.000 AS Decimal(18, 3)))
INSERT [dbo].[item] ([id], [item_name], [description], [quantity], [status], [seller_id], [buyer_id], [create_by], [create_date], [update_by], [update_date], [index_image], [detail_image1], [detail_image2], [detail_image3], [detail_image4], [detail_image5], [price]) VALUES (22, N'dasdasdasdasbbxvbvb', N'Enter text here...', 2, 2, 1, NULL, N'vinh', CAST(0x0000A8DB0110A90B AS DateTime), N'vinh', CAST(0x0000A8DB0110A90B AS DateTime), N'9076d0b6-42b9-4e27-ae5f-e3380edf31e4.jpg', N'44763292-24a0-4065-952b-fe1d43a9980d.PNG', N'', NULL, NULL, NULL, CAST(2.000 AS Decimal(18, 3)))
INSERT [dbo].[item] ([id], [item_name], [description], [quantity], [status], [seller_id], [buyer_id], [create_by], [create_date], [update_by], [update_date], [index_image], [detail_image1], [detail_image2], [detail_image3], [detail_image4], [detail_image5], [price]) VALUES (23, N'demoViVi', N'abc', 1000, 1, 7, NULL, N'Vi', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[item] OFF
SET IDENTITY_INSERT [dbo].[item_status] ON 

INSERT [dbo].[item_status] ([id], [status], [description], [create_by], [create_date], [update_by], [update_date]) VALUES (1, N'Có hàng', N'Có hàng', N'vinh', CAST(0x0000A8BD014552D0 AS DateTime), N'vinh', CAST(0x0000A8BD014558AC AS DateTime))
INSERT [dbo].[item_status] ([id], [status], [description], [create_by], [create_date], [update_by], [update_date]) VALUES (2, N'Hết hàng', N'Hết hàng', NULL, NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[item_status] OFF
INSERT [dbo].[Order] ([item_id], [user_id], [status], [username], [item_name]) VALUES (11, 4, 1, N'team04', N'Bag')
INSERT [dbo].[Order] ([item_id], [user_id], [status], [username], [item_name]) VALUES (12, 4, 0, N'team04', N'Watch')
INSERT [dbo].[Order] ([item_id], [user_id], [status], [username], [item_name]) VALUES (13, 4, 1, N'team04', N'Shoe/Sneaker')
SET IDENTITY_INSERT [dbo].[role] ON 

INSERT [dbo].[role] ([id], [role_name], [description], [create_by], [create_date], [update_by], [update_date]) VALUES (1, N'User', N'User', NULL, CAST(0x0000A8B70171B424 AS DateTime), NULL, CAST(0x0000A8B70171B7A8 AS DateTime))
SET IDENTITY_INSERT [dbo].[role] OFF
SET IDENTITY_INSERT [dbo].[security_question] ON 

INSERT [dbo].[security_question] ([id], [question], [is_active]) VALUES (1, N'Cha của bạn sinh ra ở tỉnh/thành phố nào?', 1)
INSERT [dbo].[security_question] ([id], [question], [is_active]) VALUES (2, N'Bạn thân nhất thời thơ ấu của bạn có họ gì?', 1)
INSERT [dbo].[security_question] ([id], [question], [is_active]) VALUES (3, N'Tên trường tiểu học của bạn là gì?', 1)
INSERT [dbo].[security_question] ([id], [question], [is_active]) VALUES (4, N'Con đường nơi bạn lớn lên tên gì?', 1)
INSERT [dbo].[security_question] ([id], [question], [is_active]) VALUES (5, N'Con vật cưng đầu tiên của bạn tên gì?', 1)
INSERT [dbo].[security_question] ([id], [question], [is_active]) VALUES (6, N'Tên của nhạc sĩ bạn thích nhất?', 1)
INSERT [dbo].[security_question] ([id], [question], [is_active]) VALUES (7, N'Giáo viên bạn thích nhất khi học tiểu học có họ gì?', 1)
INSERT [dbo].[security_question] ([id], [question], [is_active]) VALUES (8, N'Lúc nhỏ bạn thích xem phim hoạt hình nào nhất?', 1)
INSERT [dbo].[security_question] ([id], [question], [is_active]) VALUES (9, N'Món khoái khẩu của bạn lúc còn nhỏ là gì?', 1)
INSERT [dbo].[security_question] ([id], [question], [is_active]) VALUES (10, N'Nhân vật trong phim mà bạn thích nhất là ai?', 1)
INSERT [dbo].[security_question] ([id], [question], [is_active]) VALUES (11, N'Ai là tác giả bạn thích nhất?', 1)
INSERT [dbo].[security_question] ([id], [question], [is_active]) VALUES (12, N'Đội thể thao bạn thích nhất tên là gì?', 1)
INSERT [dbo].[security_question] ([id], [question], [is_active]) VALUES (13, N'Tựa quyển sách bạn thích đọc nhất?', 1)
SET IDENTITY_INSERT [dbo].[security_question] OFF
SET IDENTITY_INSERT [dbo].[user] ON 

INSERT [dbo].[user] ([id], [username], [password], [name], [role], [email], [id_security_question], [answer_security_question], [is_active], [last_login_date], [last_logout_date], [ip_last_login], [ip_last_logout], [create_by], [create_date], [update_by], [update_date]) VALUES (1, N'vinh', N'e10adc3949ba59abbe56e057f20f883e', N'Nguyễn Phạm Hữu Vinh', 1, N'vinh_nguyen1211@yahoo.com', 8, N'Disney Channel', 1, CAST(0x0000A8EA0148BCFF AS DateTime), CAST(0x0000A8E900EAFACE AS DateTime), N'::1', N'27.3.254.236', N'vinh', CAST(0x0000A8C200A6C107 AS DateTime), N'vinh', CAST(0x0000A8C200A6C107 AS DateTime))
INSERT [dbo].[user] ([id], [username], [password], [name], [role], [email], [id_security_question], [answer_security_question], [is_active], [last_login_date], [last_logout_date], [ip_last_login], [ip_last_logout], [create_by], [create_date], [update_by], [update_date]) VALUES (3, N'admin', N'e10adc3949ba59abbe56e057f20f883e', N'account_admin', 1, N'admin@vlutrading.com', 11, N'minhvo', 1, CAST(0x0000A8E2010BD977 AS DateTime), CAST(0x0000A8E2010BD29E AS DateTime), N'::1', N'::1', N'admin', CAST(0x0000A8DA00EBD9B0 AS DateTime), N'admin', CAST(0x0000A8DA00EBD9B0 AS DateTime))
INSERT [dbo].[user] ([id], [username], [password], [name], [role], [email], [id_security_question], [answer_security_question], [is_active], [last_login_date], [last_logout_date], [ip_last_login], [ip_last_logout], [create_by], [create_date], [update_by], [update_date]) VALUES (4, N'team04', N'e10adc3949ba59abbe56e057f20f883e', N'team04', 1, N'team4@vlutrading.com', 4, N'nguyen khac nhu', 1, CAST(0x0000A8EA012630DB AS DateTime), CAST(0x0000A8EA01266060 AS DateTime), N'::1', N'::1', N'team04', CAST(0x0000A8DB00A7E28A AS DateTime), N'team04', CAST(0x0000A8DB00A7E28A AS DateTime))
INSERT [dbo].[user] ([id], [username], [password], [name], [role], [email], [id_security_question], [answer_security_question], [is_active], [last_login_date], [last_logout_date], [ip_last_login], [ip_last_logout], [create_by], [create_date], [update_by], [update_date]) VALUES (5, N't1506666', N'ae93d52a2c4aeb6b8d173e16a494f411', N't1506666@gmail.com', 1, N't1506666@gmail.com', 1, N't1506666@gmail.com', 1, CAST(0x0000A8DD011A2D8E AS DateTime), CAST(0x0000A8DD011AA46C AS DateTime), N'125.234.238.130', N'125.234.238.130', N't1506666', CAST(0x0000A8DD011A189C AS DateTime), N't1506666', CAST(0x0000A8DD011A189C AS DateTime))
INSERT [dbo].[user] ([id], [username], [password], [name], [role], [email], [id_security_question], [answer_security_question], [is_active], [last_login_date], [last_logout_date], [ip_last_login], [ip_last_logout], [create_by], [create_date], [update_by], [update_date]) VALUES (7, N'thuysvii', N'49202492d86f9acef357a1bb9ad22915', N'Vi', 1, N'thuysvii@gmail.com', 13, N'Se co thien than thay anh yeu em', 1, CAST(0x0000A8E900C3E488 AS DateTime), CAST(0x0000A8E900BF335A AS DateTime), N'::1', N'::1', N'thuysvii', CAST(0x0000A8E4015F2949 AS DateTime), N'thuysvii', CAST(0x0000A8E4015F2949 AS DateTime))
INSERT [dbo].[user] ([id], [username], [password], [name], [role], [email], [id_security_question], [answer_security_question], [is_active], [last_login_date], [last_logout_date], [ip_last_login], [ip_last_logout], [create_by], [create_date], [update_by], [update_date]) VALUES (8, N'khai', N'49202492d86f9acef357a1bb9ad22915', N'Khai?', 1, N'vinguyen66@vanlanguni.vn', 13, N'Se co thien than thay anh yeu em', 1, CAST(0x0000A8E40179AA7A AS DateTime), NULL, N'::1', NULL, N'khai', CAST(0x0000A8E40179AA7A AS DateTime), N'khai', CAST(0x0000A8E40179AA7A AS DateTime))
INSERT [dbo].[user] ([id], [username], [password], [name], [role], [email], [id_security_question], [answer_security_question], [is_active], [last_login_date], [last_logout_date], [ip_last_login], [ip_last_logout], [create_by], [create_date], [update_by], [update_date]) VALUES (9, N'V', N'49202492d86f9acef357a1bb9ad22915', N'Vi', 1, N'V@vanlanguni.vn', 13, N'Se co thien than thay anh yeu em', 1, CAST(0x0000A8E4017BFF5E AS DateTime), NULL, N'::1', NULL, N'V', CAST(0x0000A8E4017BFF5E AS DateTime), N'V', CAST(0x0000A8E4017BFF5E AS DateTime))
INSERT [dbo].[user] ([id], [username], [password], [name], [role], [email], [id_security_question], [answer_security_question], [is_active], [last_login_date], [last_logout_date], [ip_last_login], [ip_last_logout], [create_by], [create_date], [update_by], [update_date]) VALUES (10, N'Vi', N'49202492d86f9acef357a1bb9ad22915', N'V', 1, N'thuyvi2478@vanlanguni.vn', 13, N'Se co thien than thay anh yeu em', 1, CAST(0x0000A8E4017D376E AS DateTime), NULL, N'::1', NULL, N'Vi', CAST(0x0000A8E4017D376E AS DateTime), N'Vi', CAST(0x0000A8E4017D376E AS DateTime))
INSERT [dbo].[user] ([id], [username], [password], [name], [role], [email], [id_security_question], [answer_security_question], [is_active], [last_login_date], [last_logout_date], [ip_last_login], [ip_last_logout], [create_by], [create_date], [update_by], [update_date]) VALUES (11, N'Vii', N'49202492d86f9acef357a1bb9ad22915', N'V', 1, N'thuyvii@vanlanguni.vn', 13, N'Se co thien than thay anh yeu em', 1, CAST(0x0000A8E4017E0B08 AS DateTime), NULL, N'::1', NULL, N'Vii', CAST(0x0000A8E4017E0B08 AS DateTime), N'Vii', CAST(0x0000A8E4017E0B08 AS DateTime))
INSERT [dbo].[user] ([id], [username], [password], [name], [role], [email], [id_security_question], [answer_security_question], [is_active], [last_login_date], [last_logout_date], [ip_last_login], [ip_last_logout], [create_by], [create_date], [update_by], [update_date]) VALUES (12, N'Viii', N'49202492d86f9acef357a1bb9ad22915', N'Viii', 1, N'thuyviii@vanlanguni.vn', 13, N'S', 1, CAST(0x0000A8E4017E6205 AS DateTime), NULL, N'::1', NULL, N'Viii', CAST(0x0000A8E4017E6205 AS DateTime), N'Viii', CAST(0x0000A8E4017E6205 AS DateTime))
INSERT [dbo].[user] ([id], [username], [password], [name], [role], [email], [id_security_question], [answer_security_question], [is_active], [last_login_date], [last_logout_date], [ip_last_login], [ip_last_logout], [create_by], [create_date], [update_by], [update_date]) VALUES (13, N'minhvo', N'ff462901f02545a0bb4ffcdce90be3e1', N'minh', 1, N'minhvo@vlutrading.com', 3, N'minhvo', 1, CAST(0x0000A8E900BEFD3A AS DateTime), CAST(0x0000A8E900C0CE6C AS DateTime), N'::1', N'::1', N'minhvo', CAST(0x0000A8E900B97744 AS DateTime), N'minhvo', CAST(0x0000A8E900B97744 AS DateTime))
INSERT [dbo].[user] ([id], [username], [password], [name], [role], [email], [id_security_question], [answer_security_question], [is_active], [last_login_date], [last_logout_date], [ip_last_login], [ip_last_logout], [create_by], [create_date], [update_by], [update_date]) VALUES (14, N'Thang', N'e10adc3949ba59abbe56e057f20f883e', N'Thang Nguyen', 1, N'thangnguyen@gmail.com', 3, N'Tran Quang Vinh', 1, CAST(0x0000A8E900C2B1BF AS DateTime), CAST(0x0000A8E900C2BDD5 AS DateTime), N'::1', N'::1', N'Thang', CAST(0x0000A8E900C14558 AS DateTime), N'Thang', CAST(0x0000A8E900C14558 AS DateTime))
INSERT [dbo].[user] ([id], [username], [password], [name], [role], [email], [id_security_question], [answer_security_question], [is_active], [last_login_date], [last_logout_date], [ip_last_login], [ip_last_logout], [create_by], [create_date], [update_by], [update_date]) VALUES (15, N'hai', N'ff462901f02545a0bb4ffcdce90be3e1', N'haicao', 1, N'hai@vlutrading.com', 5, N'meo', 1, CAST(0x0000A8E900C5A7B4 AS DateTime), NULL, N'::1', NULL, N'hai', CAST(0x0000A8E900C5A7B4 AS DateTime), N'hai', CAST(0x0000A8E900C5A7B4 AS DateTime))
INSERT [dbo].[user] ([id], [username], [password], [name], [role], [email], [id_security_question], [answer_security_question], [is_active], [last_login_date], [last_logout_date], [ip_last_login], [ip_last_logout], [create_by], [create_date], [update_by], [update_date]) VALUES (16, N'toai', N'ff462901f02545a0bb4ffcdce90be3e1', N'toaihuynh', 1, N'toai@vlutrading.com', 13, N'ma', 0, CAST(0x0000A8E900C6E856 AS DateTime), NULL, N'::1', NULL, N'toai', CAST(0x0000A8E900C6E856 AS DateTime), N'toai', CAST(0x0000A8E900C6E856 AS DateTime))
SET IDENTITY_INSERT [dbo].[user] OFF
ALTER TABLE [dbo].[item]  WITH CHECK ADD  CONSTRAINT [FK_item_item_status] FOREIGN KEY([status])
REFERENCES [dbo].[item_status] ([id])
GO
ALTER TABLE [dbo].[item] CHECK CONSTRAINT [FK_item_item_status]
GO
ALTER TABLE [dbo].[item]  WITH CHECK ADD  CONSTRAINT [FK_item_userBuyerId] FOREIGN KEY([buyer_id])
REFERENCES [dbo].[user] ([id])
GO
ALTER TABLE [dbo].[item] CHECK CONSTRAINT [FK_item_userBuyerId]
GO
ALTER TABLE [dbo].[item]  WITH CHECK ADD  CONSTRAINT [FK_item_userSellerId] FOREIGN KEY([seller_id])
REFERENCES [dbo].[user] ([id])
GO
ALTER TABLE [dbo].[item] CHECK CONSTRAINT [FK_item_userSellerId]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Order_item] FOREIGN KEY([item_id])
REFERENCES [dbo].[item] ([id])
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK_Order_item]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Order_user] FOREIGN KEY([user_id])
REFERENCES [dbo].[user] ([id])
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK_Order_user]
GO
ALTER TABLE [dbo].[user]  WITH CHECK ADD  CONSTRAINT [FK_user_roleId] FOREIGN KEY([role])
REFERENCES [dbo].[role] ([id])
GO
ALTER TABLE [dbo].[user] CHECK CONSTRAINT [FK_user_roleId]
GO
ALTER TABLE [dbo].[user]  WITH CHECK ADD  CONSTRAINT [FK_user_security_questionId] FOREIGN KEY([id_security_question])
REFERENCES [dbo].[security_question] ([id])
GO
ALTER TABLE [dbo].[user] CHECK CONSTRAINT [FK_user_security_questionId]
GO
ALTER TABLE [dbo].[user_personal_information]  WITH CHECK ADD  CONSTRAINT [FK_user_personal_information_userId] FOREIGN KEY([id_user])
REFERENCES [dbo].[user] ([id])
GO
ALTER TABLE [dbo].[user_personal_information] CHECK CONSTRAINT [FK_user_personal_information_userId]
GO
USE [master]
GO
ALTER DATABASE [vlutrading3545] SET  READ_WRITE 
GO
