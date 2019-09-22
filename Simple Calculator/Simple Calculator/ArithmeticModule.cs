using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Calculator
{
    public struct UnitProblemContextDescription {
        public int head;
        public int tail;
        public string problem;
        public string context;
        public string solution;
    };

    public struct StringWordContextDescription
    {
        public int head;
        public int tail;
        public string sentence;
        public string context;
    };

    class ArithmeticModule
    {
        public ArithmeticModule()
        {
        }

        public string eval(string problem)
        {
            problem = "(" + problem + ")";
            System.Diagnostics.Debug.WriteLine("Evaluating Problem: " + problem);

            UnitProblemContextDescription context = getNextUnitProblemContextDescription(problem);

            while (true)
            {
                // .... solve problem in context
                System.Diagnostics.Debug.WriteLine("Evaluating New Context: " + context.context);
                context.solution = solveContextProblem(context.context);

                // .... replace solved context in problem with solution
                context.problem = replaceSubString(context.problem, context.solution, context.head, context.tail);
                context = getNextUnitProblemContextDescription(context.problem);

                // .... if problem is solved
                if (context.head == context.tail)
                {
                    if (context.problem == null)
                    {
                        return "";
                    }
                    else
                    {
                        return context.problem;
                    }
                }
            }
        }

        public string solveContextProblem(string contextProblem)
        {
            System.Diagnostics.Debug.WriteLine("Solving Context Problem: " + contextProblem);

            // let's deal with (%) percentages first
            contextProblem = solvePercentages(contextProblem);

            // let's deal with (*) multiplications next
            contextProblem = solveMultiplications(contextProblem);

            // let's deal with (/) divisions next
            contextProblem = solveDivisions(contextProblem);

            // let's deal with (+) additions next
            contextProblem = solveAdditions(contextProblem);

            // let's deal with (-) subtractions next
            contextProblem = solveSubtractions(contextProblem);

            return contextProblem;
        }

        public string solvePercentages(string contextProblem)
        {
            while (contextProblem.Contains("%"))
            {
                System.Diagnostics.Debug.WriteLine("Solving Percentages for : " + contextProblem);

                StringWordContextDescription contextDescription = getStringWordContextBeforeIndex(contextProblem, contextProblem.IndexOf('%'));
                contextProblem = replaceSubString(contextProblem, percent(contextDescription.context), contextDescription.head, contextDescription.tail + 1);
            }

            return contextProblem;
        }

        public string solveMultiplications(string contextProblem)
        {
            while (contextProblem.Contains("*"))
            {
                System.Diagnostics.Debug.WriteLine("Solving Multiplication for : " + contextProblem);

                StringWordContextDescription contextDescriptionA = getStringWordContextBeforeIndex(contextProblem, contextProblem.IndexOf('*')-1);
                StringWordContextDescription contextDescriptionB = getStringWordContextAfterIndex(contextProblem, contextProblem.IndexOf('*') + 1);

                contextProblem = replaceSubString(contextProblem, mul(contextDescriptionA.context, contextDescriptionB.context), contextDescriptionA.head, contextDescriptionB.tail);
            }

            return contextProblem;
        }

        public string solveDivisions(string contextProblem)
        {
            while (contextProblem.Contains("/"))
            {
                System.Diagnostics.Debug.WriteLine("Solving Divisions for : " + contextProblem);

                StringWordContextDescription contextDescriptionA = getStringWordContextBeforeIndex(contextProblem, contextProblem.IndexOf('/') - 1);
                StringWordContextDescription contextDescriptionB = getStringWordContextAfterIndex(contextProblem, contextProblem.IndexOf('/') + 1);

                contextProblem = replaceSubString(contextProblem, div(contextDescriptionA.context, contextDescriptionB.context), contextDescriptionA.head, contextDescriptionB.tail);
            }

            return contextProblem;
        }

        public string solveAdditions(string contextProblem)
        {
            while (contextProblem.Contains("+"))
            {
                System.Diagnostics.Debug.WriteLine("Solving Additions for : " + contextProblem);

                StringWordContextDescription contextDescriptionA = getStringWordContextBeforeIndex(contextProblem, contextProblem.IndexOf('+') - 1);
                StringWordContextDescription contextDescriptionB = getStringWordContextAfterIndex(contextProblem, contextProblem.IndexOf('+') + 1);

                contextProblem = replaceSubString(contextProblem, add(contextDescriptionA.context, contextDescriptionB.context), contextDescriptionA.head, contextDescriptionB.tail);
            }

            return contextProblem;
        }

        public string solveSubtractions(string contextProblem)
        {
            while (contextProblem.Contains("—"))
            {
                System.Diagnostics.Debug.WriteLine("Solving Subtractions for : " + contextProblem);

                StringWordContextDescription contextDescriptionA = getStringWordContextBeforeIndex(contextProblem, contextProblem.IndexOf('—') - 1);
                StringWordContextDescription contextDescriptionB = getStringWordContextAfterIndex(contextProblem, contextProblem.IndexOf('—') + 1);

                contextProblem = replaceSubString(contextProblem, sub(contextDescriptionA.context, contextDescriptionB.context), contextDescriptionA.head, contextDescriptionB.tail);
            }

            //return mul(contextDescriptionA.context, contextDescriptionA.context) + contextDescriptionA.head.ToString() + ":" + contextDescriptionA.tail.ToString() + " vs " + contextDescriptionB.head.ToString() + ":" + contextDescriptionB.tail.ToString();
            return contextProblem;
        }

        public StringWordContextDescription getStringWordContextBeforeIndex(string sentence, int endIndex)
        {
            StringWordContextDescription contextDescription = new StringWordContextDescription();
            contextDescription.sentence = sentence;
            contextDescription.tail = --endIndex;
            contextDescription.head = endIndex;
            contextDescription.context = "";

            for (int i=endIndex; i>=0; i--)
            {
                char c = sentence.ElementAt(i);
                if (!isNumber(c) && c != '.' && c != '-')
                {
                    break; // terminal character reached
                }

                contextDescription.head = i;
            }

            try
            {
                contextDescription.context = contextDescription.sentence.Substring(contextDescription.head, contextDescription.tail - contextDescription.head + 1);
            }
            catch (System.ArgumentOutOfRangeException e)
            {
                contextDescription.context = "ERROR";
            }

            return contextDescription;
        }

        public StringWordContextDescription getStringWordContextAfterIndex(string sentence, int startIndex)
        {
            StringWordContextDescription contextDescription = new StringWordContextDescription();
            contextDescription.sentence = sentence;
            contextDescription.tail = ++startIndex;
            contextDescription.head = startIndex;
            contextDescription.context = ":";

            for (int i=startIndex; i < sentence.Length; i++)
            {
                char c = sentence.ElementAt(i);
                if (!isNumber(c) && c != '.' && c != '-')
                {
                    break; // terminal character reached
                }

                contextDescription.tail = i;
            }

            try
            {
                contextDescription.context = contextDescription.sentence.Substring(contextDescription.head, contextDescription.tail - contextDescription.head + 1);
            }
            catch (System.ArgumentOutOfRangeException e)
            {
                contextDescription.context = "ERROR";
            }

            return contextDescription;
        }

        public string replaceSubString(string sentence, string subString, int head, int tail)
        {
            return sentence.Substring(0, head) + subString + sentence.Substring(tail + 1);
        }

        public UnitProblemContextDescription getNextUnitProblemContextDescription(string problem)
        {
            UnitProblemContextDescription context = new UnitProblemContextDescription();
            context.problem = problem;

            for (int i=0; i<problem.Length; i++)
            {
                switch (problem.ElementAt<char>(i))
                {
                    case '(':
                        context.head = i;
                        break;

                    case ')':
                        context.tail = i;
                        context.context = problem.Substring(context.head + 1, context.tail - context.head - 1);
                        return context;
                }
            }

            return context;
        }

        public Boolean isNumber(string str)
        {
            try
            {
                int number = int.Parse(str);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public Boolean isNumber(char c)
        {
            return c >= '0' && c <= '9';
        }

        // throws FormatException
        public string add(string a, string b)
        {
            return (float.Parse(a) + float.Parse(b)).ToString();
        }

        public string sub(string a, string b)
        {
            return (float.Parse(a) - float.Parse(b)).ToString();
        }

        public string mul(string a, string b)
        {
            return (float.Parse(a) * float.Parse(b)).ToString();
        }

        public string div(string a, string b)
        {
            return (float.Parse(a) / float.Parse(b)).ToString();
        }

        public string percent(string a)
        {
            return (float.Parse(a) / 100.0).ToString();
        }
    }
}
