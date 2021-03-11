CREATE TABLE [dbo].[ShipPlacements]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Carrier1] VARCHAR(10) NOT NULL, 
    [Carrier2] VARCHAR(10) NOT NULL, 
    [Carrier3] VARCHAR(10) NOT NULL, 
    [Carrier4] VARCHAR(10) NOT NULL, 
    [Carrier5] VARCHAR(10) NOT NULL, 
    [Battleship1] VARCHAR(10) NOT NULL, 
    [Battleship2] VARCHAR(10) NOT NULL, 
    [Battleship3] VARCHAR(10) NOT NULL, 
    [Battleship4] VARCHAR(10) NOT NULL, 
    [Cruiser1] VARCHAR(10) NOT NULL, 
    [Cruiser2] VARCHAR(10) NOT NULL, 
    [Cruiser3] VARCHAR(10) NOT NULL, 
    [Submarine1] VARCHAR(10) NOT NULL, 
    [Submarine2] VARCHAR(10) NOT NULL, 
    [Submarine3] VARCHAR(10) NOT NULL, 
    [Destroyer1] VARCHAR(10) NOT NULL, 
    [Destroyer2] VARCHAR(10) NOT NULL
)
