create database OnlineShop
go
use OnlineShop
go

create table UserAccount(
UserID uniqueidentifier default newid() primary key,
FirstName nvarchar(50),
LastName nvarchar(50),
Avatar image,
Gender bit not null,
Birthday datetime not null,
Email varchar(254) COLLATE SQL_Latin1_General_CP1_CS_AS not null,
EmailConfirmed bit not null,
EmailConfirmCode varchar(8),
CreateEmailConfirmCodeAt datetime,
Password varchar(max) COLLATE SQL_Latin1_General_CP1_CS_AS not null,
Phone varchar(10) not null,
Address nvarchar(300),
CreateAt datetime default getdate() not null,
Status bit default 0 not null,
)

create table EmployeeAccount(
EmployeeID uniqueidentifier default newid() primary key,
FirstName nvarchar(50),
LastName nvarchar(50),
Avatar image,
Email varchar(254) COLLATE SQL_Latin1_General_CP1_CS_AS not null,
Password varchar(max) COLLATE SQL_Latin1_General_CP1_CS_AS not null,
Phone varchar(10) not null,
Address nvarchar(300),
Role int not null,
CreateAt datetime default getdate() not null,
Status bit default 0 not null,
)

create table Supplier(
SupplierID varchar(7) primary key,
SupplierName nvarchar(300) not null,
ContractDate datetime not null,
Phone varchar(10),
Email varchar(254),
CreateAt datetime default getdate() not null,
UpdateAt datetime default getdate() not null,
UpdateBy varchar(254) not null,
Status bit default 0 not null
)

create table ProductCategory(
ProductCategoryID int identity(1,1) primary key,
ProductCategoryName nvarchar(300) not null,
CreateAt datetime default getdate() not null,
UpdateAt datetime default getdate() not null,
UpdateBy varchar(254) not null,
Status bit default 0 not null
)

create table Age(
AgeID int identity(1,1) primary key,
AgeName nvarchar(300) not null,
CreateAt datetime default getdate() not null,
UpdateAt datetime default getdate() not null,
UpdateBy varchar(254) not null,
Status bit default 0 not null
)

create table Size(
SizeID varchar(7) primary key,
SizeName nvarchar(50) not null,
CreateAt datetime default getdate() not null,
UpdateAt datetime default getdate() not null,
UpdateBy varchar(254) not null,
Status bit default 0 not null
)

create table Color(
ColorID varchar(7) primary key,
ColorName nvarchar(50) not null,
CreateAt datetime default getdate() not null,
UpdateAt datetime default getdate() not null,
UpdateBy varchar(254) not null,
Status bit default 0 not null
)

create table Product(
ProductID varchar(20) primary key,
ProductName nvarchar(300) not null,
MetaTitle varchar(350) not null,
SupplierID varchar(7) foreign key references Supplier(SupplierID),
ProductCategoryID int foreign key references ProductCategory(ProductCategoryID),
AgeID int foreign key references Age(AgeID),
Gender bit not null,
Description ntext,
Warranty int not null,
ScoreRating float default 0 not null,
UnitImportPrice decimal(18,0) default 0 not null,
UnitSellPrice decimal(18,0) default 0 not null,
DiscountPercent float default 0 not null,
ShowOnHome bit default 0 not null,
CreateAt datetime default getdate() not null,
UpdateAt datetime default getdate() not null,
UpdateBy varchar(254) not null,
Status bit default 0 not null
)

create table ProductPhoto(
ProductPhotoID uniqueidentifier default newid() primary key,
ProductID varchar(20) foreign key references Product(ProductID),
Photo image,
CreateAt datetime default getdate() not null,
UpdateAt datetime default getdate() not null,
UpdateBy varchar(254) not null,
Status bit default 0 not null
)

create table Stock(
ProductID varchar(20) foreign key references Product(ProductID),
SizeID varchar(7) foreign key references Size(SizeID),
ColorID varchar(7) foreign key references Color(ColorID),
Quantity int default 0 not null,
CreateAt datetime default getdate() not null,
UpdateAt datetime default getdate() not null,
UpdateBy varchar(254) not null,
Status bit default 0 not null,
primary key (ProductID, SizeID, ColorID)
)

