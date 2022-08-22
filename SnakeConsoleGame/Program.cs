const int width = 32;
const int height = 16;
const int baseSpeed = 1500;

var cells = Enumerable.Range(0, height).SelectMany(e => Enumerable.Range(0, width), (y, x) => (x, y)).ToList();

Queue<(int x, int y)> snake = new(new List<(int x, int y)> { (3, height / 2), (4, height / 2) , (5, height / 2) }) ;
var head = (5, height / 2);
(int x, int y) applePosition = (11, 12);

var directionDictionary = new Dictionary<ConsoleKey, (int x, int y)>
{
    [ConsoleKey.LeftArrow] =  (-1, 0),
    [ConsoleKey.RightArrow] =  (1, 0),
    [ConsoleKey.UpArrow] =  (0, -1),
    [ConsoleKey.DownArrow] =  (0, 1),
};

var direction = directionDictionary[ConsoleKey.RightArrow];


PrintField();

while (true)
{
    await Task.Delay(baseSpeed / snake.Count);
    ReadDirectionChange();
    PrintField();
    if (UpdateSnake())
    {
        break;
    }
}

Console.WriteLine("\n ----> Game lost");

bool UpdateSnake()
{
    var (xs, ys) = head;
    var (xd, yd) = direction;

    var newHead = (xs + xd, ys + yd);
    
    snake.Enqueue(newHead);
    if (head != applePosition)
    {
        _ = snake.Dequeue();
    }
    else
    {
        applePosition = (Random.Shared.Next(1, width - 1), Random.Shared.Next(1, height - 1));
    }

    head = newHead;
    return newHead.Item1 is 0 or width - 1 || newHead.Item2 is 0 or height - 1 || snake.SkipLast(1).Contains(newHead);
}

void PrintField()
{
    Console.Clear();
    foreach (var cell in cells)
    {
        if (snake.Contains(cell))
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("S");
            Console.ResetColor();
        }
        else if (cell.x == 0)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("\n#");
            Console.ResetColor();
        }
        else if (cell.x == width - 1 || cell.y is 0 or height - 1)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("#");
            Console.ResetColor();
        }
        else if (cell == applePosition)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("A");
            Console.ResetColor();
        }
        else
        {
            Console.Write(" ");
        }
    }
}

void ReadDirectionChange()
{
    if (!Console.KeyAvailable)
    {
        return;
    }

    var consoleKey = Console.ReadKey(true).Key;
    var isValid = directionDictionary.TryGetValue(consoleKey, out var directionFromUser);
    if (isValid)
    {
        direction = directionFromUser;
    }
}
