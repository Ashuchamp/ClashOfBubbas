using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

internal class ScoreBoard
{

    int[] scores;
    String path;
    public ScoreBoard()
    {
        scores = new int[10];
        String fileName = "scores.txt";
        path = Path.Combine(Environment.CurrentDirectory, @"C\", fileName);
        String[] scoreLines = System.IO.File.ReadAllLines(path);
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
    public void modifyScoreBoard(int score)
    {
        if (score > scores[scores.Length - 1])
        {
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