create table Payment(
PaymentID varchar(6) primary key,
PaymentName nvarchar(300) not null
)

create table ProductOrder(
ProductOrderID uniqueidentifier default newid() primary key,
OrderDate datetime default getdate() not null,
ShipDate datetime,
PaymentID varchar(6) foreign key references Payment(PaymentID),
PaymentOnlineID varchar(30),
UserID uniqueidentifier foreign key references UserAccount(UserID),
Status bit default 0 not null,
)

create table ProductOrderDetail(
ProductOrderID uniqueidentifier foreign key references ProductOrder(ProductOrderID),
ProductID varchar(20),
SizeID varchar(7),
ColorID varchar(7),
Price decimal(18,0) default 0 not null,
Quantity int default 1 not null,
foreign key (ProductID, SizeID, ColorID) references Stock(ProductID, SizeID, ColorID),
primary key (ProductOrderID, ProductID, SizeID, ColorID)
)

create table Invoice(
InvoiceID uniqueidentifier default newid() primary key,
ExportDate datetime default getdate() not null,
TotalPayment decimal(18,0) default 0 not null,
CustomerConfirm bit default 0 not null,
ProductOrderID uniqueidentifier foreign key references ProductOrder(ProductOrderID),
EmployeeID uniqueidentifier foreign key references EmployeeAccount(EmployeeID),
Status bit default 0 not null
)

create table Feedback(
FeedbackID int identity(1,1) primary key,
UserID uniqueidentifier foreign key references UserAccount(UserID),
ProductID varchar(20) foreign key references Product(ProductID),
FeedbackDate datetime default getdate() not null,
Content nvarchar(max)
)

create table Favourite(
UserID uniqueidentifier foreign key references UserAccount(UserID),
ProductID varchar(20) foreign key references Product(ProductID),
primary key (UserID, ProductID)
)

create table Evaluation(
UserID uniqueidentifier foreign key references UserAccount(UserID),
ProductID varchar(20) foreign key references Product(ProductID),
ScoreRating tinyint default 0 not null,
primary key (UserID, ProductID)
)

create table ShoppingCart(
UserID uniqueidentifier foreign key references UserAccount(UserID),
ProductID varchar(20),
SizeID varchar(7),
ColorID varchar(7),
Quantity int default 1 not null,
foreign key (ProductID, SizeID, ColorID) references Stock(ProductID, SizeID, ColorID),
primary key (UserID, ProductID, SizeID, ColorID)
)


--create table Menu(
--MenuID int identity(1,1) primary key,
--Text nvarchar(300) not null,
--Link varchar(350) not null,
--Target varchar(50) not null,
--CreateAt datetime default getdate() not null,
--UpdateAt datetime default getdate() not null,
--UpdateBy varchar(254) not null,
--Status bit default 0 not null
--)

--create table About(
--AboutID int identity(1,1) primary key,
--Title nvarchar(200) not null,
--MetaTitle varchar(250) not null,
--Content ntext,
--Photo image,
--CreateAt datetime default getdate() not null,
--UpdateAt datetime default getdate() not null,
--UpdateBy varchar(254) not null,
--Status bit not null
--)

create table Contact(
ContactID int identity(1,1) primary key,
Address nvarchar(300),
LocationOnGoogleMap varchar(1000),
Phone varchar(10),
Email varchar(254),
FacebookLink varchar(500),
InstagramLink varchar(500),
TwitterLink varchar(500),
CreateAt datetime default getdate() not null,
UpdateAt datetime default getdate() not null,
UpdateBy varchar(254) not null,
Status bit default 0 not null
)

create table Slide(
SlideID int identity(1,1) primary key,
Photo image not null,
PositionAppear int not null,
CreateAt datetime default getdate() not null,
UpdateAt datetime default getdate() not null,
UpdateBy varchar(254) not null,
Status bit default 0 not null
)




