// See https://aka.ms/new-console-template for more information
using System.Reflection.Metadata.Ecma335;

Console.WriteLine("Hello, Robo");
string[] commands = File.ReadAllLines("Command.txt");
Robot robo = new Robot();
bool placed = false;
foreach (string command in commands)
{
    Console.Write(command);
    try
    {
        int x = 0;
        int y = 0;
        Face f;
        string cmd;
        
        Position p;

        if (command.Trim().Split(" ").Length > 1)
        {
            cmd = command.Trim().Split(" ")[0];
            string arguments = command.Trim().Split(" ")[1];
            x = int.Parse(arguments.Trim().Split(",")[0]);
            y = int.Parse(arguments.Trim().Split(",")[1]);
            f = Face.Parse<Face>(arguments.Trim().Split(",")[2]);
            placed = robo.Place(x, y, f);
            if (placed)
            {
                Console.WriteLine(" Executed");
            }
            else
            {
                Console.WriteLine(" Ignored");
            }
        }
        else
        {
            cmd = command.Trim();
        
            if (placed)
            {
                if (cmd.ToUpper() == "REPORT")
                {
                    p = robo.Report();
                    Console.WriteLine(" Output: " + p.X.ToString() + ", " + p.Y.ToString() + ", " + p.F.ToString()); break;

                } else
                {
                    if (robo.RunCommand(cmd.ToUpper()))
                    {
                        Console.WriteLine(" Executed");
                    }
                    else
                    {
                        Console.WriteLine(" Ignored");
                    }
                }
                                   
            }
            else
            {
                Console.WriteLine(" Ignored");
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
    
}

Console.ReadLine();

class Robot
{
    Position position;
    public bool Place(int x, int y, Face f)
    {
        if (x < 5 && y < 5 && x >= 0 && y >= 0)
        {
            position = new Position(x,y,f);  
            return true;
        }
        return false; 
    }

    public bool RunCommand(string command)
    {
        switch (command.ToUpper())
        {
            case "MOVE":
                return Move();                    
            case "LEFT": 
                Left(); return true;
            case "RIGHT": 
                Right(); return true;            
           
        }
        return false;
    }

    public void Left()
    {
        position.F = (Face)(((int)position.F + 4 - 1) % 4);
    
    }
    public void Right()
    {
        position.F = (Face)(((int)position.F + 1) % 4);
       
    }

    public bool Move()
    {
        bool isSafe = IsMoveSafe();
        if (isSafe)
        {
            switch(position.F)
            {
                case Face.EAST: position.X++; break;
                case Face.WEST: position.X--; break;
                case Face.NORTH: position.Y++; break;
                case Face.SOUTH: position.Y--; break;
            }
        }
        return isSafe;
      
    }

    public Position Report()
    {
        return position;
    }

    private bool IsMoveSafe()
    {
        bool isSafe = true;
        if ((position.F == Face.EAST && position.X == 4)
            || (position.F == Face.WEST && position.X == 0)
            || (position.F == Face.NORTH && position.Y == 4)
            || (position.F == Face.SOUTH && position.Y == 0))
        {
            isSafe = false;
        }
        return isSafe;

    }

    

}

class Position
{
    public int X { get; set; }
    public int Y { get; set; }
    public Face F { get; set; }
    

    public Position() { }
    public Position(int x, int y, Face f)
    {
        X = x;
        Y = y;
        F = f;
    }

}

enum Face
{
    EAST = 0,
    SOUTH = 1,
    WEST = 2,
    NORTH = 3
}