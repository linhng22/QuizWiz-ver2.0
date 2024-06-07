using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;

namespace ProjectStudyTool.Converter;

public class TextConverter
{
    public static (Card[], List<List<string>>) convertTextToCardArray(string input)
    {
        (string[] questions, string[] correctAnswers, List<List<string>> possibleAnswers) = extractText(input);
        // Create an array of cards
        var cards = new Card[Math.Min(questions.Length, correctAnswers.Length)];
        for (int i = 0; i < cards.Length; i++)
        {
            var card = new Card
            {
                Question = questions[i],
                Answer = correctAnswers[i]
            };
            cards[i] = card;
        } 
        
        return (cards, possibleAnswers);
    }

    public static (string[], string[], List<List<string>>) extractText(string input) {
        // Split the input into each line
        string[] lines = Regex.Split(input, @"\n");

        // Extract the question
        string[] questions = lines
            .Where(line => line.StartsWith("QUESTION"))
            .Select(line => Regex.Replace(line, @"^QUESTION \d+:", string.Empty).Trim())
            .ToArray();

        // Extract the correct answers
        string[] correctAnswers = lines
            .Where(line => line.Contains("CORRECT ANSWER"))
            .Select(line => Regex.Replace(line, @"^([A-E]\) )?(- )?CORRECT ANSWER: (- |[A-E]\) )?", string.Empty).Trim())
            .ToArray();


        // Extract the possible answers
        List<List<string>> possibleAnswers = new List<List<string>>();
        List<string> currentAnswers = new List<string>();
        string currentCorrectAnswer = string.Empty;

        foreach (var line in lines)
        {
            if (line.Contains("CORRECT ANSWER"))
            {
                currentCorrectAnswer = Regex.Replace(line, @"^([A-E]\) )?(- )?CORRECT ANSWER: (- |[A-E]\) )?", string.Empty).Trim();
                if (currentAnswers.Contains(currentCorrectAnswer)) {
                    currentAnswers.Remove(currentCorrectAnswer);
                }
            }
            else if (Regex.IsMatch(line, @"^[A-E]\)"))
            {
                string possibleAnswer = line.Substring(3).Trim();
                if (possibleAnswer != currentCorrectAnswer)
                {
                    currentAnswers.Add(possibleAnswer);
                } else {
                    currentAnswers.Remove(possibleAnswer);
                }
            }
            else if (line.Contains("POSSIBLE ANSWER"))
            {
                string possibleAnswer = line.Substring(line.IndexOf(":") + 1).Trim();
                if (possibleAnswer != currentCorrectAnswer)
                {
                    currentAnswers.Add(possibleAnswer);
                } else {
                    currentAnswers.Remove(possibleAnswer);
                }
            }
            else if (line.StartsWith("QUESTION") && currentAnswers.Count > 0)
            {
                possibleAnswers.Add(currentAnswers);
                currentAnswers = new List<string>();
            }
        }
        if (currentAnswers.Count > 0)
        {
            possibleAnswers.Add(currentAnswers);
        }

        return (questions, correctAnswers, possibleAnswers);
    }
}
