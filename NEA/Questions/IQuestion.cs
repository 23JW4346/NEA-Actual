using NEA.Questions.Loci;
using System.Collections.Generic;

namespace NEA
{
    public interface IQuestion
    {
        //returns the question as a string to be printed of by console
        string PrintQuestion();
        //Calculates the answer to the question with the randomly generated numbers
        void Calculate();
        //returns the answer as a string, and either saying correct or incorrect
        string PrintAnswer(bool correct);
        //returns true/false dependant on the user input
        bool CheckAnswer(string answer);
        //returns a list to get put into the main List<list<string>> in program to be saved when the program is closed
        List<string> SaveQuestion();
        //opens the file and then gets the question off of the file
        bool GetQuestion(string filename);
        //if the question requires an argand diagram, this will get called to 
        void LoadDiagram(ArgandDiagram diagram);
        //if the question requires a diagram, close it to save 
        void CloseDiagram(ArgandDiagram diagram);
    }
}
