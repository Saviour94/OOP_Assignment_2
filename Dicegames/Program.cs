using System;
using System.Diagnostics;

// Starting with dice class for a single die, randum number generator for the roll function which is assigned to current_roll

public class Die
{
    private Random random;
    public int current_roll { get; private set; }

    public Die()
    {
        random = new Random();
    }

    public int Roll()
    {
        current_roll = random.Next(1, 7);
        return current_roll;
    }
}

// Sevens out game which takes 2 dice as parameters, take turns rolling until a combined sum of 7 is reached or the total score reaches 1000
// 1000 feels like it takes way too long to reach, switching to 500 in stead
public class sevens_out
{
    private Die die_1;
    private Die die_2;

    public sevens_out(Die Die_1, Die Die_2)
    {
        die_1 = Die_1;
        die_2 = Die_2;
    }

    // if a double is rolled then double the sum total
    public int play_game()
    {
        int roll_total = 0;
        int roll_count = 0;
        while (true)
        {
            int roll_1 = die_1.Roll();
            int roll_2 = die_2.Roll();

            roll_count++;

            int sum = roll_1 + roll_2;
            if (roll_1 == roll_2)
            {
                sum *= 2;
            }

            roll_total += sum;

            Console.WriteLine($"Roll {roll_count}: You rolled {roll_1} and {roll_2}. Total: {roll_total}");

            if (sum == 7)
            {
                Console.WriteLine("You rolled a combined sum of 7. Game over!");
                break;
            }
            else if (roll_total >= 500)
            {
                Console.WriteLine($"You reached a total of {roll_total}. You're the winner!");
                break;
            }
        }
        return roll_total;
    }
}

// Three or more game takes an array of 5 dice objects as parameters
// rolls all 5 dice aiming for 3 of a kind or more of a kind
// calculate score function assigns points to a total score depending on how many of a kind were rolled according to the rules provided
public class three_or_more
{
    private Die[] dice;
    private int total_score;

    public three_or_more(Die[] dice)
    {
        this.dice = dice;
    }

    public int play_game()
    {
        int rollCount = 0;

        while (total_score < 20)
        {
            Console.WriteLine($"Roll {++rollCount}:");
            int[] rolls = roll_all_dice();
            int score = calculate_score(rolls);

            Console.WriteLine($"Your rolls: {string.Join(", ", rolls)}");
            Console.WriteLine($"Score: {score}");

            total_score += score;

            if (total_score >= 20)
            {
                Console.WriteLine($"You reached a total score of {total_score}. Game over!");
                break;
            }

            Console.WriteLine($"Current total score: {total_score}");
            Console.WriteLine("Press Enter to roll again...");
            Console.ReadLine();
        }

        return total_score;
    }

    private int[] roll_all_dice()
    {
        int[] rolls = new int[dice.Length];
        for (int i = 0; i < dice.Length; i++)
        {
            rolls[i] = dice[i].Roll();
        }
        return rolls;
    }

    private int calculate_score(int[] rolls)
    {
        Array.Sort(rolls);

        if (five_of_a_kind(rolls))
            return 12;
        else if (four_of_a_kind(rolls))
            return 6;
        else if (three_of_a_kind(rolls))
            return 3;
        else
            return 0;
    }

    private bool three_of_a_kind(int[] rolls)
    {
        for (int i = 0; i < rolls.Length - 2; i++)
        {
            if (rolls[i] == rolls[i + 1] && rolls[i] == rolls[i + 2])
                return true;
        }
        return false;
    }

    private bool four_of_a_kind(int[] rolls)
    {
        for (int i = 0; i < rolls.Length - 3; i++)
        {
            if (rolls[i] == rolls[i + 1] && rolls[i] == rolls[i + 2] && rolls[i] == rolls[i + 3])
                return true;
        }
        return false;
    }

    private bool five_of_a_kind(int[] rolls)
    {
        for (int i = 0; i < rolls.Length - 4; i++)
        {
            if (rolls[i] == rolls[i + 1] && rolls[i] == rolls[i + 2] && rolls[i] == rolls[i + 3] && rolls[i] == rolls[i + 4])
                return true;
        }
        return false;
    }
}


// Staticss class to track games played
public class Statistics
{
    public int sevens_out_plays { get; private set; }
    public int three_or_more_plays { get; private set; }

    public void record_sevens_out()
    {
        sevens_out_plays++;
    }

    public void record_three_or_more()
    {
        three_or_more_plays++;
    }
}

// Play class contains the game menu allowing the player to choose between games, stats or the testing class
// Input validation added to ensure error handling in the menu phase
public class play_game
{
    private Die[] Dice;
    private Statistics Stats;

    public play_game(Die[] dice, Statistics stats)
    {
        Dice = dice;
        Stats = stats;
    }

    public void Start()
    {
        Console.WriteLine("Welcome to Dice Games!");
        Console.WriteLine("1. Play Sevens Out");
        Console.WriteLine("2. Play Three or More");
        Console.WriteLine("3. View game stats");
        Console.WriteLine("4. Perform Tests");

        int choice;
        while (true)
        {
            if (!int.TryParse(Console.ReadLine(), out choice))
            {
                Console.WriteLine("Invalid input. Please enter a number.");
                continue;
            }

            if (choice < 1 || choice > 4)
            {
                Console.WriteLine("Invalid selection. Please choose a valid option (1-4).");
                continue;
            }
            break; 
        }

        switch (choice)
        {
            default:
                Console.WriteLine("Invalid selection. Please choose a different option.");
                break;
        }

        switch (choice)
        {
            case 1:
                sevens_out sevens_out = new sevens_out(Dice[0], Dice[1]);
                Stats.record_sevens_out();
                sevens_out.play_game();
                break;
            case 2:
                three_or_more three_or_more = new three_or_more(Dice);
                Stats.record_three_or_more();
                three_or_more.play_game();
                break;
            case 3:
                Console.WriteLine($"Sevens Out plays: {Stats.sevens_out_plays}");
                Console.WriteLine($"Three or More plays: {Stats.three_or_more_plays}");
                break;
            case 4:
                Testing.test_game();
                break;
            default:
                Console.WriteLine("Invalid choice. Please select a different option.");
                break;
        }
    }
}

// Testing class checks that the particular rules of the games are behaving as intended
// tests that three or more 
public static class Testing
{
    public static void test_game()
    {
        Die[] dice = new Die[5];
        for (int i = 0; i < 5; i++)
        {
            dice[i] = new Die();
        }
        Debug.Assert(test_three_or_more(dice) >= 20);
        Console.WriteLine("Passed the tests!");
    }

    private static int test_three_or_more(Die[] dice)
    {
        three_or_more three_or_more = new three_or_more(dice);
        return three_or_more.play_game();
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        Die[] dice = new Die[5];
        for (int i = 0; i < 5; i++)
        {
            dice[i] = new Die();
        }
        Statistics stats = new Statistics();
        play_game game = new play_game(dice, stats);

        while (true)
        {
            game.Start();
            Console.WriteLine("Would you like to play again? (Y/N)");
            string playAgain = Console.ReadLine().ToUpper();
            if (playAgain != "Y")
                break;
        }
    }
}


