    --use masterrrr
    --drop database FootyPunditsDB
    --go

    CREATE DATABASE FootyPunditsDB;
    GO
    USE FootyPunditsDB;
    GO

    CREATE TABLE UserAccount(
        AccountID INT identity(1,1) NOT NULL,
        AccName NVARCHAR(255) NOT NULL,
        Email NVARCHAR(255) NOT NULL,
        Username NVARCHAR(255) NOT NULL,
        UPass NVARCHAR(255) NOT NULL,
        ProfilePicture NVARCHAR(255) NOT NULL,
        IsAdmin BIT NOT NULL,
        FavoriteTeam INT NOT NULL,
        SignUpDate DATE NOT NULL DEFAULT GETDATE(),
        RankID INT NOT NULL
    );
    ALTER TABLE
        UserAccount ADD CONSTRAINT account_accountid_primary PRIMARY KEY(AccountID);
    CREATE UNIQUE INDEX account_email_unique ON
        UserAccount(Email);
    CREATE UNIQUE INDEX account_username_unique ON
        UserAccount(Username);
    CREATE INDEX account_isadmin_index ON
        UserAccount(IsAdmin);
    CREATE TABLE PlayerRating(
        PlayerID INT NOT NULL,
        GameID INT NOT NULL,
        PlayerRating INT NOT NULL,
        AccountID INT NOT NULL
    );
    ALTER TABLE
        PlayerRating ADD CONSTRAINT playerrating_playerid_primary PRIMARY KEY(PlayerID);
    CREATE TABLE AccMessage(
        MessageID INT NOT NULL IDENTITY(1000, 1),
        AccountID INT NOT NULL,
        Content NVARCHAR(255) NOT NULL,
        SentDate DATE NOT NULL,
        Upvotes INT NOT NULL,
        Downvotes INT NOT NULL,
        ChatGameID INT NOT NULL,   
    );
    ALTER TABLE
        AccMessage ADD CONSTRAINT accmessage_messageid_primary PRIMARY KEY(MessageID);
    CREATE TABLE VotesHistory(
        VoteID INT NOT NULL,
        AccountIDFKEY INT NOT NULL,
        VotedDate DATE NOT NULL,
        VoteType INT NOT NULL,
        MessageID INT NOT NULL,
        IsUpvote BIT NOT NULL
    );
    ALTER TABLE
        VotesHistory ADD CONSTRAINT voteshistory_voteid_primary PRIMARY KEY(VoteID);
    CREATE TABLE Ranks(
        RankID INT NOT NULL,
        MinUpvotes INT NOT NULL,
        RankName NVARCHAR(255) NOT NULL,
        RankLogo NVARCHAR(255) NOT NULL
    );
    ALTER TABLE
        Ranks ADD CONSTRAINT ranks_rankid_primary PRIMARY KEY(RankID);
    ALTER TABLE
        VotesHistory ADD CONSTRAINT voteshistory_accountidfkey_foreign FOREIGN KEY(AccountIDFKEY) REFERENCES UserAccount(AccountID);
    ALTER TABLE
        AccMessage ADD CONSTRAINT accmessage_accountid_foreign FOREIGN KEY(AccountID) REFERENCES UserAccount(AccountID);
    ALTER TABLE
        UserAccount ADD CONSTRAINT useraccount_rankid_foreign FOREIGN KEY(RankID) REFERENCES Ranks(RankID);
    ALTER TABLE
        PlayerRating ADD CONSTRAINT playerrating_accountidfkey_foreign FOREIGN KEY(AccountID) REFERENCES UserAccount(AccountID);
    ALTER TABLE
        VotesHistory ADD CONSTRAINT voteshistory_messageid_foreign FOREIGN KEY(MessageID) REFERENCES AccMessage(MessageID);


    insert into Ranks values (0, 100, 'Kuku1', 'Kuku1.png')
    insert into UserAccount (AccName, Email, Username, UPass, RankId, ProfilePicture, IsAdmin, FavoriteTeam) VALUES ('yoval', 'yoval@yuval.com', 'yuval', '1234', 0, '1.jpg', 1, 1)

    