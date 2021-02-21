using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

internal class ScoreBoard
{
    //array storing the ten highest scores (local)
    int[] scores;
    //the path of the text file that stores the high scores
    String path;
    //Constructor
    public ScoreBoard()
    {
        scores = new int[10];
        String fileName = "scores.txt";
        path = Path.Combine(Environment.CurrentDirectory, @"C\", fileName);
        //Read the contents of the text file with the scores
        String[] scoreLines = System.IO.File.ReadAllLines(path);
        //Convert each numerical string in the file to integers for the array
        //The text file will have a space separating the score's ranking from the score's value
        //ex. 3 4500
        foreach (String str in scoreLines)
        {
            String[] lineArr = str.Split();
            int ranking = int.Parse(lineArr[0]);
            int score = int.Parse(lineArr[1]);
            scores[ranking - 1] = score;
        }
        Console.WriteLine();
    }

    //No priority queues in C#, so array had to be used
    /**
     * Update the scoreboard with the score of the most recent game
     */
    public void modifyScoreBoard(int score)
    {
        //If the score is high enough to make the top 10
        if (score > scores[scores.Length - 1])
        {
            //Insert the most recent score in its proper spot on the leaderboard
            int i = 0;
            while (i < scores.Length && score < scores[i])
            {
                i++;
            }
            for (int j = scores.Length - 1; j > i; j--)
            {
                scores[j] = scores[j - 1];
            }
            scores[i] = score;
            outputScoreBoard();
        }
    }

    /**
     * Replace the existing scoreboard text file with the updated version 
     */
    public void outputScoreBoard()
    {
        String[] output = new String[10];
        for (int i = 0; i < scores.Length; i++)
        {
            output[i] = (i + 1) + " " + scores[i];
        }
        foreach (int score in scores)
        {
            File.WriteAllLines(path, output);
        }
    }
}