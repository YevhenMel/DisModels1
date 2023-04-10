using System.Xml.Linq;

namespace DisModels
{
    internal class Program
    {

        static void Main(string[] args)
        {
            Prym prim = new Prym();

            prim.Resolve();
        }

        public class Prym
        {
            public void Resolve()
            {
                int sum = 0;
                List<List<Vertex>> vertex;

                int[,] array;
                var streamReader = new StreamReader("C:/Users/admin/Desktop/DM/files/dm1.txt");
                var dimension = streamReader.ReadLine();
                var numRows = int.Parse(dimension);
                array = new int[numRows, numRows];

                for (int i = 0; i < numRows; i++)
                {
                    string line = streamReader.ReadLine();
                    string[] values = line.Split(' ');
                    for (int j = 0; j < numRows; j++)
                    {
                        array[i, j] = int.Parse(values[j]);
                    }
                }
                streamReader.Close();

                for (int pass = 0; pass < 2; pass++)
                {
                    int passed = 1;
                    vertex = new List<List<Vertex>>();

                    String mapName = "abcdefghijk";
                    for (int i = 0; i < array.GetLength(0); i++)
                    {
                        vertex.Add(new List<Vertex>());
                        for (int j = 0; j < array.GetLength(0); j++)
                        {
                            vertex.ElementAt<List<Vertex>>(i).Add(new Vertex(i, j, array[i, j] == 0 ? true : false, array[i, j], mapName.ElementAt(i).ToString() + mapName.ElementAt(j).ToString()));
                        }
                    }

                    int maxValue = array[0, 0];
                    for (int i = 0; i < array.GetLength(0); i++)
                    {
                        if (array[0, i] > maxValue)
                        {
                            maxValue = array[0, i];
                        }
                    }
                    int minValue = maxValue;
                    for (int i = 0; i < array.GetLength(0); i++)
                    {
                        if (array[0, i] < minValue && array[0, i] != 0)
                        {
                            minValue = array[0, i];
                        }
                    }

                    if (pass == 0)
                    {
                        sum += maxValue;
                    }
                    else
                    {
                        sum = 0;
                        sum += minValue;
                    }

                    Vertex current = vertex.ElementAt(0).Find(x => x.Amount() == (pass == 0 ? maxValue : minValue));
                    Vertex previus = vertex.ElementAt(0).Find(x => x.Amount() == (pass == 0 ? maxValue : minValue));
                    vertex.ElementAt(current.Vertical()).ElementAt(current.Line()).Visited();

                    Console.WriteLine("Link: " + current.Label() + " Value: " + current.Amount());

                    while (true)
                    {
                        if (passed == 7)
                            break;

                        Vertex nextVertex = null;
                        int targetValue = pass == 0 ? 0 : 500;

                        for (int i = 0; i < array.GetLength(0); i++)
                        {
                            int potentialValue = array[current.Line(), i];
                            if (pass == 0 ? (potentialValue > targetValue) : (potentialValue < targetValue && potentialValue != 0))
                            {
                                if (i != current.Vertical() && vertex.ElementAt(current.Line()).ElementAt(i).IsVisited() == false)
                                {
                                    targetValue = potentialValue;
                                }
                            }
                            potentialValue = array[i, current.Vertical()];
                            if (pass == 0 ? (potentialValue > targetValue) : (potentialValue < targetValue && potentialValue != 0))
                            {
                                if (i != current.Line() && vertex.ElementAt(i).ElementAt(current.Vertical()).IsVisited() == false)
                                {
                                    targetValue = potentialValue;
                                }
                            }
                        }
                        nextVertex = vertex.ElementAt(current.Line()).Find(x => x.Amount() == targetValue && x.Vertical() != current.Vertical());
                        if (nextVertex == null)
                        {
                            foreach (List<Vertex> rowNodes in vertex)
                            {
                                nextVertex = rowNodes.Find(x => x.Vertical() == current.Vertical() && x.Amount() == targetValue && x.Line() != current.Line());
                                if (nextVertex != null)
                                    break;
                            }
                        }

                        current = nextVertex;
                        sum += current.Amount();
                        vertex.ElementAt(current.Vertical()).ElementAt(current.Line()).Visited();
                        Console.WriteLine("Link: " + current.Label() + " Value: " + current.Amount());
                        if (current.Line() != previus.Line())
                        {
                            vertex.ElementAt(previus.Line()).ForEach(x => x.Visited());
                        }
                        else if (current.Vertical() != previus.Vertical())
                        {
                            foreach (List<Vertex> rowNodes in vertex)
                            {
                                rowNodes.Find(x => x.Vertical() == previus.Vertical()).Visited();
                            }
                        }
                        previus = current;

                        passed++;
                    }

                    Console.WriteLine((pass == 0 ? "Max" : "Min") + " sum: " + sum);
                }
            }


            public class Vertex
            {
                public int line;
                public int vertical;
                public int amount;
                public bool isVisited;
                public String label = "";

                public Vertex()
                {

                }
                public Vertex(int line, int vertical, bool isVisited, int amount, String label)
                {
                    this.line = line;
                    this.vertical = vertical;
                    this.isVisited = isVisited;
                    this.amount = amount;
                    this.label = label;
                }

                public String Label()
                {
                    return this.label;
                }
                public int Line()
                {
                    return this.line;
                }
                public int Vertical()
                {
                    return this.vertical;
                }
                public bool IsVisited()
                {
                    return this.isVisited;
                }
                public void Visited()
                {
                    this.isVisited = true;
                }
                public int Amount()
                {
                    return this.amount;
                }
            }
        }
    }
}