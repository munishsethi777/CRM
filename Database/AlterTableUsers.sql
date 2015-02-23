


alter table users
add 
    [name] varchar(100)  NULL,
    
    [password] varchar(50)  NULL,
    [mobile] varchar(50)  NULL,
    [address1] varchar(200)  NULL,
    [address2] varchar(200)  NULL,
    [city] varchar(50)  NULL,
    [state] varchar(50)  NULL,
    [country] varchar(50)  NULL,
    [zip] varchar(50)  NULL,
    [phone] varchar(50)  NULL,
    [usertype] varchar(50)  NULL,
    [isactive] tinyint  NULL,
    [createdon] datetime  NULL,
    [createdby] bigint  NULL,
    [lastmodifiedon] datetime  NULL