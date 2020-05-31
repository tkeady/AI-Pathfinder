using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using System.IO;

namespace Pathfinder
{
	class Dijkstra
	{ //task a
		public bool[,] closed; //whether or not location is closed
		public float[,] cost; //cost value for each location
		public Coord2[,] link; //link for each location = coords
							   //of a neighbouring location
		public bool[,] inPath; //whether or not a location
							   //is in the final path

		public Dijkstra()
		{
			closed = new bool[40, 40];
			cost = new float[40, 40];
			link = new Coord2[40, 40];
			inPath = new bool[40, 40];

			//closed[10, 10] = true; //grid position is closed
			//cost[27,5] = 0; //sets the cost location to 0
			//inPath[12, 3] = true; //sets the rid position as in the path
		}
		//task b
		public void Build(Level level, AiBotBase bot, Player plr)
		{
			for (int i = 0; i < 40; i++)
			{
				for (int j = 0; j < 40; j++)
				{
					closed[i, j] = false;
					cost[i, j] = 1000000;
					link[i, j] = new Coord2(-1, -1);
					inPath[i, j] = false;
				}
			}
			closed[bot.GridPosition.X, bot.GridPosition.Y] = false;

			cost[bot.GridPosition.X, bot.GridPosition.Y] = 0;

			Coord2 pos;
			pos.X = 0;
			pos.Y = 0;

			while (!closed[plr.GridPosition.X, plr.GridPosition.Y])
			{
				float min = 1000000;

				//step 1
				for (int i = 0; i < 40; i++)
				{
					for (int j = 0; j < 40; j++)
					{
						if (cost[i, j] < min && closed[i, j] == false && level.ValidPosition(new Coord2(i, j)))
						{
							min = cost[i, j];
							pos.X = i;
							pos.Y = j;
						}
					}
				}
				//step 2
				closed[pos.X, pos.Y] = true;

				//step 3
				float pairCost = 0;

				for (int i = pos.X - 1; i < pos.X + 2; i++)
				{
					for (int j = pos.Y - 1; j < pos.Y + 2; j++)
					{
						//step 3a
						if (level.ValidPosition(new Coord2(i, j)) && !closed[i, j])
						{
							if (i != pos.X && j != pos.Y) pairCost = 1.4f;
							else if ((i != pos.X && j == pos.Y) || (i == pos.X && j != pos.Y)) pairCost = 1.0f;
							else pairCost = 0.0f;

							//step 3b
							if (cost[pos.X, pos.Y] + pairCost < cost[i, j])
							{
								cost[i, j] = cost[pos.X, pos.Y] + pairCost;

								//step 4
								link[i, j] = pos;
							}
						}
					}
				}

				//step 5
			}
			//task c
			bool done = false; //set to true when we are back at the bot position
			Coord2 nextClosed = plr.GridPosition; //Start of path   

			while (!done)
			{
				inPath[nextClosed.X, nextClosed.Y] = true;
				nextClosed = link[nextClosed.X, nextClosed.Y];
				if (nextClosed == bot.GridPosition) done = true;
			}
		}
	}
}
