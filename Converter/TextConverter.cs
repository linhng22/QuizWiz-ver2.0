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
    public static (Card[], string[]) convertTextToCardArray(string input)
    {
        (string[] questions, string[] correctAnswers, string[] possibleAnswers) = extractText(input);
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

    public static (string[], string[], string[]) extractText(string input) {
        // Split the input into two parts: questions and answers
        string[] parts = Regex.Split(input,@"(\n)");


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
        string[] possibleAnswers = lines
            .Where(line => line.Contains("POSSIBLE ANSWER"))
            .Select(line => Regex.Replace(line, @"^([A-E]\) )?(- )?POSSIBLE ANSWER: (- |[A-E]\) )?", string.Empty).Trim())
            .ToArray();

        
        return (questions, correctAnswers, possibleAnswers);
    }
}
