USE [comp2007]
GO

/****** Object:  Table [dbo].[Ratings]    Script Date: 18-Aug-2017 12:00:39 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Ratings](
	[RatingId] [nvarchar](128) NOT NULL,
	[UserId] [nvarchar](128) NOT NULL,
	[MovieId] [nvarchar](128) NOT NULL,
	[MovieRating] [decimal](18, 2) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[EditDate] [datetime] NOT NULL,
	[Rank] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_dbo.Ratings] PRIMARY KEY CLUSTERED 
(
	[RatingId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Ratings] ADD  CONSTRAINT [DF_dbo.Ratings_RatingId]  DEFAULT (newid()) FOR [RatingId]
GO

ALTER TABLE [dbo].[Ratings] ADD  CONSTRAINT [DF_dbo.Ratings_CreateDate]  DEFAULT (getutcdate()) FOR [CreateDate]
GO

ALTER TABLE [dbo].[Ratings] ADD  CONSTRAINT [DF_dbo.Ratings_EditDate]  DEFAULT (getutcdate()) FOR [EditDate]
GO

ALTER TABLE [dbo].[Ratings] ADD  DEFAULT ((0)) FOR [Rank]
GO

ALTER TABLE [dbo].[Ratings]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Ratings_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[Ratings] CHECK CONSTRAINT [FK_dbo.Ratings_dbo.AspNetUsers_UserId]
GO

ALTER TABLE [dbo].[Ratings]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Ratings_dbo.Movies_MovieId] FOREIGN KEY([MovieId])
REFERENCES [dbo].[Movies] ([MovieId])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[Ratings] CHECK CONSTRAINT [FK_dbo.Ratings_dbo.Movies_MovieId]
GO


